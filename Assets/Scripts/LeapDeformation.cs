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

        Vector3 lastPosRight;
        Vector3 lastPosLeft;

        Finger index_right;
        Finger index_left;

        public void PinchRightDetected()
        {
            //Debug.Log("Pinch Right Detected");
            GameObject Right = GameObject.Find("Capsule Hand Right");
            pinch_right = true;
            if (Right == null)
                return;
            Hand hand = Right.GetComponent<HandModelBase>().GetLeapHand();
            index_right = hand.GetThumb();
            Vector3 origin = index_right.TipPosition.ToVector3();
            Vector3 dir = index_right.Direction.ToVector3();
            lastPosRight = origin;
            LaunchRay(origin, dir);
        }

        public void PinchLeftDetected()
        {
            //Debug.Log("Pinch Left Detected");
            GameObject Left = GameObject.Find("Capsule Hand Left");
            pinch_left = true;
            if (Left == null)
                return;
            Hand hand = Left.GetComponent<HandModelBase>().GetLeapHand();
            index_left = hand.GetThumb();
            Vector3 origin = index_left.TipPosition.ToVector3();
            Vector3 dir = index_left.Direction.ToVector3();
            lastPosLeft = origin;
            LaunchRay(origin, dir);
        }

        public void PinchRightEnded()
        {
            //Debug.Log("Pinch Right Ended");
            pinch_right = false;
            meshHit.UnselectVertices();
            // Debug.Log("JE SUIS LA!");
        }

        public void PinchLeftEnded()
        {
            //Debug.Log("Pinch Left Ended");
            pinch_left = false;
            meshHit.UnselectVertices();
            // Debug.Log("JE SUIS LA!");
        }

        /*
        void LaunchClosest(Vector3 origin)
        {
            MeshDeformerMove[] meshList = FindObjectsOfType<MeshDeformerMove>();
            Vector3 closestPoint = meshList[0].GetComponent<MeshCollider>().ClosestPoint(origin); //ici on triche il faudra le changer meshList[0] avec le mesh selectionné
            meshList[0].SelectVertices(closestPoint);
            //Debug.Log("launchClosest end" + closestPoint);
        }*/


        public void Update()
        {
           //Debug.Log("Je suis dans Update");
           if(pinch_right == true)
            {
                //Debug.Log("pinch_right == true");
                Vector3 diff = index_right.TipPosition.ToVector3() - lastPosRight;
                meshHit.MoveVertices(diff);
                lastPosRight = index_right.TipPosition.ToVector3();
            }
           if(pinch_left == true)
            {
                //Debug.Log("pinch_left == true");
                Vector3 diff = index_left.TipPosition.ToVector3() - lastPosLeft;
                meshHit.MoveVertices(diff);
                lastPosLeft = index_left.TipPosition.ToVector3();
            }
        }

        void LaunchRay(Vector3 origin, Vector3 dir)
        {
            Debug.Log("launchRay");
            Ray ray = new Ray(origin, dir.normalized);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100, 1 << 9))
            {
                Debug.Log("je suis dans le premier if");
                MeshDeformerMove current_mesh = hit.collider.GetComponent<MeshDeformerMove>();
                if (current_mesh != null)
                {
                    current_mesh.SelectVertices(hit.point);
                    meshHit = current_mesh;
                    Debug.Log("j'ai terminé !");
                }
            }
        }
    }
}
