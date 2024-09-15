using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEditor.Rendering.LookDev;

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
    SpriteRenderer BodySprite;
    [SerializeField]
    GameObject projectile;

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
        Instantiate(projectile, transform.position+(lookDir*2), BodySprite.transform.rotation);
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

        // print(transform.right);
        // print(Vector3.right);
        BodySprite.transform.rotation = Quaternion.LookRotation(transform.forward, lookDir);
        //to lerp the gun
        // transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(body.velocity), rotAlpha);
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
    }

    //damage
    public void TakeDmg() {
        if(inv){
            return;
        }
        health--;
        if(health <= 0){
            dead = true;
            move.Disable();
            slide.Disable();
            clickInput.Disable();
            StartCoroutine(GameOver());
        }else{
            invDmg = true;
            inv = true;
            StartCoroutine(invManager());
        }
    }

    private IEnumerator invManager(){
        yield return new WaitForSeconds(1);
        invDmg = false;
    }

    private IEnumerator GameOver(){
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(3);
    }
}
