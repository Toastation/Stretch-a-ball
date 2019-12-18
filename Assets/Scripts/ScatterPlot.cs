using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScatterPlot : MonoBehaviour
{

    /** The path to the CSV file containing the data. Set by the user */
    public string csvPath;

    public GameObject PointPrefab;

    /** The list of points in the scatterplot */
    private List<DataPoint> dataPoints;

    void Start()
    {
        // load the points from the csv and prints the number of points loaded
        dataPoints = LoadCSV.LoadCSVFile(csvPath);
        Debug.Log("Loaded " + dataPoints.Count + " points");

        //instantiate prefab
        foreach (DataPoint dp in dataPoints)
        {
            GameObject dpInstance = Instantiate(PointPrefab, dp.GetPos(), Quaternion.identity);
            dpInstance.GetComponent<Renderer>().material.color = dp.GetColor();
            dp.SetGameObject(dpInstance);
        }
    }

    void Update()
    {
        /** TEMPORARY **/
        if (Input.GetKeyDown(KeyCode.R))
        {
            MeshDeformerMove[] volumes = FindObjectsOfType<MeshDeformerMove>();
            foreach (DataPoint dp in dataPoints)
            {
                Debug.Log(volumes[0].Contains(dp.GetPos()));
            }
        }
    }

    /**
     * Returns a list of all datapoints contained in the given volume 
     */
    private List<DataPoint> GetSelectedPoints(ref MeshDeformerMove volume)
    {
        List<DataPoint> pointsInVolume = new List<DataPoint>();
        foreach (DataPoint dp in dataPoints) 
        {
            if (volume.Contains(dp.GetPos())) 
            {
                pointsInVolume.Add(dp);
            }
        }
        return pointsInVolume;
    }

    /**
     * Returns a list of all datapoints contained in all volumes in the scene 
     */
    private List<DataPoint> GetAllSelectedPoints() 
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
    

}
