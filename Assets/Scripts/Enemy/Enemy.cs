using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;
    public float damage;
    public GameObject bossHealthBar;
    public Image healthBar;
    public Text bossname;
    public bool isCombat = false;



    public Animator anim;
    void Start()
    {
        
    }

    public void TakeDamage(float damage){
        currentHealth -= damage;
        anim.SetTrigger("TakeDamage");
        print(currentHealth);
        if (currentHealth <= 0){
            //die
            isCombat = false;
            anim.SetBool("isDead",true);
            print("dead");
            Invoke("Destroy",2f);

        }
    }
    void Destroy(){
        Destroy(gameObject);
    }
}
