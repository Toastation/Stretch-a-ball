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
        GameObject currentSelection;
        public Camera cam;
        Vector3 lastPosition;
        Vector3 lastPositionR;
        Vector3 lastPositionL;
        float referenceWait = 1;
        float wait = 0;
        int LA_VARIABLE = 0;
        //float FOV;
        //float referenceSize = 48.86F; //Calculated thanks to trigonometry and Thales theorem (angle of Leap Motion: 150°)

        

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

            switch (LA_VARIABLE)
            {
                case 0: //CREATION
                    LeapCreation.creationMain(ref nb_pinch, ref creating, scriptPDL, scriptPDR, ref currentSelection, cam,
            ref lastPosition, ref lastPositionR, ref lastPositionL, referenceWait, ref wait);
                    break;
                case 1: //SELECTION (!!! ptet avec un modulo pour gérer les différents sous-cas de la sélection)

                    break;
                case 2:

                    break;
            }






            
        }
    }
}

