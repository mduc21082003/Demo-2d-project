using System.Collections;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private float speed=8f;
    private float jump=500f;
    private float move ;
    private bool isJumping;
    private bool isFacingRight = true;
    [SerializeField]private Rigidbody2D rb;
    private SpriteRenderer rend;
    public Animator anim;
    public float attackRate = 2f;
    private float nextAttackTime = 0f;
    public float dashingPower = 40f;
    private float dashingTime = 0.3f;

    public float dashingCooldown = 1f;
    private bool isDashing=false;
    private bool canDash = true;
    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDashing){
            return;
        }
        Move();
        Flip();
        Jump();
        if (Time.time >= nextAttackTime && Input.GetKeyDown(KeyCode.J)){
            Strike();
            nextAttackTime = Time.time + 1f/ attackRate;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && rb.velocity.y==0){
            StartCoroutine(Dash());
        }
        anim.SetFloat("Y_velocity",rb.velocity.y);
    }


    private void OnCollisionExit2D(Collision2D col){
        if (col.gameObject.name=="Ground"){
            isJumping=true;
        }
    }
    private void OnCollisionEnter2D(Collision2D col){
        if (col.gameObject.name=="Ground"){
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
            anim.SetTrigger("Strike");
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
}
