using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionSphereSpawner : MonoBehaviour
{
    /*
     * The following script creates a visual benchmark by instanciating 120 selection meshes.
     * 
     */

    GameObject currentSelection;
    CurrentMenu cMenu;
    int i;

    // Start is called before the first frame update
    void Start()
    {
        cMenu = GameObject.Find("Palm UI L").GetComponent<CurrentMenu>();
        i = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (cMenu.GetCurrentMenu() == CurrentMenu.Menu.Creation &&
            Input.GetKeyDown("n")) 
        {
            for (i = -20; i < 20; i++)
            {
                // Gets the prefab
                currentSelection = Instantiate(Resources.Load("sphereMesh", typeof(GameObject))) as GameObject;
                //Modifies the position and scale
                currentSelection.transform.position = new Vector3(i, 0, 0);
                currentSelection.transform.localScale = new Vector3(1, 1, 1);
                /* End here*/
            }
            for (i = -20; i < 20; i++)
            {
                // Gets the prefab
                currentSelection = Instantiate(Resources.Load("sphereMesh", typeof(GameObject))) as GameObject;
                //Modifies the position and scale
                currentSelection.transform.position = new Vector3(0, i, 0);
                currentSelection.transform.localScale = new Vector3(1, 1, 1);
                /* End here*/
            }
            for (i = -20; i < 20; i++)
            {
                // Gets the prefab
                currentSelection = Instantiate(Resources.Load("sphereMesh", typeof(GameObject))) as GameObject;
                //Modifies the position and scale
                currentSelection.transform.position = new Vector3(0, 0, i);
                currentSelection.transform.localScale = new Vector3(1, 1, 1);
                /* End here*/
            }

        }
    }
}
