using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveobjectsmooth : MonoBehaviour
{
    public GameObject obj;
    public float distance;
    public float velocity;
    public float dampening = 0.1f;
    public bool x = true;
    public bool y;
    public bool z;
    float targetdim;

    // Start is called before the first frame update
    void Start()
    {
        if (x)
        {
            targetdim = obj.transform.position.x + distance;
            if (distance > 0)
            {
                StartCoroutine(MoveObj("x+"));
            }
            else
            {
                StartCoroutine(MoveObj("x-"));
            }
        }
        else if (y)
        {
            targetdim = obj.transform.position.y + distance;
            if (distance > 0)
            {
                StartCoroutine(MoveObj("y+"));
            }
            else
            {
                StartCoroutine(MoveObj("y-"));
            }
        }
        else if (z)
        {
            // check if it's a negative or positive number
            targetdim = obj.transform.position.z + distance;
            if (distance > 0)
            {         
                StartCoroutine(MoveObj("z+"));
            }
            else
            {
                StartCoroutine(MoveObj("z-"));
            }        
        }
        
    }

    IEnumerator MoveObj(string param)
    {
        yield return new WaitForSecondsRealtime(velocity / 100);
        if (param == "z+")
        {
            if (obj.transform.position.z < targetdim)
            {
                obj.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y, obj.transform.position.z + dampening);
                StartCoroutine(MoveObj("z+"));
            }
        }
        else if (param == "z-")
        {
            if (obj.transform.position.z > targetdim)
            {
                obj.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y, obj.transform.position.z - dampening);
                StartCoroutine(MoveObj("z-"));
            }
        }
        else if (param == "x+")
        {
            if (obj.transform.position.x < targetdim)
            {
                obj.transform.position = new Vector3(obj.transform.position.x + dampening, obj.transform.position.y, obj.transform.position.z);
                StartCoroutine(MoveObj("x+"));
            }
        }
        else if (param == "x-")
        {
            if (obj.transform.position.x > targetdim)
            {
                obj.transform.position = new Vector3(obj.transform.position.x - dampening, obj.transform.position.y, obj.transform.position.z);
                StartCoroutine(MoveObj("x-"));
            }
        }
        else if (param == "y+")
        {
            if (obj.transform.position.y < targetdim)
            {
                obj.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y + dampening, obj.transform.position.z);
                StartCoroutine(MoveObj("y+"));
            }
        }
        else if (param == "y-")
        {
            if (obj.transform.position.y > targetdim)
            {
                obj.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y - dampening, obj.transform.position.z);
                StartCoroutine(MoveObj("y-"));
            }
        }
    }

}
