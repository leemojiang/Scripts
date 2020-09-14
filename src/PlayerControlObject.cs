using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// PCO in bf2 
///
/// </summary>
public class PlayerControlObject:MonoBehaviour{
    [Header("BF2 settings")]
    public float mass;
    public float drag;
    public bool hasMobilePhysics=true ; //does it have a RB which means moving alone
    public AITemplate aiTemplate;
    public VehicleType setVehicleType;
    
    //not consider part
    // public float gravityModifier;
    //cockpitSubGeom
    //collisionMesh
    //setNetworkableInfo

    private AIAgent agent;
    Rigidbody rb;

    public bool isEmpty {
        set{
            if(value){
                isPlayer=false;
                isEmpty=true;
                isAI=false;
            }else{
                isEmpty=false;
            }
        }
    }
    public bool isPlayer{
        set{
            if(value){
                isPlayer=true;
                isEmpty=false;
                isAI=false;
            }else{
                isPlayer=false;
            }
        }
    }
    public bool isAI{
        set{
            if(value){
                isPlayer=false;
                isEmpty=false;
                isAI=true;
            }else{
                isAI=true;
            }
        }
    }
    void Start(){
        
        if(!TryGetComponent<AIAgent>(out agent)) agent= gameObject.AddComponent<AIAgent>();
        TryGetComponent<AITemplate>(out aiTemplate);

        //top most PCO
        if(hasMobilePhysics){
            if(!TryGetComponent<Rigidbody>(out rb)) rb=gameObject.AddComponent<Rigidbody>();
        }
        

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="flag">true for given the controll to AI</param>
    void setAIControl(bool flag){
        agent.isAIControl=flag;
        agent.setAIControl(flag);
    }
}