using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class AITemplate:MonoBehaviour{

    public Transform debug_Target;

    [Header("NavAgent  Settings")]
    public float maxDeviationAngle =5.0f; 
    //less than this angle will keep forward 
    //no need to turn
    public float minTurnInPlaceAngle=45.0f;
    public float resetNavAgentDisgance=3.0f;
    public float targetDeviationDistance=3.0f;
    //from ControlInfo
    public float throttleScale=0.5f;

    //from Mobile
    public float turnRadius=3;
    public float maxSpeed=10;
    public float lodHeight=2.0f;
    public float lodRadius=4.5f;

    
    NavMeshAgent navAgent;
    Rigidbody rb;
    public Engine engine;
    void Start(){
        //init navagent
        GameObject navMesh = new GameObject ("NavMesh");
		navMesh.transform.position = transform.position;
		navAgent = navMesh.AddComponent<NavMeshAgent> ();
        navAgent.Warp(transform.position);

        navAgent.height=lodHeight;
        navAgent.radius = lodRadius;
		navAgent.obstacleAvoidanceType = ObstacleAvoidanceType.MedQualityObstacleAvoidance;

        rb=GetComponent<Rigidbody>();
        engine=GetComponent<Engine>();

        //cal anguler speed deg/s
        navAgent.angularSpeed= 360 /(Mathf.PI * 2*  turnRadius / maxSpeed);
        

        //set angine flag
        engine.isAI=true;
    }

    public float debug_angle,debug_v,debug_h;
    public Vector3 vf,vd;
    public void Update(){

        
        
        //reset navagent if too far
        if(Vector3.Distance(navAgent.transform.position,this.transform.position)> resetNavAgentDisgance){
            navAgent.Warp(transform.position);
        }

        //nav agent controll
        //target is null
        if(!debug_Target) return;

        navAgent.SetDestination(debug_Target.position);
        
        mathControlMoving();
        
    }

    void engingControlMoving(){
        //nav agent controll
        //target is null
        if(!debug_Target) return;

        navAgent.SetDestination(debug_Target.position);
        
        Vector3 closestTargetOnPath = navAgent.steeringTarget; 

        //show the target
        Debug.DrawRay (closestTargetOnPath, Vector3.up * 500);

        
        vf=transform.forward;
        vd=navAgent.desiredVelocity;

        vf.y=0;
        vd.y=0;

        float curDeviationAngle = Vector3.Angle(vf,vd);
		Vector3 localVelocity = transform.InverseTransformDirection (navAgent.desiredVelocity);
        
        debug_angle=curDeviationAngle;
        float isForward = localVelocity.z;
        float isRight= Mathf.Sign(localVelocity.x); 
        if(curDeviationAngle > minTurnInPlaceAngle ){ //get away too far from the direction
            engine.horizontal =  isRight;
            engine.vertical=0;

            debug_v = engine.vertical;
            debug_h= engine.horizontal;
            return;
        }
        if( curDeviationAngle > maxDeviationAngle ){ //get away too far from the direction
            // engine.horizontal =  localVelocity.x;
            engine.horizontal =  isRight;
            engine.vertical= Mathf.Sign(isForward) * 0.25f; //in constant speed
            
        }else{
            engine.horizontal = 0;
            engine.vertical= Mathf.Sign(isForward) * 0.5f;
        }

        //get to target
        if(Vector3.Distance(transform.position,debug_Target.position)<targetDeviationDistance){
            engine.vertical=0;
            engine.horizontal=0;
        }

        debug_v = engine.vertical;
        debug_h= engine.horizontal;
    }

    void mathControlMoving(){
        //nav agent controll
        //target is null
        if(!debug_Target) return;

        navAgent.SetDestination(debug_Target.position);

        Vector3 pos = navAgent.transform.position;
        pos.y=transform.position.y;

        transform.position= pos;
        transform.rotation= navAgent.transform.rotation;
    }    


    
}