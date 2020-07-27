using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel:MonoBehaviour{
    
    public WheelCollider wc;
    public bool Is_Left;
    
    /*
		 * This script is attached to all the drive wheels in the tank.
		 * This script fixes the looseness of the wheels.
		*/




    public bool Drive_Flag = true;
    public float Radius = 0.3f;
    public bool Use_BrakeTurn = true;
    // << User options

    // Referred to from "Drive_Wheel_CS".
    public float Max_Angular_Velocity;
    public float Left_Angular_Drag;
    public float Right_Angular_Drag;
    public float Left_Torque;
    public float Right_Torque;

    float maxAngVelocity;
    public Engine controlScript;

    void Start()
    {
        Initialize();
    }


    void Initialize ()
    {
        // Get the "Drive_Control_CS".
        controlScript = GetComponentInParent <Engine>();
        // Set the "maxAngVelocity".
        maxAngVelocity = Mathf.Deg2Rad * ((controlScript.Max_Speed / (2.0f * Radius * Mathf.PI)) * 360.0f);
        maxAngVelocity = Mathf.Clamp (maxAngVelocity, 0.0f, controlScript.MaxAngVelocity_Limit); // To solve physics issues in the default physics quality.
        wc = GetComponent<WheelCollider>();
    }

    void Update ()
    {
        Control_Velocity_And_Torque();
    }

    void Control_Velocity_And_Torque ()
    {
        // Set the "Max_Angular_Velocity".
        Max_Angular_Velocity = controlScript.Speed_Rate * maxAngVelocity;

        // Set the brake drag.
        if (Use_BrakeTurn) {
            Left_Angular_Drag = controlScript.L_Brake_Drag;
            Right_Angular_Drag = controlScript.R_Brake_Drag;
        }
        else {
            Left_Angular_Drag = 0.0f;
            Right_Angular_Drag = 0.0f;
        }

        // Set the torque.
        if (Drive_Flag == true) {
            Left_Torque = controlScript.Left_Torque;
            Right_Torque = controlScript.Right_Torque;
        }
        else {
            Left_Torque = 0.0f;
            Right_Torque = 0.0f;
        }
    }

    void FixedUpdate ()
    {
        Control_WheelCollider();
        // Debug.Log("Update");
    }

    void Control_WheelCollider ()
    {
        float currentAngularVelocity= wc.rpm*2*Mathf.PI/60;
        if (currentAngularVelocity > maxAngVelocity)
        {
            return;
        }
        if (Is_Left) { // Left
            wc.brakeTorque = Left_Angular_Drag;
            wc.motorTorque =Left_Torque;
        } else { // Right
            wc.brakeTorque = Left_Angular_Drag;
            wc.motorTorque =Left_Torque;
        }
    }


}   