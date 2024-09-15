using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class customMuzzleAnim : MonoBehaviour
{
    SpriteRenderer spriteRenderer; 
    [SerializeField]
    List<Sprite> sprites;
    int currentIndex = 0;
    bool firing = false;

    void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public IEnumerator Fire(){
        firing = true;
        int i = 0;
        while(i == currentIndex){
            i = Random.Range(0,sprites.Count);
        }
        spriteRenderer.sprite = sprites[i];
        yield return new WaitForSeconds(.1f);
        firing = false;
        if (!firing) {
            spriteRenderer.sprite = null;
        }
    }
}
