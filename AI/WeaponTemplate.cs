using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponTemplate:MonoBehaviour{

    public float minRange,maxRange;

    public float allowedDeviation; //when it's in this cone AI will fire

    [Range(0,1)]
    public float optimalRangePercentage;

    public WeaponType type;

    
}