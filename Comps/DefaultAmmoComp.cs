using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class DefaultAmmoComp: MonoBehaviour{
    public GenericFireArm gfa;
    
    [Header("Mag settings")]
    public int magSize=5;
    public int nrOfMags=1;
    public int reloadAmount=magSize;

    [Header("Reload settings")]
    public float reloadTime=1;
    public bool autoReload=false;

    public int curBulletsInWeapon =0;
    public int remianBullets;

    void Start(){
        gfa=GetComponent<GenericFireArm>();
    
        //init with a full mag
        curBulletsInWeapon = magSize;
        remianBullets = magSize * nrOfMags;
    }


    void reload(){
        
    }
}
