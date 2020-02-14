using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;

namespace Leap.Unity{
  public class LeapCamMotion : MonoBehaviour
  {
      public HandModelBase Hl;
      public HandModelBase Hr;

      public float translationSpeed = 2.5f;
      public float rotationSpeed = 25.0f;

      Controller controller;

      Vector position;
      Vector direction;

      Frame frame;

      List<Hand> hands;
      Hand firstHand;

      // Start is called before the first frame update
      void Start()
      {
        controller = new Controller ();
      }

      // Update is called once per frame
      void Update()
      {
        frame = controller.Frame();
        if(frame.Hands.Count == 1){
          hands = frame.Hands;
          firstHand = hands [0];


        /*
        * Rotation camera haut en bas
        * Définir la normal de la main, notre référence
        *     - Si ref est superieur à main alors monter camera, laisser monter tant que main est levée
        *     - Si ref est inferieur à main alors descendre camera
        * Regarder les valeurs lorsque que la main est "à plat" puis regarder
        * valeur lorsque qu'elle "monte" ou "descend"
        */
          if (firstHand.IsRight ){
            position = firstHand.PalmPosition;
            direction = firstHand.Direction;

            Vector3 rot = new Vector3();

            //Debug.Log("Normal : " + newNormal +"\n");
            //Debug.Log("Direction : " + direction +"\n");

            if(direction.y > 0.5){
              rot.x -= rotationSpeed * Time.deltaTime;
            }

            if(direction.y < -0.5){
              rot.x += rotationSpeed * Time.deltaTime;
            }

            if(direction.x > 0.5){
              rot.y += rotationSpeed * Time.deltaTime;
            }

            if(direction.x < -0.5){
              rot.y -= rotationSpeed * Time.deltaTime;
            }

            //Debug.Log("Position : " + position +"\n");
            //z move forward <0 move backward >0
            Vector3 dir = new Vector3();
            if(position.z < -100){
                dir += new Vector3(0, 0 , 1);
            }
            if(position.z > 100){
                dir += new Vector3(0, 0 , -1);
            }

            //x right left
            if(position.x < -100){
                dir += new Vector3(-1, 0, 0);
            }
            if(position.x > 100){
                dir += new Vector3(1, 0, 0);
            }


            Vector3 p = dir * translationSpeed * Time.deltaTime;

            transform.eulerAngles += rot;
            transform.Translate(p);
          }
        }
      }
  }
}
