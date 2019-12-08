using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


namespace Leap.Unity
{
    public class LeapEventManager : MonoBehaviour
    {
        int nb_pinch = 0;
        bool creating = false;
        bool initializedRight = false;
        bool initializedLeft =  false;
        GameObject Right;
        GameObject Left;
        PinchDetector scriptPDL;
        PinchDetector scriptPDR;
        GameObject currentCreation;
        Vector3 lastPosition;
        Vector3 lastPositionR;
        Vector3 lastPositionL;

        void PinchDetected()
        {
            nb_pinch += 1;
            //Debug.Log("JE SUIS LA!");
        }

        void PinchEnded()
        {
            nb_pinch -= 1;
           // Debug.Log("JE SUIS LA!");
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (!initializedLeft)
            {
                Left = GameObject.Find("Capsule Hand Left");
                if (Left != null)
                {
                    Debug.Log("ALLO");
                    scriptPDL = Left.GetComponent<PinchDetector>();
                    scriptPDL.OnActivate.AddListener(PinchDetected);
                    scriptPDL.OnDeactivate.AddListener(PinchEnded);
                    initializedLeft = true;
                }
            }
            if (!initializedRight)
            {
                Right = GameObject.Find("Capsule Hand Right");
                if (Right != null)
                {
                    Debug.Log("ALLO");
                    scriptPDR = Right.GetComponent<PinchDetector>();
                    scriptPDR.OnActivate.AddListener(PinchDetected);
                    scriptPDR.OnDeactivate.AddListener(PinchEnded);
                    initializedRight = true;
                }
            }
            
            if(nb_pinch == 2 && !creating)
            {
                //Debug.Log(" CREATION MODE");
                creating = true;
                currentCreation = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                currentCreation.transform.position = (scriptPDL.Position + scriptPDR.Position) / 2;
                float tmp = (Vector3.Distance(scriptPDL.Position, scriptPDR.Position));
                currentCreation.transform.localScale = new Vector3(tmp, tmp, tmp);
                lastPosition = currentCreation.transform.position;
                lastPositionL = scriptPDL.Position;
                lastPositionR = scriptPDR.Position;
            }
            else if (nb_pinch == 2) //Sizing mode
            {
                //Debug.Log(" SIZING MODE");
                currentCreation.transform.position += ((scriptPDL.Position + scriptPDR.Position)/2 - lastPosition);
                float tmp = (Vector3.Distance(scriptPDL.Position, scriptPDR.Position) - Vector3.Distance(lastPositionL, lastPositionR)); 
                Debug.Log(tmp);
                currentCreation.transform.localScale += new Vector3(tmp, tmp, tmp);
                lastPosition = (scriptPDL.Position + scriptPDR.Position) / 2;
                lastPositionL = scriptPDL.Position;
                lastPositionR = scriptPDR.Position;

            } else if (nb_pinch == 1) //Deplacement mode
            {
                //Debug.Log(" DEPLACEMENT MODE");
                if (scriptPDL.IsPinching)
                {
                    currentCreation.transform.position += (scriptPDL.Position - lastPositionL);
                }
                else
                {
                    currentCreation.transform.position += (scriptPDR.Position - lastPositionR);
                }
                lastPosition = (scriptPDL.Position + scriptPDR.Position) / 2;
                lastPositionL = scriptPDL.Position;
                lastPositionR = scriptPDR.Position;
            }
            else
            {
               // Debug.Log(" FINISHED MODE");
                creating = false;
            }
        }
    }
}

