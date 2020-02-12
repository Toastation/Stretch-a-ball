using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Leap.Unity
{
    public class LeapCommon : MonoBehaviour
    {


        // wait, currentSelection, scriptPDL, scriptPDR (à remplacer par des "scripts" ou direct les objets),  lastPosition[,L,R], cam
        public static void sizingMode(ref GameObject currentSelection, PinchDetector scriptPDL, PinchDetector scriptPDR, ref Vector3 lastPosition, Camera cam, ref Vector3 lastPositionR, ref Vector3 lastPositionL)
        {
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

        public static void deplacementMode(ref GameObject currentSelection, PinchDetector scriptPDL, PinchDetector scriptPDR, ref Vector3 lastPosition, ref Vector3 lastPositionR, ref Vector3 lastPositionL)
        {
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
    }
}