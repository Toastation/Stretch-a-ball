using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ScatterPlot : MonoBehaviour
{

    /** The path to the CSV file containing the data. Set by the user */
    public string csvPath;

    public GameObject PointPrefab;

    /** The list of points in the scatterplot and their particle representation */
    private ParticleSystem pSystem;
    public static List<DataPoint> dataPoints;
    private ParticleSystem.Particle[] dataParticles;

    void Start()
    {
        // load the points from the csv and prints the number of points loaded
        var watch = System.Diagnostics.Stopwatch.StartNew();
        dataPoints = LoadCSV.LoadCSVFile(csvPath);
        watch.Stop();
        var elapsedMs = watch.ElapsedMilliseconds;
        Debug.Log("Loaded " + dataPoints.Count + " points in "+elapsedMs+" ms");

        //instantiate prefab
        pSystem = GetComponent<ParticleSystem>();
        watch = System.Diagnostics.Stopwatch.StartNew();
        InitParticles();
        watch.Stop();
        elapsedMs = watch.ElapsedMilliseconds;
        Debug.Log("Instanciated point prefabs in " + elapsedMs + " ms");
    }

    void Update()
    {
        /** TEMPORARY **/
        if (Input.GetKeyDown(KeyCode.R))
        {
            MeshDeformerMove[] volumes = FindObjectsOfType<MeshDeformerMove>();
            List<DataPoint> selection = GetSelectedPoints(ref volumes[0]);
            InitParticles();
        }
    }

    private void InitDataPoints()
    {
        foreach (DataPoint dp in dataPoints)
        {
            GameObject dpInstance = Instantiate(PointPrefab, dp.GetPos(), Quaternion.identity);
            dpInstance.GetComponent<Renderer>().material.color = dp.GetColor();
            dp.SetGameObject(dpInstance);
        }
    }

    private void InitParticles()
    {
        dataParticles = new ParticleSystem.Particle[dataPoints.Count];
        for (int i = 0; i < dataPoints.Count; i++)
        {
            dataParticles[i].position = dataPoints[i].GetPos();
            if (dataPoints[i].isSelected())
                dataParticles[i].startColor = Color.magenta;
            else
                dataParticles[i].startColor = dataPoints[i].GetColor();
            dataParticles[i].startSize = 0.01f;
        }
        pSystem.SetParticles(dataParticles, dataParticles.Length);
    }

    /**
     * Returns a list of all datapoints contained in the given volume 
     */
    public static List<DataPoint> GetSelectedPoints(ref MeshDeformerMove volume)
    {
        List<DataPoint> pointsInVolume = new List<DataPoint>();
        foreach (DataPoint dp in dataPoints) 
        {
            if (volume.IsInside(dp.GetPos())) 
            {
                pointsInVolume.Add(dp);
                dp.SetSelected(true);
            }
            else
            {
                dp.SetSelected(false);
            }
        }
        return pointsInVolume;
    }

    /**
     * Returns a list of all datapoints contained in all volumes in the scene 
     */
    public List<DataPoint> GetAllSelectedPoints() 
    {
        List<DataPoint> pointsInVolume = new List<DataPoint>();
        MeshDeformerMove[] volumes = FindObjectsOfType<MeshDeformerMove>();
        foreach (DataPoint dp in dataPoints) 
        {
            foreach (MeshDeformerMove volume in volumes)
            {
                if (volume.Contains(dp.GetPos()))
                {
                    pointsInVolume.Add(dp);
                    break;
                } 
            }
        }
        return pointsInVolume;
    }

    public int GetLoadedPointsCount()
    {
        return dataPoints.Count;
    }
}
