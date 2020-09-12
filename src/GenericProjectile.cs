using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [RequireComponent(typeof(Rigidbody))]
public class GenericProjectile:MonoBehaviour{
    
    
    [Header("Projectile Settings")]
    public float timeToLive=10;
    public float minDamage=5;
    public float damage=10;
    public float distToStartLoseDamage=0;
    public float distToMinDamage=-1;
    [Header("Game Settings")]
    public bool canPickup=false;
    public int collisionGroups; //layer mask
    [Header("Default RB")]
    public float gravityModifier;

    public Vector3 velocity;

    public Vector3 gravity=new Vector3(0,-1f,0);
    public Vector3 acceleration;

    public Rigidbody rb;
    private float curDist; //bullet traveled distance
    void Start(){
        if(canPickup){
            rb=this.gameObject.AddComponent<Rigidbody>();
            rb=GetComponent<Rigidbody>();
            rb.velocity=velocity;
        }  
        Destroy(this.gameObject,timeToLive);

        curDist=0;
    }

    Ray ray = new Ray (); //cache the ray
    RaycastHit hit;
    // public Vector3 debug;
    public void FixedUpdate(){
        
        float deltaTime = Time.fixedDeltaTime;
        ray.origin=transform.position;

        //otherwise use defalut RB
        if(!canPickup){
           
            velocity += gravity * gravityModifier * deltaTime + acceleration* deltaTime;
            transform.position += velocity * deltaTime;     
        }else{
            velocity = rb.velocity;
        }
        
        ray.direction =velocity;

        Debug.DrawRay(ray.origin,velocity * deltaTime,Color.green);
        bool ishit=Physics.Raycast (ray,out hit,velocity.magnitude* deltaTime ,collisionGroups);
        curDist+=velocity.magnitude* deltaTime;

        
        if(ishit){
            transform.position=hit.point;
            // hit.collider.gameObject.SendMessage

            //callculate damange
            float t = (curDist-distToStartLoseDamage)/(distToMinDamage-distToStartLoseDamage);
            float curdamage=Mathf.Lerp(damage,minDamage,t);

            
        }   
        
        
    }
}
