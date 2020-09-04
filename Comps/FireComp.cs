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
    public Vector3 projectileStartPosition;
    public InputMouseButtons fireInput;
    public int roundsPerMinute;
    float lastFireTime=0;
    float minFireDeltaTime; //in second

    void Start(){
        gfa=GetComponent<GenericFireArm>();
        minFireDeltaTime=60/roundsPerMinute;
    }

    void Update(){
        fire();
    }


    public override void fire( )
    {
        if (Input.GetMouseButtonDown(1))
        {   
            //not exceed max rpm
            if(Time.time-lastFireTime>minFireDeltaTime){

                //not overheat
                if (gfa.currentHeat>gfa.overheatPenalty){
                    gfa.overHeat();
                    return;
                }
                //fire
                genProjectile();
                gfa.currentHeat+=gfa.heatAddWhenFire;

            }
        }
    }

    
    void genProjectile(){
        GenericProjectile pro = Instantiate(gfa.projectileTemplate);
        Rigidbody rb= pro.GetComponent<Rigidbody>();
        rb.velocity= new Vector3(0,0,gfa.velocity) ;
    }
}

public class MultiFireComp : FireComp
{
    public override void fire( )
    {
        throw new NotImplementedException();
    }
}