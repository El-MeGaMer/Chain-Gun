using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] public Vector2 direction;
    [SerializeField] float speed;
    [SerializeField] int team;


    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject,10);
    }

    // Update is called once per frameffffffffffffffffd
    void FixedUpdate()
    {
        transform.Translate(Vector2.up*speed*Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //if its a player projectile
        if(team == 0){
            Enemy enscrt = other.GetComponent<Enemy>();
            if(enscrt){//we check if its not null, then we kill it
                enscrt.DeadTime();
                Destroy(this.gameObject, .05f);
            }else if(other.tag != "Player"){
                Destroy(this.gameObject, .05f);
            }
        }else if(other.tag == "Player"){
            //only the player has this tag so we can check if its null but eh
            Player plyscrt = other.GetComponent<Player>();
            plyscrt.TakeDmg();
            Destroy(this.gameObject, .05f);
        }
    }
}
