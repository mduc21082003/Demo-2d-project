using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;
    public Animator anim;
    void Start()
    {
        
    }

    public void TakeDamage(int damage){
        currentHealth -= damage;
        anim.SetTrigger("TakeDamage");
        print(currentHealth);
        if (currentHealth <= 0){
            //die
            anim.SetBool("isDead",true);
            print("dead");
            Invoke("Destroy",2f);
        }
    }
    void Destroy(){
        Destroy(gameObject);
    }
}
