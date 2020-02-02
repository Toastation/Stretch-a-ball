using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Interaction;

public class MenuInteraction : MonoBehaviour
{
    CurrentMenu cMenu;
    InteractionButton ButtonCreation;
    InteractionButton ButtonSelection;
    InteractionButton ButtonStatistics;
    InteractionButton ButtonHide_Show;
    InteractionButton ButtonHelp_Options;
    InteractionButton ButtonQuit;


    // Start is called before the first frame update
    void Start()
    {
        //Initialisation of the different menu buttons
        cMenu = GameObject.Find("Palm UI").GetComponent<CurrentMenu>();
        ButtonCreation = GameObject.Find("Button Creation").GetComponent<InteractionButton>();
        ButtonSelection = GameObject.Find("Button Selection").GetComponent<InteractionButton>();
        ButtonStatistics = GameObject.Find("Button Statistics").GetComponent<InteractionButton>();
        ButtonHide_Show = GameObject.Find("Button Hide/Show").GetComponent<InteractionButton>();
        ButtonHelp_Options = GameObject.Find("Button Help/Options").GetComponent<InteractionButton>();
        ButtonQuit = GameObject.Find("Button Quit").GetComponent<InteractionButton>();


    }

    // Update is called once per frame
    void Update()
    {
        // Checks whether a button is pressed

        if (ButtonCreation.isPressed)
            cMenu.SetMenu(CurrentMenu.Menu.Creation);

        if (ButtonSelection.isPressed)
            cMenu.SetMenu(CurrentMenu.Menu.Selection);

        if (ButtonStatistics.isPressed)
            cMenu.SetMenu(CurrentMenu.Menu.Statistics);

        if (ButtonHide_Show.isPressed)
            cMenu.SetMenu(CurrentMenu.Menu.Hide_Show);

        if (ButtonHelp_Options.isPressed)
            cMenu.SetMenu(CurrentMenu.Menu.Help_Options);

        if (ButtonQuit.isPressed)
            cMenu.SetMenu(CurrentMenu.Menu.Quit);
    }
}
