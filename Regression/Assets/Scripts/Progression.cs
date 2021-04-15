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
    TutorialHandler TutorialStatus;
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
        if (TutorialStatus.GetTutorialStage() > 0)
        {
            return (float)0.0f; //No curse strength is given.
        }
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
        if (SceneManager.GetActiveScene().name == "UI Scale Testing")
        {
            return;
        }
        if (TutorialStatus == null) //Cannot always find the object at the start, since it may not be initialised.
        {
            TutorialStatus = GameObject.FindWithTag("TutorialHandler").GetComponent<TutorialHandler>();
        }
        if (TutorialStatus.GetTutorialStage() > 0)
        {
            return;
        }
        if (SceneManager.GetActiveScene().name != "Game")
        {
            return;
        }
        if (!ui.GetIsPaused()) //Only run timers if the game is not paused. Paused state will also be used for round transitions.
        {
            GameObject[] wave1Enemies = waveArrays[currentWave - 1].wave;
            if (numberEnemiesSpawned == wave1Enemies.Length) //Ignore timers once all enemies have been spawned.
            {
                EnemySpawnTime[currentWave - 1] = 256;
            }
            EnemySpawnTime[currentWave - 1] = EnemySpawnTime[currentWave - 1] - Time.deltaTime;
            if (EnemySpawnTime[currentWave - 1] <= 0)
            {

                Spawn();
                numberEnemiesSpawned = numberEnemiesSpawned + 1;
                EnemySpawnTime[currentWave - 1] = EnemySpawnTimeDupe[currentWave - 1]; //Reset timer.
            }
        }
    }
    void Start()
    {
        ui = GameObject.Find("UIHandler").GetComponent<UIController>(); //Get the UI controller ready for pause/resume states.

        currentWave = 1;
        maximumWave = waveArrays.Length; //Dynamically find the maximum waves.
        EnemySpawnTimeDupe = (float[])EnemySpawnTime.Clone(); //Create a clone.
        EnemySpawnTime[0] = 0.5f;
    }
}
[System.Serializable]
public class WaveArray
{
    public GameObject[] wave;
}
