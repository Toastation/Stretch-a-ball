using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BoolOperation: MonoBehaviour
{
    private List<DataPoint> selectedPoints;
    private delegate List<DataPoint> Operator(List<DataPoint> select1, List<DataPoint> select2);
    private Operator MethodOperator;

    public static MeshDeformerMove currentVolume;
    public static List<DataPoint> currentSelectedDataPoints;
    CurrentMenu cMenu;

   private void BoolUpdate(Operator Method)
   {
        // selectedPoints = new List<DataPoint>();
        this.selectedPoints = ScatterPlot.GetSelectedPoints(currentVolume);
        this.MethodOperator = Method;
   }

   public List<DataPoint> AndBoolOperation(List<DataPoint> selec1, List<DataPoint> selec2)
    {
        List<DataPoint> newSelection = new List<DataPoint>();
        foreach (DataPoint dp in selec1)
         {
             if (selec1.Contains(dp))
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
        if (this.selectedPoints.Count != 0)
         {
            Data = MethodOperator(this.selectedPoints, Data);
            this.selectedPoints.Clear();
         }
         return Data;
     }

    void Start()
    {
        cMenu = GameObject.Find("Palm UI L").GetComponent<CurrentMenu>();
    }

    private void Update()
    {
        if(CurrentMenu.Menu.Selection == cMenu.GetCurrentMenu())
            if(CurrentMenu.Selection.SetOperation == cMenu.GetCurrentMenuSelection())
                switch(cMenu.GetCurrentMenuSetOperation())
                {
                    case CurrentMenu.SetOperation.SetUnion:
                        BoolUpdate(OrBoolOperation);
                        Debug.Log("Or");
                        break;
                    case CurrentMenu.SetOperation.SetIntersection:
                        BoolUpdate(AndBoolOperation);
                        Debug.Log("And");
                        break;
                    case CurrentMenu.SetOperation.SetRelativeComplement:
                        BoolUpdate(NotInBoolOperation);
                        Debug.Log("Without");
                        break;
                    case CurrentMenu.SetOperation.Confirm:
                        Debug.Log("Confirmation");
                        currentSelectedDataPoints = ScatterPlot.GetSelectedPoints(currentVolume);
                        currentSelectedDataPoints = BoolOperationMain(currentSelectedDataPoints);
                        Hide_Show.selectedpoints = true;
                        foreach (DataPoint datap in currentSelectedDataPoints) 
                        {
                            datap.SetSelected(true);
                            datap.SetColor(Color.magenta);
                        }

                        break;
                    default:
                        break;
                }
    }
}