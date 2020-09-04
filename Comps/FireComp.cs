using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[RequireComponent(typeof(GenericFireArm))]
[Serializable]
public abstract class FireComp:MonoBehaviour{
    public GenericFireArm gfa;

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
        fire();
    }


    public override void fire( )
    {
        if (Input.GetMouseButtonDown((int)fireInput))
        {   
            //not exceed max rpm
            if(Time.time-lastFireTime>minFireDeltaTime){

                //not overheat
                if (gfa.currentHeat>gfa.overheatPenalty){
                    gfa.overHeat();
                    return;
                }

                //have ammo
                //?????

                //fire
                genProjectile();
                gfa.currentHeat+=gfa.heatAddWhenFire;
                lastFireTime=Time.time;
            }
        }
    }

    
    void genProjectile(){
        GenericProjectile pro = Instantiate(gfa.projectileTemplate,projectileStartPosition.position,projectileStartPosition.rotation);
        Debug.Log(projectileStartPosition.rotation);
        Debug.Log(projectileStartPosition.localRotation);
        Rigidbody rb= pro.GetComponent<Rigidbody>();
        rb.velocity= projectileStartPosition.forward * gfa.velocity;
    }
}

public class MultiFireComp : FireComp
{
    public override void fire( )
    {
        throw new NotImplementedException();
    }
}