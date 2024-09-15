using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    GameManager gameManager;

    public List<Mob> Mobs = new List<Mob>();//List where we store the Mob Class
    public int CurrentWave;//number of the wave
    private int WaveCost;//The value which dictates how many mobs you can spawn
    public List<GameObject> SpawningMobs = new List<GameObject>();//List where we store the Mobs that are spawning
    private float SpawnInterval;//Value to indicate spawning interval
    private float TimeUntilSpawn;//Values to indicate how much time passes until the spawn

    //Function to generate Wave
    public void WaveGenerate()
    {
        WaveCost = CurrentWave * 10;
        SpawnMobs();

        SpawnInterval = 0;// 0 Value to generate them in an instant

    }
    //Function
    public void SpawnMobs()
    {
        List<GameObject> SpawnedMobs = new List<GameObject>();//List where we store the prefabs of the Mob
        while (WaveCost > 0)
        {
            int iMob = Random.Range(0, Mobs.Count);//ID for the Mobs
            int RandomMobCost = Mobs[iMob].cost;//The cost of the Mob
            if (WaveCost - RandomMobCost >= 0)
            {
                SpawnedMobs.Add(Mobs[iMob].EnemyPrefab);
                WaveCost -= RandomMobCost;
                gameManager.enemyCount++; //Add the enemy to the global counter
            }
            else
            {
                break;
            }
        }
        SpawningMobs.Clear();
        SpawningMobs = SpawnedMobs;
    }
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;

        WaveGenerate();
        TimeUntilSpawn = 4;
    }

    // Update is called once per frame
    void Update()
    {
        if (TimeUntilSpawn <= 0)
        {
            if (SpawningMobs.Count > 0)
            {
                Instantiate(SpawningMobs[0], transform.position - new Vector3(Random.Range(-10.0f, 10.0f), Random.Range(-10.0f, 10.0f), 0), Quaternion.identity);
                SpawningMobs.RemoveAt(0);
                TimeUntilSpawn = SpawnInterval;
            }
        }
        else
        {
            TimeUntilSpawn -= Time.deltaTime;
        }


    }
}
[System.Serializable]
public class Mob
{
    public GameObject EnemyPrefab;//Where we store the enemy prefab 
    public int cost;//The cost of the enemy to use it for the Wave Cost
}
