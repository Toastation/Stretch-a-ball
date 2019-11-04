using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshDeformerInput : MonoBehaviour
{

    /** The force of the deformation, the greater the force the more the mesh is deformed */
    public float force = 10f;
    /** Offset of the contact point across the normal of the surface hit, when the offset increased the displacement of each vertex becomes more parallel */
    public float forceOffset = 0.1f;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetMouseButton(0)) {
            Deform(true);
        } else if (Input.GetMouseButton(1)) {
            Deform(false);
        }
    }

    private void Deform(bool push)
    {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition); // ray going from the camera to the current mouse position
        RaycastHit hit;
        if (Physics.Raycast(inputRay, out hit)) {
            MeshDeformer deformer = hit.collider.GetComponent<MeshDeformer>();
            if (deformer) {
                Vector3 contactPoint = hit.point;
                if (push)
                    contactPoint += hit.normal * forceOffset;
                else 
                    contactPoint -= hit.normal * forceOffset;
                deformer.SpreadForce(contactPoint, force);
            }
        }
    }
}
