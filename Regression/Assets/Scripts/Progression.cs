using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Progression : MonoBehaviour
{
    // Start is called before the first frame update
    float currentWave, maximumWave; //Know the current wave and the maximum wave.

    public GameObject enemySpawner;
    /*
     * Wave arrays. Each wave is stored in memory, with an array of enemies that will be spawned randomly.
     * IMPORTANT - REMEMBER TO UPDATE CURRENT PROGRESSION PERCENTAGE ENEMY COUNT IF ADDING/REMOVING ENEMIES.
     * Wave breakdowns:
     * Wave 1 - 
     * Wave 2 - 
     * Wave 3 - 
     * Wave 4 - 
     * Wave 5 - 
     */
    public GameObject[] wave1Enemies = new GameObject[5];
    public GameObject[] wave2Enemies = new GameObject[5];
    public GameObject[] wave3Enemies = new GameObject[5];
    public GameObject[] wave4Enemies = new GameObject[5];
    public GameObject[] wave5Enemies = new GameObject[5];

    //Measuring progression of the wave. Callable by enemy before the enemy is destroyed.
    public float currentProgressionPercentage()
    {
        float progress = 100f;
        float enemiesAlive = 0;
        switch (currentWave)
        {
            case 1:
                //Enemies in Wave 1: 16.
                foreach(GameObject obj in wave1Enemies)
                {
                    if (obj != null)
                    {
                        enemiesAlive = enemiesAlive + 1;
                    }
                }
                progress = progress / enemiesAlive;
                break;
            case 2:
                //Enemies in Wave 2:
                foreach (GameObject obj in wave2Enemies)
                {
                    if (obj != null)
                    {
                        enemiesAlive = enemiesAlive + 1;
                    }
                }
                progress = progress / enemiesAlive;
                break;
            case 3:
                //Enemies in Wave 3:
                foreach (GameObject obj in wave3Enemies)
                {
                    if (obj != null)
                    {
                        enemiesAlive = enemiesAlive + 1;
                    }
                }
                progress = progress / enemiesAlive;
                break;
            case 4:
                //Enemies in Wave 4:
                foreach (GameObject obj in wave4Enemies)
                {
                    if (obj != null)
                    {
                        enemiesAlive = enemiesAlive + 1;
                    }
                }
                progress = progress / enemiesAlive;
                break;
            case 5:
                //Enemies in Wave 5:
                foreach (GameObject obj in wave5Enemies)
                {
                    if (obj != null)
                    {
                        enemiesAlive = enemiesAlive + 1;
                    }
                }
                progress = progress / enemiesAlive;
                break;
            default: //Error out if not a wave that has been programmed.
                Debug.Log("ERROR: Current wave in progress is UNKNOWN.");
                progress = -1f; 
                break;
        }
        return progress;
    }
    void Spawn()
    {
        System.Random r = new System.Random();
        int waveLength = 0;
        switch (currentWave) 
        {
            case 1:
                waveLength = wave1Enemies.Length;
                GameObject W1Spawn = wave1Enemies[r.Next(0, waveLength)];
                Instantiate(W1Spawn, enemySpawner.transform.position, enemySpawner.transform.rotation);
                break;
            case 2:
                waveLength = wave1Enemies.Length;
                GameObject W2Spawn = wave1Enemies[r.Next(0, waveLength)];
                Instantiate(W2Spawn, enemySpawner.transform.position, enemySpawner.transform.rotation);
                break;
            case 3:
                waveLength = wave1Enemies.Length;
                GameObject W3Spawn = wave1Enemies[r.Next(0, waveLength)];
                break;
            case 4:
                waveLength = wave1Enemies.Length;
                GameObject W4Spawn = wave1Enemies[r.Next(0, waveLength)];
                Instantiate(W4Spawn, enemySpawner.transform.position, enemySpawner.transform.rotation);
                break;
            case 5:
                waveLength = wave1Enemies.Length;
                GameObject W5Spawn = wave1Enemies[r.Next(0, waveLength)];
                Instantiate(W5Spawn, enemySpawner.transform.position, enemySpawner.transform.rotation);
                break;
        }
    }
    void ForceSpawn(int EnemyArraySlotID) //If a certain enemy must be spawned. For example, the 5th enemy could be 100% chance of being a powerful enemy.
    {
        switch (currentWave)
        {
            case 1:
                break;
            case 2: 
                break;
            case 3: 
                break;
            case 4: 
                break;
            case 5:
                break;
        }
    }
    void nextWave()
    {
        if (currentWave < maximumWave)
        {
            currentWave = currentWave + 1;
        }
        else
        {
            //Code here to transfer points to the leaderboard.
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void Start()
    {
    }
}
