using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class WindWarrior : MonoBehaviour
{
    public GameObject pointA;
    public GameObject pointB;
    public Rigidbody2D player;
    public Rigidbody2D rb;
    private Animator anim;
    private Transform currentPoint;
    private float waitingTime=1f;
    private float speed=2f;
    private bool isCombat = false;

    void Start()
    {
        anim =GameObject.Find("Wind warrior/Sprite").GetComponent<Animator>();
        currentPoint= pointB.transform;
        anim.SetBool("isRunning",true);
    }


    void Update()
    {
        if (!isCombat){
                if (currentPoint == pointB.transform){
                    rb.velocity = new Vector2(speed,0); 
                }
                if (currentPoint == pointA.transform){
                    rb.velocity = new Vector2(-speed,0);
                }
            
            if (pointB.transform.position.x-rb.position.x<0.1f && currentPoint == pointB.transform && speed!=0){
                print("abc");
                StartCoroutine(StopMoving(pointA.transform));
            }
            if (pointA.transform.position.x-rb.position.x>0.1f && currentPoint == pointA.transform && speed!=0){
                print("abc");
                StartCoroutine(StopMoving(pointB.transform));
            }
        }
        else{

        }
    }
    private void Flip(){
        Vector2 localscale = transform.localScale;
        localscale.x *=-1;
        transform.localScale = localscale;
    }
    private IEnumerator StopMoving(Transform targetPos){
        speed = 0f;
        anim.SetBool("isRunning",false);
        yield return new WaitForSeconds(waitingTime);
        Flip();
        anim.SetBool("isRunning",true);
        speed=2f;
        currentPoint = targetPos;

    }
}
