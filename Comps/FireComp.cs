using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[RequireComponent(typeof(GenericFireArm))]
[Serializable]
public abstract class FireComp:MonoBehaviour{
    public GenericFireArm gfa;
    public bool isAI=false;
    public abstract void fire( );
    
    
}

[Serializable]
public class SingleFireComp : FireComp
{   
    public Transform projectileStartPosition;
    public InputMouseButtons fireInput;
    public int roundsPerMinute=100;
    float lastFireTime=0;
    float minFireDeltaTime; //in second

    void Start(){
        gfa=GetComponent<GenericFireArm>();
        minFireDeltaTime=60/roundsPerMinute;

        //in case forget to set it!
        if(projectileStartPosition==null) projectileStartPosition=GetComponentsInChildren<Transform>()[0];
    }

    void Update(){
        if (Input.GetMouseButtonDown((int)fireInput) && !isAI)
        {   
            if (!(gfa.isReloading || Mathf.Abs(gfa.ammo.curBulletsInWeapon) < 1) )
            fire();
        }
    }


    public override void fire( )
    {
        if (gfa.isReloading || Mathf.Abs(gfa.ammo.curBulletsInWeapon) < 1) return;
        

        //not exceed max rpm
        if(Time.time-lastFireTime>minFireDeltaTime){
            
            //not overheat
            if (gfa.currentHeat>gfa.overheatPenalty){
                gfa.overHeat();
                return;
            }

            //have ammo
            //Unimplemented

            //fire
            gfa.ammo.curBulletsInWeapon--; //decrease one ammo
            genProjectile();
            gfa.currentHeat+=gfa.heatAddWhenFire;
            lastFireTime=Time.time; 
        }

        if(gfa.ammo.curBulletsInWeapon ==0 && gfa.ammo.autoReload && !gfa.isReloading &&  Mathf.Abs(gfa.ammo.curNrOfMags) > 0){
            StartCoroutine(gfa.ammo.reload());
        }
    }

    
    void genProjectile(){
        GenericProjectile pro = Instantiate(gfa.projectileTemplate,projectileStartPosition.position,projectileStartPosition.rotation);
        // Debug.Log(projectileStartPosition.rotation);
        // Debug.Log(projectileStartPosition.localRotation);
        Rigidbody rb= pro.GetComponent<Rigidbody>();

        if(isAI){

                
            return;
        }

        //humanpart
        //set vecolicity
        rb.velocity= projectileStartPosition.forward * gfa.velocity;

        //deviation
        //not Unimplemented
    }
}

public class MultiFireComp : FireComp
{
    public override void fire( )
    {
        throw new NotImplementedException();
    }
}