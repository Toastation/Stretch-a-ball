using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.IO;
public class PositionDisplay : MonoBehaviour
{
    private Canvas PositionCanvas; // Assign in inspector
    Text positionDisplay;

    // Start is called before the first frame update
    void Start()
    {
        PositionCanvas = GetComponent<Canvas>();
        positionDisplay = GameObject.Find("Position").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        positionDisplay.text = "Position" + Camera.main.transform.position;
    }
}
