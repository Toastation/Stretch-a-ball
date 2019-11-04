  
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

    public void Deform( Vector3 point, float radius, float force )
    {
        Vector3 center = transform.InverseTransformPoint(point); // Transform the point from world space to local space.
        // go through all vertices and move the ones inside the radius of contact
        for (int i = 0; i < vertices.Length; i++) {
            Vector3 vertex = vertices[i];
            float dxSqr = Mathf.Pow(vertex.x - center.x, 2);
            float dySqr = Mathf.Pow(vertex.y - center.y, 2);
            float dzSqr = Mathf.Pow(vertex.z - center.z, 2);
            if (dxSqr + dySqr + dzSqr < Mathf.Pow(radius, 2)) {
                vertices[i] += normals[i] * force * Time.deltaTime;
            }
        }
    }
}