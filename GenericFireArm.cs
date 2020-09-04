using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GenericFireArm:MonoBehaviour{

    [Header("Heat settings")]
    public float coolDownPerSec=0.0f;
    public float heatAddWhenFire=0.0f;
    public float overheatPenalty=0.0f; //how many to over heat

    public float currentHeat=0.0f;
    [Header("Projectile settings")]
    public GenericProjectile projectileTemplate;
    public float velocity;

    
    protected FireComp fire;

    void Start(){


        //Init 
        //User a single firecomp for testing 
        fire=GetComponent<SingleFireComp>();
        if (fire==null) fire = gameObject.AddComponent<SingleFireComp>();
        
        fire.gfa=this;
    }



    void Update(){
        if(currentHeat >0) currentHeat-=coolDownPerSec/Time.deltaTime;
        
    }

    public void overHeat(){

    }


    public void aimingTarget(){

    }

    void initRotationalBundles(){

    }
}