using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshDeformer : MonoBehaviour
{
    private Mesh deformingMesh;
    private Vector3[] originalVertices, displacedVertices;
    private Vector3[] vertexVelocities;

    /** Used to adjust the rate of deformation according to the scale of the mesh */
    private float uniformScale = 1f;
    /** Controls how much the speed of the deformation slows down after the mouse button is released */
    public float deceleration = 5f;
    /** Used for perf, controls the maximum from the contact point where vertices are affected by the deformation */
    public float maxDistThreshold = 100;

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
        for (int i = 0; i < displacedVertices.Length; i++) {
            UpdateVertex(i);
        }   
        deformingMesh.vertices = displacedVertices;
        deformingMesh.RecalculateNormals();
    }

    public void SpreadForce(Vector3 contactPoint, float force) 
    {
        Debug.DrawLine(Camera.main.transform.position, contactPoint);
        for (int i = 0; i < displacedVertices.Length; i++) {
            ApplyForceToVertex(i, contactPoint, force);
        }
    }

    void ApplyForceToVertex(int i, Vector3 point, float force) 
    {
        Vector3 pointToVertex = displacedVertices[i] - point;
        pointToVertex *= uniformScale;
        float sqrMag = pointToVertex.sqrMagnitude;
        if (sqrMag > maxDistThreshold) return;
        float attenuatedForce = force / (1f + sqrMag);
        float velocity = attenuatedForce * Time.deltaTime;
        vertexVelocities[i] += pointToVertex.normalized * velocity;
        Debug.DrawLine(point, point + pointToVertex.normalized, Color.red);
    }

    void UpdateVertex(int i)
    {
        Vector3 velocity = vertexVelocities[i];
        velocity *= 1f - deceleration * Time.deltaTime;
        vertexVelocities[i] = velocity;
        displacedVertices[i] += velocity * (Time.deltaTime / uniformScale);
    }
}
