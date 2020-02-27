using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Interaction;
using System.Timers;
using System.Diagnostics;
using System;

public class MenuInteraction : MonoBehaviour
{
    static int FrameCounter;
    static bool ChangeMenuPage;

    CurrentMenu cMenu;
    //Listing of all the buttons
    InteractionButton ButtonCreation;
    InteractionButton ButtonSelection;
    InteractionButton ButtonStatistics;
    InteractionButton ButtonHide_Show;
    InteractionButton ButtonHelp_Options;
    InteractionButton ButtonQuit;


    private bool MenuManager() // MenuManager returns true if it changed a page
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
                                {
                                    cMenu.ResetSetOperation();
                                    cMenu.SetSetOperation(CurrentMenu.SetOperation.SetIntersection);
                                }
            

                                if (ButtonSelection.isPressed)
                                {
                                    cMenu.ResetSetOperation();
                                    cMenu.SetSetOperation(CurrentMenu.SetOperation.SetUnion);
                                }

                                if (ButtonStatistics.isPressed)
                                {
                                    cMenu.ResetSetOperation();
                                    cMenu.SetSetOperation(CurrentMenu.SetOperation.SetRelativeComplement);
                                }

                                if (ButtonHide_Show.isPressed)
                                {
                                    cMenu.ConfirmSetOperation();
                                    cMenu.SetSetOperation(CurrentMenu.SetOperation.NoOperation);
                                }
                               

                                if (ButtonHelp_Options.isPressed)
                                {
                                    cMenu.ResetSetOperation();
                                    cMenu.SetSetOperation(CurrentMenu.SetOperation.Return);
                                    cMenu.SetSelection(CurrentMenu.Selection.NoSelection);
                                    cMenu.SetSetOperation(CurrentMenu.SetOperation.NoOperation);
                                    return true;
                                }



                                break;
                            }

                        default: //Selection Menu behavior
                            {
                                if (ButtonCreation.isPressed)
                                    cMenu.SetSelection(CurrentMenu.Selection.Modification);

                                if (ButtonSelection.isPressed)
                                    cMenu.SetSelection(CurrentMenu.Selection.Erase);
                                    

                                if (ButtonStatistics.isPressed)
                                {
                                    cMenu.SetSelection(CurrentMenu.Selection.SetOperation);
                                    return true;
                                }

                                if (ButtonHide_Show.isPressed)
                                {
                                    cMenu.SetSelection(CurrentMenu.Selection.Return);
                                    cMenu.SetMenu(CurrentMenu.Menu.NoMenuSelected);
                                    cMenu.SetSelection(CurrentMenu.Selection.NoSelection);
                                    return true;
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
                        return true;
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

        return false;
       
    }

    private void MenuPreview() // MenuPreview gives a preview of the button's action
    {
        // Checks whether a button is hovered

        switch (cMenu.GetCurrentMenu())
        {
            
            case CurrentMenu.Menu.Selection:
                {
                    switch (cMenu.GetCurrentMenuSelection())
                    {
                        case CurrentMenu.Selection.SetOperation: // Set Operation Menu behavior
                            {
                                if (ButtonCreation.isPrimaryHovered)
                                {
                                    cMenu.DisplaySetOperation(CurrentMenu.SetOperation.SetIntersection);
                                    break;
                                }

                                if (ButtonSelection.isPrimaryHovered)
                                {
                                    cMenu.DisplaySetOperation(CurrentMenu.SetOperation.SetUnion);
                                    break;
                                }

                                if (ButtonStatistics.isPrimaryHovered)
                                {
                                    cMenu.DisplaySetOperation(CurrentMenu.SetOperation.SetRelativeComplement);
                                    break;
                                }

                                if (ButtonHide_Show.isPrimaryHovered)
                                {
                                    cMenu.DisplaySetOperation(CurrentMenu.SetOperation.Confirm);
                                    break;
                                }

                                if (ButtonHelp_Options.isPrimaryHovered)
                                {
                                    cMenu.DisplaySetOperation(CurrentMenu.SetOperation.Return);
                                    break;
                                }

                                cMenu.DisplaySetOperation(cMenu.GetCurrentMenuSetOperation());
                                break;
                            }

                        default: //Selection Menu behavior
                            {
                                if (ButtonCreation.isPrimaryHovered)
                                {
                                    cMenu.DisplaySelection(CurrentMenu.Selection.Modification);
                                    break;
                                }

                                if (ButtonSelection.isPrimaryHovered)
                                {
                                    cMenu.DisplaySelection(CurrentMenu.Selection.Erase);
                                    break;
                                }


                                if (ButtonStatistics.isPrimaryHovered)
                                {
                                    cMenu.DisplaySelection(CurrentMenu.Selection.SetOperation);
                                    break;
                                }

                                if (ButtonHide_Show.isPrimaryHovered)
                                {
                                    cMenu.DisplaySelection(CurrentMenu.Selection.Return);
                                    break;
                                }

                                cMenu.DisplaySelection(cMenu.GetCurrentMenuSelection());
                                break;
                            }
                    }

                    break;

                }

            default: // General Menu Behavior
                {
                    if (ButtonCreation.isPrimaryHovered)
                    {
                        cMenu.DisplayMenu(CurrentMenu.Menu.Creation);
                        break;
                    }
                    
                    if (ButtonSelection.isPrimaryHovered)
                    {
                        cMenu.DisplayMenu(CurrentMenu.Menu.Selection);
                        break;
                    }

                    if (ButtonStatistics.isPrimaryHovered)
                    {
                        cMenu.DisplayMenu(CurrentMenu.Menu.Statistics);
                        break;
                    }

                    if (ButtonHide_Show.isPrimaryHovered)
                    {
                        cMenu.DisplayMenu(CurrentMenu.Menu.Hide_Show);
                        break;
                    }

                    if (ButtonHelp_Options.isPrimaryHovered)
                    {
                        cMenu.DisplayMenu(CurrentMenu.Menu.Help_Options);
                        break;
                    }

                    if (ButtonQuit.isPrimaryHovered)
                    {
                        cMenu.DisplayMenu(CurrentMenu.Menu.Quit);
                        break;
                    }

                    cMenu.DisplayMenu(cMenu.GetCurrentMenu());
                    break;
                }

        }

        return;

    }

    // Start is called before the first frame update
    void Start()
    {
        //Initialisation of the different menu buttons
        cMenu = GameObject.Find("Palm UI L").GetComponent<CurrentMenu>();
        ButtonCreation = GameObject.Find("Button Creation").GetComponent<InteractionButton>(); // Button for Creation/Modification/Intersection
        ButtonSelection = GameObject.Find("Button Selection").GetComponent<InteractionButton>(); // Button for Selection/Erase/Union
        ButtonStatistics = GameObject.Find("Button Statistics").GetComponent<InteractionButton>(); // Button for Statsistics/Operation/Relative Complement
        ButtonHide_Show = GameObject.Find("Button Hide/Show").GetComponent<InteractionButton>(); // Button for Hide_Show/Return/Return
        ButtonHelp_Options = GameObject.Find("Button Help/Options").GetComponent<InteractionButton>(); // Button for Help_Options
        ButtonQuit = GameObject.Find("Button Quit").GetComponent<InteractionButton>(); // Button for Quit

        FrameCounter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
        /*
            Important notice : when changing pages, the menu waits 120 frames before allowing the user to change current mode or page.
         */

        if(ChangeMenuPage && FrameCounter < 120) // 120 should be defined according to system specs.
        {
            FrameCounter++;
        }
        else
        {
            FrameCounter = 0;
            ChangeMenuPage = MenuManager();
        }

        MenuPreview();

    }
}
