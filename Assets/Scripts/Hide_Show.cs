﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hide_Show : MonoBehaviour
{
    Vector3 Min;
    Vector3 Max;
    Vector3 Moy;
    int nbGraduation;

    //List of axis
    List<GameObject> L1 = new List<GameObject>();
    List<GameObject> L2 = new List<GameObject>();
    List<GameObject> L3 = new List<GameObject>();

    //Main Axis
    GameObject AxeX = new GameObject();
    GameObject AxeY = new GameObject();
    GameObject AxeZ = new GameObject();

    // Start is called before the first frame update
    public Hide_Show(List<DataPoint> dataPoints, int Graduation)
    {
        nbGraduation = Graduation;
        foreach (DataPoint dp in dataPoints)
        {
            Min = Vector3.Min(dp.GetPos(), Min);
            Max = Vector3.Max(dp.GetPos(), Max);
        }
        Moy = (Max - Min) / 2;
    }

    public void Show()
    {
        AxeX = Instantiate(Resources.Load("Prefab/MainCylinder", typeof(GameObject)), new Vector3(Moy.x, 0, 0) + Min, Quaternion.Euler(0, 0, 90)) as GameObject;
        AxeY = Instantiate(Resources.Load("Prefab/MainCylinder", typeof(GameObject)), new Vector3(0, Moy.y, 0) + Min, Quaternion.Euler(0, 90, 0)) as GameObject;
        AxeZ = Instantiate(Resources.Load("Prefab/MainCylinder", typeof(GameObject)), new Vector3(0, 0, Moy.z) + Min, Quaternion.Euler(90, 0, 0)) as GameObject;
        Vector3 ecart = Max - Min;
        Vector3 increm = ecart / nbGraduation;
        int j = 0;
        while (j < nbGraduation)
        {
            L1.Add(Instantiate(Resources.Load("Prefab/Cylinder", typeof(GameObject)), new Vector3(Moy.x, 0, increm.z) + Min, Quaternion.Euler(0, 0, 90)) as GameObject);
            L1.Add(Instantiate(Resources.Load("Prefab/Cylinder", typeof(GameObject)), new Vector3(Moy.x, increm.y, 0) + Min, Quaternion.Euler(0, 0, 90)) as GameObject);
            L2.Add(Instantiate(Resources.Load("Prefab/Cylinder", typeof(GameObject)), new Vector3(0, Moy.y, increm.z) + Min, Quaternion.Euler(0, 90, 0)) as GameObject);
            L2.Add(Instantiate(Resources.Load("Prefab/Cylinder", typeof(GameObject)), new Vector3(increm.x, Moy.y, 0) + Min, Quaternion.Euler(0, 90, 0)) as GameObject);
            L3.Add(Instantiate(Resources.Load("Prefab/Cylinder", typeof(GameObject)), new Vector3(increm.x, 0, Moy.z) + Min, Quaternion.Euler(90, 0, 0)) as GameObject);
            L3.Add(Instantiate(Resources.Load("Prefab/Cylinder", typeof(GameObject)), new Vector3(0, increm.y, Moy.z) + Min, Quaternion.Euler(90, 0, 0)) as GameObject);
            increm += ecart / nbGraduation;
            j++;

        }
        AxeX.transform.localScale = new Vector3((float)0.5, Moy.x, (float)0.5);
        AxeY.transform.localScale = new Vector3((float)0.5, Moy.y, (float)0.5);
        AxeZ.transform.localScale = new Vector3((float)0.5, Moy.z, (float)0.5);
        foreach (GameObject i in L1)
        {
            i.transform.localScale = new Vector3((float)0.05, Moy.x, (float)0.05);
        }
        foreach (GameObject i in L2)
        {
            i.transform.localScale = new Vector3((float)0.05, Moy.y, (float)0.05);
        }
        foreach (GameObject i in L3)
        {
            i.transform.localScale = new Vector3((float)0.05, Moy.z, (float)0.05);
        }
    }

    // Update is called once per frame
    public void Hide()
    {
        Destroy(AxeX);
        Destroy(AxeY);
        Destroy(AxeZ);
        foreach (GameObject i in L1)
        {
            Destroy(i);
        }
        foreach (GameObject i in L2)
        {
            Destroy(i);
        }
        foreach (GameObject i in L3)
        {
            Destroy(i);
        }
    }
}
