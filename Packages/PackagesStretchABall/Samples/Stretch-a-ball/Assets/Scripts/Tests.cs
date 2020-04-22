using System;
using System.Collections.Generic;
using UnityEngine;

namespace StretchABall
{
    public class Tests : MonoBehaviour
    {
        //Variable object
        BoolOperation BO;
        int FrameCount;

        //Constructor of the test class
        public Tests()
        {
        }

        //Verify if there is the same datapoints in dp1 and dp2
        private bool verif(List<DataPoint> dp1, List<DataPoint> dp2)
        {
            foreach (DataPoint dp in dp1)
            {
                if (!dp2.Contains(dp))
                    return false;
            }
            foreach (DataPoint dp in dp2)
            {
                if (!dp1.Contains(dp))
                    return false;
            }
            return true;
        }

        //Test the function in BoolOperation
        public bool Test__BoolOperation()
        {
            List<DataPoint> volume1 = new List<DataPoint>();
            List<DataPoint> volume2 = new List<DataPoint>();
            List<DataPoint> AND = new List<DataPoint>();
            List<DataPoint> OR = new List<DataPoint>();
            List<DataPoint> WITHOUT1 = new List<DataPoint>();
            List<DataPoint> WITHOUT2 = new List<DataPoint>();
            System.Random aleatoire = new System.Random();
            for (int i = 0; i < 30; i++)
            {
                DataPoint dp = new DataPoint(new Vector3(i, i, i), Color.magenta);
                int entier = aleatoire.Next(1, 4);
                switch (entier)
                {
                    case 1:
                        volume1.Add(dp);
                        OR.Add(dp);
                        WITHOUT1.Add(dp);
                        break;
                    case 2:
                        volume2.Add(dp);
                        OR.Add(dp);
                        WITHOUT2.Add(dp);
                        break;
                    default:
                        volume1.Add(dp);
                        volume2.Add(dp);
                        OR.Add(dp);
                        AND.Add(dp);
                        break;
                }
            }
            return (verif(OR, BO.OrBoolOperation(volume1, volume2)) && verif(AND, BO.AndBoolOperation(volume1, volume2)) && verif(WITHOUT1, BO.NotInBoolOperation(volume1, volume2)) && verif(WITHOUT2, BO.NotInBoolOperation(volume2, volume1)));
        }

        //Start function which launch tests
        void Start()
        {
            BO = GameObject.Find("Others/BooleanOperation").GetComponent<BoolOperation>();
            FrameCount = 0;
        }

        //Update function with the Menu
        private void Update()
        {
            if (Input.GetKey("n"))
            {
                if (FrameCount < 0)
                {
                    Debug.Log("Test__BoolOperation :" + Test__BoolOperation());
                    FrameCount = 120;
                }
            }
            FrameCount--;
        }
    }
}