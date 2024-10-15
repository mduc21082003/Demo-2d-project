using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class WindWarrior : Enemy
{
    public GameObject pointA;
    public GameObject pointB;
    public Rigidbody2D player;
    public Rigidbody2D rb;

    private Transform currentPoint;
    private float waitingTime=1f;
    private float speed=4.5f;
    private bool isCombat = false;
    private bool isFacingRight =true;


    //combat
    private int combatState = 1;
    private int combatFury = 0;

    private float teleportCooldown = 0f;
    private float attackCooldown = 0f;
    private float ATK1Cooldown = 0f;
    private float ATK2Cooldown = 0f;
    private float ATK3Cooldown = 0f;
    private float attackCD = 3f;
    private float teleportCD = 14f;
    private float ATK1CD =3f;
    private float ATK2CD =8f;
    private float ATK3CD = 16f;


    void Start()
    {
        currentHealth = maxHealth ;
        anim =GetComponent<Animator>();
        currentPoint= pointB.transform;
        anim.SetBool("isRunning",true);
    }
    

    void Update()
    {
        combatState = Random.Range(1,3);
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
        if (isCombat==false){
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

            if (Vector2.Distance(rb.position,player.position)<2f && attackCooldown<=0f){
                attackCooldown=attackCD;
                if(combatFury ==8){
                    combatFury = 0;
                    SpecialAttack();
                }
                else if (combatFury!=20){
                    Attack();
                }
            }

            if (Vector2.Distance(rb.position,player.position)<6f && Vector2.Distance(rb.position,player.position) > 2f){
                if (teleportCooldown <= 0f ){
                    anim.SetTrigger("Teleport");
                    teleportCooldown = teleportCD;
                }
                else{
                    Chase();
                }
            }

            if (Vector2.Distance(rb.position,player.position)<12f && Vector2.Distance(rb.position,player.position)>6f){
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
    public void TelePort(){
        rb.position = new Vector2(player.position.x-1f,player.position.y);
    }
    public void TelePort2(){
        rb.position = new Vector2(player.position.x+1f,player.position.y);
    }
    public void Attack(){
        combatState = Random.Range(1,3);
        
        if (combatState==1 && ATK1Cooldown <=0f){
            print("ATK1");
            ATK1Cooldown = ATK1CD;
            combatFury ++;
            return;
        }
        if (ATK1Cooldown >0){
            combatState =2;
        }
        if (combatState==2 && ATK2Cooldown <=0f){
            print("ATK2");
            ATK2Cooldown = ATK2CD;
            combatFury ++;
            return;
        }
        if (ATK2Cooldown >0){
            combatState =3;
        }
        if (combatState==3 && ATK3Cooldown <=0f){
            print("ATK3");
            ATK3Cooldown = ATK3CD;
            combatFury ++;
            return;
        }
        
    }
    public void ATK1(){

    }
    public void AirATK(){

    }
    public void ATK3(){

    }
    public void SpecialAttack(){
        print("SPATK");
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
        if(attackCooldown>0){
            attackCooldown -=Time.deltaTime;
        }
        if (ATK1Cooldown>0){
            ATK1Cooldown -= Time.deltaTime;
        }
        if (ATK2Cooldown>0){
            ATK2Cooldown -= Time.deltaTime;
        }
        if (ATK3Cooldown>0){
            ATK3Cooldown -= Time.deltaTime;
        }
    }
}
