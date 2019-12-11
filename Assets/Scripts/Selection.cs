using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selection
{
    private List<DataPoint> selectedPoints;

    public Selection()
    {
        selectedPoints = new List<DataPoint>();
    }

    public Selection AndBoolOperation(Selection selec1, Selection selec2)
    {   
        Selection newSelection = new Selection();
        foreach (DataPoint dp in selec1.selectedPoints)
        {
            if (selec1.selectedPoints.Contains(dp))
            {
                newSelection.selectedPoints.Add(dp);
            }
        }
        return newSelection;
    }

    public Selection OrBoolOperation(Selection selec1, Selection selec2)
    {
        Selection newSelection = new Selection();
        foreach (DataPoint dp in selec1.selectedPoints)
        {
            newSelection.selectedPoints.Add(dp);
        }
        foreach (DataPoint dp in selec2.selectedPoints)
        {
            if (!newSelection.selectedPoints.Contains(dp))
            {
                newSelection.selectedPoints.Add(dp);
            }
        }
        return newSelection;
    }

    public Selection NotInBoolOperation(Selection selec1, Selection selec2)
    {
        Selection newSelection = new Selection();
        foreach (DataPoint dp in selec1.selectedPoints)
        {
            if (!selec2.selectedPoints.Contains(dp))
            {
                newSelection.selectedPoints.Add(dp);
            }
        }
        return newSelection;
    }
}