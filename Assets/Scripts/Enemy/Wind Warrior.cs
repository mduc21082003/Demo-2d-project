using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class WindWarrior : Enemy
{
    public GameObject pointA;
    public GameObject pointB;
    public Rigidbody2D player;
    public Rigidbody2D rb;

    private Transform currentPoint;
    private float waitingTime=1f;
    private float speed=4.5f;
    
    private bool isFacingRight =true;

    //ui
    




    //combat
    public Transform attackPoint;
    public float attackRange = 0.9f;
    public LayerMask enemyLayers;
    public GameObject tornado;

    private int combatState = 1;
    [SerializeField]private int combatFury = 0;



    private float rollCooldown = 0f;
    private float teleportCooldown = 0f;
    [SerializeField]private float attackCooldown = 0f;
    private float ATK1Cooldown = 0f;
    private float AirATKCooldown = 0f;
    private float ATK3Cooldown = 0f;


    private float attackCD = 1f;
    private float teleportCD = 8f;
    private float ATK1CD =2f;
    private float AirATKCD =4f;
    private float ATK3CD = 10f;


    void Start()
    {
        bossHealthBar.SetActive(false);
        bossname.text = "The Wind"; 
        currentHealth = maxHealth ;
        anim =GetComponent<Animator>();
        currentPoint= pointB.transform;
    }
    

    void Update()
    {
        if (currentHealth <= 0){
            anim.SetBool("isDead",true);
            print("dead");
            Invoke("Destroy",2f);

        }
        if (isCombat==false){
            bossHealthBar.SetActive(false);
        }
        healthBar.fillAmount = currentHealth/maxHealth;
        SetCooldown();
        if(rb.velocity.x < 0 && isFacingRight == true){
            Flip();
        }
        if(rb.velocity.x > 0 && isFacingRight == false){
            Flip();
        }
        //combat check
        if (Vector2.Distance(player.position,rb.position) < 7f && currentHealth > 0){
            isCombat=true;
            bossHealthBar.SetActive(true);
            anim.SetBool("isRunning",false);
        }
        if (Vector2.Distance(player.position,rb.position) > 14f && isCombat == true){
            isCombat=false;
            currentHealth=maxHealth;
        }
        if (isCombat==false && currentHealth>0){
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
                rb.velocity = new Vector2(0,0);
                if(combatFury >= 6 && attackCooldown <= 0){
                    combatFury = 0;
                    attackCooldown = attackCD*3;
                    anim.SetTrigger("SpecialATK");
                    StartCoroutine(LockMovement(1f));
                }
                if (combatFury < 6 && attackCooldown <= 0){
                    Attack();
                    StartCoroutine(LockMovement(1f));
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
    private IEnumerator LockMovement(float lockTime){
        speed = 0f;
        yield return new WaitForSeconds(lockTime);
        speed = 3f;
    }
    public void TelePort(){
        rb.position = new Vector2(player.position.x-0.85f,player.position.y);
        SetDirection();
    }
    public void TelePort2(){
        rb.position = new Vector2(player.position.x+0.85f,player.position.y);
        SetDirection();
    }
    public void Attack(){
        attackCooldown=attackCD;
        combatState = Random.Range(1,4);
        if (combatState==1 && ATK1Cooldown <=0f){
            anim.SetTrigger("ATK1");
            ATK1Cooldown = ATK1CD;
            combatFury ++;
            return;
        }
        if (ATK1Cooldown >0){
            combatState =2;
        }
        if (combatState==2 && AirATKCooldown <=0f){
            anim.SetTrigger("AirATK");
            AirATKCooldown = AirATKCD;
            combatFury ++;
            return;
        }
        if (AirATKCooldown >0){
            combatState =3;
        }
        if (combatState==3 && ATK3Cooldown <=0f){
            anim.SetTrigger("ATK3");
            attackCooldown = attackCD*2;
            ATK3Cooldown = ATK3CD;
            combatFury ++;
            return;
        }
        if (AirATKCooldown >0){
            combatState =1;
        }
        
    }
    public void LightAttack(){
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position,attackRange,enemyLayers);
        foreach (Collider2D enemy in hitEnemies){
            print(enemy.name);
            enemy.GetComponent<PlayerMovement>().TakeDamage(damage*1);
        }
    }
    public void AirATK(){
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position,attackRange,enemyLayers);
        foreach (Collider2D enemy in hitEnemies){
            print(enemy.name);
            enemy.GetComponent<PlayerMovement>().TakeDamage(damage*0.7f);
        }
    }
    public void QuickAttack(){
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position,attackRange,enemyLayers);
        foreach (Collider2D enemy in hitEnemies){
            print(enemy.name);
            enemy.GetComponent<PlayerMovement>().TakeDamage(damage*0.4f);
        }
    }
    public void SpawnTornado(){
        tornado.SetActive(true);
        tornado.transform.position = new Vector2(rb.position.x,rb.position.y);
        Invoke("DestroyTornado",0.75f);
    }
    
    public void DestroyTornado(){
        tornado.SetActive(false);
    }
    public void SetDirection(){
        if (rb.position.x-player.position.x >0){
            Vector2 localscale = transform.localScale;
            localscale.x = -4;
            transform.localScale = localscale;
        }
        if (rb.position.x-player.position.x <0){
            Vector2 localscale = transform.localScale;
            localscale.x = 4;
            transform.localScale = localscale;
        }
    }
    public void Chase(){
        SetDirection();
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
        if (rollCooldown>0){
            rollCooldown -= Time.deltaTime;
        }
        if(teleportCooldown>0){
            teleportCooldown -= Time.deltaTime;
        }
        if(attackCooldown>0){
            attackCooldown -=Time.deltaTime;
        }
        if (ATK1Cooldown>0){
            ATK1Cooldown -= Time.deltaTime;
        }
        if (AirATKCooldown>0){
            AirATKCooldown -= Time.deltaTime;
        }
        if (ATK3Cooldown>0){
            ATK3Cooldown -= Time.deltaTime;
        }
    }
    void OnDrawGizmosSelected(){
        Gizmos.DrawWireSphere(attackPoint.position,attackRange);
    }
}
