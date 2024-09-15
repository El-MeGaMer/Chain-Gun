using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    //Singleton
    public static GameManager Instance;

    //Player info
    public int score = 0;
    public int enemyCount = 0;
    [SerializeField] Vector2 respawnPosition = new Vector2(-20, 0);
    // [SerializeField] GameObject player;
    // Rigidbody2D playerRb;
    [SerializeField] Image[] UIHearts;
    [SerializeField] Color UIHeartsActiveColor;
    [SerializeField] Color UIHeartsInactiveColor;

    //Level info
    [SerializeField] GameObject[] levelPrefabs; //First level is empty
    GameObject currentLevel;
    Animator currentLevelAnimator;

    //UI
    [SerializeField] TMP_Text scoreText;

    //Level Switching
    [SerializeField] GameObject blackTransition;
    Animator transitionAnimator;
    [SerializeField] float animationDuration = 1;

    AudioSource sors;
    [SerializeField]  AudioClip loopedSong;

    void Awake()
    {
        //Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        // sors = GetComponent<AudioSource>();
        // Invoke("shit", sors.clip.length);
        //First level startup
        UpdateScoreText();
        currentLevel = Instantiate(levelPrefabs[0], new Vector2(0, 0), quaternion.identity); //Loads free level
        currentLevelAnimator = currentLevel.GetComponent<Animator>();

        //Get player rb
        // playerRb = player.GetComponentInChildren<Rigidbody2D>();


        //Transition startup
        transitionAnimator = blackTransition.GetComponent<Animator>();
        blackTransition.SetActive(true);
        transitionAnimator.SetBool("GoBlack", false);
    }

    // void shit(){
    //     print("shit");
    //     sors.clip = loopedSong;
    //     sors.loop = true;
    //     sors.Play();
    // }

    // Update is called once per frame
    void Update()
    {
        // if(!sors.isPlaying){
        //     shit();
        // }
        if(currentLevel){
            //Check for animation
            if (enemyCount <= 0)
            {
                currentLevelAnimator.SetBool("DoorIsOpen", true);
            }
            else
            {
                currentLevelAnimator.SetBool("DoorIsOpen", false);
            }
        }
    }

    public void switchLevels()
    {
        score++;
        UpdateScoreText();
        transitionAnimator.SetBool("GoBlack", true);
        // playerRb.constraints = RigidbodyConstraints2D.FreezeAll;
        StartCoroutine(levelSwitchCoroutine());

    }

    IEnumerator levelSwitchCoroutine()
    {
        SceneManager.LoadScene(2);
        yield return new WaitForSeconds(animationDuration);
        currentLevel = Instantiate(levelPrefabs[Random.Range(1, levelPrefabs.Length)], new Vector2(0, 0), quaternion.identity);
        currentLevelAnimator = currentLevel.GetComponent<Animator>();
        // player.transform.position = respawnPosition;
        transitionAnimator.SetBool("GoBlack", false);
        // playerRb.constraints = RigidbodyConstraints2D.FreezeRotation;

    }

    void UpdateScoreText()
    {
        scoreText.text = "Score: " + score;
    }

    public void SetUIHealth(int health)
    {
        for (int i = 0; i < 3; i++)
        {
            UIHearts[i].color = UIHeartsInactiveColor;
            if (i < health)
            {
                UIHearts[i].color = UIHeartsActiveColor;
            }
        }
    }
}
