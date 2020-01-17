using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMotion : MonoBehaviour
{
    float mainSpeed = 2.5f;
    float camSens = 100.0f;
     
    void Update () {
        // rotations (pitch and yaw)
        Vector3 rot = new Vector3();
        if (Input.GetKey(KeyCode.UpArrow)) {
            rot.x -= camSens * Time.deltaTime;
        } else if (Input.GetKey(KeyCode.DownArrow)) {
            rot.x += camSens * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.LeftArrow)) {
            rot.y -= camSens * Time.deltaTime;
        } else if (Input.GetKey(KeyCode.RightArrow)) {
            rot.y += camSens * Time.deltaTime;
        }
        transform.eulerAngles += rot;

        // translations    
        Vector3 p = GetDir();
        p = p * mainSpeed * Time.deltaTime;
        Vector3 newPosition = transform.position;
        if (Input.GetKey(KeyCode.Space)) {
            transform.Translate(p);
            newPosition.x = transform.position.x;
            newPosition.z = transform.position.z;
            transform.position = newPosition;
        }
        else {
            transform.Translate(p);
        }   
    }
     
    private Vector3 GetDir() { 
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
        return dir;
    }
}
