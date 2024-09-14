using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{

    [Header("Movement")]
    [SerializeField]
    public float speed;
    [SerializeField]
    public float coeficient;
    [SerializeField]
    public float maxSpeed;
    [SerializeField]
    public float dashMulti;

    Vector2 direction = Vector2.zero;

    public InputAction move;

    [SerializeField]
    private Vector2 inertia;


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

    private void OnEnable(){
        move.Enable();
    }
    private void OnDisable(){
        move.Disable();
    }

    // Start is called before the first frame update
    void Start(){
        

    }

    // Update is called once per frame
    void Update(){
        direction = move.ReadValue<Vector2>();

        mouseVec = playerCam.ScreenToWorldPoint(Input.mousePosition);
        
    }

    void FixedUpdate()
    {
        MoveTime();

        MouseTime();
    }

    private void MouseTime() {
        Vector3 lookDir = new Vector3(mouseVec.x-transform.position.x, mouseVec.y-transform.position.y, 0); 
        float distnace = lookDir.magnitude;
        lookDir = lookDir.normalized;
        Vector3 newCamPos = camBasePos+Vector3.ClampMagnitude(lookDir*distnace, maxCamDistance);
        playerCam.transform.localPosition = Vector3.Lerp(camBasePos, newCamPos, camSpeed*Time.fixedDeltaTime);

        // print(lookDir);
        // print(transform.right);
        // print(Vector3.right);
        BodySprite.transform.rotation = Quaternion.LookRotation(transform.forward, lookDir);
        //to lerp the gun
        // transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(body.velocity), rotAlpha);
    }

    private void MoveTime() {
        Vector2 movement= new Vector2(direction.x,direction.y);
        float trueSpeed = speed;
        if(Mathf.Abs(inertia.magnitude) <= 3){
            trueSpeed*=dashMulti;
        }
        //we round speed if its greater than maxspeed
        inertia = Vector3.ClampMagnitude(inertia+movement*trueSpeed, maxSpeed);

        Vector2 friction = inertia.normalized*-1*coeficient;
        inertia +=friction;
        //if friction would give a negative, instead we round to zero
        if(Mathf.Abs(inertia.magnitude) <= 3) {
            inertia= Vector2.zero;
        }
        transform.Translate(inertia*Time.deltaTime);
    }
}
