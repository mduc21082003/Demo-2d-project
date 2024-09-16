using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    private float Followspeed = 2f;
    private float yOffset = 2f;
    public Transform target;


    void Update()
    {
        Vector2 newpos =  new Vector2(target.position.x,target.position.y+yOffset);
        transform.position =  Vector3.Slerp(transform.position,newpos,Followspeed*Time.deltaTime);
    }
}
