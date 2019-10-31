﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshDeformerInput : MonoBehaviour
{
    public float force = 10f;
    public float forceOffset = 0.1f;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetMouseButton(0)) {
            HandleInput();
        }  
    }

    private void HandleInput()
    {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition); // ray going from the camera to the current mouse position
        RaycastHit hit;
        if (Physics.Raycast(inputRay, out hit)) {
            MeshDeformer deformer = hit.collider.GetComponent<MeshDeformer>();
            if (deformer) {
                Vector3 point = hit.point;
                point += hit.normal * forceOffset;
                deformer.AddDeformingForce(point, force);
            }
        }
    }
}
