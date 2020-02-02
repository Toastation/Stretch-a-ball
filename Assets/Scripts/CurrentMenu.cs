using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentMenu : MonoBehaviour
{
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

    TextMesh textObject; // textObject is the text displayed on the menu screen
    private Menu currentMenu; // Menu is private for oriented object programmming purposes

    // GetCurrentMenu returns the current menu
    Menu GetCurrentMenu()
    {
        return currentMenu;
    }

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
                textObject.text = "Statistics";
                break;
            case Menu.Hide_Show:
                textObject.text = "Hide/Show";
                break;
            case Menu.Help_Options:
                textObject.text = "Help/Options";
                break;
            case Menu.Quit:
                textObject.text = "Quit";
                break;
            default:
                textObject.text = "Error    ";
                break;
                return 0;
        }
        return 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Finds the display screen
        textObject = GameObject.Find("MenuName").GetComponent<TextMesh>();
        // Initiates the menu value 
        currentMenu = Menu.NoMenuSelected;
        textObject.text = "No Menu Selected";
        
    }

    // Update is called once per fram
    void Update()
    {
        






        
    }
}
