using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] public Vector2 direction;
    [SerializeField] float speed;
    [SerializeField] bool ignorePlayer = true;
    [SerializeField] bool autoDie = true;
    [SerializeField] bool makeChain = false;
    [SerializeField] String ignoreTag;


    // Start is called before the first frame update
    void Start()
    {
        if(autoDie)
            Destroy(this.gameObject,10);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(Vector2.up*speed*Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == ignoreTag){
            return;
        }
        //if its a player projectile
        if(other.tag == "Player" && !ignorePlayer){
            //only the player has this tag so we can check if its null but eh
            Player plyscrt = other.GetComponent<Player>();
            plyscrt.TakeDmg();
            if(autoDie)
                Destroy(this.gameObject, .05f);
        }else {
            Enemy enscrt = other.GetComponent<Enemy>();
            if(enscrt){//we check if its not null, then we kill it
                enscrt.DeadTime();
                if(makeChain){
                    chainManager.chainInstance.makeChain(other.transform.position);
                }
                if(autoDie)
                    Destroy(this.gameObject, .05f);
            }else if(other.tag != "Player"){
                if(makeChain){
                    chainManager.chainInstance.makeChain(transform.position);
                }
                if(autoDie)
                    Destroy(this.gameObject, .05f);
            }
        }
    }
}
