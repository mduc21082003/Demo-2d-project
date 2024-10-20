using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    //playerstats
    public float playerDamage=20;
    public float playerMaxHealth=100;
    [SerializeField]private float currentHealth;
    public bool invulnerable=false;


    //ui
    public Image healthBar;
    public Text potion;

    //movement
    private float speed=8f;
    private float jump=500f;
    private float move ;
    private bool isJumping;
    private bool isFacingRight = true;
    [SerializeField]private Rigidbody2D rb;
    private SpriteRenderer rend;
    public Animator anim;


    //heal 
    private int potionStack=3;
    private float healRate=3f;
    private float nextHealTime=0f;

    //attack
    public float attackRate = 2f;
    private float nextAttackTime = 0f;


    public Transform attackPoint;
    public float attackRange = 0.9f;
    public LayerMask enemyLayers;

    //Dash
    public float dashingPower = 40f;
    private float dashingTime = 0.3f;

    public float dashingCooldown = 1f;
    private bool isDashing=false;
    private bool canDash = true;
    void Start()
    {
        currentHealth = playerMaxHealth;
        Physics2D.IgnoreLayerCollision(8,7,true);
        rend = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        potion.text = potionStack.ToString();
        SetCoolDown();
        healthBar.fillAmount =currentHealth/ playerMaxHealth;
        if (isDashing){
            return;
        }
        Move();
        Flip();
        Jump();
        if (nextHealTime <=0 && Input.GetKeyDown(KeyCode.E) && potionStack>0){
            potionStack -= 1;
            nextHealTime = healRate;
            currentHealth += 35f;
            if (currentHealth>playerMaxHealth){
                currentHealth = playerMaxHealth;
            }
        }

        if (Time.time >= nextAttackTime && Input.GetKeyDown(KeyCode.J)){
            anim.SetTrigger("Strike");
            nextAttackTime = Time.time + 1f/ attackRate;
        }


        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && rb.velocity.y==0){
            StartCoroutine(Dash());
        }
        anim.SetFloat("Y_velocity",rb.velocity.y);
    }


    private void OnCollisionExit2D(Collision2D col){
        if (col.gameObject.CompareTag("Ground")){
            isJumping=true;
        }
    }
    private void OnCollisionEnter2D(Collision2D col){
        if (col.gameObject.CompareTag("Ground")){
            isJumping=false;
        }
    }
    private void Flip(){
        if (rb.velocity.x<0 && isFacingRight == true){
            isFacingRight = false;
            rend.flipX = true;
        }
        if (rb.velocity.x>0 && isFacingRight == false){
            isFacingRight = true;
            rend.flipX = false;
        }
    }
    private void Jump(){
        if (Input.GetButtonDown("Jump") && isJumping ==false)
        {
            rb.AddForce(new Vector2(rb.velocity.x,jump));
            
        }
    }
    private void Move(){
        move = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(speed * move, rb.velocity.y);
        anim.SetFloat("Speed",Mathf.Abs(rb.velocity.x));
    }
    private void Strike(){
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position,attackRange,enemyLayers);
        foreach (Collider2D enemy in hitEnemies){
            print(enemy.name);
            enemy.GetComponent<Enemy>().TakeDamage(playerDamage);
        }
    }
    private IEnumerator Dash(){
        anim.SetTrigger("Dash");
        canDash = false;
        isDashing = true;
        if (isFacingRight){
            rb.velocity = new Vector2(15f*dashingPower, 0f);
        }
        else{
            rb.velocity = new Vector2(-15f*dashingPower, 0f);
        }
        yield return new WaitForSeconds(dashingTime);
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
    void OnDrawGizmosSelected(){
        Gizmos.DrawWireSphere(attackPoint.position,attackRange);
    }
    public void TakeDamage(float damage){
        if (invulnerable==false){
            currentHealth -= damage;
            anim.SetTrigger("Hurt");
            if (currentHealth <= 0){
                //dead
                invulnerable=true;
                anim.SetBool("isDead",true);
            }}
    }
    public void SetCoolDown(){
        if (nextHealTime >0){
            nextHealTime -= Time.deltaTime;
        }
    }
}
