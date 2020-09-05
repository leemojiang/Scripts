using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class AITemplate:MonoBehaviour{

    public Transform debug_Target;

    [Header("NavAgent  Settings")]
    public float maxDeviateAngle =5.0f; 
    //less than this angle will keep forward 
    //no need to turn
    public float minTurnInPlaceAngle=45.0f;
    public float resetNavAgentDisgance=3.0f;

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
        
        Vector3 closestTargetOnPath = navAgent.steeringTarget; 

        //show the target
        Debug.DrawRay (closestTargetOnPath, Vector3.up * 500);

        
        vf=transform.forward;
        vd=navAgent.desiredVelocity;

        vf.y=0;
        vd.y=0;

        float curDeviateAngle = Vector3.Angle(vf,vd);
		Vector3 localVelocity = transform.InverseTransformDirection (navAgent.desiredVelocity);
        
        debug_angle=curDeviateAngle;
        float isForward = localVelocity.z;

        if(curDeviateAngle > minTurnInPlaceAngle ){ //get away too far from the direction
            engine.horizontal =  Mathf.Sign(curDeviateAngle);
            engine.vertical=0;

            debug_v = engine.vertical;
            debug_h= engine.horizontal;
            return;
        }
        if( curDeviateAngle > maxDeviateAngle ){ //get away too far from the direction
            // engine.horizontal =  localVelocity.x;
            engine.horizontal =  Mathf.Sign(curDeviateAngle);
            engine.vertical= Mathf.Sign(isForward) * 0.5f; //in constant speed
            
        }else{
            engine.vertical= Mathf.Sign(isForward) * 0.8f;
        }
        debug_v = engine.vertical;
        debug_h= engine.horizontal;
        
    }

    


    
}