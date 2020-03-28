using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.IO;

public class Statistics : MonoBehaviour
{
    private Canvas StatisticsCanvas; // Assign in inspector
    CurrentMenu cMenu;
    ScatterPlot pointCloud;
    int pointCount;
    MeshDeformerMove[] volumes;
    Text GeneralStats;
    Text DynamicStats;

    Vector3 AveragePosition(List<DataPoint> points)
    {
        Vector3 Average = new Vector3();
        Vector3 tmp= new Vector3();
        for(int i = 0; i < points.Count; i++)
        {
            tmp = points[i].GetPos();
            Average += tmp;
        }
        Average /= points.Count;
        return Average;
    }

    Color DominantColor(List<DataPoint> points)
    {
        Color Max;
        Vector4 tmp = new Vector4();
        for (int i = 0; i < points.Count; i++)
        {
            tmp += (Vector4) points[i].GetColor();
        }
        if(tmp[0] > tmp[1])
        {
            if(tmp[0] > tmp[2])
            {
                Max = new Color(1.0f, 0.0f, 0.0f, 1.0f); // r dominant
            }
            else
            {
                Max = new Color(0.0f, 0.0f, 1.0f, 1.0f); // b dominant
            }
        }
        else if(tmp[2] > tmp[1])
        {
            Max = new Color(0.0f, 0.0f, 1.0f, 1.0f); // b dominant
        }
        else
        {
            Max = new Color(0.0f, 1.0f, 0.0f, 1.0f); // b dominant
        }

        return Max;
    }

    int toCSV(List<DataPoint> points, int number)
    {
        //Writes the data to a csv file
        string path = "Assets/Resources/selection" + number + ".csv";
        File.WriteAllText(path, "x,y,z,color");
        StreamWriter sr = new StreamWriter(path, true);
        sr.WriteLine("");
        foreach(DataPoint point in points)
        {
            char color = 'x';
            if(point.GetColor() == Color.red) 
                color = 'r';
            if (point.GetColor() == Color.green)
                color = 'g';
            if (point.GetColor() == Color.blue)
                color = 'b';
            Vector3 pos = point.GetPos();
            sr.WriteLine(pos.x + "," + pos.y + "," + pos.z + "," + color);
            //writer.WriteLine(selectionSets[i].ToString());
        }
        sr.Close();
        return 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        StatisticsCanvas = GetComponent<Canvas>();
        cMenu = GameObject.Find("Palm UI L").GetComponent<CurrentMenu>();
        pointCloud = GameObject.Find("ScatterPlot").GetComponent<ScatterPlot>();
        GeneralStats = GameObject.Find("GeneralStats").GetComponent<Text>();
        DynamicStats = GameObject.Find("DynamicStats").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (cMenu.GetCurrentMenu() == CurrentMenu.Menu.Statistics)
        {
            if(!StatisticsCanvas.enabled)
            {
                StatisticsCanvas.enabled = !StatisticsCanvas.enabled; // enables the canvas

                //Loads and displays the general statistics
                pointCount = pointCloud.GetLoadedPointsCount();
                GeneralStats.text = "NUMBER OF POINTS : " + pointCount + "\n \n ";
                
                //Loads and displays the dynamic statistics
                volumes = FindObjectsOfType<MeshDeformerMove>();
                GeneralStats.text += "NUMBER OF MESHES : " + volumes.Length + "\n";
                DynamicStats.text = "THESE ARE THE MESHES YOU HAVE CREATED SO FAR :\n \n \n";
                List<List<DataPoint>> selectionSets = new List<List<DataPoint>>();
                Debug.Log("volumes " + volumes.Length);
                Debug.Log("Je fonctionne st");


                for (int i = 0; i < volumes.Length; i++)
                {
                    selectionSets.Add(ScatterPlot.GetSelectedPoints(ref volumes[i]));
                    DynamicStats.text += (" - Set number " + i + " contains : " + selectionSets[i].Count + " points \n");
                    DynamicStats.text += (" Average position : " + AveragePosition(selectionSets[i]) + "\n");
                    DynamicStats.text += (" Dominant color : " + DominantColor(selectionSets[i]) +  "\n \n");

                    //Saves a csv file
                    toCSV(selectionSets[i], i);
                }


            }



        }
        else
        {
            if (StatisticsCanvas.enabled)
                StatisticsCanvas.enabled = !StatisticsCanvas.enabled;
        }
    }
}
