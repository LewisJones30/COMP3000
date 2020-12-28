using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour //THE GAMEOBJECT THAT THIS SCRIPT IS ATTACHED TO IS CREATED WHEN THE GAME IS LOADED, AND IS ALWAYS KEPT BETWEEN SCENES.
{
    public bool isPaused = false;
    bool buttonpressed = false;
    PowerBehaviour powerController;
    public GameObject activePowers, healthObj, UIPause, UIWaveComplete;
    Text health, activepowers, powerdrainedtext, UIPauseText, UIWaveCompText, UITempWaveText;
    Player player;
    bool waveCompletePause = false;
    double pointsGained;
    // Start is called before the first frame update
    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        powerController = GameObject.FindGameObjectWithTag("PowerHandler").GetComponent<PowerBehaviour>();
        if (SceneManager.GetActiveScene().name == "Game")
        {
            GetGameObjectsGame(); //Call method to get gameobject.
            WeaponSelection();
        }
    }
    void GetGameObjectsGame()
    {
        //Firstly, get the gameobjects.
        GameObject powersText = GameObject.Find("ModifiersText");
        GameObject healthText = GameObject.Find("PlayerHealth");
        GameObject PowerDrainedMessage = GameObject.Find("PowerDrainedMessage");

        //Find GameObject section.

        powerController = GameObject.Find("PowerHandler").GetComponent<PowerBehaviour>();
        player = GameObject.Find("Player").GetComponent<Player>();
        UIPause = GameObject.Find("UIPauseText");
        UIWaveComplete = GameObject.Find("WaveCompleteText");


        //Get component section

        activepowers = powersText.GetComponent<Text>();
        health = healthText.GetComponent<Text>();
        powerdrainedtext = PowerDrainedMessage.GetComponent<Text>();
        UIWaveCompText = UIWaveComplete.GetComponent<Text>();

        //Enable/Disable section

        powerdrainedtext.enabled = false;
        UIWaveCompText.enabled = false;

        //Modify values section
        //UITempWaveText.text = "Wave 1/2";
    }

    void WeaponSelection()
    {
        //This script initialises weapon selection for the player.
        //If the difficulty is hard, or expert, then the game will automatically skip this check and spawn a random weapon for the player.
        isPaused = true;
        //Game is paused here.
        foreach (GameObject obj in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
        {
            if (obj.tag == "RemoveWhenPaused")
            {
                obj.SetActive(false);
            }
            if (obj.name == "Choose a Weapon")
            {
                obj.SetActive(true);
            }
        }




    }
    string getUpdatePlayerHP()
    {
        double health = player.health;
        health = Math.Round(health);
        string returnValue = "HP: " + Convert.ToString(health);
        return returnValue;
    }
    //Points modifier methods
    public void AddPoints(double pointsToAdd)
    {
        pointsToAdd = pointsToAdd * player.pointsModifier;
        pointsGained = pointsGained + pointsToAdd;
        
    }
    public void ResetPoints()
    {
        pointsGained = 0; 
    }
    public void removePoints(double pointsToRemove)
    {
        pointsGained = pointsGained - pointsToRemove;
        if (pointsGained < 0)
        {
            pointsGained = 0;
        }
    }

    //Methods to trigger start of the game
    public void startGameEasy()
    {
        powerController.StartGameEasy();
        
    }
    public void startGameMedium()
    {
        powerController.StartGameNormal();
    }
    public void startGameHard()
    {
        powerController.StartGameHard();
    }
    public void startGameExpert()
    {
        powerController.StartGameExpert();
    }
    //Satanic difficulty is currently planned as an MAP and may not be added. The trigger will be added, however.
    public void startGameSatanic()
    {
        powerController.StartGameSatanic();
    }
    void startTutorial()
    {
        powerController.StartGameTutorial();
    }
    //Methods called by PowerBehaviour to draw certain UI elements and redraw others

    public void LostPowerMessage(int PowerDrainedID)
    {
        StartCoroutine("ShowMessage", PowerDrainedID);

    }
    IEnumerator ShowMessage(int PowerDrainedID)
    {
        powerdrainedtext.enabled = true;
        if (PowerDrainedID == 3)
        {
            powerdrainedtext.fontSize = 55;
        }
        else
        {
            powerdrainedtext.fontSize = 70;
        }
        powerdrainedtext.text = "As the wave ends, you feel " + powerController.powerHandler[PowerDrainedID].PowerName + " fade away...";
        activepowers.text = powerController.ModifierText();
        yield return new WaitForSeconds(4);
        powerdrainedtext.enabled = false;
    }
    //=============================================Methods to control the menus================================================
    void loadSettings() //Triggered when the player clicks settings on the home page
    {

    }
    //Load leaderboards
    void loadLeaderboardEasy()
    {

    }
    void loadLeaderboardNormal()
    {

    }
    void loadLeaderboardHard()
    {

    }
    void loadLeaderboardExpert()
    {

    }
    void loadLeaderboardSatanic()
    {

    }
    void loadMainMenu()
    {

    }
    public void WaveCompleteText()
    {
        StartCoroutine("WaveCompleteMethod");
    }
    IEnumerator WaveCompleteMethod()
    {
        UIWaveCompText.enabled = true;
        isPaused = true;
        waveCompletePause = true;
        yield return new WaitForSeconds(7.5f);
        UIWaveCompText.enabled = false;
        waveCompletePause = false;
        powerController.loseAllPowers(); //Drain all of the player's powers.
        isPaused = false;
        
    }
    public void GameCompleteText()
    {
        StartCoroutine("GameCompleteRoutine");
    }
    IEnumerator GameCompleteRoutine()
    {
        UIWaveCompText.enabled = true;
        UIWaveCompText.text = "Congratulations! You have completed the prototype. Press escape to exit. \n You will otherwise be returned to the home page shortly.";
        isPaused = true;
        yield return new WaitForSeconds(7.5f);
        UIWaveCompText.enabled = false;
        SceneManager.LoadScene("UI Scale Testing");
        Cursor.lockState = CursorLockMode.None;
        isPaused = false;
    }

    //=========================================================Pause functionality=========================================================
    void GamePausedFunction() //This function will control the UI when the game is paused.
    {
        GameObject[] objsToHide = GameObject.FindGameObjectsWithTag("RemoveWhenPaused");
        foreach (GameObject obj in objsToHide)
        {
            obj.SetActive(false);
        }
        foreach (GameObject obj in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
        {
            if (obj.tag == "RemoveWhenResumed")
            {
               obj.SetActive(true);
            } 
        }

        
    }

    private void GameUnpausedFunction() //This function will control the UI when the game is resumed by the player.
    {
        foreach (GameObject obj in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
        {
            if (obj.tag == "RemoveWhenPaused")
            {
                obj.SetActive(true);
            }
            if (obj.tag == "RemoveWhenResumed")
            {
                obj.SetActive(false);
            }
            if (obj.name == "Exit Confirmation")
            {
                obj.SetActive(false);
            }
            if (obj.name == "Choose a Weapon")
            {
                obj.SetActive(false);
            }

        }
    }

    public void ResumeButton()
    {
        Debug.Log("Game resumed!");
        GameUnpausedFunction();
        Cursor.lockState = CursorLockMode.Locked;
        isPaused = false;
    }
    public void ReturnToPauseScreen()
    {
        foreach (GameObject obj in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
        {
            if (obj.name == "Exit Confirmation")
            {
                obj.SetActive(false);
            }
          if (obj.name == "Pause Menu")
            {
                obj.SetActive(true);
            }
        }
    }    
    public void ExitGameConfirmScreen()
    {
        foreach (GameObject obj in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
        {
            if (obj.tag == "RemoveWhenResumed")
            {
                obj.SetActive(false);
            }
            if (obj.name == "Exit Confirmation")
            {
                obj.SetActive(true);
            }

        }
    }
    public void ExitGame()
    {
#if UNITY_EDITOR //Close game within Unity editor. Ignored once in standalone executable!
        {
            EditorApplication.isPlaying = false;
        }
#endif
        //Close game.
        Application.Quit();
    }



    //======================================Methods to get information and update UI


    public void EnableFireWarning() //Called by Player when the player is on fire
    {
            foreach (GameObject obj in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
            {
                if (obj.tag == "FireWarning")
                {
                    obj.SetActive(true);
                    return;
                }
            }
        return;
        
    }
    public void DisableFireWarning()
    {
        GameObject.FindGameObjectWithTag("FireWarning").SetActive(false);
    }
    void FixedUpdate()
    {
        if (SceneManager.GetActiveScene().name != "Game")
        {
            return;
        }
        if (isPaused == false)
        {
            activepowers.text = powerController.ModifierText();
        }

    }
    //Used when the game moves to another scene.
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GetGameObjectsGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "UI Scale Testing")
        {
            if (Input.GetKeyDown(KeyCode.Escape) == true)
            {
#if UNITY_EDITOR
                if (EditorApplication.isPlaying)
                {
                    EditorApplication.isPlaying = false;
                }
#endif
                Application.Quit();
            }
        }
        if (SceneManager.GetActiveScene().name == "Game") //Ensure player is in main game when checking for pause
        {
            if (isPaused == false)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            if (Input.GetKeyDown(KeyCode.Escape) == true)
            {
                if (isPaused == false)
                {
                    Debug.Log("Escape pressed, game paused!");
                    GamePausedFunction();
                    Cursor.lockState = CursorLockMode.None;
                    isPaused = true;
                    return;
                }
                else
                {
                    Debug.Log("Game resumed!");
                    GameUnpausedFunction();
                    Cursor.lockState = CursorLockMode.Locked;
                    isPaused = false;
                }
            }
            else
            {
                
                if (Input.GetKeyDown(KeyCode.R) == true && buttonpressed == false)
                {
                    powerController.losePowerHard(); //Testing.
                    buttonpressed = true;
                }
                else
                {
                    buttonpressed = false;
                }
            }
            health.text = getUpdatePlayerHP(); //Update health every frame.
        }


    }
}
