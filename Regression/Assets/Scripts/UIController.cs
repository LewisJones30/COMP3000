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
    public GameObject activePowers, healthObj, UIWaveComplete;
    [Tooltip("This array should be the same length as the number of powers.")]
    [SerializeField]
    Sprite[] sprites;
    [SerializeField]
    GameObject[] spriteUIElements;
    [SerializeField]
    GameObject PowerDrain1, PowerDrain2, powerName1, powerName2, powerDesc1, powerDesc2;
    [SerializeField]
    GameObject summaryDifficulty, summaryPoints, summaryWave;
    Text health, activepowers, powerdrainedtext, UIPauseText, UIWaveCompText, UITempWaveText;
    Player player;
    bool waveCompletePause = false;
    float pointsGained;
    [SerializeField]
    Text easyScore, normalScore, hardScore, expertScore, satanicScore, currentScore;
    float escapePushCooldown = 0.5f;
    public int losePower1, losePower2; //Storage for losing powers
    [SerializeField]
    GameObject postObj, postWave, postDifficulty, postFinal, postPowersRemaining, postPowersDrained, HPBar;
    double currentHPercentage;
    [Space(10)]
    //UI Controller tracking stages.
    [SerializeField]
    GameObject WelcomeImage, WelcomeText, JumpingArrow;//Part 1 of tutorial.
    int TutorialStage = 1; //Track state of the tutorial
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
        UIWaveComplete = GameObject.Find("WaveCompleteText");


        //Get component section





        //Enable/Disable section

        powerdrainedtext.enabled = false;

        //Modify values section
        //ShowAllPowersInGame();
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
            HPBarMethod(health, player.getMaxHP());
            string returnValue = "HP: " + Convert.ToString(health);
            return returnValue;
        }
        return "HP: ";

    }
    void HPBarMethod(double health, double maxHealth)
    {
        double HealthPercentage = health / maxHealth;
        if (HealthPercentage == currentHPercentage)
        {
            return;
        }
        else
        {
            HPBar.GetComponent<Image>().fillAmount = (float)HealthPercentage;
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

        foreach (GameObject obj in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
        {
            if (obj.name == "BackgroundPowerDrain")
            {
                obj.SetActive(true);
                break;
            }
        }
        isPaused = true;
        waveCompletePause = true;
        yield return new WaitForSeconds(7.5f);
        GameObject.Find("BackgroundPowerDrain").SetActive(false);
        if (PlayerPrefs.GetInt("DifficultyChosen") <= 3) //Player can drain powers on Easy, Normal & Hard.
        {
            PowerDrainScreen();
        }

        
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
        PausedSummaryScreen();
        
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

    private void PausedSummaryScreen()
    {
        //Get current points
        summaryPoints.GetComponent<Text>().text = "Current Points: " + pointsGained;

        //Get current wave
        GameObject progression = GameObject.Find("ProgressionHandler");
        Progression progObj = progression.GetComponent<Progression>();
        summaryWave.GetComponent<Text>().text = "Current Wave: " + progObj.GetCurrentWave();
        //Get current difficulty
        int difficultyLevel = PlayerPrefs.GetInt("DifficultyChosen");
        switch (difficultyLevel)
        {
            case 1:
                {
                    summaryDifficulty.GetComponent<Text>().text = "Current Difficulty: Easy"; 
                    return;
                }
            case 2:
                {
                    summaryDifficulty.GetComponent<Text>().text = "Current Difficulty: Normal";
                    return;
                }
            case 3:
                {
                    summaryDifficulty.GetComponent<Text>().text = "Current Difficulty: Hard";
                    return;
                }
            case 4:
                {
                    summaryDifficulty.GetComponent<Text>().text = "Current Difficulty: Expert";
                    return;
                }
            case 5:
                {
                    summaryDifficulty.GetComponent<Text>().text = "Current Difficulty: Satanic";
                    return;
                }
            case 6:
                {
                    summaryDifficulty.GetComponent<Text>().text = "You are currently in the tutorial.";
                    return;
                }
            default:
                {
                    summaryDifficulty.GetComponent<Text>().text = "An error has occurred.";
                    return;
                }
        }


    }

    //Tutorial code


    /// <summary>
    /// This is the code where the tutorial is programmed. TutorialStage is used to track the current position, and run specific code involved in the flow of the tutorial.
    /// Depending on the current TutorialStage depends on how far through the user is. 
    /// The stages are listed below:
    /// 
    /// 
    /// 0 - Player not in tutorial. A check is determined as to whether the player has been in the tutorial before.
    /// 1 - Player has not done tutorial before. Tutorial code is therefore run when the player loads into the game. This greets the player with the welcome screen.
    /// 2 - Awaiting user to press left click to continue. Advanced in update loop.
    /// 3 - Playing animation for next section.
    /// 4 - Awaiting user to press left click to continue.
    /// 5 - Health bar introduction.
    /// 6 - Awaiting user to press left click to continue.
    /// 7 - Powers information.
    /// 8 - Awaiting user to press left click to continue.
    /// 9 - Points information.
    /// 10 - Awaiting user to press left click to continue.
    /// </summary>

    private void DisableUIForTutorial()
    {
        foreach (GameObject obj in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
        {
            if (obj.tag == "In-GameUI")
            {
                obj.SetActive(false);
                return;
            }
        }
    }
    private void DisableTutorialUI()
    {
        foreach (GameObject obj in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
        {
            if (obj.tag == "In-GameUI")
            {
                obj.SetActive(false);
                return;
            }
        }
    }
    private void Tutorial()
    {
        if (TutorialStage == 1)
        {
            isPaused = true;
            TutorialStage = 2;
            WelcomeImage.SetActive(true);
            WelcomeText.SetActive(true);
            WelcomeText.GetComponent<Text>().text = "Welcome to Regression!\nPlease press left click to continue.";
            WelcomeText.transform.localPosition = new Vector3(-244, 108, 0);

        }
        if (TutorialStage == 3)
        {
            StartCoroutine("TutState2");
        }
        if (TutorialStage == 5)
        {

            StartCoroutine("TutState5");
        }
        if (TutorialStage == 7)
        {
            StartCoroutine("TutState7");
        }

    }
    IEnumerator TutState2()
    {
        Animation anim = WelcomeImage.GetComponent<Animation>();
        anim["ExpandTB1"].wrapMode = WrapMode.Once;
        anim.Play("ExpandTB1");
        Animation animText = WelcomeText.GetComponent<Animation>();
        animText["TextDisappear"].wrapMode = WrapMode.Once;
        animText.Play("TextDisappear");
        yield return new WaitForSeconds(0.5f);
        WelcomeText.GetComponent<Text>().text = "In this game, you start with buffs known as powers.\n" +
            "You will be fighting hordes of monsters from the depths of hell.\n" +
            "As you progress, Hell's influence over you will increase, and you will\n" +
            "be forced to channel this into your powers, losing their effect.\n" +
            "In this tutorial, you will be able to understand the basic premise\n" +
            "of this game.\n\n" +
            "Please press left click to continue.";
        WelcomeText.transform.localPosition = new Vector3(-566, 311, 0);
        animText["TextAppear"].wrapMode = WrapMode.Once;
        animText.Play("TextAppear");
        TutorialStage = 4;
    }
    IEnumerator TutState5()
    {

        Animation anim = WelcomeImage.GetComponent<Animation>();
        Animation animText = WelcomeText.GetComponent<Animation>();
        anim["FadeTB1"].wrapMode = WrapMode.Once;
        anim.Play("FadeTB1");
        animText.Play("TextDisappear");
        WelcomeImage.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        Player player = GameObject.Find("Player").GetComponent<Player>();
        player.SpawnSpecificWeapon(1); //Staff is ID 1.
        ResumeButton();
        WelcomeImage.transform.localScale = new Vector3(17.72895f, 1.921767f, 2.03252f);
        WelcomeImage.transform.localPosition = new Vector3(-18, -210, 0);
        WelcomeText.transform.localPosition = new Vector3(-412, -162, 0);
        WelcomeText.GetComponent<Text>().text = "This is your health bar. Make sure it doesn't reach 0!";
        anim["ShowTB1"].wrapMode = WrapMode.Once;
        anim.Play("ShowTB1");
        animText.Play("TextAppear");
        JumpingArrow.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        TutorialStage = 6;
    }
    IEnumerator TutState7()
    {
        Animation anim = WelcomeImage.GetComponent<Animation>();
        Animation animText = WelcomeText.GetComponent<Animation>();
        Animation animArrow = JumpingArrow.GetComponent<Animation>();
        animText.Play("TextDisappear");
        animArrow.Stop();
        animArrow.Play("jumping arrow disappear");
        yield return new WaitForSeconds(0.5f);
        JumpingArrow.transform.localPosition = new Vector3(-481, -8, 0);
        JumpingArrow.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, -90));
        WelcomeText.transform.localPosition = new Vector3(-318, 42, 0);
        animText.Play("TextAppear");
        animArrow["jumping arrow appear"].wrapMode = WrapMode.Once;
        animArrow.Play("jumping arrow appear");
        yield return new WaitForSeconds(0.5f);
        animArrow.Play("Jumping arrow left");
        TutorialStage = 8;

    }















































    public void GameSummaryScreen()
    {
        Cursor.lockState = CursorLockMode.None;
        postObj.SetActive(true);
        currentScore.gameObject.SetActive(false); //Set the currentscore obj to false.
        //Get current wave ID.
        GameObject progression = GameObject.Find("ProgressionHandler");
        Progression progObj = progression.GetComponent<Progression>();
        postWave.GetComponent<Text>().text = "Died on wave: " + progObj.GetCurrentWave();
        //Get the final score
        postFinal.GetComponent<Text>().text = "Final score: " + pointsGained;
        //Get the number of powers drained
        postPowersDrained.GetComponent<Text>().text = "Powers drained: " + powerController.GetPowersDrainedCount();
        //Get the number of powers remaining
        int powersRemaining = 13 - powerController.GetPowersDrainedCount();
        postPowersRemaining.GetComponent<Text>().text = "Powers remaining: " + powersRemaining;

        
        //Get current difficulty
        int difficultyLevel = PlayerPrefs.GetInt("DifficultyChosen");
        switch (difficultyLevel)
        {
            case 1:
                {
                    postDifficulty.GetComponent<Text>().text = "Current Difficulty: Easy";
                    if (pointsGained > PlayerPrefs.GetInt("HighScoreEasy"))
                    {
                        postFinal.GetComponent<Text>().text = "Final score: " + pointsGained + " (NEW HIGHSCORE!)";
                    }
                    return;
                }
            case 2:
                {
                    postDifficulty.GetComponent<Text>().text = "Current Difficulty: Normal";
                    if (pointsGained > PlayerPrefs.GetInt("HighScoreNormal"))
                    {
                        postFinal.GetComponent<Text>().text = "Final score: " + pointsGained + " (NEW HIGHSCORE!)";
                    }
                    return;
                }
            case 3:
                {
                    postDifficulty.GetComponent<Text>().text = "Current Difficulty: Hard";
                    if (pointsGained > PlayerPrefs.GetInt("HighScoreHard"))
                    {
                        postFinal.GetComponent<Text>().text = "Final score: " + pointsGained + " (NEW HIGHSCORE!)";
                    }
                    return;
                }
            case 4:
                {
                    postDifficulty.GetComponent<Text>().text = "Current Difficulty: Expert";
                    if (pointsGained > PlayerPrefs.GetInt("HighScoreExpert"))
                    {
                        postFinal.GetComponent<Text>().text = "Final score: " + pointsGained + " (NEW HIGHSCORE!)";
                    }
                    return;
                }
            case 5:
                {
                    postDifficulty.GetComponent<Text>().text = "Current Difficulty: Satanic";
                    if (pointsGained > PlayerPrefs.GetInt("HighScoreSatanic"))
                    {
                        postFinal.GetComponent<Text>().text = "Final score: " + pointsGained + " (NEW HIGHSCORE!)";
                    }
                    return;
                }
            case 6:
                {
                    postDifficulty.GetComponent<Text>().text = "You are currently in the tutorial.";
                    return;
                }
            default:
                {
                    postDifficulty.GetComponent<Text>().text = "An error has occurred.";
                    return;
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

    public void PowerDrainScreen() //Will be called by the Progression handler when the player completes a wave.
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
        powerDesc1.GetComponent<Text>().text = p.PowerDescription;
        p = powerController.RandomPower();
        losePower2 = p.ID;
        while (losePower1 ==losePower2)
        {
            p = powerController.RandomAvailablePower();
            losePower2 = p.ID;
        }
        PowerDrain2.GetComponent<Image>().sprite = sprites[losePower2];
        powerDesc2.GetComponent<Text>().text = p.PowerDescription;
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
        Cursor.lockState = CursorLockMode.Locked;
        ShowAllPowersInGame();

    }

    public void ResumeButton()
    {
        if (TutorialStage > 0)
        {
            GetGameObjectsGame();
            GameUnpausedFunction();
            currentScore.text = "Points: " + pointsGained;
            ShowAllPowersInGame();
            return;
        }
        isPaused = false;
        GetGameObjectsGame();
        Debug.Log("Game resumed!");
        GameUnpausedFunction();
        Cursor.lockState = CursorLockMode.Locked;
        currentScore.text = "Points: " + pointsGained;
        ShowAllPowersInGame();
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
        if (highScore == 0)
        {
            if (easyScore == null)
            {
                easyScore = GameObject.Find("HighScoreTextEasy").GetComponent<Text>();
            }
            easyScore.text = "No highscore!";
        }
        else
        {
            if (easyScore == null)
            {
                easyScore = GameObject.Find("HighScoreTextEasy").GetComponent<Text>();
            }
            easyScore.text = "Highscore: " + highScore;
        }
        highScore = PlayerPrefs.GetInt("HighScoreHard");
        if (highScore == 0)
        {
            if (hardScore == null)
            {
                hardScore = GameObject.Find("HighScoreTextEasy").GetComponent<Text>();
            }
            hardScore.text = "No highscore!";
        }
        else
        {
            if (hardScore == null)
            {
                hardScore = GameObject.Find("HighScoreTextEasy").GetComponent<Text>();
            }
            hardScore.text = "Highscore: " + highScore;
        }

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
            if (TutorialStage > 0)
            {
                if (TutorialStage == 1)
                {
                    DisableUIForTutorial();
                    Tutorial();
                }
                if (TutorialStage == 2 && Input.GetMouseButton(0))
                {
                    TutorialStage = 3;
                    Tutorial();
                }
                if (TutorialStage == 4 && Input.GetMouseButton(0))
                {
                    TutorialStage = 5;
                    Tutorial();
                }
                if (TutorialStage == 6 && Input.GetMouseButton(0))
                {
                    TutorialStage = 7;
                    Tutorial();
                }
                return;
            }
            if (Input.GetKeyDown(KeyCode.F10) == true)
            {
                WaveCompleteText();
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
