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

    void Start(){
        at=GetComponent<AITemplate>();
        weapons = GetComponentsInChildren<WeaponTemplate>();
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
}