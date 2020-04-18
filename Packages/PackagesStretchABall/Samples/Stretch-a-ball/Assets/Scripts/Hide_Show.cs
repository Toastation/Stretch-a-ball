using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hide_Show : MonoBehaviour
{
    public float sharpness; //used to size the axis 
    public uint nbGraduation; //Graduation for each axis

    //Varibles used to adapt the size the length of the axis
    Vector3 Min;
    Vector3 Max;
    Vector3 Moy;

    List<DataPoint> dataPoints; //DataPoint in the grid pattern

    //List of axis
    List<GameObject> L1;
    List<GameObject> L2;
    List<GameObject> L3;

    //Main Axis
    GameObject AxeX;
    GameObject AxeY;
    GameObject AxeZ; 

    CurrentMenu cMenu; //The Menu variable
    bool HS; //Indicate when to enable/disable the display
    public static bool selectedpoints; //The point selected in order to 

    //Create and show the axis
    public void Show()
    {
        L1 = new List<GameObject>();
        L2 = new List<GameObject>();
        L3 = new List<GameObject>();

        dataPoints = ScatterPlot.GetDataPoints();
        //Compute the max and min position
        foreach (DataPoint dp in dataPoints)
        {
            Min = Vector3.Min(dp.GetPos(), Min);
            Max = Vector3.Max(dp.GetPos(), Max);
        }
        Moy = (Max - Min) / 2;
        //Creation of the main axis in black
        AxeX = Instantiate(Resources.Load("Prefab/MainCylinder", typeof(GameObject)), new Vector3(Moy.x, 0, 0) + Min, Quaternion.Euler(0, 0, 90)) as GameObject;
        AxeX.transform.parent = gameObject.transform;
        AxeY = Instantiate(Resources.Load("Prefab/MainCylinder", typeof(GameObject)), new Vector3(0, Moy.y, 0) + Min, Quaternion.Euler(0, 90, 0)) as GameObject;
        AxeY.transform.parent = gameObject.transform;
        AxeZ = Instantiate(Resources.Load("Prefab/MainCylinder", typeof(GameObject)), new Vector3(0, 0, Moy.z) + Min, Quaternion.Euler(90, 0, 0)) as GameObject;
        AxeZ.transform.parent = gameObject.transform;
        Vector3 ecart = Max - Min;
        Vector3 increm = ecart / nbGraduation;
        int j = 0;
        //Creation of the numbered axis in white
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
        //Resize of the different axis
        AxeX.transform.localScale = new Vector3((float)2*sharpness, Moy.x, (float)2*sharpness);
        AxeY.transform.localScale = new Vector3((float)2*sharpness, Moy.y, (float)2*sharpness);
        AxeZ.transform.localScale = new Vector3((float)2*sharpness, Moy.z, (float)2*sharpness);
        foreach (GameObject i in L1)
        {
            i.transform.localScale = new Vector3((float)sharpness, Moy.x, (float)sharpness);
            i.transform.parent = gameObject.transform;
        }
        foreach (GameObject i in L2)
        {
            i.transform.localScale = new Vector3((float)sharpness, Moy.y, (float)sharpness);
            i.transform.parent = gameObject.transform;
        }
        foreach (GameObject i in L3)
        {
            i.transform.localScale = new Vector3((float)sharpness, Moy.z, (float)sharpness);
            i.transform.parent = gameObject.transform;
        }
    }

    // Hide the axis
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

    //Initialisation function
    void Start()
    {
        HS = false;
        selectedpoints = false;
        cMenu = GameObject.Find("Palm UI L").GetComponent<CurrentMenu>();
    }

    //Update function that able/enable the display of axis
    public void Update()
    {
        if (cMenu.GetCurrentMenu() == CurrentMenu.Menu.Hide_Show)
        {
            if (!HS)
            {
                HS = !HS;
                Show();
                cMenu.SetMenu(CurrentMenu.Menu.NoMenuSelected);
            }
            else
            {
                HS = !HS;
                Hide();
                cMenu.SetMenu(CurrentMenu.Menu.NoMenuSelected);
            }
        }
        else
        {
        }

    }

}
