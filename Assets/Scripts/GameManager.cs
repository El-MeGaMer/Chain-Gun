using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    int score = 0;
    public int enemyCount = 0;

    [SerializeField] GameObject levelPrefab;
    [SerializeField] GameObject emptyNextLevelPrefab;
    [SerializeField] GameObject currentLevel;
    [SerializeField] GameObject nextLevel;

    [SerializeField] TMP_Text scoreText;

    void Awake()
    {
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
        UpdateScore();
        /*currentLevel = Instantiate(currentLevel, new Vector3(0, 0), quaternion.identity);
        GameObject muroDerecha = currentLevel.transform.Find("Muro derecha").gameObject;
        Vector3 nextLevelPosition = new Vector3(muroDerecha.transform.position.x + 12, 0);
        nextLevel = Instantiate(nextLevel, nextLevelPosition, quaternion.identity);*/
    }

    // Update is called once per frame
    void Update()
    {

    }

    void UpdateScore()
    {
        scoreText.text = "Score: " + score;
    }
}
