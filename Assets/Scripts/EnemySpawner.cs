using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    [SerializeField]
    private GameObject Enemy;
    [SerializeField]
    private float MinSpawnTime;
    [SerializeField]
    private float MaxSpawnTime;

    private float TimeUntilSpawn;

    private void SetTimeUntilSpawn(){
        TimeUntilSpawn=Random.Range(MinSpawnTime, MaxSpawnTime);
    }
    void Awake(){
        SetTimeUntilSpawn();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TimeUntilSpawn-=Time.deltaTime;
        if(TimeUntilSpawn <= 0){
            Instantiate(Enemy, transform.position, Quaternion.identity);
            SetTimeUntilSpawn();
        }
    }
}
