using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMotion : MonoBehaviour
{
    public float translationSpeed = 2.5f;
    public float rotationSpeed = 100.0f;
     
    void Update () {
        // rotation (pitch)
        Vector3 rot = new Vector3();
        if (Input.GetKey(KeyCode.UpArrow)) {
            rot.x -= rotationSpeed * Time.deltaTime;
        } else if (Input.GetKey(KeyCode.DownArrow)) {
            rot.x += rotationSpeed * Time.deltaTime;
        }
        // rotation (yaw)
        if (Input.GetKey(KeyCode.LeftArrow)) {
            rot.y -= rotationSpeed * Time.deltaTime;
        } else if (Input.GetKey(KeyCode.RightArrow)) {
            rot.y += rotationSpeed * Time.deltaTime;
        }
        // translations    
        Vector3 p = GetTranslationDir() * translationSpeed * Time.deltaTime;
        
        transform.eulerAngles += rot;
        transform.Translate(p);
    }
     
    private Vector3 GetTranslationDir() { 
        Vector3 dir = new Vector3();
        if (Input.GetKey (KeyCode.Z)){
            dir += new Vector3(0, 0 , 1);
        }
        if (Input.GetKey (KeyCode.S)){
            dir += new Vector3(0, 0, -1);
        }
        if (Input.GetKey (KeyCode.Q)){
            dir += new Vector3(-1, 0, 0);
        }
        if (Input.GetKey (KeyCode.D)){
            dir += new Vector3(1, 0, 0);
        }
        if (Input.GetKey(KeyCode.Space)) {
            dir += new Vector3(0, 1, 0);
        }
        if (Input.GetKey(KeyCode.LeftShift)) {
            dir += new Vector3(0, -1, 0);
        }
        return dir;
    }
}
