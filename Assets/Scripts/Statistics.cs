using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Statistics : MonoBehaviour
{
    private Canvas StatisticsCanvas; // Assign in inspector
    CurrentMenu cMenu;
    ScatterPlot pointCloud;
    int pointCount;
    Text GeneralStats;

    // Start is called before the first frame update
    void Start()
    {
        StatisticsCanvas = GetComponent<Canvas>();
        cMenu = GameObject.Find("Palm UI L").GetComponent<CurrentMenu>();
        pointCloud = GameObject.Find("ScatterPlot").GetComponent<ScatterPlot>();
        GeneralStats = GameObject.Find("GeneralStats").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (cMenu.GetCurrentMenu() == CurrentMenu.Menu.Statistics)
        {
            if(!StatisticsCanvas.enabled)
                StatisticsCanvas.enabled = !StatisticsCanvas.enabled;

            pointCount = pointCloud.GetLoadedPointsCount();
            GeneralStats.text = "Loaded points count : " + pointCount;

        }
        else
        {
            if (StatisticsCanvas.enabled)
                StatisticsCanvas.enabled = !StatisticsCanvas.enabled;
        }
    }
}
