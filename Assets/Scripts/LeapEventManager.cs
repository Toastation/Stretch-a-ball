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
        bool initializedMenu = false;
        GameObject Right;
        GameObject Left;
        CurrentMenu Menu;
        PinchDetector scriptPDL;
        PinchDetector scriptPDR;
        CurrentMenu cMenu;

        ExtendedFingerDetector scriptEFDL;
        ExtendedFingerDetector scriptEFDR;

        GameObject currentSelection;
        List<DataPoint> currentSelectedDataPoints;
        BoolOperation Operation;

        public Camera cam;
        int LA_VARIABLE = 1;

        Vector3 lastPosition;
        Vector3 lastPositionR;
        Vector3 lastPositionL;
        float referenceWait = 1;
        float wait = 0;
        int Pointing = 0; // Pas un bool dans le cas où les deux mains pointent en même temps, et que l'une arrête de pointer
        
        //float FOV;
        //float referenceSize = 48.86F; //Calculated thanks to trigonometry and Thales theorem (angle of Leap Motion: 150°)

        
    

        void ExtendedFingerDetected()
        {
            //Debug.Log("Extend detected");
            Pointing += 1;
        }

        void ExtendedFingerEnded()
        {
            Pointing -= 1;
        }

        void PinchDetected()
        {
            //Debug.Log("Pinch Detected");
            nb_pinch += 1;
            //Debug.Log("JE SUIS LA!");
        }

        void PinchEnded()
        {
            //Debug.Log("Pinch Ended");
            nb_pinch -= 1;
           // Debug.Log("JE SUIS LA!");
        }

        // Start is called before the first frame update
        void Start()
        {
            Operation = new BoolOperation();
        }

        // Update is called once per frame
        void Update()
        {
            if (!initializedMenu)
            {
                cMenu = GameObject.Find("Palm UI 1").GetComponent<CurrentMenu>();
                if (cMenu != null)
                {
                    Debug.Log("IL EST INITIALISEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE");
                    initializedMenu = true;
                }
            }
            if (!initializedLeft)
            {
                Left = GameObject.Find("Capsule Hand Left");
                if (Left != null)
                {
                    //Debug.Log("ALLO");
                    scriptPDL = Left.GetComponent<PinchDetector>();
                    scriptEFDL = Left.GetComponent<ExtendedFingerDetector>();
                    //Debug.Log(scriptEFDL, scriptPDL);
                    scriptPDL.OnActivate.AddListener(PinchDetected);
                    scriptPDL.OnDeactivate.AddListener(PinchEnded);
                    scriptEFDL.OnActivate.AddListener(ExtendedFingerDetected);
                    scriptEFDL.OnDeactivate.AddListener(ExtendedFingerEnded);
                    initializedLeft = true;
                }
            }
            if (!initializedRight)
            {
                Right = GameObject.Find("Capsule Hand Right");
                if (Right != null)
                {
                    //Debug.Log("ALLO");
                    scriptPDR = Right.GetComponent<PinchDetector>();
                    scriptEFDR = Right.GetComponent<ExtendedFingerDetector>();
                    //Debug.Log(scriptEFDR, scriptPDR);
                    scriptPDR.OnActivate.AddListener(PinchDetected);
                    scriptPDR.OnDeactivate.AddListener(PinchEnded);
                    scriptEFDR.OnActivate.AddListener(ExtendedFingerDetected);
                    scriptEFDR.OnDeactivate.AddListener(ExtendedFingerEnded);
                    initializedRight = true;
                }
            }

            if (initializedLeft && initializedRight && initializedMenu)
            {
                switch (cMenu.GetCurrentMenu())
                {
                    case CurrentMenu.Menu.NoMenuSelected:
                        break;

                    case CurrentMenu.Menu.Creation: //CREATION
                        LeapCreation.creationMain(ref nb_pinch, ref creating, scriptPDL, scriptPDR, ref currentSelection, cam,
                ref lastPosition, ref lastPositionR, ref lastPositionL, referenceWait, ref wait);
                        break;

                    case CurrentMenu.Menu.Selection: //SELECTION (!!! ptet avec un modulo pour gérer les différents sous-cas de la sélection)
                        switch (cMenu.GetCurrentMenuSelection())
                        {
                            case CurrentMenu.Selection.SetOperation:
                                switch (cMenu.GetCurrentMenuSetOperation())
                                {
                                    case CurrentMenu.SetOperation.SetUnion:
                                        Operation.BoolUpdate(currentSelectedDataPoints, Operation.OrBoolOperation);
                                        break;
                                    case CurrentMenu.SetOperation.SetIntersection:
                                        Operation.BoolUpdate(currentSelectedDataPoints, Operation.AndBoolOperation);
                                        break;
                                    case CurrentMenu.SetOperation.SetRelativeComplement:
                                        Operation.BoolUpdate(currentSelectedDataPoints, Operation.NotInBoolOperation);
                                        break;
                                    case CurrentMenu.SetOperation.Confirm:
                                        currentSelectedDataPoints = Operation.BoolOperationMain(currentSelectedDataPoints);
                                    default:
                                        break;
                                }
                                break;
                            default:
                                break;
                        }
                        Vector3 PointingDirection = new Vector3( 0, 0, 0 );
                        Vector3 Fingertip = new Vector3(0, 0, 0);
                        //if (currentSelection != null)
                        //Debug.Log("CURRENT OBJECT:", currentSelection);
                        Finger f = null;
                        Hand hl = Left.GetComponent<HandModelBase>().GetLeapHand();
                        Hand hr = Right.GetComponent<HandModelBase>().GetLeapHand();
                        if (Pointing > 0)
                        {
                            if (hl.Fingers[1].IsExtended)
                            {
                                f = hl.GetIndex();
                            }
                            else
                            {
                                f = hr.GetIndex();
                            }
                            PointingDirection = f.Direction.ToVector3();
                            Fingertip = f.TipPosition.ToVector3();
                        }
                        Collider tempo = LeapSelection.selectionMain((Pointing > 0), PointingDirection, Fingertip, ref nb_pinch, ref creating, scriptPDL, scriptPDR, ref currentSelection, cam,
                ref lastPosition, ref lastPositionR, ref lastPositionL);
                        if (tempo != null)
                        { 
                            if (currentSelection != null)
                                currentSelection.GetComponent<Renderer>().material.color = Color.white;
                            currentSelection = tempo.gameObject; //transform.parent.
                            currentSelection.GetComponent<Renderer>().material.color = Color.red;
                        }
                        else
                        {
                            if (currentSelection != null)
                                currentSelection.GetComponent<Renderer>().material.color = Color.white;
                            currentSelection = null;
                        }
                        
                        break;

                }
                lastPositionL = scriptPDL.Position;
                lastPositionR = scriptPDR.Position;
            }






            
        }
    }
}

