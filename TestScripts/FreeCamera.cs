using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 
/// </summary>
public class FreeCamera : MonoBehaviour
{
    [Range(0,5)]
    public float speed=1.0f;
    [Range(0,5)]
    public float distance=2.0f; //distance from camera to the target
   
    [Range(0,5)]
    public float rotDamp=2.0f;
    [Range(0,50)]
    public float rotSpeed=32.0f;
    public Transform target;

    public Vector3 targetPos;
    private Quaternion rot,targetRot;
    
    public InputAxis X;
    public InputAxis Y;


    void Start(){
        if(target!=null) targetPos= target.position;
        
        transform.rotation=Quaternion.identity;
    }

    void Update(){
        ProcessInput();

        //update rot
        rot = Quaternion.Slerp(rot,targetRot,Time.deltaTime*rotDamp);
        transform.rotation=rot;
        // transform.rotation = Quaternion.Slerp(transform.rotation,targetRot,Time.deltaTime*rotDamp);
        
        //updatetargetPosition
        transform.position=targetPos -rot* new Vector3(0,0,distance);
        
    }

    void ProcessInput(){
        float dx= Input.GetAxis(X.ToString());
        float dy= Input.GetAxis(Y.ToString());
        
        #region  process KeyCode
        if (Input.GetKey(KeyCode.W))
        {
            targetPos += transform.forward * speed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            targetPos -= transform.forward * speed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            targetPos += transform.right * speed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            targetPos -= transform.right * speed;
        }
        if (Input.GetKey(KeyCode.R))
        {
            targetPos += Vector3.up * speed;
        }
        if (Input.GetKey(KeyCode.F))
        {
            targetPos -= Vector3.up * speed;
        }
        #endregion

        if (Input.GetMouseButton(1))
        {   
            dx*=rotSpeed;
            dy*=rotSpeed;

            Vector3 angles= transform.rotation.eulerAngles;

            angles.x=Mathf.Repeat(angles.x+180f,360f)-180f;
            angles.y+=dx; //这里x轴Y轴的旋转 正好需要和XY反过来
            angles.x+=-dy;//这里方向也需要反一下
            angles.z=0;
            targetRot.eulerAngles= angles;

            //updates
            // transform.rotation=rot;

            // //updatetargetPosition
            // transform.targetition= targetPos -rot* new Vector3(0,0,distance);

            //updatetargetPosition method 2
            // transform.position =targetPos - transform.forward * distance;
        }

        // //zoom
        // float zoom= Input.GetAxis("Mouse ScrollWheel");
        // if (zoom!=0){
            
        // }
    }
}