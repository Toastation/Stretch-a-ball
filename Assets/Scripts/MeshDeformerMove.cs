  
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
    private Vector3 lastHitPoint;

    // drag data
    private Vector3 dragOffset;
    private float zOffset;

    private void Start()
    {
        mesh = gameObject.GetComponent<MeshFilter>().mesh;
        mesh.MarkDynamic(); // optimize mesh for frequent update

        meshCollider = gameObject.GetComponent<MeshCollider>();
        meshCollider.sharedMesh = mesh;

        vertices = mesh.vertices;
        lastHitPoint = new Vector3();
        originPos = new Vector3[vertices.Length];
        selectedVertices = new bool[vertices.Length];
        intensities = new float[vertices.Length];
        for (int i = 0; i < selectedVertices.Length; i++) selectedVertices[i] = false;
    }

    private void Update() 
    {
        // if (Input.GetMouseButton(0)) {
        //     Debug.Log("hehehe" + lastHitPoint + " "+ Camera.main.transform.position);
            Debug.DrawLine(Camera.main.transform.position, transform.position, Color.red);
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

    private Vector3 GetMouseWorldPos() 
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = zOffset;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }

    private void OnMouseDown() 
    {
        zOffset = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        dragOffset = gameObject.transform.position - GetMouseWorldPos();
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100)) {
            lastMousePosAtHit = GetMouseWorldPos();
            lastHitPoint = hit.point;
            Vector3 center = transform.InverseTransformPoint(hit.point); 
            for (int i = 0; i < selectedVertices.Length; i++) {
                Vector3 vertex = vertices[i];
                float dxSqr = Mathf.Pow(vertex.x - center.x, 2);
                float dySqr = Mathf.Pow(vertex.y - center.y, 2);
                float dzSqr = Mathf.Pow(vertex.z - center.z, 2);
                if (dxSqr + dySqr + dzSqr < Mathf.Pow(0.2f, 2)) {
                    selectedVertices[i] = true;
                    originPos[i] = vertices[i];
                    Vector3 dist = vertices[i] - center;
                    intensities[i] =  1 / (1f + 100 * dist.sqrMagnitude);
                }
            }
        } 
    }

    private void OnMouseUp() 
    {
        for (int i = 0; i < selectedVertices.Length; i++) {
            selectedVertices[i] = false;
        }
    }

    private void OnMouseDrag() 
    {
        Vector3 disp = GetMouseWorldPos() - lastMousePosAtHit; 
        for (int i = 0; i < selectedVertices.Length; i++) {
            if (selectedVertices[i]) {
                vertices[i] = originPos[i] + disp * intensities[i];
            }
        }
    }

    public bool Contains(Vector3 point) {
        return meshCollider.bounds.Contains(point);
    }

}