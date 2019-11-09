  
using UnityEngine;
using Unity.Collections;
using Unity.Jobs;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshCollider))]
public class MeshDeformerLite : MonoBehaviour
{
    private Mesh mesh;
    private MeshCollider meshCollider;

    private Vector3[] vertices;
    private Vector3[] normals;

    private void Start()
    {
        mesh = gameObject.GetComponent<MeshFilter>().mesh;
        mesh.MarkDynamic(); // optimize mesh for frequent update

        meshCollider = gameObject.GetComponent<MeshCollider>();
        meshCollider.sharedMesh = mesh;

        // This memory setup assumes the vertex count will not change.
        vertices = mesh.vertices;
        normals = mesh.normals;
    }

    // The deformation should always take place after the main update
    private void LateUpdate() {
        mesh.vertices = vertices;

        // no need to update normals since the displacement is colinear to the normal
        mesh.RecalculateBounds();

        // bug with the mesh collider, the mesh is updated as expected but internally it still uses the unmodified mesh.
        meshCollider.enabled = false;
        meshCollider.enabled = true;
    }

    public void Deform(Vector3 point, float radius, float force)
    {
        Vector3 center = transform.InverseTransformPoint(point);
        // go through all vertices and move the ones inside the radius of contact
        for (int i = 0; i < vertices.Length; i++) {
            Vector3 dist = vertices[i] - center;
            float reducedForce =  1 / (1f + 100 * dist.sqrMagnitude);
            vertices[i] += normals[i] * reducedForce * Time.deltaTime;
        }
    }
}