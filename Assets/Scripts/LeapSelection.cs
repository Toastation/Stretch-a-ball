using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Leap.Unity {
    public class LeapSelection : MonoBehaviour
    {

        static RaycastHit[] SphereArray;
        static RaycastHit[] AlreadyHit = { };
        static float timer = 0; // initialisation normalement inutile, mais à voir
        static float timerDelay = 0.5f; // temps entre le changement de couche
        static Collider currentCollider = null;


        static bool isIn(RaycastHit o, RaycastHit[] a)
        {
            int len = a.Length;
            //Debug.Log(len);
            for (int i = 0; i < len; i++)
            {
                if (GameObject.ReferenceEquals(a[i].collider, o.collider))
                {
                    //Debug.Log("True");
                    return true;
                }
            }
            //Debug.Log("False");
            return false;
        }

        static Collider chooseSphere()
        {
            int len = SphereArray.Length;
            int lenp = AlreadyHit.Length;
            if (len != 0)
            {
                int i = 0;
                while (i < len && isIn(SphereArray[i], AlreadyHit))
                {
                    i++;
                }
                if (i < len) // On n'est pas allé au bout du tableau, donc un objet touché n'a pas encore été sélectionné
                {

                    currentCollider = SphereArray[i].collider;
                    RaycastHit[] tempo = new RaycastHit[lenp + 1];
                    for (int j = 0; j < lenp; j++) // Pour une raison inconnue, Array.copy ne copiait pas tout ...
                    {
                        tempo[j] = AlreadyHit[j];
                    }
                    tempo[lenp] = SphereArray[i];
                    //print(tempo.ToString());
                    AlreadyHit = tempo;

                } else // Tous les éléments du tableau ont déjà été sélectionnés, on reboucle donc
                {
                    //Debug.Log("ON REBOUCLE !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                    currentCollider = SphereArray[0].collider;
                    AlreadyHit = new RaycastHit[1] { SphereArray[0] };
                }
            } else // Aucun objet n'a été touché par le raycast
            {
                currentCollider = null;
            }
            return currentCollider;
        }

        public static Collider selectionMain(bool Pointing, Vector3 PointingDirection, Vector3 Fingertip, ref int nb_pinch, ref bool creating, PinchDetector scriptPDL, PinchDetector scriptPDR, ref GameObject currentSelection, Camera cam,
                ref Vector3 lastPosition, ref Vector3 lastPositionR, ref Vector3 lastPositionL) // si le doigt pointe, un Vector3 (direction de raycast), le bout du doigt
        {
            if (nb_pinch == 1)
            {
                Debug.Log("1 pinch");
                LeapCommon.deplacementMode(ref currentSelection, scriptPDL, scriptPDR, ref lastPosition, ref lastPositionR, ref lastPositionL);
            } else if (nb_pinch == 2)
            {
                Debug.Log("2 pinch");
                LeapCommon.sizingMode(ref currentSelection, scriptPDL, scriptPDR, ref lastPosition, cam, ref lastPositionR, ref lastPositionL);
            }
            //LineRenderer lr = new LineRenderer();
            //lr.positionCount = 2;
            if (Pointing) //si je pointe avec mon doigt
            {
                //lr.SetPosition(0, Fingertip);
                //lr.SetPosition(1, Fingertip + PointingDirection * 10);
                if (currentCollider == null || timer >= timerDelay) //dans le cas où j'arrive pour la première fois ou que je peux changer de couche
                {
                    timer = 0;
                    Ray r = new Ray(Fingertip, PointingDirection);
                    SphereArray = Physics.RaycastAll(r); // PTET FAIRE GAFFE AU MASK UTILISE POUR PAS SELECTIONNER LES POINTS
                    Debug.Log(SphereArray.Length);
                    return chooseSphere();
                } else // le cas où je dois attendre avant de changer de couche
                {
                    timer += Time.deltaTime;
                    return currentCollider;
                }
            } else
            {
                timer = timerDelay;
                return currentCollider;
            }

        }
    }
}
