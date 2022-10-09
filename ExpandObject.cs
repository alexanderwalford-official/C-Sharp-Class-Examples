using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpandObject : MonoBehaviour
{
    public bool ExpandX = true;
    public bool ExpandY = true;
    public bool ExpandZ = true;

    public float MaxXSize = 1;
    public float MaxYSize = 1;
    public float MaxZSize = 1;

    public float ExpandSpeed = 0.1f;
    public Vector3 StartSize = new Vector3(1, 1, 1);
    public bool PreserveStartSize = false;

    void Start()
    {
        // reset object scale

        if (PreserveStartSize)
        {
            StartSize = this.GetComponent<Transform>().localScale;
        }
        else
        {
            this.GetComponent<Transform>().localScale = StartSize;
        }
    }

    void Update()
    {
        // expands the scale of the object to the desired parameters

        float ObjXSize = this.GetComponent<Transform>().localScale.x;
        float ObjYSize = this.GetComponent<Transform>().localScale.y;
        float ObjZSize = this.GetComponent<Transform>().localScale.z;

        if (ExpandX)
        {
            if (ObjXSize < MaxXSize)
            {
                this.GetComponent<Transform>().localScale = new Vector3(ObjXSize + ExpandSpeed, ObjYSize, ObjZSize);
            }
        }
        if (ExpandY)
        {
            if (ObjYSize < MaxYSize)
            {
                this.GetComponent<Transform>().localScale = new Vector3(ObjXSize, ObjYSize + ExpandSpeed, ObjZSize);
            }
        }
        if (ExpandZ)
        {
            if (ObjZSize < MaxZSize)
            {
                this.GetComponent<Transform>().localScale = new Vector3(ObjXSize, ObjYSize, ObjZSize + ExpandSpeed);
            }
        }
    }
}
