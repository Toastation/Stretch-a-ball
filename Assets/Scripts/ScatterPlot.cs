using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScatterPlot : MonoBehaviour
{

    /** The path to the CSV file containing the data. Set by the user */
    public string csvPath;

    /** The list of points in the scatterplot */
    private List<DataPoint> dataPoints;

    void Start()
    {
        // load the points from the csv and prints the number of points loaded
        dataPoints = LoadCSV.LoadCSVFile(csvPath);   
        Debug.Log("nb of points : " + dataPoints.Count);
    }

    void Update()
    {
        
    }
}
