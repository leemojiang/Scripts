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

    // public bool isEmpty {
    //     set{
    //         if(value){
    //             isPlayer=false;
    //             isEmpty=true;
    //             isAI=false;
    //         }else{
    //             isEmpty=false;
    //         }
    //     }
    //     get{
    //         return isAI || isEmpty;
    //     }
    // }
    // public bool isPlayer{
    //     set{
    //         if(value){
    //             isPlayer=true;
    //             isEmpty=false;
    //             isAI=false;
    //         }else{
    //             isPlayer=false;
    //         }
    //         setControl();
    //     }
    //     get{
    //         return isPlayer;
    //     }
    // }
    // public bool isAI{
    //     set{
    //         if(value){
    //             isPlayer=false;
    //             isEmpty=false;
    //             isAI=true;
    //         }else{
    //             isAI=true;
                
    //         }
    //         setControl();
    //     }  
    //     get{
    //         return isAI;
    //     }
    // }

    public bool isAI,isPlayer,isEmpty;

    public PCO[] PCOs;
    // public PlayerControlObject[] PlayerControlObjs;
    void Start(){
        
        if(!TryGetComponent<AIAgent>(out agent)) agent= gameObject.AddComponent<AIAgent>();
        TryGetComponent<AITemplate>(out aiTemplate);

        //top most PCO
        if(hasMobilePhysics){
            if(!TryGetComponent<Rigidbody>(out rb)) rb=gameObject.AddComponent<Rigidbody>();
            rb.mass=mass;
            rb.drag=drag;
        }
        
        PCOs= GetComponentsInChildren<PCO>(); //will get all PCO's including 
        // PlayerControlObjs=GetComponentsInChildren<PlayerControlObject>();

        getSelfPCOs();
    }

    [ContextMenu("set")]
   /// <summary>
   /// 
   /// </summary>
   /// <param name="flag">For ai</param>
   /// <param name="flag2">For </param>
    public void setControl(){
        agent.isAIControl=isAI;
        agent.setAIControl(isAI);
        
        for (int i = 0; i < PCOs.Length; i++)
        {
            PCOs[i].isAI=isAI;
            PCOs[i].isPlayer=isPlayer;
        }

        // for (int i = 1; i < PlayerControlObjs.Length; i++)
        // {
        //     PlayerControlObjs[i].setControl();
        // }
        
    }

    public void getSelfPCOs(){
        List<PCO> pco= new List<PCO>();
        for (int i = 0; i < PCOs.Length; i++)
        {
            if (this == PCOs[i].GetComponentInParent<PlayerControlObject>()){
                pco.Add(PCOs[i]);
            };
        }

        PCOs=pco.ToArray();
    }

}