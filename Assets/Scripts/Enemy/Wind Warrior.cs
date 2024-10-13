using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class WindWarrior : Enemy
{
    public GameObject pointA;
    public GameObject pointB;
    public Rigidbody2D player;
    public Rigidbody2D rb;

    private Transform currentPoint;
    private float waitingTime=1f;
    private float speed=3f;
    private bool isCombat = false;
    private bool isFacingRight =true;
    //combat
    private float teleportCooldown = 14f;
    private float normalATKCooldown = 4f;


    void Start()
    {
        currentHealth = maxHealth ;
        anim =GetComponent<Animator>();
        currentPoint= pointB.transform;
        anim.SetBool("isRunning",true);
    }
    

    void Update()
    {
        SetCooldown();
        if(rb.velocity.x < 0 && isFacingRight == true){
            Flip();
        }
        if(rb.velocity.x > 0 && isFacingRight == false){
            Flip();
        }
        //combat check
        if (Vector2.Distance(player.position,rb.position)<7f){
            isCombat=true;
            anim.SetBool("isRunning",false);
        }
        if (Vector2.Distance(player.position,rb.position)>14f && isCombat == true){
            isCombat=false;
            currentHealth=maxHealth;
        }
        if (!isCombat){
                if (currentPoint == pointB.transform){
                    rb.velocity = new Vector2(speed,0); 
                }
                if (currentPoint == pointA.transform){
                    rb.velocity = new Vector2(-speed,0);
                }
            
            if (pointB.transform.position.x-rb.position.x<0.1f && currentPoint == pointB.transform && speed!=0){
                StartCoroutine(StopMoving(pointA.transform));
            }
            if (pointA.transform.position.x-rb.position.x>0.1f && currentPoint == pointA.transform && speed!=0){
                StartCoroutine(StopMoving(pointB.transform));
            }
        }



        //combat 
        else{
            if (Vector2.Distance(rb.position,player.position)<2f){

            }
            if (Vector2.Distance(rb.position,player.position)<6f && Vector2.Distance(rb.position,player.position) > 2f){
                if (teleportCooldown <= 0.001f ){
                    anim.SetTrigger("Teleport");
                    teleportCooldown = 14f;
                }
                else{
                    Chase();
                }
            }
            if (Vector2.Distance(rb.position,player.position)<12f&&Vector2.Distance(rb.position,player.position)>6f){
                Chase();
            }
        }
    }
    private void Flip(){
        isFacingRight = !isFacingRight;
        Vector2 localscale = transform.localScale;
        localscale.x *=-1;
        transform.localScale = localscale;
    }
    private IEnumerator StopMoving(Transform targetPos){
        speed = 0f;
        anim.SetBool("isRunning",false);
        yield return new WaitForSeconds(waitingTime);
        anim.SetBool("isRunning",true);
        speed=3f;
        currentPoint = targetPos;
    }
    void TelePort(){
        rb.position = new Vector2(player.position.x-1f,player.position.y);
    }
    public void Chase(){
        Vector2 direction = new Vector2(rb.position.x-player.position.x,0f);
        anim.SetBool("isRunning",true);
        if (direction.x<0f){
            rb.velocity = new Vector2(speed,0f);
        }
        else{
            rb.velocity = new Vector2(-speed,0f);
        }
    }
    public void SetCooldown(){
        if(teleportCooldown>0){
            teleportCooldown -= Time.deltaTime;
        }
        if (normalATKCooldown>0){
            normalATKCooldown -= Time.deltaTime;
        }
    }
}
