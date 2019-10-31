using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshDeformer : MonoBehaviour
{
    Mesh deformingMesh;
    Vector3[] originalVertices, displacedVertices;
    Vector3[] vertexVelocities;
    float damping = 5f;
    float uniformScale = 1f;

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
        float attenuatedForce = force / (1f + pointToVertex.sqrMagnitude);
        float velocity = attenuatedForce * Time.deltaTime;
        vertexVelocities[i] += pointToVertex.normalized * velocity;
    }

    void UpdateVertex(int i)
    {
        Vector3 velocity = vertexVelocities[i];
        velocity *= 1f - damping * Time.deltaTime;
        vertexVelocities[i] = velocity;
        displacedVertices[i] -= velocity * (Time.deltaTime / uniformScale);
    }
}
