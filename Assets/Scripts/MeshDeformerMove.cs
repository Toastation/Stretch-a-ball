  
using UnityEngine;
using Unity.Collections;
using Unity.Jobs;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshCollider))]
public class MeshDeformerMove : MonoBehaviour
{
    private Mesh mesh;
    private MeshCollider meshCollider;

    // vertices data
    private Vector3[] vertices;
    private Vector3[] originPos;
    private bool[] selectedVertices;
    private float[] intensities;
    private Vector3 lastMousePosAtHit;

    // drag data
    private float zOffset;

    // user data
    public float deformArea = 0.2f;

    private void Start()
    {
        mesh = gameObject.GetComponent<MeshFilter>().mesh;
        mesh.MarkDynamic(); // optimize mesh for frequent update

        meshCollider = gameObject.GetComponent<MeshCollider>();
        meshCollider.sharedMesh = mesh;

        vertices = mesh.vertices;
        originPos = new Vector3[vertices.Length];
        selectedVertices = new bool[vertices.Length];
        intensities = new float[vertices.Length];
        for (int i = 0; i < selectedVertices.Length; i++) selectedVertices[i] = false;
    }

    private void Update() 
    {
        // Draw a line from the camera to the mouse position when the left mouse button is pressed
        // if (Input.GetMouseButton(0)) {
            // Debug.DrawLine(Camera.main.transform.position, transform.position, Color.red);
        // }    
    }

    // The deformation should always take place after the main update
    private void LateUpdate() {
        mesh.vertices = vertices;

        // no need to update normals since the displacement is colinear to the normal
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        // bug with the mesh collider, the mesh is updated as expected but internally it still uses the unmodified mesh.
        meshCollider.enabled = false;
        meshCollider.enabled = true;
    }

    /**
    * Converts the mouse position (2D) to world coordinate (3D)
    */
    private Vector3 GetMouseWorldPos() 
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = zOffset;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }

    /**
    * Launch a ray from the camera towards the mouse position and determines if 
    * the mesh is hit. If so, the vertices near the contact point (user param)
    * are marked and their origin position saved.
    */
    private void OnMouseDown() 
    {
        zOffset = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        float radiusOfEffect = deformArea * Mathf.Min(mesh.bounds.size.x, mesh.bounds.size.y, mesh.bounds.size.z) * transform.localScale.x;
        if (Physics.Raycast(ray, out hit, 100)) {
            lastMousePosAtHit = GetMouseWorldPos();
            Vector3 center = transform.InverseTransformPoint(hit.point); 
            for (int i = 0; i < selectedVertices.Length; i++) {
                Vector3 dist = center - vertices[i];
                dist *= transform.localScale.x;
                float distSqrMag = dist.sqrMagnitude;
                //if (distSqrMag < Mathf.Pow(radiusOfEffect, 2)) {
                    selectedVertices[i] = true;
                    originPos[i] = vertices[i];
                    intensities[i] =  1 / ((1f + 100 * distSqrMag) * transform.localScale.x);
                //}
            }
        } 
    }

    /**
    * Deselect all vertices
    */
    private void OnMouseUp()
    {
        for (int i = 0; i < selectedVertices.Length; i++) {
            selectedVertices[i] = false;
        }
    }

    /**
    * Move the selected vertices on the mesh based on their distances with the contact point
    */
    private void OnMouseDrag() 
    {
        Vector3 disp = GetMouseWorldPos() - lastMousePosAtHit; 
        for (int i = 0; i < selectedVertices.Length; i++) {
            if (selectedVertices[i]) {
                vertices[i] = originPos[i] + disp * intensities[i];
            }
        }
    }

    /**
    * Returns whether or not the given point is inside the mesh
    */
    public bool Contains(Vector3 point) {
        return meshCollider.bounds.Contains(point);
    }

}