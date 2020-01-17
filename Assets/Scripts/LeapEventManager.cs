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
        public Camera cam;
        Vector3 lastPosition;
        Vector3 lastPositionR;
        Vector3 lastPositionL;
        float referenceWait = 1;
        float wait = 0;
        float FOV;
        float referenceSize = 48.86F; //Calculated thanks to trigonometry and Thales theorem (angle of Leap Motion: 150°)

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
           
            if (creating)
            {
                if (nb_pinch == 1)
                {
                    //Debug.Log(" DEPLACEMENT MODE");
                    wait = 0;
                    if (scriptPDL.IsPinching)
                    {
                        currentCreation.transform.position += (scriptPDL.Position - lastPositionL);
                    }
                    else
                    {
                        currentCreation.transform.position += (scriptPDR.Position - lastPositionR);
                    }
                    lastPosition = (scriptPDL.Position + scriptPDR.Position) / 2;
                    
                }
                else if (nb_pinch == 2)
                {
                    //Debug.Log(" SIZING MODE");
                    wait = 0;
                    currentCreation.transform.position += ((scriptPDL.Position + scriptPDR.Position) / 2 - lastPosition);
                    float TanFOV = (float)Math.Tan((double)cam.fieldOfView * 0.5 * Math.PI/180);
                    float distCam = Vector3.Distance(cam.transform.position,currentCreation.transform.position);
                    float tmp = ((Vector3.Distance(scriptPDL.Position, scriptPDR.Position) - Vector3.Distance(lastPositionL, lastPositionR))) * TanFOV * 2 * distCam ;
                    Debug.Log(tmp);
                    Vector3 t = new Vector3(tmp, tmp, tmp);
                    if ((currentCreation.transform.localScale + t).x > 0)
                    {
                        currentCreation.transform.localScale += t;
                    }
                    lastPosition = (scriptPDL.Position + scriptPDR.Position) / 2;
                    
                }
                else
                {
                    // Debug.Log(" FINISHED MODE");
                    wait += Time.deltaTime;
                    if (wait >= referenceWait)
                    {
                        creating = false;
                    }   
                }
                lastPositionL = scriptPDL.Position;
                lastPositionR = scriptPDR.Position;
            }
            else if (!creating)
            {
                if (nb_pinch == 2)
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
            }
        }
    }
}

