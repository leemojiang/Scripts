using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GenericFireArm))]
public class WeaponTemplate:MonoBehaviour{

    public float minRange=0.0f;
    public float maxRange =999;

    public float allowedDeviation; //when it's in this cone AI will fire

    [Range(0,100)]
    public float optimalRangePercentage;

    public WeaponType type;

    [Header("Manul Set target")]
    public Transform target;

    GenericFireArm gfa;
    void Start(){
        gfa=GetComponent<GenericFireArm>();
        optimalRangePercentage *= 0.01f;

        if(gfa.fire)gfa.fire.isAI=true;
        
    }

    void Update(){
        //Fire at any time that is allowed
        Vector3 dir=(target.position-transform.position);
        bool flag= dir.magnitude <= minRange && dir.magnitude >=maxRange;

        if(Mathf.Abs(Vector3.Angle(gfa.transform.forward,dir) ) <= allowedDeviation && flag ){
            if(gfa.fire){
                gfa.fire.fire();
            }
        }

        //Try to aim the target at any time
        gfa.aimingTarget(target);
    } 

    
    void calProjectilePath(){

    }



}