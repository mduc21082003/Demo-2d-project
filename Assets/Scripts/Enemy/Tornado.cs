using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tornado : MonoBehaviour
{
    public GameObject tornado;
    public Transform player;
    public Transform wind;
    void Start(){
        Physics2D.IgnoreLayerCollision(7,2,true);
        Physics2D.IgnoreLayerCollision(8,2,true);
    }
    void Update()
    {
        TornadoMove();
    }
    void TornadoMove(){
        if (wind.position.x > player.position.x){
            tornado.transform.position = Vector2.Lerp(tornado.transform.position,new Vector2(tornado.transform.position.x-9f,tornado.transform.position.y),0.4f*Time.deltaTime);
        }
        if (wind.position.x < player.position.x){
            tornado.transform.position = Vector2.Lerp(tornado.transform.position,new Vector2(tornado.transform.position.x+9f,tornado.transform.position.y),0.4f*Time.deltaTime);
        }
    }
    
}
