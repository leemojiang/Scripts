using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PCO :MonoBehaviour{

    public bool isAI=false;
    public bool isPlayer=false;
    public  virtual void ProcessInput(){

    }

}


public interface IPCO{
    // public bool isAI;
}