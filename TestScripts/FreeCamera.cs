using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 
/// </summary>
[RequireComponent(typeof(Camera))]
public class FreeCamera : MonoBehaviour
{
    [Range(0,5)]
    public float speed=1.0f;
    [Range(0,5)]
    public float distance=2.0f; //distance from camera to the target
    public Transform target;

    public Vector3 pos;
    private Quaternion rot;

    public InputAxis X;
    public InputAxis Y;
    void Start(){
        pos= target.position;
        transform.rotation=Quaternion.identity;

    }

    void Update(){
        float dx= Input.GetAxis(X.ToString());
        float dy= Input.GetAxis(Y.ToString());

        if (Input.GetMouseButton(1))
        {
            Vector3 angles= transform.rotation.eulerAngles;

            angles.x=Mathf.Repeat(angles.x+180f,360f)-180f;
            angles.y+=dx; //这里x轴Y轴的旋转 正好需要和XY反过来
            angles.x+=-dy;//这里方向也需要反一下
            angles.z=0;
            rot.eulerAngles= angles;
            transform.rotation=rot;

            //update position
            transform.position= pos -rot* new Vector3(0,0,distance);

            //update position method 2
            // transform.position = pos - transform.forward * distance;
        }
    }
}