using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Interaction;

public class MenuInteraction : MonoBehaviour
{
    CurrentMenu cMenu;
    InteractionButton Button;

    // Start is called before the first frame update
    void Start()
    {
        cMenu = GameObject.Find("Palm UI").GetComponent<CurrentMenu>();
        Button = GameObject.Find("Button Creation").GetComponent<InteractionButton>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Button.isPressed)
            cMenu.SetMenu(CurrentMenu.Menu.Creation);
    }
}
