using System.Collections.Generic;
using UnityEngine;

/**
 * This class represents data points in a 3D scatterplot.
 */
public class DataPoint
{
    /** The position of the point in 3D space */
    private Vector3 pos;

    /** The color of the point */
    private Color color;

    /** Dictionary of possible attributes, the key is the name of the attribute */
    private Dictionary<string, Object> attributes;

    private bool selected;

    private GameObject gameObject;

    private SphereCollider collider;

    /**
     * Creates a DataPoint from a position, the default color is black
     */
    public DataPoint(Vector3 pos) : this(pos, new Vector4(0, 0, 0, 1)) {
    
    }


    /**
     * Creates a DataPoint from a position and a color
     */
    public DataPoint(Vector3 pos, Color color) {
        this.pos = pos;
        this.color = color;
        this.attributes = new Dictionary<string, Object>();
        this.selected = false;
    }

    public void SetGameObject(GameObject gameObject) 
    {
        this.gameObject = gameObject;
    }

    public void SetColor(Color color)
    {
        this.color = color;
    }

    public void SetSelected(bool selected)
    {
        this.selected = selected;
    }

    public Vector3 GetPos()
    {
        return this.pos;
    }

    public Color GetColor()
    {
        return this.color;
    }

    public bool isSelected()
    {
        return this.selected;
    }

    public Dictionary<string, Object> GetAttributes()
    {
        return this.attributes;
    }

    public Renderer GetRenderer()
    {
        if (this.gameObject == null) Debug.LogError("undefined gameObject in DataPoint");
        return this.gameObject.GetComponent<Renderer>();
    }

}
