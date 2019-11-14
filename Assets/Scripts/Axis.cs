using UnityEngine;

public class Axis : MonoBehaviour
{
    public GameObject objetOrigin;

    void OnDrawGizmos()
    {
        Vector3 origin = new Vector3();
        origin = objetOrigin.transform.position;
        Vector3 origin2 = new Vector3(0, 0, 0);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(origin2, Vector3.right);
        Gizmos.DrawLine(origin2, Vector3.up);
        Gizmos.DrawLine(origin2, Vector3.forward);
    }

}
