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


    [SerializeField]float curX,curY,curZ; //current speed rate always>0 
    [SerializeField]float inputX,inputY,inputZ;
    [SerializeField]float dirX,dirY,dirZ; //direction for x,y,z used for deacceleration

    //current rotation speed
    [SerializeField] Vector3 targetEulerAngle;
    
    void Update(){
        continousRotation();
        manualRotation();
    }

    void continousRotation(){
        transform.Rotate(setContinousRotationSpeed,Space.Self);
    }


    void manualRotation(){
        //good for keyboard rotation but bad for mouse

        inputX= Input.GetAxis(setInputToRoll.ToString());
        inputY=Input.GetAxis(setInputToYaw.ToString());
        inputZ=Input.GetAxis(setInputToPitch.ToString());
        
        if (inputX!=0)
        {
            curX=Mathf.MoveTowards(curX,setMaxSpeed.x,setAcceleration.x*Time.deltaTime);
            dirX=Mathf.Sign(inputX);
        }else{
            curX=Mathf.MoveTowards(curX,0,setDeAcceleration.x*Time.deltaTime);         
        }

        targetEulerAngle.x=targetEulerAngle.x+curX*dirX*Time.deltaTime;
        //enable full cycle
        if (targetEulerAngle.x <=-180)
        {
            targetEulerAngle.x+=360;
        }
        if (targetEulerAngle.x >= 180)
        {
            targetEulerAngle.x-=360;
        }

        targetEulerAngle.x= Mathf.Clamp(targetEulerAngle.x, setMinRotation.x, setMaxRotation.x);

        transform.localEulerAngles=targetEulerAngle;        
    }

    



    [ContextMenu("Test")]
    void Test(){
        Debug.Log(setInputToPitch.ToString());
    }
     
} 