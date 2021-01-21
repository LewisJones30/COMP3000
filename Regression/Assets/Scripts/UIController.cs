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
    public PowerBehaviour powerController;
    public GameObject activePowers, healthObj, UIPause, UIWaveComplete;
    [Tooltip("This array should be the same length as the number of powers.")]
    [SerializeField]
    Sprite[] sprites;
    [SerializeField]
    GameObject[] spriteUIElements;
    [SerializeField]
    GameObject PowerDrain1, PowerDrain2, powerName1, powerName2;
    Text health, activepowers, powerdrainedtext, UIPauseText, UIWaveCompText, UITempWaveText;
    Player player;
    bool waveCompletePause = false;
    float pointsGained;
    [SerializeField]
    Text easyScore, normalScore, hardScore, expertScore, satanicScore, currentScore;
    float escapePushCooldown = 0.5f;
    public int losePower1, losePower2; //Storage for losing powers
    // Start is called before the first frame update
    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;


        if (SceneManager.GetActiveScene().name == "Game")
        {
            WeaponSelection();

        }
        else
        {
            LoadHighscores();
        }
    }
    void GetGameObjectsGame()
    {
        //Firstly, get the gameobjects.
        foreach(GameObject obj in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
        {
            if (obj.name == "PlayerHealth")
            {
                GameObject healthText = obj;
                health = healthText.GetComponent<Text>();
            }
            else if (obj.name == "PowerDrainedMessage")
            {
                GameObject PowerDrainedMessage = obj;
                powerdrainedtext = PowerDrainedMessage.GetComponent<Text>();
            }
            else if (obj.name == "PointsText")
            {
                currentScore = obj.GetComponent<Text>();
            }
        }

        //Find GameObject section.

        powerController = GameObject.Find("PowerHandler").GetComponent<PowerBehaviour>();
        player = GameObject.Find("Player").GetComponent<Player>();
        UIPause = GameObject.Find("UIPauseText");
        UIWaveComplete = GameObject.Find("WaveCompleteText");


        //Get component section




        UIWaveCompText = UIWaveComplete.GetComponent<Text>();

        //Enable/Disable section

        powerdrainedtext.enabled = false;
        UIWaveCompText.enabled = false;

        //Modify values section
        //UITempWaveText.text = "Wave 1/2";
        ShowAllPowersInGame();
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
        if (health != null)
        {
            double health = player.health;
            health = Math.Round(health);
            string returnValue = "HP: " + Convert.ToString(health);
            return returnValue;
        }
        return "HP: ";

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
        ShowAllPowersInGame();
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
        storeHighscores(powerController.difficultyLevel);
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
        PausedUIPowersOpacity();

        
    }

    private void PausedUIPowersOpacity() //Set the opacity of the Powers 1-15 images. If they are disabled, set a lower opacity.
    //The HoverOverScript.cs controls the information on the UI screen.
    {
        //Check through all powers 1-14 and update the opacity if they are FALSE.
        Image j;
        for (int i = 0; i < 14; i++)
        {
            if (powerController.powerHandler[i].PowerActive == false)
            {
                float opacityValue = 0.25f;
                switch (i)
                {
                    case 0:
                        {
                            Image j0 = GameObject.Find("Power1").GetComponent<Image>(); //First, get the image of the power.
                            var ColorChange = j0.color; //Store the current colour info in a temporary variable.
                            ColorChange.a = opacityValue; //Update the alpha of the temp variable to be 0.5.
                            j0.color = ColorChange; //Apply the color change.
                            break;

                        }
                    case 1:
                        {
                            Image j1 = GameObject.Find("Power2").GetComponent<Image>(); //First, get the image of the power.
                            var ColorChange = j1.color; //Store the current colour info in a temporary variable.
                            ColorChange.a = opacityValue; //Update the alpha of the temp variable to be 0.5.
                            j1.color = ColorChange; //Apply the color change.
                            break;
                        }
                    case 2:
                        {
                            Image j2 = GameObject.Find("Power3").GetComponent<Image>(); //First, get the image of the power.
                            var ColorChange = j2.color; //Store the current colour info in a temporary variable.
                            ColorChange.a = opacityValue; //Update the alpha of the temp variable to be 0.5.
                            j2.color = ColorChange; //Apply the color change.
                            break;
                        }
                    case 3:
                        {
                            Image j3 = GameObject.Find("Power4").GetComponent<Image>(); //First, get the image of the power.
                            var ColorChange = j3.color; //Store the current colour info in a temporary variable.
                            ColorChange.a = opacityValue; //Update the alpha of the temp variable to be 0.5.
                            j3.color = ColorChange; //Apply the color change.
                            break;
                        }
                    case 4:
                        {
                            Image j4 = GameObject.Find("Power5").GetComponent<Image>(); //First, get the image of the power.
                            var ColorChange = j4.color; //Store the current colour info in a temporary variable.
                            ColorChange.a = opacityValue; //Update the alpha of the temp variable to be 0.5.
                            j4.color = ColorChange; //Apply the color change.
                            break;
                        }
                    case 5:
                        {
                            Image j5 = GameObject.Find("Power6").GetComponent<Image>(); //First, get the image of the power.
                            var ColorChange = j5.color; //Store the current colour info in a temporary variable.
                            ColorChange.a = opacityValue; //Update the alpha of the temp variable to be 0.5.
                            j5.color = ColorChange; //Apply the color change.
                            break;
                        }
                    case 6:
                        {
                            Image j6 = GameObject.Find("Power7").GetComponent<Image>(); //First, get the image of the power.
                            var ColorChange = j6.color; //Store the current colour info in a temporary variable.
                            ColorChange.a = opacityValue; //Update the alpha of the temp variable to be 0.5.
                            j6.color = ColorChange; //Apply the color change.
                            break;
                        }
                    case 7:
                        {
                            Image j7 = GameObject.Find("Power8").GetComponent<Image>(); //First, get the image of the power.
                            var ColorChange = j7.color; //Store the current colour info in a temporary variable.
                            ColorChange.a = opacityValue; //Update the alpha of the temp variable to be 0.5.
                            j7.color = ColorChange; //Apply the color change.
                            break;
                        }
                    case 8:
                        {
                            Image j8 = GameObject.Find("Power9").GetComponent<Image>(); //First, get the image of the power.
                            var ColorChange = j8.color; //Store the current colour info in a temporary variable.
                            ColorChange.a = opacityValue; //Update the alpha of the temp variable to be 0.5.
                            j8.color = ColorChange; //Apply the color change.
                            break;
                        }
                    case 9:
                        {
                            Image j9 = GameObject.Find("Power10").GetComponent<Image>(); //First, get the image of the power.
                            var ColorChange = j9.color; //Store the current colour info in a temporary variable.
                            ColorChange.a = opacityValue; //Update the alpha of the temp variable to be 0.5.
                            j9.color = ColorChange; //Apply the color change.
                            break;
                        }
                    case 10:
                        {
                            Image j10 = GameObject.Find("Power11").GetComponent<Image>(); //First, get the image of the power.
                            var ColorChange = j10.color; //Store the current colour info in a temporary variable.
                            ColorChange.a = opacityValue; //Update the alpha of the temp variable to be 0.5.
                            j10.color = ColorChange; //Apply the color change.
                            break;
                        }
                    case 11:
                        {
                            Image j11 = GameObject.Find("Power12").GetComponent<Image>(); //First, get the image of the power.
                            var ColorChange = j11.color; //Store the current colour info in a temporary variable.
                            ColorChange.a = opacityValue; //Update the alpha of the temp variable to be 0.5.
                            j11.color = ColorChange; //Apply the color change.
                            break;
                        }
                    case 12:
                        {
                            Image j12 = GameObject.Find("Power13").GetComponent<Image>(); //First, get the image of the power.
                            var ColorChange = j12.color; //Store the current colour info in a temporary variable.
                            ColorChange.a = opacityValue; //Update the alpha of the temp variable to be 0.5.
                            j12.color = ColorChange; //Apply the color change.
                            break;
                        }
                    case 13:
                        {
                            Image j13 = GameObject.Find("Power14").GetComponent<Image>(); //First, get the image of the power.
                            var ColorChange = j13.color; //Store the current colour info in a temporary variable.
                            ColorChange.a = opacityValue; //Update the alpha of the temp variable to be 0.5.
                            j13.color = ColorChange; //Apply the color change.
                            break;
                        }
                    case 14:
                        {
                            //Currently unused.
                            Image j14 = GameObject.Find("Power15").GetComponent<Image>(); //First, get the image of the power.
                            var ColorChange = j14.color; //Store the current colour info in a temporary variable.
                            ColorChange.a = opacityValue; //Update the alpha of the temp variable to be 0.5.
                            j14.color = ColorChange; //Apply the color change.
                            break;
                        }
                    default: //This should never be hit!
                        {
                            Debug.Log("ERROR. Default triggered in PausedUIOpacity script!");
                            break;
                        }


                }


               
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
    private void PowerDrainScreen()
    {
        Cursor.lockState = CursorLockMode.None;
        foreach (GameObject obj in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
        {
            if (obj.tag == "RemoveWhenPaused")
            {
                obj.SetActive(false);
            }
            if (obj.tag == "LosePowerScreen")
            {
                obj.SetActive(true);
            }
        }
        isPaused = true;
        PowerBehaviour.Power p = powerController.RandomAvailablePower();
        losePower1 = p.ID;
        PowerDrain1.GetComponent<Image>().sprite = sprites[losePower1];
        powerName1.GetComponent<Text>().text = p.PowerName;
        p = powerController.RandomPower();
        losePower2 = p.ID;
        while (losePower1 ==losePower2)
        {
            p = powerController.RandomAvailablePower();
            losePower2 = p.ID;
        }
        PowerDrain2.GetComponent<Image>().sprite = sprites[losePower2];
        powerName2.GetComponent<Text>().text = p.PowerName;

    }
    //Called by ButtonCaller
    public void returnToMainGame()
    {
        isPaused = false;
        foreach (GameObject obj in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
        {
            if (obj.tag == "RemoveWhenPaused")
            {
                obj.SetActive(true);
            }
            if (obj.tag == "LosePowerScreen")
            {
                obj.SetActive(false);
            }
        }
        Cursor.lockState = CursorLockMode.Confined;
        ShowAllPowersInGame();

    }

    public void ResumeButton()
    {
        isPaused = false;
        GetGameObjectsGame();
        Debug.Log("Game resumed!");
        GameUnpausedFunction();
        Cursor.lockState = CursorLockMode.Locked;
        currentScore.text = "Points: " + pointsGained;
        //ShowAllPowersInGame();

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

    public void ShowAllPowersInGame()
    {
        int ObjsFilled = 0;
        for (int i = 0; i < spriteUIElements.Length; i++)
        {
            spriteUIElements[i].GetComponent<Image>().enabled = false;
        }
        for (int i = 0; i < powerController.powerHandler.Length; i++)
        {
            if (powerController.powerHandler[i].PowerActive == true)
            {
                spriteUIElements[ObjsFilled].GetComponent<Image>().sprite = sprites[i];
                spriteUIElements[ObjsFilled].GetComponent<Image>().enabled = true;
                ObjsFilled = ObjsFilled + 1;
            }
        }
    }








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
        if (Input.GetKeyDown(KeyCode.Escape) == true)
        {
            if (isPaused == false)
            {
                isPaused = true;
                Debug.Log("Escape pressed, game paused!");
                GamePausedFunction();
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                isPaused = false;
                Debug.Log("Game resumed!");
                GameUnpausedFunction();
                Cursor.lockState = CursorLockMode.Locked;

            }
        }

    }
    //Used when the game moves to another scene.
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "UI Scale Testing")
        {
            LoadHighscores();
        }
        else
        {
            GetGameObjectsGame();
            powerController.gameStarted = false;
        }

    }

    //Points modifiers======================================
    public void AddPoints(float pointsToAdd)
    {
        pointsToAdd = pointsToAdd * player.pointsModifier;
        pointsGained = pointsGained + pointsToAdd;
        if (currentScore != null)
        {
            currentScore.text = "Points: " + pointsGained;
        }

    }
    public void ResetPoints()
    {
        pointsGained = 0;
    }
    public void removePoints(float pointsToRemove)
    {
        pointsGained = pointsGained - pointsToRemove;
        if (pointsGained < 0)
        {
            pointsGained = 0;
        }
    }
    void LoadHighscores()
    {
        int highScore = PlayerPrefs.GetInt("HighScoreEasy");
        easyScore.text = "Highscore: " + highScore;

    }
    public void storeHighscores(int difficulty) //Pass in difficulty, find relevant highscore and complete the check.
    {
        switch (difficulty)
        {
            case 1:
            {
                    int playerHighScore1 = PlayerPrefs.GetInt("HighScoreEasy");
                    if (playerHighScore1 > pointsGained)
                    {
                        int roundedScore = Mathf.FloorToInt(pointsGained);
                        PlayerPrefs.SetInt("HighScoreEasy", roundedScore);
                        
                    }
                    return;
            }
            case 2:
                {
                    int playerHighScore2 = PlayerPrefs.GetInt("HighScoreNormal");
                    if (playerHighScore2 > pointsGained)
                    {
                        int roundedScore = Mathf.FloorToInt(pointsGained);
                        PlayerPrefs.SetInt("HighScoreEasy", roundedScore);

                    }
                    return;
                }
            case 3:
                int playerHighScore3 = PlayerPrefs.GetInt("HighScoreHard");
                if (playerHighScore3 > pointsGained)
                {
                    int roundedScore = Mathf.FloorToInt(pointsGained);
                    PlayerPrefs.SetInt("HighScoreEasy", roundedScore);

                }
                return;
            case 4:
                int playerHighScore4 = PlayerPrefs.GetInt("HighScoreExpert");
                if (playerHighScore4 > pointsGained)
                {
                    int roundedScore = Mathf.FloorToInt(pointsGained);
                    PlayerPrefs.SetInt("HighScoreEasy", roundedScore);

                }
                return;
            case 5:
                int playerHighScore5 = PlayerPrefs.GetInt("HighScoreSatanic");
                if (playerHighScore5 > pointsGained)
                {
                    int roundedScore = Mathf.FloorToInt(pointsGained);
                    PlayerPrefs.SetInt("HighScoreEasy", roundedScore);

                }
                return;

        }
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
            if (Input.GetKeyDown(KeyCode.F10) == true)
            {
                //DEBUG.
                PowerDrainScreen();
            }
            if (Input.GetKeyDown(KeyCode.Escape) == true)
            {
               if (isPaused == false)
               {
                   escapePushCooldown = 0;
                   isPaused = true;
                   Debug.Log("Escape pressed, game paused!");
                   GamePausedFunction();
                   Cursor.lockState = CursorLockMode.None;
               }
               else if (escapePushCooldown > 0.5f)
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
            if (health != null)
            {
                health.text = getUpdatePlayerHP(); //Update health every frame.
            }

        }





    }
}
