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
    private float viewDistance;
    //Type 1 = Melee; Type 2 = Medium Shooter; Type 3 = Far Shooter
    public int typeEnemy;

    [SerializeField]
    GameObject projectile;
    [SerializeField]
    SpriteRenderer GunSprite;
    [SerializeField]
    SpriteRenderer BodySprite;
    [SerializeField]
    customMuzzleAnim muzzle;
    private float shootInterval;

        
    [Header("Audio")]
    [SerializeField]
    List<AudioClip> clips;
    [SerializeField]
    AudioSource sors;
    [SerializeField]
    GameObject caseThing;

    int shit = 1;

    private float shootTimer;
    
    [SerializeField] Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        //there is always one player script on the scene, so it would be faster to just return the first player that appears
        player = FindObjectOfType<Player>().GameObject();
    }


    // Update is called once per frame
    void Update()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);
        Vector3 direction = player.transform.position - transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        shootInterval = Random.Range(3, 5);
        // SlashTimer();

        switch (typeEnemy)
        {
            case 1:
                viewDistance = 20;
                break;
            case 2:
                viewDistance = 30;
                break;
            case 3:
                viewDistance = 40;
                break;
        }

        if (distance < viewDistance)
        {
            animator.Play("enemyWalk");
            transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
            GunSprite.transform.rotation = Quaternion.LookRotation(transform.forward, direction);

            //set z priority
            // if(direction.y >.8 || direction.x <.05){
            //     GunSprite.sortingOrder = BodySprite.sortingOrder-1;
            // }else{
            //     GunSprite.sortingOrder = BodySprite.sortingOrder+1;
            // }

            Vector3 rotVec = GunSprite.gameObject.transform.right*(direction.x <.1?-1:1);
            GunSprite.gameObject.transform.localPosition = 
                rotVec*(.5f*(1-Mathf.Abs(direction.y)))+
                Vector3.up*((1.2f-Mathf.Abs(direction.x))*(direction.y <0? -1: 0));
            //flip the gun
            GunSprite.flipX = direction.x < 0;



            if (typeEnemy != 1)
            {
                if (shootTimer <= 0)
                {
                    Shoot(direction);
                    shootTimer = shootInterval;
                }
            }
            // else if (typeEnemy == 1)
            // {
            //     if (distance <= 5)
            //     {
            //         slashTime-=Time.deltaTime;
            //         Slash();
            //         slashTime=1;
            //     }
            // }
            shootTimer-= Time.deltaTime;
        }
        else{
            animator.Play("enIdle");
        }
    //    if (distance <= 1)
    //    {
    //        // GameOver();
    //    }
    }

    public void DeadTime()
    {
        
        GameManager.Instance.enemyCount-=shit;
        shit=0;
        Destroy(muzzle.gameObject);
        GetComponent<BoxCollider2D>().enabled = false;
        // GetComponent<SpriteRenderer>().sprite = dedSprite;
        // GetComponent<SpriteRenderer>().color = dedColor;
        sors.PlayOneShot(clips[Random.Range(1,3)]);
        animator.Play("ded");
        Destroy(this);
    }
    private void Shoot(Vector3 direction)
    {
        Instantiate(projectile, muzzle.transform.position, Quaternion.LookRotation(transform.forward, direction));

        StartCoroutine(muzzle.Fire());
        sors.PlayOneShot(clips[0]);
        //spawn a casing
        var a = Instantiate(caseThing, 
            GunSprite.gameObject.transform.position, 
             Quaternion.LookRotation(transform.forward,
        GunSprite.gameObject.transform.right*(direction.x <.1?1:-1))* Quaternion.Euler(0, 0, Random.Range(0, 30)*(direction.x < .1 ? -1 : 1)));


    }

    // private void Slash()
    // {
    //     if(!isSlashing){
    //         projectile.SetActive(true);
    //         isSlashing=true;
    //     }
        
    // }

    // private void SlashTimer(){
    //     if (isSlashing){
    //         slashTimer+=Time.deltaTime;
    //         if (slashTimer > 1){
    //             slashTimer=0;
    //             isSlashing=false;
    //             projectile.SetActive(false);
    //         }
    //     }
    // }

}
