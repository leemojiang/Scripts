using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GenericFireArm:MonoBehaviour{

    [Header("Heat settings")]
    public float coolDownPerSec=0.0f;
    public float heatAddWhenFire=0.0f;
    public float overheatPenalty=0.0f; //how many to over heat

    [Header("Projectile settings")]
    public GenericProjectile projectileTemplate;
    public float velocity;

    // [Header("Single Fire Comp")]
    // public SingleFireComp fireComp;

    //two rotation bundle 
    void Start(){
        // fireComp=GetComponent<SingleFireComp>();
    }

    public void aimingTarget(){

    }

    void initRotationalBundles(){
        
    }
}