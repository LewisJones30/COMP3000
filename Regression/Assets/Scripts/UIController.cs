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
    public GameObject powerdrainedMessage, activePowers, healthObj, UIPause;
    Text health, activepowers, powerdrainedtext, UIPauseText;
    Player player;
    // Start is called before the first frame update
    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        powerController = GameObject.FindGameObjectWithTag("PowerHandler").GetComponent<PowerBehaviour>();
        if (SceneManager.GetActiveScene().name == "Game")
        {

            //Set GameObjects from game scene.
            powerController = GameObject.Find("PowerHandler").GetComponent<PowerBehaviour>();
            player = GameObject.Find("Player").GetComponent<Player>();
            GameObject powersText = GameObject.Find("ModifiersText");
            activepowers = powersText.GetComponent<Text>();
            GameObject healthText = GameObject.Find("PlayerHealth");
            health = healthText.GetComponent<Text>();
            GameObject PowerDrainedMessage = GameObject.Find("PowerDrainedMessage");
            powerdrainedtext = PowerDrainedMessage.GetComponent<Text>();
            powerdrainedtext.enabled = false;
            UIPause = GameObject.Find("UIPauseText");
            UIPauseText = UIPause.GetComponent<Text>();
            UIPauseText.enabled = false;
        }
    }

    string getUpdatePlayerHP()
    {
        double health = player.health;
        health = Math.Round(health);
        string returnValue = "HP: " + Convert.ToString(health);
        return returnValue;
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

    //Methods to get information and update UI

    private void FixedUpdate()
    {
        if (isPaused == false)
        {
            UIPauseText.enabled = false;
            activepowers.text = powerController.ModifierText();
        }

    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        powerController = GameObject.Find("PowerHandler").GetComponent<PowerBehaviour>();
        player = GameObject.Find("Player").GetComponent<Player>();
        GameObject powersText = GameObject.Find("ModifiersText");
        activepowers = powersText.GetComponent<Text>();
        GameObject healthText = GameObject.Find("PlayerHealth");
        health = healthText.GetComponent<Text>();
        GameObject PowerDrainedMessage = GameObject.Find("PowerDrainedMessage");
        powerdrainedtext = PowerDrainedMessage.GetComponent<Text>();
        powerdrainedtext.enabled = false;
        UIPause = GameObject.Find("UIPauseText");
        UIPauseText = UIPause.GetComponent<Text>();
        UIPauseText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
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
                    //Code here to trigger UI
                    Cursor.lockState = CursorLockMode.None;
                    UIPauseText.enabled = true;
                    isPaused = true;
                    return;
                }
                else
                {
#if UNITY_EDITOR
                    if (EditorApplication.isPlaying) //Stop the game running in the Unity Editor, testing purposes.
                    {
                        //SceneManager.LoadScene("TestScene"); This will be main menu once it has been coded.
                        Debug.Log("Player quit game!");
                        EditorApplication.isPlaying = false;
                    }
                    else
                    {
                        //SceneManager.LoadScene("TestScene"); //This will be main menu once it has been coded.
                        Debug.Log("Player quit game!");
                        Application.Quit();
                    }
#endif
                    Application.Quit();
                }
            }
            if (isPaused == true && Input.anyKeyDown == true)
            {
                isPaused = false;
                UIPauseText.enabled = false;
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
