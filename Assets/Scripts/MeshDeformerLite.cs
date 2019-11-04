using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshDeformer : MonoBehaviour
{
    Mesh deformingMesh;
    Vector3[] originalVertices, displacedVertices;
    /** Scaling of the force so the deformation is uniform regardless of the size of the shape */
    float uniformScale = 1f;
    /** Max distance from which vertices are affected by a deformation */
    float maxDistThreshold;

    void Start()
    {
        deformingMesh = GetComponent<MeshFilter>().mesh;
        originalVertices = deformingMesh.vertices;
        displacedVertices = new Vector3[originalVertices.Length];
        for (int i = 0; i < originalVertices.Length; i++) {
            displacedVertices[i] = originalVertices[i];
        }
        vertexVelocities = new Vector3[originalVertices.Length];
    }

    void Update()
    {
        uniformScale = transform.localScale.x;
    }

    public void Deform(Vector3 contactPoint, float force) 
    {
        Debug.DrawLine(Camera.main.transform.position, contactPoint);
        for (int i = 0; i < displacedVertices.Length; i++) {
            MoveVertex(i, contactPoint, force);
        }
    }

    void MoveVertex(int i, Vector3 point, float force) 
    {
        Vector3 distToOrigin = displacedVertices[i] - point;
        distToOrigin *= uniformScale;
        float sqrMag = distToOrigin.sqrMagnitude;
        if (sqrMag > maxDistThreshold) return;
        float attenuatedForce = force / (1f + sqrMag);
        // update the mesh
        deformingMesh.vertices = displacedVertices;
        deformingMesh.RecalculateNormals();
    }
}
