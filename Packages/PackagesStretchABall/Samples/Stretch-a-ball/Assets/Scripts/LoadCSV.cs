using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LoadCSV
{
    /**
     * Loads and parses the given CSV file
     * Returns a list of DataPoint
     */
    public static List<DataPoint> LoadCSVFile(string filePath)
    {
        List<DataPoint> dataPoints = new List<DataPoint>();
        int count = 0;
        using(var reader = new StreamReader(filePath))
        {
            while (!reader.EndOfStream) {
                string line = reader.ReadLine();
                if (line != "") {
                    if (count++ == 0) continue; // skip the first line
                    string[] attributes = line.Split(',');
                    Vector3 pos = new Vector3();
                    pos.x = System.Convert.ToSingle(attributes[0], System.Globalization.CultureInfo.InvariantCulture); // Single <--> Float
                    pos.y = System.Convert.ToSingle(attributes[1], System.Globalization.CultureInfo.InvariantCulture);
                    pos.z = System.Convert.ToSingle(attributes[2], System.Globalization.CultureInfo.InvariantCulture);
                    DataPoint point = new DataPoint(pos, getColorFromChar(attributes[3][0]));
                    dataPoints.Add(point);
                }
            }
        }
        return dataPoints;
    }

    /**
     * Auxiliary function used to convert single character to colors
     * Returns a color or white if the given character is invalid
     */
    private static Color getColorFromChar(char c) {
        switch (c) {
            case 'r':
                return Color.red;
            case 'b':
                return Color.blue;
            case 'g':
                return Color.green;
        }
        return Color.white;
    }
}
