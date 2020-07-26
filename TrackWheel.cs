using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackWheel:MonoBehaviour{
    
    public Rigidbody This_Rigidbody;
    public bool Is_Left;
    
    /*
		 * This script is attached to all the drive wheels in the tank.
		 * This script fixes the looseness of the wheels.
		*/


    // User options >>
    public Transform This_Transform;
    
    public Vector3 Initial_Locoal_Pos;
    public float Initial_Pos_Y;
    public Vector3 Initial_Angles;

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
    Vector3 currentPosition;
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

        This_Rigidbody = GetComponent<Rigidbody>();
    }

    void Update ()
    {
        stable();
        Control_Velocity_And_Torque();
    }

    void stable(){
         // Stabilize the position.
        currentPosition = This_Transform.localPosition;
        currentPosition.y = Initial_Pos_Y;

        // Stabilize the angle.
        Initial_Angles.y = This_Transform.localEulerAngles.y;

        // Set the position and rotation.
        This_Transform.localPosition = currentPosition;
        This_Transform.localEulerAngles = Initial_Angles;
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
        Control_Rigidbody();
        // Debug.Log("Update");
    }

    void Control_Rigidbody ()
    {
        // Set the "maxAngularVelocity" of the rigidbody.
        This_Rigidbody.maxAngularVelocity = Max_Angular_Velocity;
        // Set the "angularDrag" of the rigidbody, and add torque to it.
        if (Is_Left) { // Left
    
            This_Rigidbody.angularDrag = Left_Angular_Drag;
            This_Rigidbody.AddRelativeTorque (0.0f, Left_Torque, 0.0f);
        } else { // Right
            This_Rigidbody.angularDrag = Right_Angular_Drag;
            This_Rigidbody.AddRelativeTorque (0.0f, Right_Torque, 0.0f);
        }
    }


}   