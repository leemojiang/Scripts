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


    float curX,curY,curZ; //current speed rate always>0 
    float inputX,inputY,inputZ;
    float dirX,dirY,dirZ; //direction for x,y,z used for deacceleration

    //current rotation speed
    Vector3 targetEulerAngle; //every frame updated once //indicate where the rotation should be in one frame constrained by the acc and maxspeed

    
    void Start(){
        initRotation=transform.localRotation;
        targetEulerAngle=transform.localEulerAngles;
    }

    public Transform model;
    void Update(){
        continousRotation();
        // manualRotation2();


        //test auto rotation
        aimingAroundY_UP(model.position);
        Debug.DrawRay(transform.position,transform.forward);
    }

    void continousRotation(){
        transform.Rotate(setContinousRotationSpeed,Space.Self);
    }


    void manualRotation()
    {
        //good for keyboard rotation but bad for mouse

        inputX= Input.GetAxis(setInputToRoll.ToString());
        inputY=Input.GetAxis(setInputToYaw.ToString());
        inputZ=Input.GetAxis(setInputToPitch.ToString());
        
        if (inputX!=0)
        {
            curX=Mathf.MoveTowards(curX,setMaxSpeed.x,setAcceleration.x*Time.deltaTime);
            dirX=Mathf.Sign(inputX);
            isRotating=true;
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
        // rotateToTarget();    
    }
    
    [Range(1,199)]
    public float sensitivX;
    void manualRotation2(){
        inputX= Input.GetAxis(setInputToRoll.ToString());
        inputY=Input.GetAxis(setInputToYaw.ToString());
        inputZ=Input.GetAxis(setInputToPitch.ToString());
        if (inputX!=0)
        {
            curX=Mathf.MoveTowards(curX,setMaxSpeed.x,setAcceleration.x*Time.deltaTime);
            dirX=Mathf.Sign(inputX);
            isRotating=true;
        }else{
            curX=Mathf.MoveTowards(curX,0,setDeAcceleration.x*Time.deltaTime);         
        }


        float deltaX=Mathf.Clamp(Mathf.Abs(inputX*sensitivX),0,curX*Time.deltaTime);
        // deltaX=Mathf.Clamp(Mathf.Abs(inputX*sensitivX),0,setMaxSpeed.x*Time.deltaTime);

        targetEulerAngle.x=targetEulerAngle.x+deltaX*dirX;
        
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
    bool isRotating=false;
    private Quaternion initRotation;
    void rotateToTarget_Transform(){
        if (!isRotating)
        {             
            return;
        }

        float targetX,targetY,targetZ;

        targetX=Mathf.DeltaAngle(transform.localEulerAngles.x,targetEulerAngle.x);
        targetY=Mathf.DeltaAngle(transform.localEulerAngles.y,targetEulerAngle.y);
        targetZ=Mathf.DeltaAngle(transform.localEulerAngles.z,targetEulerAngle.z);

        if (Mathf.Abs (targetX) < 0.01f && Mathf.Abs (targetY) < 0.01f && Mathf.Abs (targetZ) < 0.01f) {
			isRotating = false;
		}

        Quaternion rot=initRotation * Quaternion.Euler(targetEulerAngle.x,targetEulerAngle.y,targetEulerAngle.z);
        transform.localRotation=Quaternion.Slerp(transform.localRotation,rot,Time.deltaTime);
    }

    public Vector3 debugpos,debugpos2;
    public float debugAngle;
    public void aimingAroundY_UP(Vector3 targetPos){
        //transform to locoal 
        targetPos=transform.worldToLocalMatrix.MultiplyPoint(targetPos);
        float tarAngleXZ = Vector2.Angle(new Vector2 (0, 1),
        new Vector2(targetPos.x,targetPos.z));

        debugAngle=tarAngleXZ;
        debugpos=targetPos;

        if (tarAngleXZ> setMinRotation.y && tarAngleXZ < setMaxRotation.y) //rotate around which axis
        {   
            targetPos.y=0;
            Quaternion tarQ=Quaternion.FromToRotation(new Vector3(0,0,1),targetPos);
            debugpos2 = transform.InverseTransformVector(transform.forward).normalized;
            tarQ=tarQ * transform.localRotation;
            transform.localRotation = Quaternion.RotateTowards(transform.localRotation,tarQ,setMaxSpeed.y * Time.deltaTime);
        }    
    }

    [ContextMenu("Test")]
    void Test(){
        Debug.Log(setInputToPitch.ToString());
    }
     
} 