using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BoolOperation: MonoBehaviour
{
    private delegate List<DataPoint> Operator(List<DataPoint> select1, List<DataPoint> select2);
    private Operator MethodOperator;
    private List<List<DataPoint>> OperationData;

    public static List<DataPoint> selectedPoints;
    public static MeshDeformerMove currentVolume;
    public static List<DataPoint> currentSelectedDataPoints;
    CurrentMenu cMenu;

    int test;

   private void BoolUpdate(Operator Method)
   {
        // selectedPoints = new List<DataPoint>();
       // selectedPoints = ScatterPlot.GetSelectedPoints(currentVolume);
        MethodOperator = Method;
   }

   public List<List<DataPoint>> GetOperationData()
    {
        return OperationData;
    }

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

    public List<DataPoint> BoolOperationMain(List<DataPoint> Data)
     {
        if (selectedPoints.Count != 0)
         {
            Data = MethodOperator(selectedPoints, Data);
            selectedPoints.Clear();
         }
         return Data;
     }

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

    void Start()
    {
        currentSelectedDataPoints = new List<DataPoint>();
        OperationData = new List<List<DataPoint>>();
        test = 3;
        cMenu = GameObject.Find("Palm UI L").GetComponent<CurrentMenu>();
    }

    private void Update()
    {
        if (CurrentMenu.Menu.Selection == cMenu.GetCurrentMenu())
            if (CurrentMenu.Selection.SetOperation == cMenu.GetCurrentMenuSelection())
            {
                if (currentVolume != null)    
                    switch (cMenu.GetCurrentMenuSetOperation())
                    {
                        case CurrentMenu.SetOperation.SetUnion:
                            if (test != 0)
                            {
                                if (currentSelectedDataPoints.Count == 0)
                                    selectedPoints = ScatterPlot.GetSelectedPoints(currentVolume);
                                else
                                    selectedPoints = currentSelectedDataPoints;
                                BoolUpdate(OrBoolOperation);
                                test = 0;
                            }
                            break;
                        case CurrentMenu.SetOperation.SetIntersection:
                            if (test != 1)
                            {
                                if (currentSelectedDataPoints.Count == 0)
                                    selectedPoints = ScatterPlot.GetSelectedPoints(currentVolume);
                                else
                                    selectedPoints = currentSelectedDataPoints;
                                BoolUpdate(AndBoolOperation);
                                test = 1;
                            }
                            break;
                        case CurrentMenu.SetOperation.SetRelativeComplement:
                            if (test != 2)
                            {
                                if (currentSelectedDataPoints.Count == 0)
                                    selectedPoints = ScatterPlot.GetSelectedPoints(currentVolume);
                                else
                                    selectedPoints = currentSelectedDataPoints;
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
                                // Hide_Show.selectedpoints = true;
                                cMenu.ResetSetOperation();
                                test = 3;

                            }
                            break;
                    }
            }
    }
}