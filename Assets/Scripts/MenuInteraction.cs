﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Interaction;
using System.Timers;
using System;

namespace StretchABall
{
    public class MenuInteraction : MonoBehaviour
    {
        static int FrameCounter;
        static bool ButtonWasPressed;
        public int ButtonDelay;

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
                                    if (ButtonCreation.isPressed || Input.GetKey("c"))
                                    {
                                        cMenu.ResetSetOperation();
                                        cMenu.SetSetOperation(CurrentMenu.SetOperation.SetIntersection);
                                        return true;
                                    }


                                    if (ButtonSelection.isPressed || Input.GetKey("e"))
                                    {
                                        cMenu.ResetSetOperation();
                                        cMenu.SetSetOperation(CurrentMenu.SetOperation.SetUnion);
                                        return true;
                                    }

                                    if (ButtonStatistics.isPressed || Input.GetKey("t"))
                                    {
                                        cMenu.ResetSetOperation();
                                        cMenu.SetSetOperation(CurrentMenu.SetOperation.SetRelativeComplement);
                                        return true;
                                    }

                                    if (ButtonHide_Show.isPressed || Input.GetKey("h"))
                                    {
                                        cMenu.ConfirmSetOperation();
                                        cMenu.SetSetOperation(CurrentMenu.SetOperation.NoOperation);
                                        return true;
                                    }


                                    if (ButtonHelp_Options.isPressed || Input.GetKey("o"))
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
                                    if (ButtonCreation.isPressed || Input.GetKey("c"))
                                    {
                                        cMenu.SetSelection(CurrentMenu.Selection.Modification);
                                        return true;
                                    }

                                    if (ButtonSelection.isPressed || Input.GetKey("e"))
                                    {
                                        cMenu.SetSelection(CurrentMenu.Selection.Erase);
                                        return true;
                                    }


                                    if (ButtonStatistics.isPressed || Input.GetKey("t"))
                                    {
                                        cMenu.SetSelection(CurrentMenu.Selection.SetOperation);
                                        return true;
                                    }

                                    if (ButtonHide_Show.isPressed || Input.GetKey("h"))
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
                        if (ButtonCreation.isPressed || Input.GetKeyDown("c"))
                        {
                            cMenu.SetMenu(CurrentMenu.Menu.Creation);
                            return true;
                        }

                        if (ButtonSelection.isPressed || Input.GetKeyDown("e"))
                        {
                            cMenu.SetMenu(CurrentMenu.Menu.Selection);
                            return true;
                        }

                        if (ButtonStatistics.isPressed || Input.GetKeyDown("t"))
                        {
                            /* For the statistics, press once the button to show the stats.
                             * Press it again to hide the stats
                             */

                            if (cMenu.GetCurrentMenu() == CurrentMenu.Menu.Statistics)
                            {
                                cMenu.SetMenu(CurrentMenu.Menu.NoMenuSelected);
                            }
                            else
                            {
                                cMenu.SetMenu(CurrentMenu.Menu.Statistics);
                            }
                            return true;
                        }

                        if (ButtonHide_Show.isPressed || Input.GetKeyDown("h"))
                        {
                            if (cMenu.GetCurrentMenu() == CurrentMenu.Menu.Hide_Show)
                            {
                                cMenu.SetMenu(CurrentMenu.Menu.NoMenuSelected);
                            }
                            else
                            {
                                cMenu.SetMenu(CurrentMenu.Menu.Hide_Show);
                            }
                            return true;
                        }

                        if (ButtonHelp_Options.isPressed || Input.GetKeyDown("o"))
                        {
                            /* For the help, press once the button to show the them.
                             * Press it again to hide the stats
                             */

                            if (cMenu.GetCurrentMenu() == CurrentMenu.Menu.Help_Options)
                            {
                                cMenu.SetMenu(CurrentMenu.Menu.NoMenuSelected);
                            }
                            else
                            {
                                cMenu.SetMenu(CurrentMenu.Menu.Help_Options);
                            }
                            return true;
                        }

                        if (ButtonQuit.isPressed || Input.GetKeyDown("escape"))
                        {
                            cMenu.SetMenu(CurrentMenu.Menu.Quit);
                            Application.Quit();
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
                Important notice : when changing pages, the menu waits 120 frames when a button is pressed to smooth out the experience
             */

            if (ButtonWasPressed && FrameCounter < ButtonDelay) // 120 should be defined according to system specs.
            {
                FrameCounter++;
            }
            else
            {
                FrameCounter = 0;
                ButtonWasPressed = MenuManager();
            }

            MenuPreview();

        }
    }
}
