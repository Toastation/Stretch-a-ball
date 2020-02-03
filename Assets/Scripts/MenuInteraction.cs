using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Interaction;
using System.Timers;

public class MenuInteraction : MonoBehaviour
{
    private int TimeOut;

    private void MyTimer(int Time)
    {
        Timer T = new System.Timers.Timer();
        T.Interval = Time;
        T.Enabled = true;
        return;
    }

    CurrentMenu cMenu;
    //Listing of all the buttons
    InteractionButton ButtonCreation;
    InteractionButton ButtonSelection;
    InteractionButton ButtonStatistics;
    InteractionButton ButtonHide_Show;
    InteractionButton ButtonHelp_Options;
    InteractionButton ButtonQuit;


    void MenuManager()
    {
        // Checks whether a button is pressed

        switch (cMenu.GetCurrentMenu())
        {
            case CurrentMenu.Menu.Selection:
                {
                    switch (cMenu.GetCurrentMenuSelection())
                    {
                        case CurrentMenu.Selection.SetOperation: // Set Operation Menu behavior
                            {
                                if (ButtonCreation.isPressed)
                                    cMenu.SetSetOperation(CurrentMenu.SetOperation.SetIntersection);

                                if (ButtonSelection.isPressed)
                                    cMenu.SetSetOperation(CurrentMenu.SetOperation.SetUnion);

                                if (ButtonStatistics.isPressed)
                                    cMenu.SetSetOperation(CurrentMenu.SetOperation.SetRelativeComplement);

                                if (ButtonHide_Show.isPressed)
                                {
                                    cMenu.SetSetOperation(CurrentMenu.SetOperation.Return);
                                    cMenu.SetSelection(CurrentMenu.Selection.NoSelection);
                                    cMenu.SetSetOperation(CurrentMenu.SetOperation.NoOperation);
                                }

                                break;
                            }

                        default: //Selection Menu behavior
                            {
                                if (ButtonCreation.isPressed)
                                    cMenu.SetSelection(CurrentMenu.Selection.Modification);

                                if (ButtonSelection.isPressed)
                                {
                                    cMenu.SetSelection(CurrentMenu.Selection.Erase);
                                }

                                if (ButtonStatistics.isPressed)
                                    cMenu.SetSelection(CurrentMenu.Selection.SetOperation);

                                if (ButtonHide_Show.isPressed)
                                {
                                    cMenu.SetSelection(CurrentMenu.Selection.Return);
                                    cMenu.SetMenu(CurrentMenu.Menu.NoMenuSelected);
                                    cMenu.SetSelection(CurrentMenu.Selection.NoSelection);
                                }

                                break;
                            }
                    }

                    break;
                    
                }

            default: // General Menu Behavior
                {
                    if (ButtonCreation.isPressed)
                        cMenu.SetMenu(CurrentMenu.Menu.Creation);

                    if (ButtonSelection.isPressed)
                    {
                        cMenu.SetMenu(CurrentMenu.Menu.Selection);
                    }

                    if (ButtonStatistics.isPressed)
                        cMenu.SetMenu(CurrentMenu.Menu.Statistics);

                    if (ButtonHide_Show.isPressed)
                        cMenu.SetMenu(CurrentMenu.Menu.Hide_Show);

                    if (ButtonHelp_Options.isPressed)
                        cMenu.SetMenu(CurrentMenu.Menu.Help_Options);

                    if (ButtonQuit.isPressed)
                    {
                        cMenu.SetMenu(CurrentMenu.Menu.Quit);
                        UnityEditor.EditorApplication.isPlaying = false;
                    }
                    break;
                }
                
        }

        return;
       
    }

    // Start is called before the first frame update
    void Start()
    {
        //Initialisation of the different menu buttons
        cMenu = GameObject.Find("Palm UI").GetComponent<CurrentMenu>();
        ButtonCreation = GameObject.Find("Button Creation").GetComponent<InteractionButton>(); // Button for Creation/Modification/Intersection
        ButtonSelection = GameObject.Find("Button Selection").GetComponent<InteractionButton>(); // Button for Selection/Erase/Union
        ButtonStatistics = GameObject.Find("Button Statistics").GetComponent<InteractionButton>(); // Button for Statsistics/Operation/Relative Complement
        ButtonHide_Show = GameObject.Find("Button Hide/Show").GetComponent<InteractionButton>(); // Button for Hide_Show/Return/Return
        ButtonHelp_Options = GameObject.Find("Button Help/Options").GetComponent<InteractionButton>(); // Button for Help_Options
        ButtonQuit = GameObject.Find("Button Quit").GetComponent<InteractionButton>(); // Button for Quit

        //Initialisation of the timer
        TimeOut = 500;


    }

    // Update is called once per frame
    void Update()
    {
        MenuManager();
    }
}
