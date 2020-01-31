  
using UnityEngine;
using Unity.Collections;
using Unity.Jobs;

using System;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshCollider))]
public class MeshDeformerMove : MonoBehaviour
{
    private Mesh mesh;
    private MeshCollider meshCollider;
    private MeshFilter meshFilter;

    // vertices data
    private Vector3[] vertices;
    private Vector3[] normals;
    private Vector3[] originPos;
    private bool[] selectedVertices;
    private float[] intensities;
    private Dictionary<int, HashSet<int>> network;

    // drag data
    private float zOffset;
    private Vector3 lastMousePosAtHit;
    private Vector3 lastMousePos;

    // user data
    [SerializeField, Range(0f, 1f)] public float deformArea = 0.2f;
    [SerializeField, Range(1f, 1000f)] public float influence = 30.0f;

    private void Start()
    {
        meshFilter = gameObject.GetComponent<MeshFilter>();
        mesh = meshFilter.mesh;
        mesh.MarkDynamic(); // optimize mesh for frequent update

        meshCollider = gameObject.GetComponent<MeshCollider>();
        meshCollider.sharedMesh = mesh;

        vertices = mesh.vertices;
        normals = mesh.normals;
        originPos = new Vector3[vertices.Length];
        selectedVertices = new bool[vertices.Length];
        intensities = new float[vertices.Length];
        for (int i = 0; i < selectedVertices.Length; i++) selectedVertices[i] = false;
        network = new Dictionary<int, HashSet<int>>();
        this.ComputeNetwork();
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
        normals = mesh.normals;

        // bug with the mesh collider, the mesh is updated as expected but internally it still uses the unmodified mesh.
        meshCollider.enabled = false;
        meshCollider.enabled = true;


        if (Input.GetKeyUp(KeyCode.L))
        {
            this.LaplacianSmooth();
        }
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
        //zOffset = Camera.main.WorldToScreenPoint(gameObject.transform.position).z; // the z coordinate of the the mouse will be the z coordinate of the object
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        float radiusOfEffect = deformArea * Mathf.Min(mesh.bounds.size.x, mesh.bounds.size.y, mesh.bounds.size.z) * transform.localScale.x;
        if (Physics.Raycast(ray, out hit, 100)) {
            this.zOffset = Camera.main.WorldToScreenPoint(hit.point).z; 
            lastMousePosAtHit = lastMousePos = GetMouseWorldPos();
            Vector3 center = transform.InverseTransformPoint(hit.point); 
            for (int i = 0; i < selectedVertices.Length; i++) {
                Vector3 dist = center - vertices[i];
                dist *= transform.localScale.x;
                float distSqrMag = dist.sqrMagnitude;
                //if (distSqrMag < Mathf.Pow(radiusOfEffect, 2)) {
                    selectedVertices[i] = true;
                    originPos[i] = vertices[i];
                    intensities[i] =  1 / ((1f + influence * distSqrMag) * transform.localScale.x);
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
        Vector3 currentMousePos = GetMouseWorldPos();
        Vector3 disp = currentMousePos - this.lastMousePos;
        for (int i = 0; i < selectedVertices.Length; i++) {
            if (selectedVertices[i]) {
                vertices[i] +=  disp * intensities[i];
            }
        }
        /*if (disp.magnitude > 0)
            this.LaplacianSmooth();*/
        this.lastMousePos = currentMousePos;
    }

    /**
    * Smooth the mesh using the laplacian smoothing algo
    */
    private void LaplacianSmooth()
    {
        Vector3[] origin = new Vector3[vertices.Length];
        Array.Copy(vertices, origin, vertices.Length);
        for (int i = 0; i < origin.Length; i++)
        {
            if (!selectedVertices[i]) continue;
            Vector3 sumPos = Vector3.zero;
            foreach (int neighbor in network[i])
            {
                sumPos += origin[neighbor];
            }
            vertices[i] = sumPos / network[i].Count;
        }
    }

    /**
    * Computes the adjacence network;
    */
    private void ComputeNetwork()
    {
        int[] triangles = mesh.triangles;
        for (int i = 0; i < triangles.Length; i += 3)
        {
            int a = triangles[i];
            int b = triangles[i + 1];
            int c = triangles[i + 2];
            if (!network.ContainsKey(a)) network.Add(a, new HashSet<int>());
            if (!network.ContainsKey(b)) network.Add(b, new HashSet<int>());
            if (!network.ContainsKey(c)) network.Add(c, new HashSet<int>());
            network[a].Add(b);
            network[a].Add(c);
            network[b].Add(a);
            network[b].Add(c);
            network[c].Add(a);
            network[c].Add(b);
        }
    }

    /**
    * Returns whether or not the given point is inside the mesh
    */
    public bool Contains(Vector3 point) {
        return meshCollider.bounds.Contains(point);
    }

}