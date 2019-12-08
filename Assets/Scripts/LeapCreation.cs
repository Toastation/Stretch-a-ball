using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Leap.Unity
{
    public class LeapCreation : MonoBehaviour
    {
        PinchDetector PD1 ;
        PinchDetector PD2 ;
        int creating = 0;
        int initPD = 0; //Il faut s'assurer que les deux mains sont bien présentes à l'écran
        GameObject currentCreation;
        public HandModelBase H1;
        public HandModelBase H2;

        public static int ToInt(bool value)
        {
            return value ? 1 : 0;
        }


        void eventDetected()
        {
            
        }

        void eventEnded()
        {

        }

        // Start is called before the first frame update
        void Start()
        {
            Debug.Log("J'ai bien exécuté la fonction Start! \n");
            PD1 = new PinchDetector();
            PD2 = new PinchDetector();//this.AddComponent(typeof(PinchDetector));
        }

        // Update is called once per frame
        void Update()
        {
            if (H1 != null && H2 != null)
            {
                Debug.Log(H1 + " " + H2);
                if (initPD == 0)
                {
                    PD1.HandModel = H1;
                    PD2.HandModel = H2;
                    PD1.Activate();
                    initPD = 1;
                }
                int nb_pinch = ToInt(PD1.IsPinching) + ToInt(PD2.IsPinching);
                if (creating == 0)
                {
                    if (nb_pinch == 2)
                    {
                        creating = 1;
                        currentCreation = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                        currentCreation.transform.position = new Vector3(0, 0, 0);
                    }
                }
            } 
            else
            {
                initPD = 0;
            }

        }
    }
}
