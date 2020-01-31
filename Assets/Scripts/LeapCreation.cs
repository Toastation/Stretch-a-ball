using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Leap.Unity
{
    public class LeapCreation : MonoBehaviour
    {




        public static void creationMain(ref int nb_pinch, ref bool creating, PinchDetector scriptPDL, PinchDetector scriptPDR, ref GameObject currentSelection, Camera cam,
            ref Vector3 lastPosition, ref Vector3 lastPositionR, ref Vector3 lastPositionL, float referenceWait, ref float wait)
        {
            if (creating)
            {
                if (nb_pinch == 1)
                {
                    //Debug.Log(" DEPLACEMENT MODE");
                    wait = 0;
                    if (scriptPDL.IsPinching)
                    {
                        currentSelection.transform.position += (scriptPDL.Position - lastPositionL);
                    }
                    else
                    {
                        currentSelection.transform.position += (scriptPDR.Position - lastPositionR);
                    }
                    lastPosition = (scriptPDL.Position + scriptPDR.Position) / 2;

                }
                else if (nb_pinch == 2)
                {
                    //Debug.Log(" SIZING MODE");
                    wait = 0;
                    currentSelection.transform.position += ((scriptPDL.Position + scriptPDR.Position) / 2 - lastPosition);
                    float TanFOV = (float)Math.Tan((double)cam.fieldOfView * 0.5 * Math.PI / 180);
                    float distCam = Vector3.Distance(cam.transform.position, currentSelection.transform.position);
                    float tmp = ((Vector3.Distance(scriptPDL.Position, scriptPDR.Position) - Vector3.Distance(lastPositionL, lastPositionR))) * TanFOV * 2 * distCam;
                    Debug.Log(tmp);
                    Vector3 t = new Vector3(tmp, tmp, tmp);
                    if ((currentSelection.transform.localScale + t).x > 0)
                    {
                        currentSelection.transform.localScale += t;
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
                    currentSelection = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    currentSelection.transform.position = (scriptPDL.Position + scriptPDR.Position) / 2;
                    float tmp = (Vector3.Distance(scriptPDL.Position, scriptPDR.Position));
                    currentSelection.transform.localScale = new Vector3(tmp, tmp, tmp);
                    lastPosition = currentSelection.transform.position;
                    lastPositionL = scriptPDL.Position;
                    lastPositionR = scriptPDR.Position;
                }
            }
        }








    }
}
