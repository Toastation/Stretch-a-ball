﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnselectableAxis : MonoBehaviour
{
    public struct LineDrawer
    {
        private LineRenderer lineRenderer;
        private float lineSize;

        public LineDrawer(float lineSize = 0.2f)
        {
            GameObject lineObj = new GameObject("LineObj");
            lineRenderer = lineObj.AddComponent<LineRenderer>();
            //Particles/Additive
            lineRenderer.material = new Material(Shader.Find("Hidden/Internal-Colored"));

            this.lineSize = lineSize;
        }

        private void init(float lineSize = 0.2f)
        {
            if (lineRenderer == null)
            {
                GameObject lineObj = new GameObject("LineObj");
                lineRenderer = lineObj.AddComponent<LineRenderer>();
                //Particles/Additive
                lineRenderer.material = new Material(Shader.Find("Hidden/Internal-Colored"));

                this.lineSize = lineSize;
            }
        }

        //Draws lines through the provided vertices
        public void DrawLineInGameView(Vector3 start, Vector3 end, Color color)
        {
            if (lineRenderer == null)
            {
                init(0.05f);
            }

            //Set color
            lineRenderer.startColor = color;
            lineRenderer.endColor = color;

            //Set width
            lineRenderer.startWidth = lineSize;
            lineRenderer.endWidth = lineSize;

            //Set line count which is 2
            lineRenderer.positionCount = 2;

            //Set the postion of both two lines
            lineRenderer.SetPosition(0, start);
            lineRenderer.SetPosition(1, end);
        }

        public void Destroy()
        {
            if (lineRenderer != null)
            {
                UnityEngine.Object.Destroy(lineRenderer.gameObject);
            }
        }
    }

    LineDrawer lineDrawerX;
    LineDrawer lineDrawerY;
    LineDrawer lineDrawerZ;



    // Start is called before the first frame update
    void Start()
    {
        lineDrawerX = new LineDrawer();
        lineDrawerY = new LineDrawer();
        lineDrawerZ = new LineDrawer();
        lineDrawerX.DrawLineInGameView(Vector3.zero, Vector3.forward, Color.black);
        lineDrawerY.DrawLineInGameView(Vector3.zero, Vector3.up, Color.black);
        lineDrawerZ.DrawLineInGameView(Vector3.zero, Vector3.right, Color.black);


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
