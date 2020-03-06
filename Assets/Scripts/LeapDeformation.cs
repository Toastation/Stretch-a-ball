using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Leap.Unity
{
    public class LeapDeformation : MonoBehaviour
    {
        bool pinch_right = false;
        bool pinch_left = false;

        MeshDeformerMove meshHit;

        public void PinchRightDetected()
        {
            Debug.Log("Pinch Right Detected");
            GameObject Right = GameObject.Find("Capsule Hand Right");
            if (Right == null)
                return;
            Hand hand = Right.GetComponent<HandModelBase>().GetLeapHand();
            Finger index = hand.GetIndex();
            Vector3 origin = index.TipPosition.ToVector3();
            Vector3 dir = index.Direction.ToVector3();
            LaunchClosest(origin);
        }

        public void PinchLeftDetected()
        {
            Debug.Log("Pinch Left Detected");
            GameObject Left = GameObject.Find("Capsule Hand Left");
            if (Left == null)
                return;
            Hand hand = Left.GetComponent<HandModelBase>().GetLeapHand();
            Finger index = hand.GetIndex();
            Vector3 origin = index.TipPosition.ToVector3();
            Vector3 dir = index.Direction.ToVector3();
            LaunchClosest(origin);
        }

        public void PinchRightEnded()
        {
            //Debug.Log("Pinch Right Ended");
            // Debug.Log("JE SUIS LA!");
        }

        public void PinchLeftEnded()
        {
            //Debug.Log("Pinch Left Ended");
            // Debug.Log("JE SUIS LA!");
        }

        void LaunchClosest(Vector3 origin)
        {
            MeshDeformerMove[] meshList = FindObjectsOfType<MeshDeformerMove>();
            Vector3 closestPoint = meshList[0].GetComponent<MeshCollider>().ClosestPoint(origin); //ici on triche il faudra le changer meshList avec le mesh selectionné
        }

        /*void LaunchRay(Vector3 origin, Vector3 dir)
        {
            Debug.Log("launchRay");
            Ray ray = new Ray(origin, dir);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100, 1 << 9))
            {
                Debug.Log("je suis dans le premier if");
                MeshDeformerMove current_mesh = hit.collider.GetComponent<MeshDeformerMove>();
                if (current_mesh != null)
                {
                    current_mesh.SelectVertices(hit);
                    meshHit = current_mesh;
                    Debug.Log("j'ai terminé !");
                }
            }
        }*/
    }
}
