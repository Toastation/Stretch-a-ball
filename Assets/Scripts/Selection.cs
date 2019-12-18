using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointSelection
{
    private List<DataPoint> selectedPoints;

    public PointSelection()
    {
        selectedPoints = new List<DataPoint>();
    }

    public PointSelection AndBoolOperation(PointSelection selec1, PointSelection selec2)
    {   
        PointSelection newSelection = new PointSelection();
        foreach (DataPoint dp in selec1.selectedPoints)
        {
            if (selec1.selectedPoints.Contains(dp))
            {
                newSelection.selectedPoints.Add(dp);
            }
        }
        return newSelection;
    }

    public PointSelection OrBoolOperation(PointSelection selec1, PointSelection selec2)
    {
        PointSelection newSelection = new PointSelection();
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

    public PointSelection NotInBoolOperation(PointSelection selec1, PointSelection selec2)
    {
        PointSelection newSelection = new PointSelection();
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