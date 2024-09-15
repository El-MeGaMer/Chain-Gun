using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Singleton
    public static GameManager Instance;

    //Player info
    int score = 0;
    public int enemyCount = 0;
    [SerializeField] Vector2 respawnPosition = new Vector2(-20, 0);
    [SerializeField] GameObject player;
    Rigidbody2D playerRb;

    //Level info
    [SerializeField] GameObject[] levelPrefabs;
    GameObject currentLevel;

    //UI
    [SerializeField] TMP_Text scoreText;

    //Level Switching
    [SerializeField] GameObject blackTransition;
    Animator transitionAnimator;
    [SerializeField] float animationDuration = 1;

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
        //First level startup
        UpdateScoreText();
        currentLevel = Instantiate(levelPrefabs[0], new Vector2(0, 0), quaternion.identity);

        //Get player rb
        playerRb = player.GetComponentInChildren<Rigidbody2D>();


        //Transition startup
        transitionAnimator = blackTransition.GetComponent<Animator>();
        blackTransition.SetActive(true);
        transitionAnimator.SetBool("GoBlack", false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void switchLevels()
    {
        score++;
        UpdateScoreText();
        transitionAnimator.SetBool("GoBlack", true);
        playerRb.constraints = RigidbodyConstraints2D.FreezeAll;
        StartCoroutine(levelSwitchCoroutine());

    }

    IEnumerator levelSwitchCoroutine()
    {
        yield return new WaitForSeconds(animationDuration);
        player.transform.position = respawnPosition;
        transitionAnimator.SetBool("GoBlack", false);
        playerRb.constraints = RigidbodyConstraints2D.None;
    }

    void UpdateScoreText()
    {
        scoreText.text = "Score: " + score;
    }
}
