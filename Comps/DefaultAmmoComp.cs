using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(GenericFireArm))]
public class DefaultAmmoComp: MonoBehaviour{
    public GenericFireArm gfa;
    
    [Header("Mag settings")]
    public int magSize=5;
    public int nrOfMags=1; //if -1 means infinity
    public int reloadAmount=0;

    [Header("Reload settings")]
    public float reloadTime=1;
    public bool autoReload=false;

    public int curBulletsInWeapon =0;
    public int curNrOfMags;
    public int remianBullets;

    void Start(){
        gfa=GetComponent<GenericFireArm>();
        gfa.ammo=this;
        //init with a full mag
        curBulletsInWeapon = magSize;
        remianBullets = magSize * nrOfMags;
        curNrOfMags = nrOfMags;
        if(reloadAmount < 1){
            reloadAmount= magSize;
        }

        waitTime=new WaitForSeconds(reloadTime);
    }

    void Update(){
        // if (curBulletsInWeapon == 0){
        //     if ((autoReload || Input.GetKeyDown(KeyCode.R))&& (!gfa.isReloading) &&  Mathf.Abs(curNrOfMags) > 0){
                
        //     }
        // } 
        if ((Input.GetKeyDown(KeyCode.R))&& (!gfa.isReloading) &&  Mathf.Abs(curNrOfMags) > 0){
            StartCoroutine(reload());
        }
    }
    WaitForSeconds waitTime;
    public IEnumerator reload(){
        //no ammo remain 
        // if( Mathf.Abs(curNrOfMags) <= 0 ) yield break;
        //already checked outside 
        Debug.Assert(Mathf.Abs(curNrOfMags) > 0);

        gfa.isReloading=true;
        yield return waitTime;

        if (curBulletsInWeapon + reloadAmount <= magSize) //can't have one more in the gun
        {
            curBulletsInWeapon += reloadAmount;
        }else{
            if(curBulletsInWeapon > 0){
            curBulletsInWeapon = reloadAmount+1;
            
            }else{
                curBulletsInWeapon = reloadAmount;
            }
        }
        curNrOfMags--;
        gfa.isReloading=false;
    }
}
