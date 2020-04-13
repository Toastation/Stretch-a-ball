
using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;

/**
 * Attach to a bounding volume. Handles the deformation of the mesh.
 */
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshCollider))]
public class MeshDeformerMove : MonoBehaviour
{
    private Mesh mesh;                  // mesh data
    private MeshFilter meshFilter;      // mesh renderer
    private MeshCollider meshCollider;  // collision mesh

    private Vector3[] vertices;         // mesh vertices
    private Vector3[] normals;          // mesh normals
    private Vector3[] originPos;        // saved vertices before each deformation
    private int[] triangles;            // mesh triangles
    private bool[] selectedVertices;    // vertices that are affected by the deformation
    private float[] intensities;        // intensity of the deformation for each vertices
    private Dictionary<int, HashSet<int>> network; // (k,v) where k is a vertex index and v the set of its neighbors index

    private float zOffset;              // z-coord of the deformation point when the mouse is used
    private Vector3 lastMousePos;       // save of the last mouse coordinate (TODO: outsource)
    private bool meshUpdated;           // true if the mesh needs to be updated

    //private JobHandle handle;
    //private SelfIntersectionJob job;

    // user data (can be adjusted in unity editor)
    [SerializeField, Range(0f, 1f)] public float deformArea = 0.4f;     // size of the deformation zone [0,1]

    private void Start()
    {
        gameObject.layer = 9; // volume layer

        meshFilter = gameObject.GetComponent<MeshFilter>();
        mesh = meshFilter.mesh;
        mesh.MarkDynamic(); // optimize mesh for frequent update
        meshCollider = gameObject.GetComponent<MeshCollider>();
        meshCollider.sharedMesh = mesh;

        vertices = mesh.vertices;
        normals = mesh.normals;
        originPos = new Vector3[vertices.Length];
        triangles = mesh.triangles;
        selectedVertices = new bool[vertices.Length];
        intensities = new float[vertices.Length];
        for (int i = 0; i < selectedVertices.Length; i++) selectedVertices[i] = false;
        network = new Dictionary<int, HashSet<int>>();
        this.ComputeNetwork();

        meshUpdated = false;
    }

