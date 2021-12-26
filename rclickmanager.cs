using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rclickmanager : MonoBehaviour
{
    public float norm_FieldOfView = 64;
    public float zoom_FieldOfView = 40;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            if (Camera.main.fieldOfView != zoom_FieldOfView)
            {
                Camera.main.fieldOfView = Camera.main.fieldOfView - 0.5f;
            }   
        }
        else
        {
            if (Camera.main.fieldOfView != norm_FieldOfView)
            {
                Camera.main.fieldOfView = Camera.main.fieldOfView + 0.5f;
            }
        }
    }
}
