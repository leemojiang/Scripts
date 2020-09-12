using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// For enviorment and veichle collider not for hitbox and Damange
/// </summary>
[RequireComponent(typeof(Collider))]
public class ColliderTemplate:MonoBehaviour{

    public CollisionMaterial material;
    public int collisionGroup; //layer mask

    public GameObject root;
    
    void Start(){

        //the closeted CollisionManager should be the CollisionManager for the obj
        root= GetComponentInParent<CollisionManager>().transform.gameObject; //
        // transform.root;
    }
}


public class VeichlePart:ColliderTemplate{

    [Header("Armor settings")]
    public float armorType; //multipler for armor
    public float thickness;

    [Header("Function")]
    public float hp;
    public ColliderFunctionType type;

}


