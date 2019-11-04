using UnityEngine;

[RequireComponent(typeof(Camera))]
public class MeshDeformerLiteInput: MonoBehaviour
{
    public float distance = 100;
    public float radius = 0.2f;
    public float force = 0.3f;
    
    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawLine(ray.origin, ray.origin + ray.direction * distance);

        if(Input.GetMouseButton(0)) {
            RaycastHit hit;
            if( Physics.Raycast(ray, out hit, distance)) {
                MeshDeformerLite deformer = hit.collider.GetComponent<MeshDeformerLite>();
                if( deformer ) {
                    Debug.DrawLine(ray.origin, ray.origin + ray.direction * distance, Color.green);
                    deformer.Deform(hit.point, radius, force);
                }
            } else {
                Debug.DrawLine(ray.origin, ray.origin + ray.direction * distance);
            }
        }


    }
}