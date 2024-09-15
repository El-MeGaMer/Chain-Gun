using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEditor.Rendering.LookDev;
using UnityEngine.InputSystem.Android;

public class Player : MonoBehaviour
{

    [Header("Stats")]
    [SerializeField]
    private int health = 3;
    private bool inv = false;
    private bool invDmg = false;
    private bool dead = false;

    [Header("Movement")]
    [SerializeField]
    public float speed;
    [SerializeField]
    public float coeficient;
    [SerializeField]
    public float maxSpeed;
    [SerializeField]
    public float dashMulti;

    [Header("Inputs")]
    [SerializeField]
    private Vector2 inertia;
    Vector2 direction = Vector2.zero;
    Vector3 lookDir;
    public InputAction move;
    public InputAction clickInput;
    public InputAction slide;
    private bool isSlidin;



    [Header("Camera")]
    [SerializeField]
    Camera playerCam;
    [SerializeField]
    Vector3 mouseVec;
    [SerializeField]
    Vector3 camBasePos;
    [SerializeField]
    float camSpeed;
    [SerializeField]
    float maxCamDistance;
    [Header("Visuals")]
    [SerializeField]
    SpriteRenderer GunSprite;
    [SerializeField]
    SpriteRenderer BodySprite;
    [SerializeField]
    GameObject projectile;
    [SerializeField]
    Animator animContr;
    [SerializeField]
    customMuzzleAnim muzzle;
    [SerializeField]
    Animator hitvfx;

    private void OnEnable(){
        move.Enable();
        slide.Enable();
        clickInput.Enable();
    }
    private void OnDisable(){
        move.Disable();
        slide.Disable();
        clickInput.Disable();
    }

    // Start is called before the first frame update
    void Awake(){
        clickInput.performed += clickTime;
        slide.started += slideTime;
        slide.canceled += slideTimeEnd;
    }

    // Update is called once per frame
    void Update(){
        direction = move.ReadValue<Vector2>();
        mouseVec = playerCam.ScreenToWorldPoint(Input.mousePosition);
        
    }


    void FixedUpdate()
    {
        if(!dead){
            MoveTime();
            MouseTime();
        }
    }

    private void clickTime(InputAction.CallbackContext context){
        Instantiate(projectile, muzzle.transform.position, GunSprite.transform.rotation);
        StartCoroutine(muzzle.Fire());
    }
    private void slideTime(InputAction.CallbackContext context){
        isSlidin = true;
    }
    private void slideTimeEnd(InputAction.CallbackContext context){
        isSlidin = false;
    }

    private void MouseTime() {
        lookDir = new Vector3(mouseVec.x-transform.position.x, mouseVec.y-transform.position.y, 0); 
        float distnace = lookDir.magnitude;
        lookDir = lookDir.normalized;
        Vector3 newCamPos = camBasePos+Vector3.ClampMagnitude(lookDir*distnace, maxCamDistance);
        playerCam.transform.localPosition = Vector3.Lerp(camBasePos, newCamPos, camSpeed*Time.fixedDeltaTime);

        GunSprite.transform.rotation = Quaternion.LookRotation(transform.forward, lookDir);

        //set z priority
        if(lookDir.y >.8 || lookDir.x <.05){
            GunSprite.sortingOrder = BodySprite.sortingOrder-1;
        }else{
            GunSprite.sortingOrder = BodySprite.sortingOrder+1;
        }

        Vector3 rotVec = GunSprite.gameObject.transform.right*(lookDir.x <.1?-1:1);
        GunSprite.gameObject.transform.localPosition = 
            rotVec*(.5f*(1-Mathf.Abs(lookDir.y)))+
            Vector3.up*((1.2f-Mathf.Abs(lookDir.x))*(lookDir.y <0? -1: 0));
        //flip the gun
        GunSprite.flipX = lookDir.x < .1;
        if(!isSlidin)
            BodySprite.flipX = lookDir.x < .1;
    }

    private void MoveTime() {
        Vector2 movement= !isSlidin ? new Vector2(direction.x,direction.y) : Vector2.zero;
        float trueSpeed = speed;
        if(Mathf.Abs(inertia.magnitude) <= 3){
            trueSpeed*=dashMulti;
        }
        //we round speed if its greater than maxspeed
        inertia = Vector3.ClampMagnitude(inertia+movement*trueSpeed, maxSpeed);

        Vector2 friction = inertia.normalized*-1*coeficient;
        if(!isSlidin) {
            inertia +=friction;
        }
        //if friction would give a negative, instead we round to zero
        if(Mathf.Abs(inertia.magnitude) <= 3) {
            inertia= Vector2.zero;
        }
        inv = (Mathf.Abs(inertia.magnitude) != 0 && isSlidin ) || invDmg;
        transform.Translate(inertia*Time.fixedDeltaTime);

        if(isSlidin){
            Vector2 dir = inertia.normalized;
            bool up = Mathf.Abs(dir.y) > .8;
            bool right = Mathf.Abs(dir.x) > .8;
            bool mid = !(up || right);
            animContr.Play(!(right || up) ? "slideD" : up? "slideU" : "slideR");
            BodySprite.flipX = dir.x < 0;
            BodySprite.flipY = dir.y < 0;
            // print("up: "+up+" | r: "+right+" | mid: "+mid);
        }else{
            BodySprite.flipY = false;
            animContr.Play(Mathf.Abs(inertia.magnitude) >= .1 ? "playerWalk" : "playerIdle");
        }
    }

    //damage
    public void TakeDmg() {
        if(inv){
            return;
        }
        hitvfx.Play("hitAnim");
        health--;
        if(health <= 0){
            dead = true;
            move.Disable();
            slide.Disable();
            clickInput.Disable();
            animContr.Play("playerHit");
            StartCoroutine(GameOver());
        }else{
            invDmg = true;
            inv = true;
            StartCoroutine(invManager());
        }
    }

    private IEnumerator invManager(){
        Time.timeScale = 0;
        animContr.Play("playerHit");
        yield return new WaitForSecondsRealtime(.2f);
        Time.timeScale = 1;

        yield return new WaitForSeconds(1);
        invDmg = false;
    }

    private IEnumerator GameOver(){
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(.5f);
        Time.timeScale = 1;

        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(3);
    }
}
