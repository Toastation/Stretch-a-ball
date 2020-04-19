using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.IO;

namespace StretchABall
{
    public class Statistics : MonoBehaviour
    {
        private Canvas StatisticsCanvas; // Assign in inspector
        CurrentMenu cMenu;
        ScatterPlot pointCloud;
        BoolOperation boolOperation;
        int pointCount;
        MeshDeformerMove[] volumes;
        Text GeneralStats;
        Text DynamicStats;
        Text BoolStats;

        Vector3 AveragePosition(List<DataPoint> points)
        {
            // Returns the average position of a DataPoint list
            Vector3 Average = new Vector3();
            Vector3 tmp = new Vector3();
            for (int i = 0; i < points.Count; i++)
            {
                tmp = points[i].GetPos();
                Average += tmp;
            }
            Average /= points.Count;
            return Average;
        }

        Color DominantColor(List<DataPoint> points)
        {
            //Returns the dominant color in DataPoint list
            Color Max;
            Vector4 tmp = new Vector4();
            for (int i = 0; i < points.Count; i++)
            {
                tmp += (Vector4)points[i].GetColor();
            }
            if (tmp[0] > tmp[1])
            {
                if (tmp[0] > tmp[2])
                {
                    Max = new Color(1.0f, 0.0f, 0.0f, 1.0f); // r dominant
                }
                else
                {
                    Max = new Color(0.0f, 0.0f, 1.0f, 1.0f); // b dominant
                }
            }
            else if (tmp[2] > tmp[1])
            {
                Max = new Color(0.0f, 0.0f, 1.0f, 1.0f); // b dominant
            }
            else
            {
                Max = new Color(0.0f, 1.0f, 0.0f, 1.0f); // b dominant
            }

            return Max;
        }

        int toCSV(List<DataPoint> points, string type, int number)
        {
            //Writes a list of DataPoints to a csv file
            string path = "Assets/Resources/" + type + number + ".csv";
            File.WriteAllText(path, "x,y,z,color");
            StreamWriter sr = new StreamWriter(path, true);
            sr.WriteLine("");
            foreach (DataPoint point in points)
            {
                char color = 'x';
                if (point.GetColor() == Color.red)
                    color = 'r';
                if (point.GetColor() == Color.green)
                    color = 'g';
                if (point.GetColor() == Color.blue)
                    color = 'b';
                Vector3 pos = point.GetPos();
                sr.WriteLine(pos.x + "," + pos.y + "," + pos.z + "," + color);
            }
            sr.Close();
            return 0;
        }

        // Start is called before the first frame update
        void Start()
        {
            // Gets all the information about the points
            StatisticsCanvas = GetComponent<Canvas>();
            cMenu = GameObject.Find("Palm UI L").GetComponent<CurrentMenu>();
            pointCloud = GameObject.Find("ScatterPlot").GetComponent<ScatterPlot>();
            boolOperation = GameObject.Find("BooleanOperation").GetComponent<BoolOperation>();
            GeneralStats = GameObject.Find("GeneralStats").GetComponent<Text>();
            DynamicStats = GameObject.Find("DynamicStats").GetComponent<Text>();
            BoolStats = GameObject.Find("BoolStats").GetComponent<Text>();
        }

        // Update is called once per frame
        void Update()
        {
            if (cMenu.GetCurrentMenu() == CurrentMenu.Menu.Statistics)
            {
                if (!StatisticsCanvas.enabled)
                {
                    // Enables the canvas
                    StatisticsCanvas.enabled = !StatisticsCanvas.enabled;

                    //Loads and displays the general statistics
                    pointCount = pointCloud.GetLoadedPointsCount();
                    GeneralStats.text = "NUMBER OF POINTS : " + pointCount + "\n \n ";

                    //Loads and displays the dynamic statistics
                    volumes = FindObjectsOfType<MeshDeformerMove>();
                    GeneralStats.text += "NUMBER OF MESHES : " + volumes.Length + "\n";
                    DynamicStats.text = "THESE ARE THE MESHES YOU HAVE CREATED SO FAR :\n \n \n";
                    BoolStats.text = "THESE ARE THE OPERATIONS YOU HAVE MADE SO FAR : \n \n \n";
                    List<List<DataPoint>> selectionSets = new List<List<DataPoint>>();
                    List<List<DataPoint>> boolSets = new List<List<DataPoint>>();


                    for (int i = 0; i < volumes.Length; i++)
                    {
                        selectionSets.Add(ScatterPlot.GetSelectedPoints(volumes[i]));
                        DynamicStats.text += (" - Set number " + i + " contains : " + selectionSets[i].Count + " points \n");
                        DynamicStats.text += (" Average position of the points : " + AveragePosition(selectionSets[i]) + "\n");
                        DynamicStats.text += (" Position of the Mesh : " + volumes[i].transform.position + "\n");
                        DynamicStats.text += (" Dominant color : " + DominantColor(selectionSets[i]) + "\n \n");

                        //Saves a csv file
                        toCSV(selectionSets[i], "selection", i);
                    }

                    boolSets = boolOperation.GetOperationData();
                    for (int i = 0; i < boolSets.Count; i++)
                    {
                        BoolStats.text += (" - Operation number " + i + " contains : " + boolSets[i].Count + " points \n");
                        BoolStats.text += (" Average position of the points : " + AveragePosition(boolSets[i]) + "\n");
                        BoolStats.text += (" Dominant color : " + DominantColor(boolSets[i]) + "\n \n");

                        toCSV(boolSets[i], "booloperation", i);
                    }
                }



            }
            else
            {
                //Disables the canvas
                if (StatisticsCanvas.enabled)
                    StatisticsCanvas.enabled = !StatisticsCanvas.enabled;
            }
        }
    }
}