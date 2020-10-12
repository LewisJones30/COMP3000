using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour //THE GAMEOBJECT THAT THIS SCRIPT IS ATTACHED TO IS CREATED WHEN THE GAME IS LOADED, AND IS ALWAYS KEPT BETWEEN SCENES.
{
    public bool isPaused;
    PowerBehaviour powerController;
    // Start is called before the first frame update
    void Start()
    {
        powerController = GameObject.Find("PowerController").GetComponent<PowerBehaviour>(); //First, obtain the powerController.
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




    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "MainGame") //Ensure player is in main game when checking for pause
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
        }
    }
}
