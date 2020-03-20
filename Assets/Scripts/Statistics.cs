using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Statistics : MonoBehaviour
{
    private Canvas StatisticsCanvas; // Assign in inspector
    CurrentMenu cMenu;


    // Start is called before the first frame update
    void Start()
    {
        StatisticsCanvas = GetComponent<Canvas>();
        cMenu = GameObject.Find("Palm UI L").GetComponent<CurrentMenu>();
    }

    // Update is called once per frame
    void Update()
    {
        if (cMenu.GetCurrentMenu() == CurrentMenu.Menu.Statistics)
        {
            if(!StatisticsCanvas.enabled)
                StatisticsCanvas.enabled = !StatisticsCanvas.enabled;
        }
        else
        {
            if (StatisticsCanvas.enabled)
                StatisticsCanvas.enabled = !StatisticsCanvas.enabled;
        }
    }
}
