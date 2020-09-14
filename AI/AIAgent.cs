using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;


[RequireComponent(typeof(AITemplate))]
public class AIAgent:MonoBehaviour
{   
    public bool isAIControl=false;
    public AITemplate at;
    public WeaponTemplate[] weapons;
    public AISenseTemplate sense;

    public PlayerControlObject playerControlObject;
    void Start(){
        at=GetComponent<AITemplate>();
        
        // weapons=GetComponentsInChildren<WeaponTemplate>();
        getSelfWeapons();
    }

    AIBehaviour debugBehaviour = new DebugBehaviour();
    void Update(){
        if (!isAIControl)return;
    }

    public void setAIControl(bool flag){
        at.isAIControl=flag;
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].isAIControl=flag;
        }
    }

    public void getSelfWeapons(){
        List<WeaponTemplate> wl = new List<WeaponTemplate>();

        for (int i = 0; i < playerControlObject.PCOs.Length; i++)
        {   
            WeaponTemplate ls;
            if(playerControlObject.PCOs[i].TryGetComponent<WeaponTemplate>(out ls)) wl.Add(ls);
        }

        weapons= wl.ToArray();
    }
}