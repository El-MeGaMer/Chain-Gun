using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    GameManager gameManager;
    bool active = false;
    SpriteRenderer sp;
    public Color inactiveColor;
    public Color activeColor;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;
        sp = GetComponent<SpriteRenderer>();
        sp.color = inactiveColor;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.enemyCount <= 0)
        {
            active = true;
            sp.color = activeColor;
        }
        else
        {
            active = false;
            sp.color = inactiveColor;
        }
    }
}
