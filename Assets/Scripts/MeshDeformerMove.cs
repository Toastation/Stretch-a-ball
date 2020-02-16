
using System;
using System.Collections.Generic;

using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshCollider))]
public class MeshDeformerMove : MonoBehaviour
{
    private Mesh mesh;
    private MeshFilter meshFilter;
    private MeshCollider meshCollider;

    // vertices data
    private Vector3[] vertices;
    private Vector3[] normals;
    private Vector3[] originPos;
    private bool[] selectedVertices;
    private float[] intensities;
    private Dictionary<int, HashSet<int>> network; // (k,v) where k is a vertex index and v the set of its neighbors index

    // drag data
    private float zOffset;
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
    private void LateUpdate()
    {
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
    * Shoot a ray in the given direction and marks all vertices 
    * that will be affected by the deformation. It also give them
    * an intensity value according to their distance with the impact.
    */
    private void selectVertices(in Ray ray)
    {
        RaycastHit hit;
        //float radiusOfEffect = deformArea * Mathf.Min(mesh.bounds.size.x, mesh.bounds.size.y, mesh.bounds.size.z) * transform.localScale.x;
        if (Physics.Raycast(ray, out hit, 100))
        {
            this.zOffset = Camera.main.WorldToScreenPoint(hit.point).z;
            Vector3 center = transform.InverseTransformPoint(hit.point);
            lastMousePos = GetMouseWorldPos();
            for (int i = 0; i < selectedVertices.Length; i++)
            {
                Vector3 dist = center - vertices[i];
                dist *= transform.localScale.x;
                float distSqrMag = dist.sqrMagnitude;
                //if (distSqrMag < Mathf.Pow(radiusOfEffect, 2)) {
                selectedVertices[i] = true;
                originPos[i] = vertices[i];
                intensities[i] = 1 / ((1f + influence * distSqrMag) * transform.localScale.x);
                //}
            }
        }
    }

    /**
    * Unselect all vertices
    */
    private void unselectVertices()
    {
        for (int i = 0; i < selectedVertices.Length; i++)
        {
            selectedVertices[i] = false;
        }
    }

    /**
    * Move the vertices according the given displacement vector and
    * their intensity factor (this factor is set when the vertices are
    * marked to be moved in selectVertices)
    */
    private void moveVertices(in Vector3 disp)
    {
        for (int i = 0; i < selectedVertices.Length; i++)
        {
            if (selectedVertices[i])
            {
                vertices[i] += disp * intensities[i];
            }
        }
    }

    /**
    * Launch a ray from the camera towards the mouse position and determines if 
    * the mesh is hit. If so, the vertices near the contact point (user param)
    * are marked and their origin position saved.
    */
    private void OnMouseDown()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        selectVertices(ray);
    }

    /**
    * Deselect all vertices
    */
    private void OnMouseUp()
    {
        unselectVertices();
    }

    /**
    * Move the selected vertices on the mesh based on their distances with the contact point
    */
    private void OnMouseDrag()
    {
        Vector3 currentMousePos = GetMouseWorldPos();
        Vector3 disp = currentMousePos - this.lastMousePos;
        moveVertices(disp);
        this.lastMousePos = currentMousePos;
    }

    /**
    * Returns whether or not the given point is inside the mesh
    */
    public bool Contains(Vector3 point)
    {
        return meshCollider.bounds.Contains(point);
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

}