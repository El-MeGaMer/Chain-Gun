using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{

    [SerializeField]
    public float speed;

    UnityEngine.Vector2 direction = UnityEngine.Vector2.zero;

    public InputAction move;

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
        direction = move.ReadValue<UnityEngine.Vector2>();
        
    }

    void FixedUpdate()
    {
        UnityEngine.Vector2 movement= new UnityEngine.Vector2(direction.x,direction.y);
        transform.Translate(movement*speed*Time.deltaTime);
    }
}
