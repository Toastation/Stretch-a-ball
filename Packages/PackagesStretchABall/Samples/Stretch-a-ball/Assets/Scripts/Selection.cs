using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Leap.Unity
{
    public class BoolOperation
    {
        private List<DataPoint> selectedPoints;
        public delegate List<DataPoint> Operator(List<DataPoint> select1, List<DataPoint> select2);
        private Operator MethodOperator;

        public BoolOperation()
        {
            selectedPoints = new List<DataPoint>();
        }
        public void BoolUpdate(List<DataPoint> DataPoint, Operator Method)
        {
            // selectedPoints = new List<DataPoint>();
            this.selectedPoints = DataPoint;
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
    }
}