    private void Update()
    {
        // Draw a line from the camera to the mouse position when the left mouse button is pressed
        // if (Input.GetMouseButton(0)) {
        // Debug.DrawLine(Camera.main.transform.position, transform.position, Color.red);
        // }
        if (Input.GetKeyDown(KeyCode.L))
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                if (!IsInside(vertices[i] - normals[i] * 0.5f)) Debug.Log("not in bounds");
            }
        }
    }

    // The deformation should always take place after the main update
    private void LateUpdate()
    {
        if (meshUpdated)
        {
            UpdateMeshVertices();
            meshUpdated = false;
        }
    }

    private void UpdateMeshVertices()
    {
        mesh.MarkDynamic();
        mesh.vertices = vertices;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        meshFilter.sharedMesh = mesh;
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
    public void SelectVertices(Vector3 hitPoint)
    {
        float radiusOfEffect = deformArea * Mathf.Min(mesh.bounds.size.x, mesh.bounds.size.y, mesh.bounds.size.z) * transform.localScale.x;
        this.zOffset = Camera.main.WorldToScreenPoint(hitPoint).z;
        Vector3 center = transform.InverseTransformPoint(hitPoint);
        float distSqrMag = 0.0f;
        lastMousePos = GetMouseWorldPos();
        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 dist = center - vertices[i];
            dist *= transform.localScale.x;
            distSqrMag = dist.sqrMagnitude;
            if (distSqrMag < Mathf.Pow(radiusOfEffect, 2)) {
                selectedVertices[i] = true;
                originPos[i] = vertices[i];
                intensities[i] = Mathf.Clamp01(Mathf.Pow(360.0f, -Mathf.Pow(dist.magnitude / radiusOfEffect, 2.5f)));
            }
        }
    }

    /**
    * Unselect all vertices
    */
    public void UnselectVertices()
    {
        //if (DetectSelfIntersection())
        //{
        //    for (int i = 0; i < vertices.Length; i++)
        //    {
        //        if (selectedVertices[i])
        //            vertices[i] = originPos[i];
        //    }
        //    Debug.Log("Self intersection detected");
        //    UpdateMeshVertices();
        //}

        for (int i = 0; i < selectedVertices.Length; i++)
        {
            selectedVertices[i] = false;
        }

        // update collider, update point selected
        meshCollider.enabled = false;
        meshCollider.enabled = true;
        ScatterPlot.GetSelectedPoints(this);
    }

    /**
    * Move the vertices according the given displacement vector and
    * their intensity factor (this factor is set when the vertices are
    * marked to be moved in SelectVertices)
    */
    public void MoveVertices(in Vector3 disp)
    {
        //Debug.Log(disp);
        Vector3[] old = mesh.vertices;
        for (int i = 0; i < selectedVertices.Length; i++)
        {
            if (selectedVertices[i])
            {

                vertices[i] += disp * intensities[i];
            }
        }
        UpdateMeshVertices();
    }

    private bool DetectSelfIntersection()
    {
        //for (int i = 0; i < triangles.Length; i += 3)
        //{
        //    if (!selectedVertices[triangles[i]] || !selectedVertices[triangles[i + 1]] || !selectedVertices[triangles[i + 2]]) continue;
        //    for (int j = 0; j < triangles.Length; j += 3)
        //    {
        //        if (i == j) continue;
        //        bool cont = false;
        //        for (int k = 0; k < 3; k++)
        //        {
        //            if (triangles[i + k] == triangles[j] || triangles[i + k] == triangles[j + 1] || triangles[i + k] == triangles[j + 2]) cont = true;
        //        }
        //        if (selectedVertices[triangles[j]] || selectedVertices[triangles[j + 1]] || selectedVertices[triangles[j + 2]]) continue; 
        //        if (cont) continue;
        //        if (TriTriOverlap.TriTriIntersect(vertices[triangles[i]], vertices[triangles[i+1]], vertices[triangles[i+2]], vertices[triangles[j]], vertices[triangles[j + 1]], vertices[triangles[j + 2]]))
        //        {
        //            return true;
        //        }
        //    }
        //}
        return false;
    }

    /**
    * Launch a ray from the camera towards the mouse position and determines if 
    * the mesh is hit. If so, the vertices near the contact point (user param)
    * are marked and their origin position saved.
    */
    private void OnMouseDown()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (meshCollider.Raycast(ray, out hit, 100))
        {
            SelectVertices(hit.point);
        }
    }

    /**
    * Deselect all vertices
    */
    private void OnMouseUp()
    {
        UnselectVertices();
        meshFilter.sharedMesh = mesh;
        meshCollider.sharedMesh = mesh;
    }

    /**
    * Move the selected vertices on the mesh based on their distances with the contact point
    */
    private void OnMouseDrag()
    {
        Vector3 currentMousePos = GetMouseWorldPos();
        Vector3 disp = currentMousePos - this.lastMousePos;
        disp *= 1.0f / transform.localScale.x;
        MoveVertices(disp);
        this.lastMousePos = currentMousePos;
    }

    /**
    * Returns whether or not the given point is the bounding box of the mesh (NOT the inside
    * the mesh itself)
    */
    public bool Contains(Vector3 point)
    {
        return meshCollider.bounds.Contains(point);
    }

    /**
    * Returns true if the given point is inside the mesh.
    * Principle: cast a ray from a faraway point towards the
    * given point and count how many hit with the mesh there are.
    * If the number is odd the ray has entered the mesh but not
    * exited => the point is inside.
    */
    public bool IsInside(Vector3 point)
    {
        if (!meshCollider.bounds.Contains(point)) return false;
        Vector3 cur, start;
        float dist, distSave;
        cur = start = new Vector3(100, 0, 0);
        Vector3 dir = point - start;
        dist = distSave = dir.magnitude;
        dir.Normalize();
        int nbHit = 0;
        RaycastHit hit;
        Ray ray = new Ray(cur, dir);
        // count how many times the ray entered the mesh
        while (cur != point)
        {
            if (meshCollider.Raycast(ray, out hit, dist))
            {
                nbHit++;
                cur = hit.point + dir * 0.05f;
                dist -= hit.distance + 0.05f;
            }
            else
            {
                cur = point;
            }
            ray.origin = cur;
        }
        ray.direction = -dir;
        dist = distSave;
        // count how many times the ray exited the mesh
        while (cur != start)
        {
            if (meshCollider.Raycast(ray, out hit, dist))
            {
                nbHit++;
                cur = hit.point - dir * 0.05f;
                dist -= hit.distance + 0.05f;
            }
            else
            {
                cur = start;
            }
            ray.origin = cur;
        }
        return nbHit % 2 == 1;
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