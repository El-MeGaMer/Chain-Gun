using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{

    public GameObject player;

    public float speed;

    private float distance;

    // Start is called before the first frame update
    void Start()
    {
       player=GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector2.Distance(transform.position,player.transform.position);
        Vector2 direction=player.transform.position - transform.position;
        direction.Normalize();
        float angle=Mathf.Atan2(direction.y,direction.x);
        
        if (distance < 10){
        transform.position=Vector2.MoveTowards(this.transform.position,player.transform.position,speed*Time.deltaTime);
        transform.rotation=Quaternion.Euler(Vector3.forward*angle); 
        }
        if (distance <=1){
            GameOver();
        }

        
    }
    void GameOver(){
        SceneManager.LoadScene(3);
    }

}
