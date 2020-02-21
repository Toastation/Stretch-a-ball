
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
        gameObject.layer = 9;

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

        if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log("1");
            Debug.Log(Physics.Linecast(Vector3.zero, new Vector3(2, 0, 0), 1 << 9));
            Debug.Log("2");
            Debug.Log(Physics.Linecast(new Vector3(2, 0, 0), Vector3.zero, 1 << 9));
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
        if (meshCollider.Raycast(ray, out hit, 100))
        {
            this.zOffset = Camera.main.WorldToScreenPoint(hit.point).z;
            Vector3 center = transform.InverseTransformPoint(hit.point);
            float distSqrMag = 0.0f;
            lastMousePos = GetMouseWorldPos();
            for (int i = 0; i < selectedVertices.Length; i++)
            {
                Vector3 dist = center - vertices[i];
                dist *= transform.localScale.x;
                distSqrMag = dist.sqrMagnitude;
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
        Vector3 newPos;
        for (int i = 0; i < selectedVertices.Length; i++)
        {
            if (selectedVertices[i])
            {
                //newPos = vertices[i] + disp * intensities[i];
                //if (isInside(newPos - normals[i] * 0.01f))
                //{
                //    vertices[i] = newPos;
                //}
                //else
                //{
                //    Debug.Log("-");
                //}
                vertices[i] += disp * intensities[i];
            }
        }
        // todo update selection
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
    public bool isInside(Vector3 point)
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