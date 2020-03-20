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
        bool removeOn = false;

        LeapDeformation LeapDeformation;
        
        //float FOV;
        //float referenceSize = 48.86F; //Calculated thanks to trigonometry and Thales theorem (angle of Leap Motion: 150°)

        void RemoveSignalDetected()
        {
            removeOn = true;
            Debug.Log("JE DOIS SUPPRIMER");
        }

        void RemoveSignalEnded()
        {
            removeOn = false;
            Debug.Log("La suppression est finie");
        }
    

        void ExtendedFingerDetected()
        {
            //Debug.Log("Extend detected");
            Pointing += 1;
        }

        void ExtendedFingerEnded()
        {
           // Debug.Log("Extend ended");
            Pointing -= 1;
        }

        void PinchRightDetected()
        {
            //Debug.Log("Pinch R Detected");
            nb_pinch += 1;
            if (cMenu.GetCurrentMenu() == CurrentMenu.Menu.Selection && cMenu.GetCurrentMenuSelection() == CurrentMenu.Selection.Modification)
            {
                LeapDeformation.PinchRightDetected();
            }
            //Debug.Log("JE SUIS LA!");
        }

        void PinchLeftDetected()
        {
            //Debug.Log("Pinch L Detected");
            nb_pinch += 1;
            if (cMenu.GetCurrentMenu() == CurrentMenu.Menu.Selection && cMenu.GetCurrentMenuSelection() == CurrentMenu.Selection.Modification)
            {
                LeapDeformation.PinchLeftDetected();
            }
        }

        void PinchRightEnded()
        {
            //Debug.Log("Pinch R Ended");
            nb_pinch -= 1;
            if (cMenu.GetCurrentMenu() == CurrentMenu.Menu.Selection && cMenu.GetCurrentMenuSelection() == CurrentMenu.Selection.Modification)
            {
                LeapDeformation.PinchRightEnded();
            }
        }

        void PinchLeftEnded()
        {
            //Debug.Log("Pinch L Ended");
            nb_pinch -= 1;
            if (cMenu.GetCurrentMenu() == CurrentMenu.Menu.Selection && cMenu.GetCurrentMenuSelection() == CurrentMenu.Selection.Modification)
            {
                LeapDeformation.PinchLeftEnded();
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            Operation = new BoolOperation();

            LeapDeformation = new LeapDeformation();

            
        }

        // Update is called once per frame
        void Update()
        {
            if (!initializedMenu)
            {
                //Connects to the menu
                cMenu = GameObject.Find("Palm UI L").GetComponent<CurrentMenu>();
                if (cMenu != null)
                {
                    //Debug.Log("IL EST INITIALISEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE");
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
                    DetectorLogicGate scriptREML = Left.transform.GetChild(0).GetComponent<DetectorLogicGate>(); //RemoveSignalDetected
                    //Debug.Log(scriptEFDL, scriptPDL);
                    scriptPDL.OnActivate.AddListener(PinchLeftDetected);
                    scriptPDL.OnDeactivate.AddListener(PinchLeftEnded);
                    scriptEFDL.OnActivate.AddListener(ExtendedFingerDetected);
                    scriptEFDL.OnDeactivate.AddListener(ExtendedFingerEnded);
                    scriptREML.OnActivate.AddListener(RemoveSignalDetected);
                    scriptREML.OnDeactivate.AddListener(RemoveSignalEnded);
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
                    DetectorLogicGate scriptREMR = Right.transform.GetChild(0).GetComponent<DetectorLogicGate>();
                    //Debug.Log(scriptEFDR, scriptPDR);
                    scriptPDR.OnActivate.AddListener(PinchRightDetected);
                    scriptPDR.OnDeactivate.AddListener(PinchRightEnded);
                    scriptEFDR.OnActivate.AddListener(ExtendedFingerDetected);
                    scriptEFDR.OnDeactivate.AddListener(ExtendedFingerEnded);
                    scriptREMR.OnActivate.AddListener(RemoveSignalDetected);
                    scriptREMR.OnDeactivate.AddListener(RemoveSignalEnded);
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
                            case CurrentMenu.Selection.Modification:
                                LeapDeformation.Update();
                                break;
                            case CurrentMenu.Selection.Erase:
                                LeapSelection.eraseMode(removeOn);
                                break;
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
                                        break;
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
                        bool stillCorrect = Pointing > 0;
                        if (stillCorrect)
                        {
                            if (hl.Fingers[1].IsExtended && !hl.Fingers[2].IsExtended && !hl.Fingers[3].IsExtended && !hl.Fingers[4].IsExtended && nb_pinch == 0) //pinch prioritaire
                            {
                                f = hl.GetIndex();
                            }
                            if (hr.Fingers[1].IsExtended && !hr.Fingers[2].IsExtended && !hr.Fingers[3].IsExtended && !hr.Fingers[4].IsExtended && nb_pinch == 0) //de même
                            {
                                f = hr.GetIndex();
                            }
                            else
                            {
                                stillCorrect = false;
                            }
                            if (stillCorrect)
                            {
                                PointingDirection = f.Direction.ToVector3();
                                Fingertip = f.TipPosition.ToVector3();
                            }
                        }
                        Collider tempo = LeapSelection.selectionMain(stillCorrect, PointingDirection, Fingertip, ref nb_pinch, ref creating, scriptPDL, scriptPDR, ref currentSelection, cam,
                ref lastPosition, ref lastPositionR, ref lastPositionL, cMenu.GetCurrentMenuSelection());
                        if (tempo != null)
                        {
                            /*if (currentSelection != null)
                                currentSelection.transform.GetChild(0).gameObject.GetComponent<Material>().color = Color.blue;*/
                            currentSelection = tempo.gameObject.transform.parent.gameObject;
                           // currentSelection.transform.GetChild(0).gameObject.GetComponent<Material>().color = Color.red;
                        }
                        else
                        {
                            /*if (currentSelection != null)
                                currentSelection.transform.GetChild(0).gameObject.GetComponent<Material>().color = Color.blue;*/
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

