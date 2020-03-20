using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpOptions : MonoBehaviour
{
    private Canvas helpOptionsCanvas; // Assign in inspector
    CurrentMenu cMenu;


    // Start is called before the first frame update
    void Start()
    {
        helpOptionsCanvas = GetComponent<Canvas>();
        cMenu = GameObject.Find("Palm UI L").GetComponent<CurrentMenu>();
    }

    // Update is called once per frame
    void Update()
    {
        if (cMenu.GetCurrentMenu() == CurrentMenu.Menu.Help_Options)
        {
            if (!helpOptionsCanvas.enabled)
                helpOptionsCanvas.enabled = !helpOptionsCanvas.enabled;

        }
        else
        {
            if (helpOptionsCanvas.enabled)
                helpOptionsCanvas.enabled = !helpOptionsCanvas.enabled;
        }
    }
}

