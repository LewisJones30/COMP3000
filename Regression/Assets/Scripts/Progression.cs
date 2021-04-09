using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Progression : MonoBehaviour
{
    //Public variables
    [HideInInspector]
    public float points; //This is an important modifier
    [Tooltip("Change the size to specify how many waves you want!")]
    public WaveArray[] waveArrays = new WaveArray[5];
    [Tooltip("Change the array size to the number of spawn points you want.")]
    public GameObject[] enemySpawners; //Enemies will spawn from a central portal or location. Could be modified to have more than one enemy spawner and randomly choose one.
    [Tooltip("Time in seconds between an enemy's spawn. \n" +
        "Each element controls each wave. \n" +
        "Ensure the size of this array is equal to the number of waves.")]
    [Range(0.1f, 15f)]
    public float[] EnemySpawnTime; //Control the time it takes to defeat each enemy here.
    [HideInInspector]
    public int numberEnemiesKilled = 0; //Used to track how many enemies have been killed per wave.
    bool waveComplete = false;

    /*
     * Wave arrays. Each wave is stored in memory, with an array of enemies that will be spawned randomly.
     * IMPORTANT - REMEMBER TO UPDATE CURRENT PROGRESSION PERCENTAGE ENEMY COUNT IF ADDING/REMOVING ENEMIES.
     * Each wave is set in the inspector!
     */


    //Private variables
    private int currentWave, maximumWave; //Know the current wave and the maximum wave.
    private UIController ui; //Used for checking if the game is running.
    private int numberEnemiesSpawned = 0;
    private float[] EnemySpawnTimeDupe; //Used for storing an exact duplicate of the original timers, for resetting once an enemy has spawned.
    private Text temptext;

    //Measuring progression of the wave. Callable by enemy before the enemy is destroyed, as well as UI.

    //public float currentProgressionPercentage() //Currently unused in UI.
    //{
    //    float progress = 100f;
    //    float enemiesAlive = 0;
    //    //switch (currentWave)
    //    //{
    //    //    case 1:
    //    //        //Enemies in Wave 1: 16.
    //    //        foreach(GameObject obj in waveArrays[0])
    //    //        {
    //    //            if (obj != null)
    //    //            {
    //    //                enemiesAlive = enemiesAlive + 1;
    //    //            }
    //    //        }
    //    //        progress = progress / enemiesAlive;
    //    //        break;
    //    //    case 2:
    //    //        //Enemies in Wave 2:
    //    //        foreach (GameObject obj in wave2Enemies)
    //    //        {
    //    //            if (obj != null)
    //    //            {
    //    //                enemiesAlive = enemiesAlive + 1;
    //    //            }
    //    //        }
    //    //        progress = progress / enemiesAlive;
    //    //        break;
    //    //    case 3:
    //    //        //Enemies in Wave 3:
    //    //        foreach (GameObject obj in wave3Enemies)
    //    //        {
    //    //            if (obj != null)
    //    //            {
    //    //                enemiesAlive = enemiesAlive + 1;
    //    //            }
    //    //        }
    //    //        progress = progress / enemiesAlive;
    //    //        break;
    //    //    case 4:
    //    //        //Enemies in Wave 4:
    //    //        foreach (GameObject obj in wave4Enemies)
    //    //        {
    //    //            if (obj != null)
    //    //            {
    //    //                enemiesAlive = enemiesAlive + 1;
    //    //            }
    //    //        }
    //    //        progress = progress / enemiesAlive;
    //    //        break;
    //    //    case 5:
    //    //        //Enemies in Wave 5:
    //    //        foreach (GameObject obj in wave5Enemies)
    //    //        {
    //    //            if (obj != null)
    //    //            {
    //    //                enemiesAlive = enemiesAlive + 1;
    //    //            }
    //    //        }
    //    //        progress = progress / enemiesAlive;
    //    //        break;
    //    //    default: //Error out if not a wave that has been programmed.
    //    //        Debug.Log("ERROR: Current wave in progress is UNKNOWN.");
    //    //        progress = -1f; 
    //    //        break;
    //    //}
    //    //return progress;
    //}
    void Spawn()
    {
        System.Random r = new System.Random(); //Used for spawning an enemy. 
        GameObject enemySpawner; 
        System.Random ra = new System.Random(); //Used for choosing a spawner
        enemySpawner = enemySpawners[ra.Next(0, enemySpawners.Length)]; //Choose a random spawner.
        int waveLength = 0;
        GameObject[] waveEnemies = waveArrays[currentWave - 1].wave;
        waveLength = waveEnemies.Length;
        GameObject W1Spawn = waveEnemies[numberEnemiesSpawned];
        Instantiate(W1Spawn, enemySpawner.transform.position, enemySpawner.transform.rotation);
        //switch (currentWave) 
        //{
        //    case 1:
        //        GameObject[] wave1Enemies = waveArrays[0].wave;
        //        waveLength = wave1Enemies.Length;
        //        GameObject W1Spawn = wave1Enemies[r.Next(0, waveLength)];
        //        Instantiate(W1Spawn, enemySpawner.transform.position, enemySpawner.transform.rotation);
        //        break;
        //    case 2:
        //        GameObject[] wave2Enemies = waveArrays[1].wave;
        //        waveLength = wave2Enemies.Length;
        //        GameObject W2Spawn = wave2Enemies[r.Next(0, waveLength)];
        //        Instantiate(W2Spawn, enemySpawner.transform.position, enemySpawner.transform.rotation);
        //        break;
        //    case 3:
        //        GameObject[] wave3Enemies = waveArrays[2].wave;
        //        waveLength = wave1Enemies.Length;
        //        GameObject W3Spawn = wave1Enemies[r.Next(0, waveLength)];
        //        break;
        //    case 4:
        //        waveLength = wave1Enemies.Length;
        //        GameObject W4Spawn = wave1Enemies[r.Next(0, waveLength)];
        //        Instantiate(W4Spawn, enemySpawner.transform.position, enemySpawner.transform.rotation);
        //        break;
        //    case 5:
        //        waveLength = wave1Enemies.Length;
        //        GameObject W5Spawn = wave1Enemies[r.Next(0, waveLength)];
        //        Instantiate(W5Spawn, enemySpawner.transform.position, enemySpawner.transform.rotation);
        //        break;
        //}
    }
    //void ForceSpawn(int EnemyArraySlotID) //If a certain enemy must be spawned. For example, the 5th enemy could be 100% chance of being a powerful enemy.
    //{
    //    //Choose a random spawner.
    //    GameObject enemySpawner;
    //    System.Random ra = new System.Random();
    //    enemySpawner = enemySpawners[ra.Next(0, enemySpawners.Length)];
    //    switch (currentWave)
    //    {
    //        case 1:
    //            GameObject spawn = wave1Enemies[EnemyArraySlotID];
    //            Instantiate(spawn, enemySpawner.transform.position, enemySpawner.transform.rotation);
    //            break;

    //        case 2:
    //            GameObject spawn2 = wave1Enemies[EnemyArraySlotID];
    //            Instantiate(spawn2, enemySpawner.transform.position, enemySpawner.transform.rotation);
    //            break;
    //        case 3:
    //            GameObject spawn3 = wave1Enemies[EnemyArraySlotID];
    //            Instantiate(spawn3, enemySpawner.transform.position, enemySpawner.transform.rotation);
    //            break;
    //        case 4:
    //            GameObject spawn4 = wave1Enemies[EnemyArraySlotID];
    //            Instantiate(spawn4, enemySpawner.transform.position, enemySpawner.transform.rotation);
    //            break;
    //        case 5:
    //            GameObject spawn5 = wave1Enemies[EnemyArraySlotID];
    //            Instantiate(spawn5, enemySpawner.transform.position, enemySpawner.transform.rotation);
    //            break;
    //    }
    //}
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
    

    public void enemyKilled(bool finalBossMinion)
    {

        if (finalBossMinion)
        {
            return;
        }
        numberEnemiesKilled = numberEnemiesKilled + 1;
        GameObject[] waveEnemies = waveArrays[currentWave - 1].wave;
        if (numberEnemiesKilled == waveEnemies.Length)
        {
            waveComplete = true;
            currentWave = currentWave + 1;
            //Reset enemies killed/spawned.
            numberEnemiesKilled = 0;
            numberEnemiesSpawned = 0;
            if (currentWave > maximumWave)
            {
                ui.GameCompleteText();
            }
            //Wave complete!
            ui.WaveComplete();
        }
    }


    //Getters
    public int GetCurrentWave()
    {
        return currentWave;
    }
    public int GetMaximumWaves()
    {
        return maximumWave;
    }
    public float GetProgression()
    {
        if (waveComplete)
        {
            return (float)1.0f;
        }
        return (float)numberEnemiesKilled / waveArrays[GetCurrentWave() - 1].wave.Length;
    }

    //Setters

    public void SetWaveComplete(bool value)
    {
        waveComplete = value;
    }
    void FixedUpdate() //Control the enemy spawns here!
    {
        if (ui.getTutorialStage() > 0)
        {
            return;
        }
        if (SceneManager.GetActiveScene().name != "Game")
        {
            return;
        }
        if (ui.isPaused != true) //Only run timers if the game is not paused. Paused state will also be used for round transitions.
            switch (currentWave)
            {
                case 1:
                    GameObject[] wave1Enemies = waveArrays[0].wave;
                    if (numberEnemiesSpawned == wave1Enemies.Length) //Ignore timers once all enemies have been spawned.
                    {
                        EnemySpawnTime[0] = 256;
                        break;
                    }
                    EnemySpawnTime[0] = EnemySpawnTime[0] - Time.deltaTime;
                    if (EnemySpawnTime[0] <= 0)
                    {
                        
                        Spawn();
                        numberEnemiesSpawned = numberEnemiesSpawned + 1;
                        EnemySpawnTime[0] = EnemySpawnTimeDupe[0]; //Reset timer.
                        break;
                    }
                    break;
                case 2:
                    GameObject[] wave2Enemies = waveArrays[1].wave;
                    EnemySpawnTime[1] = EnemySpawnTime[1] - Time.deltaTime;
                    if (numberEnemiesSpawned == wave2Enemies.Length) //Ignore timers once all enemies have been spawned.
                    {
                        EnemySpawnTime[1] = 256;
                        break;
                    }
                    if (EnemySpawnTime[1] <= 0)
                    {
                        Spawn();
                        numberEnemiesSpawned = numberEnemiesSpawned + 1;
                        EnemySpawnTime[1] = EnemySpawnTimeDupe[1]; //Reset timer.
                        break;
                    }
                    break;
                case 3:
                    GameObject[] wave3Enemies = waveArrays[2].wave;
                    EnemySpawnTime[2] = EnemySpawnTime[2] - Time.deltaTime;
                    if (numberEnemiesSpawned == wave3Enemies.Length) //Ignore timers once all enemies have been spawned.
                    {
                        EnemySpawnTime[1] = 256;
                        break;
                    }
                    if (EnemySpawnTime[2] <= 0)
                    {
                        
                        Spawn();
                        numberEnemiesSpawned = numberEnemiesSpawned + 1;
                        EnemySpawnTime[2] = EnemySpawnTimeDupe[2]; //Reset timer.
                        break;
                    }
                    break;
                case 4:
                    GameObject[] wave4Enemies = waveArrays[3].wave;
                    EnemySpawnTime[3] = EnemySpawnTime[3] - Time.deltaTime;
                    if (numberEnemiesSpawned == wave4Enemies.Length) //Ignore timers once all enemies have been spawned.
                    {
                        EnemySpawnTime[1] = 256;
                        break;
                    }
                    if (EnemySpawnTime[3] <= 0)
                    {
                        
                        Spawn();
                        numberEnemiesSpawned = numberEnemiesSpawned + 1;
                        EnemySpawnTime[3] = EnemySpawnTimeDupe[3]; //Reset timer.
                        break;
                    }
                    break;
                case 5:
                    GameObject[] wave5Enemies = waveArrays[4].wave;
                    EnemySpawnTime[4] = EnemySpawnTime[4] - Time.deltaTime;
                    if (numberEnemiesSpawned == wave5Enemies.Length) //Ignore timers once all enemies have been spawned.
                    {
                        EnemySpawnTime[1] = 256;
                        break;
                    }
                    if (EnemySpawnTime[4] <= 0)
                    {
                        
                        Spawn();
                        numberEnemiesSpawned = numberEnemiesSpawned + 1;
                        EnemySpawnTime[4] = EnemySpawnTimeDupe[4]; //Reset timer.
                        break;
                    }
                    break;
                case 6:
                    EnemySpawnTime[5] = EnemySpawnTime[5] - Time.deltaTime;

                    GameObject[] wave6Enemies = waveArrays[5].wave;
                    if (numberEnemiesSpawned == wave6Enemies.Length) //Ignore timers once all enemies have been spawned.
                    {
                        EnemySpawnTime[1] = 256;
                        break;
                    }
                    if (EnemySpawnTime[5] <= 0)
                    {
                        
                        Spawn();
                        numberEnemiesSpawned = numberEnemiesSpawned + 1;
                        EnemySpawnTime[5] = EnemySpawnTimeDupe[5]; //Reset timer.
                        break;
                    }
                    break;
                case 7:
                    EnemySpawnTime[6] = EnemySpawnTime[6] - Time.deltaTime;
                    GameObject[] wave7Enemies = waveArrays[6].wave;
                    if (numberEnemiesSpawned == wave7Enemies.Length) //Ignore timers once all enemies have been spawned.
                    {
                        EnemySpawnTime[1] = 256;
                        break;
                    }
                    if (EnemySpawnTime[6] <= 0)
                    {
                        
                        Spawn();
                        numberEnemiesSpawned = numberEnemiesSpawned + 1;
                        EnemySpawnTime[6] = EnemySpawnTimeDupe[6]; //Reset timer.
                        break;
                    }
                    break;
                case 8:
                    EnemySpawnTime[7] = EnemySpawnTime[7] - Time.deltaTime;
                    GameObject[] wave8Enemies = waveArrays[7].wave;
                    if (numberEnemiesSpawned == wave8Enemies.Length) //Ignore timers once all enemies have been spawned.
                    {
                        EnemySpawnTime[1] = 256;
                        break;
                    }
                    if (EnemySpawnTime[7] <= 0)
                    {
                        ;
                        Spawn();
                        numberEnemiesSpawned = numberEnemiesSpawned + 1;
                        EnemySpawnTime[7] = EnemySpawnTimeDupe[7]; //Reset timer.
                        break;
                    }
                    break;
                case 9:
                    EnemySpawnTime[8] = EnemySpawnTime[8] - Time.deltaTime;
                    GameObject[] wave9Enemies = waveArrays[8].wave;
                    if (numberEnemiesSpawned == wave9Enemies.Length) //Ignore timers once all enemies have been spawned.
                    {
                        EnemySpawnTime[1] = 256;
                        break;
                    }
                    if (EnemySpawnTime[8] <= 0)
                    {
                        
                        Spawn();
                        numberEnemiesSpawned = numberEnemiesSpawned + 1;
                        EnemySpawnTime[8] = EnemySpawnTimeDupe[8]; //Reset timer.
                        break;
                    }
                    break;
                case 10:
                    GameObject[] wave10Enemies = waveArrays[9].wave;
                    EnemySpawnTime[9] = EnemySpawnTime[9] - Time.deltaTime;
                    if (numberEnemiesSpawned == wave10Enemies.Length) //Ignore timers once all enemies have been spawned.
                    {
                        EnemySpawnTime[1] = 256;
                        break;
                    }
                    if (EnemySpawnTime[4] <= 0)
                    {
                        
                        Spawn();
                        numberEnemiesSpawned = numberEnemiesSpawned + 1;
                        EnemySpawnTime[9] = EnemySpawnTimeDupe[9]; //Reset timer.
                        break;
                    }
                    break;
                case 11:
                    EnemySpawnTime[10] = EnemySpawnTime[10] - Time.deltaTime;
                    GameObject[] wave11Enemies = waveArrays[10].wave;
                    if (numberEnemiesSpawned == wave11Enemies.Length) //Ignore timers once all enemies have been spawned.
                    {
                        EnemySpawnTime[1] = 256;
                        break;
                    }
                    if (EnemySpawnTime[4] <= 0)
                    {
                        
                        Spawn();
                        numberEnemiesSpawned = numberEnemiesSpawned + 1;
                        EnemySpawnTime[10] = EnemySpawnTimeDupe[10]; //Reset timer.
                        break;
                    }
                    break;
                case 12:
                    GameObject[] wave12Enemies = waveArrays[11].wave;
                    EnemySpawnTime[11] = EnemySpawnTime[11] - Time.deltaTime;
                    if (numberEnemiesSpawned == wave12Enemies.Length) //Ignore timers once all enemies have been spawned.
                    {
                        EnemySpawnTime[1] = 256;
                        break;
                    }
                    if (EnemySpawnTime[4] <= 0)
                    {
                        
                        Spawn();
                        numberEnemiesSpawned = numberEnemiesSpawned + 1;
                        EnemySpawnTime[11] = EnemySpawnTimeDupe[11]; //Reset timer.
                        break;
                    }
                    break;
                case 13:
                    GameObject[] wave13Enemies = waveArrays[12].wave;
                    EnemySpawnTime[12] = EnemySpawnTime[12] - Time.deltaTime;
                    if (numberEnemiesSpawned == wave13Enemies.Length) //Ignore timers once all enemies have been spawned.
                    {
                        EnemySpawnTime[1] = 256;
                        break;
                    }
                    if (EnemySpawnTime[4] <= 0)
                    {
                        
                        Spawn();
                        numberEnemiesSpawned = numberEnemiesSpawned + 1;
                        EnemySpawnTime[12] = EnemySpawnTimeDupe[12]; //Reset timer.
                        break;
                    }
                    break;
            }
    }
    void Start()
    {
        ui = GameObject.Find("UIHandler").GetComponent<UIController>(); //Get the UI controller ready for pause/resume states.

        currentWave = 1;
        maximumWave = waveArrays.Length; //Dynamically find the maximum waves.
        EnemySpawnTimeDupe = (float[])EnemySpawnTime.Clone(); //Create a clone.
    }
}
[System.Serializable]
public class WaveArray
{
    public GameObject[] wave;
}
