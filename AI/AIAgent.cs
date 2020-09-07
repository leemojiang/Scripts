using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;


[RequireComponent(typeof(AITemplate))]
public class AIAgent:MonoBehaviour
{
    public AITemplate at;
    public WeaponTemplate[] weapons;
    public AISenseTemplate sense;

    void Start(){
        at=GetComponent<AITemplate>();
        weapons = GetComponentsInChildren<WeaponTemplate>();
    }

    AIBehaviour debugBehaviour = new DebugBehaviour();
    void Update(){
        
    }
}