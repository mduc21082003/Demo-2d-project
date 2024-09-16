using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float speed=8f;
    private float jump=500f;
    private float move ;
    private bool isJumping;
    private bool isFacingRight=true;
    [SerializeField]private Rigidbody2D rb;
    private SpriteRenderer rend;
    public Animator anim;
    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Flip();
        Jump();
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
}
