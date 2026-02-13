using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class playerFollow : MonoBehaviour
{
    //Transform fo the target
    private Transform target;

    //Multiple camera angles!
    public Vector3[] cameraAngles;

    //int for current selected angle.
    private int cameraAngleIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        //set target.
        target = GameObject.Find("PacoCar").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ChangeCameraAngle();
        }
    }

    private void LateUpdate()
    {
        //Make sure the target exists, if so, call the CameraPos()
        if (target != null)
        {
            CameraPos();
        }
    }

    void ChangeCameraAngle()
    {
        //Increment Camera angle index of the array of Vector 3
        //Use modulus to get the remainder and ensure that it doesn't go outside the bounds of the array
        cameraAngleIndex = (cameraAngleIndex + 1) % cameraAngles.Length;
    }

    //Function to figure out where the Camera should be
    private void CameraPos()
    {
        //Temp Vec3 vari to store the target pos
        /*Math! find where the target is the subtract its forward vec (z) then multiply by the camera angle. 
         * this sets the vec as and "offset" from the pos of the car
         * otherwise it will be at 0,0,0 at all times.
         * x is not included, because the cam shouldn't roll
         */
        Vector3 pos = target.position - target.forward * cameraAngles[cameraAngleIndex].z + target.up * cameraAngles[cameraAngleIndex].y;

        //Set cam pos to the MATH
        transform.position = pos;

        //Tranform.LookAt will rotate an object to point toward another object.
        //This takes the current target pos and adds ots Vec (+1) then * by whatever cameraAngleIndex of y is
        //that way the rotation on the y will be consistent.
        transform.LookAt(target.position + target.up * cameraAngles[cameraAngleIndex].y);
    }
}
