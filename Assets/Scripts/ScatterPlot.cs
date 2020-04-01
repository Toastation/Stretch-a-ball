using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ScatterPlot : MonoBehaviour
{

    /** The path to the CSV file containing the data. Set by the user */
    public string csvPath;


    /** Prefab used to instanciate data point (if not in particles mode) */
    public GameObject PointPrefab;


    /** The list of points in the scatterplot and their particle representation */
    private static ParticleSystem pSystem;
    public static List<DataPoint> dataPoints;
    private ParticleSystem.Particle[] dataParticles;
    private List<ParticleCollisionEvent> collisionEvents;

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

        PreprocessData();
    }

    void Update()
    {
        /** TEMPORARY **/
        if (Input.GetKeyDown(KeyCode.R))
        {
            MeshDeformerMove[] volumes = FindObjectsOfType<MeshDeformerMove>();
            List<DataPoint> selection = GetSelectedPoints(volumes[0]);
            InitParticles();
        }
    }

    /**
     * Initialize the prefab (if the scatterplot is not using particles)
     */
    private void InitDataPoints()
    {
        foreach (DataPoint dp in dataPoints)
        {
            GameObject dpInstance = Instantiate(PointPrefab, dp.GetPos(), Quaternion.identity);
            dpInstance.GetComponent<Renderer>().material.color = dp.GetColor();
            dp.SetGameObject(dpInstance);
        }
    }

    /**
     * Initialize the particles or prefab and preprocess the data
     */
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
            dataParticles[i].remainingLifetime = i; // since lifetime isn't used, we can store the corresponding datapoint id in it
        }
        pSystem.SetParticles(dataParticles, dataParticles.Length);

        collisionEvents = new List<ParticleCollisionEvent>();
    }

    /**
     * Preprocess the scatterplot data : find the max and min values and compute normalized coordinates.
     */
    private void PreprocessData()
    {
        Vector3 maxValues = GetMaxPlotValues();
        
        // compute and store normalized coordinates of each data points
        foreach (DataPoint dp in dataPoints)
        {
            dp.SetNormalizedPos(dp.GetPos().x / maxValues.x, dp.GetPos().y / maxValues.y, dp.GetPos().z / maxValues.z);
        }
    }

    /**
     * Return a vec3 containing the maximum value of each coordinate in the scatterplot
     */
    private Vector3 GetMaxPlotValues()
    {
        Vector3 max = Vector3.zero;
        foreach (DataPoint dp in dataPoints)
        {
            if (dp.GetPos().x > max.x) max.x = dp.GetPos().x;
            if (dp.GetPos().y > max.y) max.y = dp.GetPos().y;
            if (dp.GetPos().z > max.z) max.z = dp.GetPos().z;
        }
        return max;
    }

    /**
     * Returns a list of all datapoints contained in the given volume 
     */
    public static List<DataPoint> GetSelectedPoints(MeshDeformerMove volume)
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
