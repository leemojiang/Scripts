using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class RotationalBundle:MonoBehaviour{
    public Vector3 setAcceleration;//in x,y,z yaw pitch row
    public Vector3 setDeAcceleration; //in yaw pitch row
    public Vector3 setMaxSpeed; //rad/s
    public Vector3 setMaxRotation;
    public Vector3 setMinRotation;

    public Vector2 regulatePitch=new Vector2(1,1); // multipler fot the axis
    public Vector2 regulateRoll=new Vector2(1,1);
    public Vector2 regulateYaw=new Vector2(1,1);

    
    public InputAxis setInputToPitch=InputAxis.NULL; //z
    public InputAxis setInputToYaw=InputAxis.NULL; //y
    public InputAxis setInputToRoll=InputAxis.NULL; //x

    public Vector3 setContinousRotationSpeed;//in yaw pitch row

    float curX,curY,curZ; //rotation speed
    Vector3 targetEulerAngle;
    
    void Update(){
        continousRotation();
        manualRotation();
    }

    void continousRotation(){
        transform.Rotate(setContinousRotationSpeed,Space.Self);
    }

    void manualRotation(){
        float targetX,targetY,targetZ;

        targetX= Input.GetAxis(setInputToRoll.ToString());
        targetY=Input.GetAxis(setInputToYaw.ToString());
        targetZ=Input.GetAxis(setInputToPitch.ToString());

        // Debug.Log(targetX);
        if (targetX!=0)
        {
            curX=Mathf.MoveTowards(curX,setMaxSpeed.x,setAcceleration.x*Time.deltaTime);
        }else{
            curX=Mathf.MoveTowards(curX,0,setDeAcceleration.x*Time.deltaTime);
        }
        Debug.Log(curX);

        targetEulerAngle.x= Mathf.Clamp (transform.localEulerAngles.x+curX*Time.deltaTime*targetX, setMinRotation.x, setMaxSpeed.x);

        transform.localEulerAngles=targetEulerAngle;
        // InputAxis.MouseX
        
    }

    [ContextMenu("Test")]
    void Test(){
        Debug.Log(setInputToPitch.ToString());
    }
     
} 