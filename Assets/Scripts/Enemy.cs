using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{

    public GameObject player;

    public Transform gunPosition;

    public float speed;

    private float distance;
    private float viewDistance;
    //Type 1 = Melee; Type 2 = Medium Shooter; Type 3 = Far Shooter
    public int typeEnemy;

    [SerializeField]
    GameObject projectile;
    private float shootInterval;

    private float shootTimer;

    [SerializeField] Sprite dedSprite;
    [SerializeField] Color dedColor;

    // Start is called before the first frame update
    void Start()
    {
        //there is always one player script on the scene, so it would be faster to just return the first player that appears
        player = FindObjectOfType<Player>().GameObject();
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);
        Vector3 direction = player.transform.position - transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x)*Mathf.Rad2Deg;
        shootInterval = Random.Range(3, 5);

        switch(typeEnemy){
            case 1:
            viewDistance=20;
            break;
            case 2:
            viewDistance=30;
            break;
            case 3:
            viewDistance=40;
            break;
        }

        if (distance < viewDistance){
            transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(Vector3.forward * angle);
            if (typeEnemy != 1)
            {
                if (shootTimer <= 0)
                {
                    Shoot(direction);
                    shootTimer = shootInterval;
                }
                else
                {
                    shootTimer -= Time.deltaTime;
                }
            }
        }
        if (distance <= 1)
        {
            // GameOver();
        }
    }

    public void DeadTime()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().sprite = dedSprite;
        GetComponent<SpriteRenderer>().color = dedColor;
        Destroy(this);
    }

    void GameOver()
    {
        SceneManager.LoadScene(3);
    }

    private void Shoot(Vector3 direction)
    {
        Instantiate(projectile, (transform.position+(direction*2)),Quaternion.LookRotation(transform.forward, direction));
    }

}
