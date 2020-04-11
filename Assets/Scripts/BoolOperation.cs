using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BoolOperation: MonoBehaviour
{
    private delegate List<DataPoint> Operator(List<DataPoint> select1, List<DataPoint> select2); //Pointer of the boolean function
    private Operator MethodOperator;
    private List<List<DataPoint>> OperationData; //List of Data for the statistics

    public static List<DataPoint> selectedPoints; //The datapoints selected before the 
    public static MeshDeformerMove currentVolume; //The volume selectionned
    public static List<DataPoint> currentSelectedDataPoints; //The datapoints selectionned after the operation and in the currentvolume
   
    CurrentMenu cMenu; //Menu variable
    int test; //Allow to choose only one menu at once

    //Save the first selection et the operation chosen
    private void BoolUpdate(Operator Method)
   {
        if (currentSelectedDataPoints.Count == 0)
            selectedPoints = ScatterPlot.GetSelectedPoints(currentVolume);
        else
            foreach (DataPoint dp in currentSelectedDataPoints)
                selectedPoints.Add(dp);
        MethodOperator = Method;
   }

    //Getter used to make statistics
   public List<List<DataPoint>> GetOperationData()
    {
        return OperationData;
    }

    //The AND operation with two selections
   public List<DataPoint> AndBoolOperation(List<DataPoint> selec1, List<DataPoint> selec2)
    {
        List<DataPoint> newSelection = new List<DataPoint>();
        foreach (DataPoint dp in selec1)
         {
             if (selec2.Contains(dp))
              {
                newSelection.Add(dp);
              }
         }
         return newSelection;
    }

    //The OR operation with two selections
    public List<DataPoint> OrBoolOperation(List<DataPoint> selec1, List<DataPoint> selec2)
    {
        List<DataPoint> newSelection = new List<DataPoint>();
         foreach (DataPoint dp in selec1)
          {
            newSelection.Add(dp);
          }
         foreach (DataPoint dp in selec2)
          {
            if (!newSelection.Contains(dp))
               {
                  newSelection.Add(dp);
               }
            }
         return newSelection;
    }

    //The WITHOUT operation with two selections
    public List<DataPoint> NotInBoolOperation(List<DataPoint> selec1, List<DataPoint> selec2)
     {
        List<DataPoint> newSelection = new List<DataPoint>();
        foreach (DataPoint dp in selec1)
         {
            if (!selec2.Contains(dp))
             {
                newSelection.Add(dp);
             }
         }
        return newSelection;
     }

    //Launch the operation between the two selected datapoints
    public List<DataPoint> BoolOperationMain(List<DataPoint> Data)
     {
        if (selectedPoints.Count != 0)
         {
            Data = MethodOperator(selectedPoints, Data);
            selectedPoints.Clear();
         }
         return Data;
     }

    //Color the datapoints chosen and uncolor the others
    public void Coloration(List<DataPoint> point) 
    {
        for (int i = 0; i < ScatterPlot.dataPoints.Count; i++)
        {
            DataPoint dp = ScatterPlot.dataPoints[i];
            if (point.Contains(dp))
            {
                dp.SetSelected(true);
                ScatterPlot.dataParticles[i].startColor = Color.magenta;
            }
            else
            {
                dp.SetSelected(false);
                ScatterPlot.dataParticles[i].startColor = dp.GetColor();
            }
        }
        ScatterPlot.pSystem.SetParticles(ScatterPlot.dataParticles);
    }

    //Initialisation function
    void Start()
    {
        currentSelectedDataPoints = new List<DataPoint>();
        OperationData = new List<List<DataPoint>>();
        test = 3;
        cMenu = GameObject.Find("Palm UI L").GetComponent<CurrentMenu>();
    }

    //Update function with the Menu
    private void Update()
    {
        if(CurrentMenu.Menu.Selection == cMenu.GetCurrentMenu())
            if(CurrentMenu.Selection.SetOperation == cMenu.GetCurrentMenuSelection())
                switch(cMenu.GetCurrentMenuSetOperation())
                {
                    case CurrentMenu.SetOperation.SetUnion:
                        if (test != 0)
                        {
                            BoolUpdate(OrBoolOperation);
                            test = 0;
                        }
                        break;
                    case CurrentMenu.SetOperation.SetIntersection:
                        if (test != 1)
                        {
                            BoolUpdate(AndBoolOperation);
                            test = 1;
                        }
                        break;
                    case CurrentMenu.SetOperation.SetRelativeComplement:
                        if (test != 2)
                        {
                            BoolUpdate(NotInBoolOperation);
                            test = 2;
                        }
                        break;
                    default:
                        if (cMenu.SetOperationIsConfirmed() && test != 3)
                        {
                            currentSelectedDataPoints = ScatterPlot.GetSelectedPoints(currentVolume);
                            currentSelectedDataPoints = BoolOperationMain(currentSelectedDataPoints);
                            OperationData.Add(currentSelectedDataPoints);
                            Coloration(currentSelectedDataPoints);
                            cMenu.ResetSetOperation();
                            test = 3;
                        }
                        break;
                }
    }
}