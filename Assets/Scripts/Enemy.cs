using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{

    public GameObject player;

    public float speed;

    private float distance;

    [SerializeField] Sprite dedSprite;
    [SerializeField] Color dedColor;

    // Start is called before the first frame update
    void Start()
    {
        //there is always one player script on the scene, so it would be faster to just return the first player that appears
       player=FindObjectOfType<Player>().GameObject();
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector2.Distance(transform.position,player.transform.position);
        Vector2 direction=player.transform.position - transform.position;
        direction.Normalize();
        float angle=Mathf.Atan2(direction.y,direction.x);
        
        if (distance < 20){
        transform.position=Vector2.MoveTowards(this.transform.position,player.transform.position,speed*Time.deltaTime);
        transform.rotation=Quaternion.Euler(Vector3.forward*angle); 
        }
        if (distance <=1){
            // GameOver();
        }
    }

    public void DeadTime(){
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().sprite = dedSprite;
        GetComponent<SpriteRenderer>().color = dedColor;
        Destroy(this);
    }

    void GameOver(){
        SceneManager.LoadScene(3);
    }

}
