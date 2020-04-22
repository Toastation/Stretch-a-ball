using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StretchABall
{
    public class CurrentMenu : MonoBehaviour
    {

        #region Pages

        //enumerating the different Menus
        public enum Menu
        {
            NoMenuSelected,
            Creation,
            Selection,
            Statistics,
            Hide_Show,
            Help_Options,
            Quit
        }

        //enumerating the different selections
        public enum Selection
        {
            NoSelection,
            Modification,
            Erase,
            SetOperation,
            Return
        }

        //enumerating the different Set Operations
        public enum SetOperation
        {
            NoOperation,
            SetIntersection,
            SetUnion,
            SetRelativeComplement,
            Confirm,
            Return
        }

        #endregion

        #region Objects

        TextMesh textObject; // textObject is the text displayed on the menu screen

        // Stocking the different menus
        private Menu currentMenu; // Menu is private for oriented object programmming purposes
        private Selection currentSelection;
        private SetOperation currentSetOperation;

        bool flagIsConfirmed;
        //GetCurrentMenuSetOperation returns the current SetOperation

        #endregion

        #region Getters

        public SetOperation GetCurrentMenuSetOperation()
        {
            return currentSetOperation;
        }

        // GetCurrentMenuSelection returns the current menu
        public Selection GetCurrentMenuSelection()
        {
            return currentSelection;
        }

        // GetCurrentMenu returns the current menu
        public Menu GetCurrentMenu()
        {
            return currentMenu;
        }

        #endregion

        #region Set and Display
        // SetMenu changes the value of the current menu
        public int SetMenu(Menu newMenu)
        {
            currentMenu = newMenu;
            // Communicating the new menu to the 3D text
            switch (currentMenu)
            {
                case Menu.NoMenuSelected:
                    textObject.text = "No Menu Selected";
                    break;
                case Menu.Creation:
                    textObject.text = "Creation";
                    break;
                case Menu.Selection:
                    textObject.text = "Selection";
                    break;
                case Menu.Statistics:
                    textObject.text = "Statistics & Save";
                    break;
                case Menu.Hide_Show:
                    textObject.text = "Hide/Show";
                    break;
                case Menu.Help_Options:
                    textObject.text = "Help/Credits";
                    break;
                case Menu.Quit:
                    textObject.text = "Quit";
                    break;
                default:
                    textObject.text = "Error in Current Menu";
                    break;
            }
            return 0;
        }

        // DisplayMenu changes the value of the current menu
        public int DisplayMenu(Menu newMenu)
        {
            // Communicating the new menu to the 3D text
            switch (newMenu)
            {
                case Menu.NoMenuSelected:
                    textObject.text = "No Menu Selected";
                    break;
                case Menu.Creation:
                    textObject.text = "Creation";
                    break;
                case Menu.Selection:
                    textObject.text = "Selection";
                    break;
                case Menu.Statistics:
                    textObject.text = "Statistics & Save";
                    break;
                case Menu.Hide_Show:
                    textObject.text = "Hide/Show";
                    break;
                case Menu.Help_Options:
                    textObject.text = "Help/Credits";
                    break;
                case Menu.Quit:
                    textObject.text = "Quit";
                    break;
                default:
                    textObject.text = "Error in Current Menu";
                    break;
            }
            return 0;
        }

        // SetSelection changes the value of the current selection
        public int SetSelection(Selection newSelection)
        {
            currentSelection = newSelection;
            // Communicating the new menu to the 3D text
            if (currentMenu == Menu.Selection)
            {
                switch (currentSelection)
                {
                    case Selection.NoSelection:
                        textObject.text = "Selection - No Selection in current mode";
                        break;
                    case Selection.Modification:
                        textObject.text = "Selection - Modification";
                        break;
                    case Selection.Erase:
                        textObject.text = "Selection - Erase";
                        break;
                    case Selection.SetOperation:
                        textObject.text = "Selection - Set Operation";
                        break;
                    case Selection.Return:
                        textObject.text = "Selection - Return";
                        break;
                    default:
                        textObject.text = "Error in Current Selection";
                        break;
                }
            }

            return 0;
        }

        // DisplaySelection changes the value of the current selection
        public int DisplaySelection(Selection newSelection)
        {
            // Communicating the menu to the 3D text
            if (currentMenu == Menu.Selection)
            {
                switch (newSelection)
                {
                    case Selection.NoSelection:
                        textObject.text = "Selection - No Selection in current mode";
                        break;
                    case Selection.Modification:
                        textObject.text = "Selection - Modification";
                        break;
                    case Selection.Erase:
                        textObject.text = "Selection - Erase";
                        break;
                    case Selection.SetOperation:
                        textObject.text = "Selection - Set Operation";
                        break;
                    case Selection.Return:
                        textObject.text = "Selection - Return";
                        break;
                    default:
                        textObject.text = "Error in Current Selection";
                        break;
                }
            }

            return 0;
        }

        // SetSelection changes the value of the current set operation
        public int SetSetOperation(SetOperation newSetOperation)
        {
            currentSetOperation = newSetOperation;
            // Communicating the new menu to the 3D text
            if (currentSelection == Selection.SetOperation)
            {
                switch (currentSetOperation)
                {
                    case SetOperation.NoOperation:
                        textObject.text = "Set Operation - No operation in current mode";
                        break;
                    case SetOperation.SetIntersection:
                        textObject.text = "Set Operation - Intersection";
                        break;
                    case SetOperation.SetUnion:
                        textObject.text = "Set Operation - Union";
                        break;
                    case SetOperation.SetRelativeComplement:
                        textObject.text = "Set Operation - Relative Complement";
                        break;
                    case SetOperation.Confirm:
                        textObject.text = "Set Operation - Confirm";
                        break;
                    case SetOperation.Return:
                        textObject.text = "Set Operation - Return";
                        break;
                    default:
                        textObject.text = "Error in Current Selection";
                        break;
                }
            }

            return 0;
        }

        // SetSelection changes the value of the current set operation
        public int DisplaySetOperation(SetOperation newSetOperation)
        {
            // Communicating the new menu to the 3D text
            if (currentSelection == Selection.SetOperation)
            {
                switch (newSetOperation)
                {
                    case SetOperation.NoOperation:
                        textObject.text = "Set Operation - No operation in current mode";
                        break;
                    case SetOperation.SetIntersection:
                        textObject.text = "Set Operation - Intersection";
                        break;
                    case SetOperation.SetUnion:
                        textObject.text = "Set Operation - Union";
                        break;
                    case SetOperation.SetRelativeComplement:
                        textObject.text = "Set Operation - Relative Complement";
                        break;
                    case SetOperation.Confirm:
                        textObject.text = "Set Operation - Confirm";
                        break;
                    case SetOperation.Return:
                        textObject.text = "Set Operation - Return";
                        break;
                    default:
                        textObject.text = "Error in Current Selection";
                        break;
                }
            }

            return 0;
        }

        public bool SetOperationIsConfirmed()
        {
            return flagIsConfirmed;
        }

        public void ConfirmSetOperation()
        {
            flagIsConfirmed = true;
        }

        public void ResetSetOperation()
        {
            flagIsConfirmed = false;
        }

        #endregion

        #region Unity Methods

        // Start is called before the first frame update
        void Start()
        {
            //Finds the display screen
            textObject = GameObject.Find("MenuName").GetComponent<TextMesh>();

            // Initiates the menu values 
            SetMenu(Menu.NoMenuSelected);
            SetSelection(Selection.NoSelection);
            SetSetOperation(SetOperation.NoOperation);
            ResetSetOperation();


        }

        // Update is called once per fram
        void Update()
        {








        }
        #endregion
    }
}
