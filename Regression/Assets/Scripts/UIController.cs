using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour //THE GAMEOBJECT THAT THIS SCRIPT IS ATTACHED TO IS CREATED WHEN THE GAME IS LOADED, AND IS ALWAYS KEPT BETWEEN SCENES.
{
    public bool isPaused;
    PowerBehaviour powerController;
    GameObject healthText, powersText;
    Text health, activepowers;
    Player player;
    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Game")
        {
            //Set GameObjects from game scene.
            powerController = GameObject.Find("PowerHandler").GetComponent<PowerBehaviour>();
            player = GameObject.Find("Player").GetComponent<Player>();
            GameObject healthText = GameObject.Find("PlayerHealth");
            health = healthText.GetComponent<Text>();
            GameObject powersText = GameObject.Find("ModifiersText");
            activepowers = powersText.GetComponent<Text>();

            //Modify where needed.
            activepowers.text = powerController.ModifierText();
            health.text = getUpdatePlayerHP();
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
    void startGameEasy()
    {
        powerController.StartGameEasy();
        
    }
    void startGameMedium()
    {
        powerController.StartGameNormal();
    }
    void startGameHard()
    {
        powerController.StartGameHard();
    }
    void startGameExpert()
    {
        powerController.StartGameExpert();
    }
    //Satanic difficulty is currently planned as an MAP and may not be added. The trigger will be added, however.
    void startGameSatanic()
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



    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Game") //Ensure player is in main game when checking for pause
        {
            if (Input.GetKeyDown(KeyCode.Escape) == true)
            {
                if (isPaused == false)
                {
                    Debug.Log("Escape pressed, game paused!");
                    //Code here to trigger UI
                    isPaused = true;
                    return;
                }
                else if (isPaused == true)
                {
                    Debug.Log("Escape pressed, game resumed!");
                    //Code here to hide UI again
                    isPaused = false;
                    return;
                }

            }
            else
            {
                health.text = getUpdatePlayerHP(); //Update health every frame.
            }


        }
        else 
        { 
          
        }
    }
}
