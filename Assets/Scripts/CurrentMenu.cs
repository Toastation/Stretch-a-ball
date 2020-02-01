using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentMenu : MonoBehaviour
{
    public enum Menu
    {
        NoMenuSelected,
        Creation,
        Selection,
        Statistiques,
        Hide_Show,
        Help_Options,
        Quit
    }
    TextMesh textObject;
    private Menu currentMenu;

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
            case Menu.Statistiques:
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
        textObject = GameObject.Find("MenuName").GetComponent<TextMesh>();
        textObject.text = "Init";
        currentMenu = Menu.NoMenuSelected;
    }

    // Update is called once per fram
    void Update()
    {
        






        
    }
}
