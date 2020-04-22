using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;

namespace StretchABall
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
                    LeapCommon.deplacementMode(ref currentSelection, scriptPDL, scriptPDR, ref lastPosition, ref lastPositionR, ref lastPositionL);

                }
                else if (nb_pinch == 2)
                {
                    //Debug.Log(" SIZING MODE");
                    // wait, currentSelection, scriptPDL, scriptPDR (à remplacer par des "scripts" ou direct les objets),  lastPosition[,L,R], cam,   
                    wait = 0;
                    Debug.Log(currentSelection != null);
                    LeapCommon.sizingMode(ref currentSelection, scriptPDL, scriptPDR, ref lastPosition, cam, ref lastPositionR, ref lastPositionL);

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

            }
            else if (!creating)
            {
                if (nb_pinch == 2)
                {
                    /*
                    //Debug.Log(" CREATION MODE");
                    creating = true;
                    currentSelection = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    currentSelection.transform.position = (scriptPDL.Position + scriptPDR.Position) / 2;
                    float tmp = (Vector3.Distance(scriptPDL.Position, scriptPDR.Position));
                    currentSelection.transform.localScale = new Vector3(tmp, tmp, tmp);
                    */

                    /*
                     * The following code is supposed to do the exat same thing but with 
                     * the prefab sphereMesh instead of standard sphere
                     */

                    creating = true;
                    // Gets the prefab
                    GameObject volumesNode = GameObject.Find("Volumes");
                    currentSelection = Instantiate(Resources.Load("Prefab/sphereMesh", typeof(GameObject))) as GameObject;
                    if (volumesNode != null)
                    {
                        currentSelection.transform.parent = volumesNode.transform;
                    }
                    //Modifies the position and scale
                    currentSelection.transform.position = (scriptPDL.Position + scriptPDR.Position) / 2;
                    float tmp = (Vector3.Distance(scriptPDL.Position, scriptPDR.Position));
                    currentSelection.transform.localScale = new Vector3(tmp, tmp, tmp);
                    /* End here*/

                    lastPosition = currentSelection.transform.position;
                }
            }
        }
    }
}