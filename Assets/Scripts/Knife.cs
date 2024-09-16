using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Knife : MonoBehaviour


{
    
    [SerializeField] Sprite attackSprite;
    [SerializeField] Color attackColor;
    public SpriteRenderer KnifeSprite;
    public BoxCollider2D KnifeCollider;
    public float slashTime;

    void Awake(){
        SpriteRenderer KnifeSprite=GetComponent<SpriteRenderer>();
        BoxCollider2D KnifeCollider=GetComponent<BoxCollider2D>();
        slashTime=1;
    }
    // Start is called before the first frame update

    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
        // if(slashTime <=0f){
        //     Attack();
        //     slashTime=1;
        // }
        // else{
        //     slashTime-=Time.deltaTime;
        // }
        
    }

    public void Attack(){
        GetComponent<SpriteRenderer>().sprite = attackSprite;
        GetComponent<SpriteRenderer>().color= attackColor;
        KnifeCollider.size+= new Vector2(4,3);
    }
    public void DisAttack(){
        GetComponent<SpriteRenderer>().flipX=true;
        KnifeCollider.size-= new Vector2(4,3);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            //only the player has this tag so we can check if its null but eh
            Player plyscrt = other.GetComponentInParent<Player>();
            plyscrt.TakeDmg();
        }

    }
    public void Dissapear(){
        Destroy(this);
    }
}
