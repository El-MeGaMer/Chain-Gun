using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class casingScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(EndGrav());   
        GetComponent<Rigidbody2D>().AddForce(transform.up*600);
        
    }

    IEnumerator EndGrav() {
        yield return new WaitForSeconds(.8f);
        AudioSource sors = GetComponent<AudioSource>();
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        GetComponent<Rigidbody2D>().gravityScale = 0;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        // GetComponent<Rigidbody2D>().gameObject.;
        sors.pitch = Random.Range(.9f,1.5f);
        sors.Play();

        Destroy(this.gameObject,8);
    }
}
