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
    GameObject WelcomeImage, WelcomeText, JumpingArrow, Enemy1, Enemy2;//Part 1 of tutorial.
    int TutorialStage = 0; //Track state of the tutorial
    bool inPauseMenu = false;
    bool lockPauseMenu = false; //Used while certain windows are open to ensure pause menu will NOT load.
    [SerializeField]
    GameObject fireWarning;
    [SerializeField]
    GameObject expertDrainPowerImage, expertDrainPowerNameText, expertDrainPowerNameText2; //Expert difficulty drain
    [Space(5)]
    [SerializeField]
    Text firstPlace, secondPlace, thirdPlace, fourthPlace, fifthPlace, titleText;
    [SerializeField]
    GameObject leaderboardObj, leaderboardTextObj;
    // Start is called before the first frame update
    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        if (expertDrainPowerImage != null)
        {
            expertDrainPowerImage.SetActive(false);
            expertDrainPowerNameText.SetActive(false);
            expertDrainPowerNameText2.SetActive(false);
        }

        if (SceneManager.GetActiveScene().name == "Game")
        {
            WeaponSelection();
        }
        if (SceneManager.GetActiveScene().name == "Leaderboards")
        {
            loadLeaderboardEasy();
        }

    }
    void GetGameObjectsGame()
    {
        if (SceneManager.GetActiveScene().name == "Leaderboards")
        {
            return;
        }
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

        if (leaderboardObj != null)
        {
            leaderboardObj.SetActive(false);
        }



        //Enable/Disable section


        //Modify values section
        //ShowAllPowersInGame();
    }

    public void WeaponSelection()
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
    public string getUpdatePlayerHP()
    {
        if (health != null)
        {
            double health = player.health;
            health = Math.Round(health);
            if (health < 0)
            {
                health = 0;
            }
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
            healthObj.GetComponent<Text>().text = Convert.ToString(health);
        }

    }
    public void setLockPauseMenu(bool setting)
    {
        lockPauseMenu = setting;
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
        if (expertDrainPowerImage.activeInHierarchy == true || expertDrainPowerNameText.activeInHierarchy == true || expertDrainPowerNameText2.activeInHierarchy == true)
        {
            yield return new WaitForSeconds(5);
        }
        expertDrainPowerImage.SetActive(true);
        expertDrainPowerNameText.SetActive(true);
        expertDrainPowerNameText2.SetActive(true);
        expertDrainPowerImage.GetComponent<Image>().sprite = sprites[PowerDrainedID];
        expertDrainPowerNameText.GetComponent<Text>().text = powerController.powerHandler[PowerDrainedID].PowerName;
        expertDrainPowerNameText2.GetComponent<Text>().text = "Removed";
        Animation imageAnim = expertDrainPowerImage.GetComponent<Animation>();
        Animation text1Anim = expertDrainPowerNameText.GetComponent<Animation>();
        Animation text2Anim = expertDrainPowerNameText2.GetComponent<Animation>();
        imageAnim.Play("ExpertImageAppear");
        text1Anim.Play("ExpertText1Anim");
        text2Anim.Play("ExpertTextAnim2");
        ShowAllPowersInGame();
        yield return new WaitForSeconds(4);
        imageAnim.Play("ExpertImageDisappear");
        text1Anim.Play("ExpertText1Disappear");
        text2Anim.Play("ExpertText2Anim");
        yield return new WaitForSeconds(1f);
        switch (PlayerPrefs.GetInt("DifficultyChosen"))
        {
            case 1:
                {
                    if (player.getEasyWeaponChoice() == false)
                    {
                        int i = player.SpawnRandomWeapon();
                        if (i == 1)
                        {
                            expertDrainPowerImage.GetComponent<Image>().sprite = sprites[6];
                            expertDrainPowerNameText.GetComponent<Text>().text = "You are now using the staff!";
                            imageAnim.Play("ExpertImageAppear");
                            text1Anim.Play("ExpertText1Anim");
                            yield return new WaitForSeconds(2f);
                            imageAnim.Play("ExpertImageDisappear");
                            text1Anim.Play("ExpertText1Disappear");
                        }
                        if (i == 0)
                        {
                            expertDrainPowerImage.GetComponent<Image>().sprite = sprites[8];
                            expertDrainPowerNameText.GetComponent<Text>().text = "You are now using the sword!";
                            imageAnim.Play("ExpertImageAppear");
                            text1Anim.Play("ExpertText1Anim");
                            yield return new WaitForSeconds(2f);
                            imageAnim.Play("ExpertImageDisappear");
                            text1Anim.Play("ExpertText1Disappear");
                        }
                    }
                    else
                    {
                        Progression p1 = GameObject.Find("ProgressionHandler").GetComponent<Progression>();
                        expertDrainPowerNameText.GetComponent<Text>().text = "Wave " + p1.GetCurrentWave();
                        text1Anim.Play("ExpertText1Anim");
                        yield return new WaitForSeconds(3f);
                        text1Anim.Play("ExpertText1Disappear");
                        WeaponSelection();
                    }
                    break;
                }
            case 2:
                {
                    if (player.getNormalWeaponChoice() == false)
                    {
                        int i = player.SpawnRandomWeapon();
                        if (i == 0)
                        {
                            expertDrainPowerImage.GetComponent<Image>().sprite = sprites[6];
                            expertDrainPowerNameText.GetComponent<Text>().text = "You are now using the staff!";
                            imageAnim.Play("ExpertImageAppear");
                            text1Anim.Play("ExpertText1Anim");
                            yield return new WaitForSeconds(2f);
                            imageAnim.Play("ExpertImageDisappear");
                            text1Anim.Play("ExpertText1Disappear");
                        }
                        if (i == 1)
                        {
                            expertDrainPowerImage.GetComponent<Image>().sprite = sprites[8];
                            expertDrainPowerNameText.GetComponent<Text>().text = "You are now using the sword!";
                            imageAnim.Play("ExpertImageAppear");
                            text1Anim.Play("ExpertText1Anim");
                            yield return new WaitForSeconds(2f);
                            imageAnim.Play("ExpertImageDisappear");
                            text1Anim.Play("ExpertText1Disappear");
                        }
                    }
                    else
                    {
                        Progression p1 = GameObject.Find("ProgressionHandler").GetComponent<Progression>();
                        expertDrainPowerNameText.GetComponent<Text>().text = "Wave " + p1.GetCurrentWave();
                        text1Anim.Play("ExpertText1Anim");
                        yield return new WaitForSeconds(3f);
                        text1Anim.Play("ExpertText1Disappear");
                        WeaponSelection();
                    }
                    break;
                }
            case 3:
                {
                    if (player.getHardWeaponChoice() == false)
                    {
                        int i = player.SpawnRandomWeapon();
                        if (i == 0)
                        {
                            expertDrainPowerImage.GetComponent<Image>().sprite = sprites[6];
                            expertDrainPowerNameText.GetComponent<Text>().text = "You are now using the staff!";
                            imageAnim.Play("ExpertImageAppear");
                            text1Anim.Play("ExpertText1Anim");
                            yield return new WaitForSeconds(2f);
                            imageAnim.Play("ExpertImageDisappear");
                            text1Anim.Play("ExpertText1Disappear");
                        }
                        if (i == 1)
                        {
                            expertDrainPowerImage.GetComponent<Image>().sprite = sprites[8];
                            expertDrainPowerNameText.GetComponent<Text>().text = "You are now using the sword!";
                            imageAnim.Play("ExpertImageAppear");
                            text1Anim.Play("ExpertText1Anim");
                            yield return new WaitForSeconds(2f);
                            imageAnim.Play("ExpertImageDisappear");
                            text1Anim.Play("ExpertText1Disappear");
                        }
                    }
                    else
                    {
                        Progression p1 = GameObject.Find("ProgressionHandler").GetComponent<Progression>();
                        expertDrainPowerNameText.GetComponent<Text>().text = "Wave " + p1.GetCurrentWave();
                        text1Anim.Play("ExpertText1Anim");
                        yield return new WaitForSeconds(3f);
                        text1Anim.Play("ExpertText1Disappear");
                        yield return new WaitForSeconds(1f);
                        Cursor.lockState = CursorLockMode.None;
                        WeaponSelection();
                    }
                    break;
                }
            case 4:
                {
                    if (player.getExpertWeaponChoice() == false)
                    {
                        int i = player.SpawnRandomWeapon();
                        if (i == 0)
                        {
                            expertDrainPowerImage.GetComponent<Image>().sprite = sprites[6];
                            expertDrainPowerNameText.GetComponent<Text>().text = "You are now using the staff!";
                            imageAnim.Play("ExpertImageAppear");
                            text1Anim.Play("ExpertText1Anim");
                            yield return new WaitForSeconds(2f);
                            imageAnim.Play("ExpertImageDisappear");
                            text1Anim.Play("ExpertText1Disappear");
                        }
                        if (i == 1)
                        {
                            expertDrainPowerImage.GetComponent<Image>().sprite = sprites[8];
                            expertDrainPowerNameText.GetComponent<Text>().text = "You are now using the sword!";
                            imageAnim.Play("ExpertImageAppear");
                            text1Anim.Play("ExpertText1Anim");
                            yield return new WaitForSeconds(2f);
                            imageAnim.Play("ExpertImageDisappear");
                            text1Anim.Play("ExpertText1Disappear");
                        }
                    }
                    else
                    {
                        Progression p1 = GameObject.Find("ProgressionHandler").GetComponent<Progression>();
                        expertDrainPowerNameText.GetComponent<Text>().text = "Wave " + p1.GetCurrentWave();
                        text1Anim.Play("ExpertText1Anim");
                        yield return new WaitForSeconds(3f);
                        text1Anim.Play("ExpertText1Disappear");
                        WeaponSelection();
                    }
                    break;
                }
            default:
                {
                    break;
                }
        }
        Progression p = GameObject.Find("ProgressionHandler").GetComponent<Progression>();
        expertDrainPowerNameText.GetComponent<Text>().text = "Wave " + p.GetCurrentWave();
        text1Anim.Play("ExpertText1Anim");
        yield return new WaitForSeconds(3f);
        text1Anim.Play("ExpertText1Disappear");
        returnToMainGame();

    }

    public void callLeaderboards(int difficulty)
    {
        switch (difficulty)
        {
            case 1:
                {
                    loadLeaderboardEasy();
                    break;
                }
            case 2:
                {
                    loadLeaderboardNormal();
                    break;
                }
            case 3:
                {
                    loadLeaderboardHard();
                    break;
                }
            case 4:
                {
                    loadLeaderboardExpert();
                    break;
                }
        }
    }

    public void ShowPowerActivatedMessage(int ID)
    {
        StartCoroutine("ShowMessage1", ID);
    }
    IEnumerator ShowMessage1(int ID)
    {
        if (expertDrainPowerImage.activeInHierarchy == true || expertDrainPowerNameText.activeInHierarchy == true || expertDrainPowerNameText2.activeInHierarchy == true)
        {
            yield return new WaitForSeconds(5);
        }
        expertDrainPowerImage.SetActive(true);
        expertDrainPowerNameText.SetActive(true);
        expertDrainPowerNameText2.SetActive(true);
        Animation imageAnim = expertDrainPowerImage.GetComponent<Animation>();
        Animation text1Anim = expertDrainPowerNameText.GetComponent<Animation>();
        Animation text2Anim = expertDrainPowerNameText2.GetComponent<Animation>();
        expertDrainPowerImage.GetComponent<Image>().sprite = sprites[ID + 11];
        expertDrainPowerNameText.GetComponent<Text>().text = powerController.powerHandler[ID+11].PowerName;
        expertDrainPowerNameText2.GetComponent<Text>().text = "Activated";
        imageAnim.Play("ExpertImageAppear");
        text1Anim.Play("ExpertText1Anim");
        text2Anim.Play("ExpertTextAnim2");
        yield return new WaitForSeconds(3f);
        imageAnim.Play("ExpertImageDisappear");
        text1Anim.Play("ExpertText1Disappear");
        text2Anim.Play("ExpertText2Anim");
    }
    //=============================================Methods to control the menus================================================
    void loadSettings() //Triggered when the player clicks settings on the home page
    {

    }
    //Load leaderboards

    /*
     * LEADERBOARD PLAYERPREF SUMMARY
     * Example of each positioning
     * 1st place easy:
     * Score is stored in EasyFirstPlaceScore (GetInt)
     * Name is stored in EasyFirstPlaceName (GetString)
     * If there is no first place, default is "No score".
     * If there is no name, default is "Anonymous".
     * Note there is no name filtering system in place at this time.
     * 
     */
    void loadLeaderboardEasy()
    {
        int firstPoints, secondPoints, thirdPoints, fourthPoints, fifthPoints;
        titleText.text = "Easy";
        firstPoints = PlayerPrefs.GetInt("EasyFirstPlaceScore", 0);
        secondPoints = PlayerPrefs.GetInt("EasySecondPlaceScore", 0);
        thirdPoints = PlayerPrefs.GetInt("EasyThirdPlaceScore", 0);
        fourthPoints = PlayerPrefs.GetInt("EasyFourthPlaceScore", 0);
        fifthPoints = PlayerPrefs.GetInt("EasyFifthPlaceScore", 0);
        if (firstPoints > 0)
        {
            firstPlace.text = "First Place: " + PlayerPrefs.GetString("EasyFirstPlaceScoreName", "Anonymous") + " - " + firstPoints;
        }
        else
        {
            firstPlace.text = "No score";
            secondPlace.text = "No score";
            thirdPlace.text = "No score";
            fourthPlace.text = "No score";
            fifthPlace.text = "No score";
            return; //Finished loading.
        }
        if (secondPoints > 0)
        {
            secondPlace.text = "Second Place: " + PlayerPrefs.GetString("EasySecondPlaceScoreName", "Anonymous") + " - " + secondPoints;
        }
        else
        {
            secondPlace.text = "No score";
            thirdPlace.text = "No score";
            fourthPlace.text = "No score";
            fifthPlace.text = "No score";
            return;
        }
        if (thirdPoints > 0)
        {
            thirdPlace.text = "Third Place: " + PlayerPrefs.GetString("EasyThirdPlaceScoreName", "Anonymous") + " - " + thirdPoints;
        }
        else 
        {
            thirdPlace.text = "No score";
            fourthPlace.text = "No score";
            fifthPlace.text = "No score";
            return;
        }
        if (fourthPoints > 0)
        {
            fourthPlace.text = "Fourth Place: " + PlayerPrefs.GetString("EasyFourthPlaceScoreName", "Anonymous") + " - " + fourthPoints;
        }
        else
        {
            fourthPlace.text = "No score";
            fifthPlace.text = "No score";
            return;
        }
        if (fifthPoints > 0)
        {
            fifthPlace.text = "Fourth Place: " + PlayerPrefs.GetString("EasyFourthPlaceScoreName", "Anonymous") + " - " + fifthPoints;
        }
        else
        {
            fifthPlace.text = "No score";
            return;
        }
    }
    void loadLeaderboardNormal()
    {
        int firstPoints, secondPoints, thirdPoints, fourthPoints, fifthPoints;
        titleText.text = "Normal";
        firstPoints = PlayerPrefs.GetInt("NormalFirstPlaceScore", 0);
        secondPoints = PlayerPrefs.GetInt("NormalSecondPlaceScore", 0);
        thirdPoints = PlayerPrefs.GetInt("NormalThirdPlaceScore", 0);
        fourthPoints = PlayerPrefs.GetInt("NormalFourthPlaceScore", 0);
        fifthPoints = PlayerPrefs.GetInt("NormalFifthPlaceScore", 0);
        if (firstPoints > 0)
        {
            firstPlace.text = "First Place: " + PlayerPrefs.GetString("NormalFirstPlaceScoreName", "Anonymous") + " - " + firstPoints;
        }
        else
        {
            firstPlace.text = "No score";
            secondPlace.text = "No score";
            thirdPlace.text = "No score";
            fourthPlace.text = "No score";
            fifthPlace.text = "No score";
            return; //Finished loading.
        }
        if (secondPoints > 0)
        {
            secondPlace.text = "Second Place: " + PlayerPrefs.GetString("NormalSecondPlaceScoreName", "Anonymous") + " - " + secondPoints;
        }
        else
        {
            secondPlace.text = "No score";
            thirdPlace.text = "No score";
            fourthPlace.text = "No score";
            fifthPlace.text = "No score";
            return;
        }
        if (thirdPoints > 0)
        {
            thirdPlace.text = "Third Place: " + PlayerPrefs.GetString("NormalThirdPlaceScoreName", "Anonymous") + " - " + thirdPoints;
        }
        else
        {
            thirdPlace.text = "No score";
            fourthPlace.text = "No score";
            fifthPlace.text = "No score";
            return;
        }
        if (fourthPoints > 0)
        {
            fourthPlace.text = "Fourth Place: " + PlayerPrefs.GetString("NormalFourthPlaceScoreName", "Anonymous") + " - " + fourthPoints;
        }
        else
        {
            fourthPlace.text = "No score";
            fifthPlace.text = "No score";
            return;
        }
        if (fifthPoints > 0)
        {
            fifthPlace.text = "Fourth Place: " + PlayerPrefs.GetString("NormalFourthPlaceScoreName", "Anonymous") + " - " + fifthPoints;
        }
        else
        {
            fifthPlace.text = "No score";
            return;
        }
    }
    void loadLeaderboardHard()
    {
        int firstPoints, secondPoints, thirdPoints, fourthPoints, fifthPoints;
        titleText.text = "Hard";
        firstPoints = PlayerPrefs.GetInt("HardFirstPlaceScore", 0);
        secondPoints = PlayerPrefs.GetInt("HardSecondPlaceScore", 0);
        thirdPoints = PlayerPrefs.GetInt("HardThirdPlaceScore", 0);
        fourthPoints = PlayerPrefs.GetInt("HardFourthPlaceScore", 0);
        fifthPoints = PlayerPrefs.GetInt("HardFifthPlaceScore", 0);
        if (firstPoints > 0)
        {
            firstPlace.text = "First Place: " + PlayerPrefs.GetString("HardFirstPlaceScoreName", "Anonymous") + " - " + firstPoints;
        }
        else
        {
            firstPlace.text = "No score";
            secondPlace.text = "No score";
            thirdPlace.text = "No score";
            fourthPlace.text = "No score";
            fifthPlace.text = "No score";
            return; //Finished loading.
        }
        if (secondPoints > 0)
        {
            secondPlace.text = "Second Place: " + PlayerPrefs.GetString("HardSecondPlaceScoreName", "Anonymous") + " - " + secondPoints;
        }
        else
        {
            secondPlace.text = "No score";
            thirdPlace.text = "No score";
            fourthPlace.text = "No score";
            fifthPlace.text = "No score";
            return;
        }
        if (thirdPoints > 0)
        {
            thirdPlace.text = "Third Place: " + PlayerPrefs.GetString("HardThirdPlaceScoreName", "Anonymous") + " - " + thirdPoints;
        }
        else
        {
            thirdPlace.text = "No score";
            fourthPlace.text = "No score";
            fifthPlace.text = "No score";
            return;
        }
        if (fourthPoints > 0)
        {
            fourthPlace.text = "Fourth Place: " + PlayerPrefs.GetString("HardFourthPlaceScoreName", "Anonymous") + " - " + fourthPoints;
        }
        else
        {
            fourthPlace.text = "No score";
            fifthPlace.text = "No score";
            return;
        }
        if (fifthPoints > 0)
        {
            fifthPlace.text = "Fourth Place: " + PlayerPrefs.GetString("HardFourthPlaceScoreName", "Anonymous") + " - " + fifthPoints;
        }
        else
        {
            fifthPlace.text = "No score";
            return;
        }
    }
    void loadLeaderboardExpert()
    {
        int firstPoints, secondPoints, thirdPoints, fourthPoints, fifthPoints;
        titleText.text = "Expert";
        firstPoints = PlayerPrefs.GetInt("ExpertFirstPlaceScore", 0);
        secondPoints = PlayerPrefs.GetInt("ExpertSecondPlaceScore", 0);
        thirdPoints = PlayerPrefs.GetInt("ExpertThirdPlaceScore", 0);
        fourthPoints = PlayerPrefs.GetInt("ExpertFourthPlaceScore", 0);
        fifthPoints = PlayerPrefs.GetInt("ExpertFifthPlaceScore", 0);
        if (firstPoints > 0)
        {
            firstPlace.text = "First Place: " + PlayerPrefs.GetString("ExpertFirstPlaceScoreName", "Anonymous") + " - " + firstPoints;
        }
        else
        {
            firstPlace.text = "No score";
            secondPlace.text = "No score";
            thirdPlace.text = "No score";
            fourthPlace.text = "No score";
            fifthPlace.text = "No score";
            return; //Finished loading.
        }
        if (secondPoints > 0)
        {
            secondPlace.text = "Second Place: " + PlayerPrefs.GetString("ExpertSecondPlaceScoreName", "Anonymous") + " - " + secondPoints;
        }
        else
        {
            secondPlace.text = "No score";
            thirdPlace.text = "No score";
            fourthPlace.text = "No score";
            fifthPlace.text = "No score";
            return;
        }
        if (thirdPoints > 0)
        {
            thirdPlace.text = "Third Place: " + PlayerPrefs.GetString("ExpertThirdPlaceScoreName", "Anonymous") + " - " + thirdPoints;
        }
        else
        {
            thirdPlace.text = "No score";
            fourthPlace.text = "No score";
            fifthPlace.text = "No score";
            return;
        }
        if (fourthPoints > 0)
        {
            fourthPlace.text = "Fourth Place: " + PlayerPrefs.GetString("ExpertFourthPlaceScoreName", "Anonymous") + " - " + fourthPoints;
        }
        else
        {
            fourthPlace.text = "No score";
            fifthPlace.text = "No score";
            return;
        }
        if (fifthPoints > 0)
        {
            fifthPlace.text = "Fourth Place: " + PlayerPrefs.GetString("ExpertFourthPlaceScoreName", "Anonymous") + " - " + fifthPoints;
        }
        else
        {
            fifthPlace.text = "No score";
            return;
        }
    }
    void loadLeaderboardSatanic()
    {

    }
    void loadMainMenu()
    {

    }
    public void WaveComplete()
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
        yield return new WaitForSeconds(2f);
        isPaused = true;
        waveCompletePause = true;
        yield return new WaitForSeconds(5f);
        GameObject.Find("BackgroundPowerDrain").SetActive(false);
        //Switch depending on which difficulty they are on.
        switch (PlayerPrefs.GetInt("DifficultyChosen"))
        {
            case 1:
                {
                    if (player.getEasyPowerDrain() == true)
                    {
                        PowerDrainScreen();
                        break;
                    }
                    else
                    {
                        powerController.losePowerHard();
                        break;
                    }
                   
                }
            case 2:
                {
                    if (player.getNormalPowerDrain() == true)
                    {
                        PowerDrainScreen();
                        break;
                    }
                    else
                    {
                        powerController.losePowerHard();
                        break;
                    }
                }
            case 3:
                {
                    if (player.getHardPowerDrain() == true)
                    {
                        PowerDrainScreen();
                        break;
                    }
                    else
                    {
                        powerController.losePowerHard();
                        break;
                    }
                }
            case 4:
                {
                    if (player.getExpertPowerDrain() == true)
                    {
                        PowerDrainScreen();
                        break;
                    }
                    else
                    {
                        powerController.losePowerHard();
                        break;
                    }
                }
            default:
                {
                    PowerDrainScreen();
                    Debug.LogError("An error occurred: No difficulty level value defined in PlayerPrefs.");
                    break;
                }
        }
    }
    public void GameCompleteText()
    {
        StartCoroutine("GameCompleteRoutine");
    }
    IEnumerator GameCompleteRoutine()
    {

        isPaused = true;
        storeHighscores(powerController.difficultyLevel);
        GameSummaryScreen(false);
        Cursor.lockState = CursorLockMode.None;
        isPaused = false;
        yield return new WaitForEndOfFrame();
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
        if (getTutorialStage() > 0)
        {
            summaryDifficulty.GetComponent<Text>().text = "You are currently in the tutorial.";
            summaryWave.GetComponent<Text>().text = "";
            summaryPoints.GetComponent<Text>().text = "";
            return;
        }
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
    /// 11 - Unlock movement, show the controls in the top left.
    /// 12 - Await user killing enemy.
    /// 13 - Enemy killed, showing the drain power screen.
    /// 14 - Awaiting user draining power.
    /// 15 - Enemy(s) spawned. Final part of tutorial.
    /// 16 - Enemy killed. Show final message, and then the tutorial is completed. Pause game, transfer user to main menu in 5-10 seconds.
    /// </summary>


    public int getTutorialStage() //Get the current tutorial stage.
    {
        return TutorialStage;
    }
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
            powerController.powerHandler[0].PowerAvailable = true;
            powerController.powerHandler[0].PowerActive = true;
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
        if (TutorialStage == 9)
        {
            StartCoroutine("TutState9");
        }
        if (TutorialStage == 11)
        {
            StartCoroutine("TutState11");
            ResumeButton();
        }
        if (TutorialStage == 13)
        {
            StartCoroutine("TutState13");
        }
        if (TutorialStage == 15)
        {
            StartCoroutine("TutState15");
        }
        if (TutorialStage == 17)
        {
            StartCoroutine("TutState17");
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
            "of this game.\n" +
            "Please press left click to continue.";
        WelcomeText.transform.localPosition = new Vector3(-566, 333, 0);
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
        animText.GetComponent<Text>().text = "These are your powers. You can see more in the pause menu ingame!";
        animText.Play("TextAppear");
        animArrow["jumping arrow appear"].wrapMode = WrapMode.Once;
        animArrow.Play("jumping arrow appear");
        yield return new WaitForSeconds(0.5f);
        animArrow.Play("Jumping arrow left");
        TutorialStage = 8;

    }
    IEnumerator TutState9()
    {
        Animation anim = WelcomeImage.GetComponent<Animation>();
        Animation animText = WelcomeText.GetComponent<Animation>();
        Animation animArrow = JumpingArrow.GetComponent<Animation>();
        animText.Play("TextDisappear");
        animArrow.Stop();
        animArrow.Play("jumping arrow disappear");
        yield return new WaitForSeconds(0.5f);
        JumpingArrow.transform.localPosition = new Vector3(267, 459, 0);
        WelcomeText.transform.localPosition = new Vector3(460, 504, 0);
        WelcomeText.GetComponent<Text>().text = "This is your pointers counter.\nThe higher the difficulty,\n the more points you'll get!";
        animText.Play("TextAppear");
        animArrow["jumping arrow appear"].wrapMode = WrapMode.Once;
        animArrow.Play("jumping arrow appear");
        yield return new WaitForSeconds(0.5f);
        animArrow.Play("jumping arrow left points");
        TutorialStage = 10;
    }
    IEnumerator TutState11()
    {
        Animation anim = WelcomeImage.GetComponent<Animation>();
        Animation animText = WelcomeText.GetComponent<Animation>();
        Animation animArrow = JumpingArrow.GetComponent<Animation>();
        animText.Play("TextDisappear");
        animArrow.Stop();
        animArrow.Play("jumping arrow disappear");
        yield return new WaitForSeconds(0.5f);
        Enemy1.transform.position = new Vector3(19.90631f, 1.021423f, -7.240521f);
        WelcomeText.GetComponent<Text>().text = "To attack, press left click. You will fire a magical bullet at the enemy.\nKill this enemy to continue.";
        WelcomeText.transform.localPosition = new Vector3(-483, 140, 0);
        Enemy1.SetActive(true);
        animText.Play("TextAppear");
        Cursor.lockState = CursorLockMode.Locked;
        TutorialStage = 12;
    }
    IEnumerator TutState13()
    {
        Animation anim = WelcomeImage.GetComponent<Animation>();
        Animation animText = WelcomeText.GetComponent<Animation>();
        Animation animArrow = JumpingArrow.GetComponent<Animation>();
        lockPauseMenu = true;
        animText.Play("TextDisappear");
        yield return new WaitForSeconds(0.5f);
        WelcomeText.GetComponent<Text>().text = "Well done! Wave complete!";
        animText.Play("TextAppear");
        yield return new WaitForSeconds(2f);
        animText.Play("TextDisappear");
        yield return new WaitForSeconds(2f);
        animText.Play("TextAppear");
        WelcomeText.transform.localPosition = new Vector3(-233,238, 0);
        WelcomeText.GetComponent<Text>().fontSize = 45;
        animText.GetComponent<Text>().text = "As the game progresses, you must\nchannel corruption to your powers.\n\nThis means you will lose them.\nYou usually have a choice between two.\nTo continue, please drain the \nImmune to Death power.";
        isPaused = true;
        PowerDrainScreen(0, 0);

        TutorialStage = 14;

    }
    IEnumerator TutState15()
    {
        Animation anim = WelcomeImage.GetComponent<Animation>();
        Animation animText = WelcomeText.GetComponent<Animation>();
        Animation animArrow = JumpingArrow.GetComponent<Animation>();
        animText.Play("TextDisappear");
        yield return new WaitForSeconds(0.5f);
        WelcomeText.GetComponent<Text>().text = "Kill the final enemy to finish the tutorial.";
        WelcomeText.transform.localPosition = new Vector3(-547, 271, 0);
        WelcomeText.GetComponent<Text>().fontSize = 60;
        animText.Play("TextAppear");
        Enemy2.SetActive(true);
        TutorialStage = 16;

    }
    IEnumerator TutState17()
    {
        Animation anim = WelcomeImage.GetComponent<Animation>();
        Animation animText = WelcomeText.GetComponent<Animation>();
        Animation animArrow = JumpingArrow.GetComponent<Animation>();
        animText.Play("TextDisappear");
        lockPauseMenu = true;
        yield return new WaitForSeconds(0.5f);
        WelcomeText.GetComponent<Text>().text = "Congratulations! Tutorial complete.\nReturning to the main menu in 10 seconds.";
        WelcomeText.transform.localPosition = new Vector3(-547, 271, 0);
        WelcomeText.GetComponent<Text>().fontSize = 60;
        animText.Play("TextAppear");
        isPaused = true;
        Cursor.lockState = CursorLockMode.None;
        yield return new WaitForSeconds(10f);
        SceneManager.LoadScene("UI Scale Testing");
        PlayerPrefs.SetInt("TutorialCompleteStatus", 1);
    }
    public void TutorialEnemyKilled()
    {
        if (TutorialStage == 12)
        {
            TutorialStage = 13;
            Tutorial();
        }
        if (TutorialStage == 14)
        {
            TutorialStage = 15;
            Tutorial();
        }
        if (TutorialStage == 16)
        {
            TutorialStage = 17;
            Tutorial();
        }
    }

    public void GameSummaryScreen(bool playerDied)
    {
        Cursor.lockState = CursorLockMode.None;
        postObj.SetActive(true);
        currentScore.gameObject.SetActive(false); //Set the currentscore obj to false
        //Get current wave ID.
        GameObject progression = GameObject.Find("ProgressionHandler");
        Progression progObj = progression.GetComponent<Progression>();
        if (playerDied)
        {
            postWave.GetComponent<Text>().text = "Died on wave: " + progObj.GetCurrentWave();
        }
        else
        {
            postWave.GetComponent<Text>().text = "All waves successfully completed!";
        }
        //Get the number of powers drained
        postPowersDrained.GetComponent<Text>().text = "Powers drained: " + powerController.GetPowersDrainedCount();
        //Get the number of powers remaining
        int powersRemaining = 13 - powerController.GetPowersDrainedCount();
        postPowersRemaining.GetComponent<Text>().text = "Powers remaining: " + powersRemaining;

        switch (PlayerPrefs.GetInt("LastLeaderboardPosition"))
        {
            case 0:
                {
                    postFinal.GetComponent<Text>().text = "Final Score: " + pointsGained;
                    return;
                }
            case 1:
                {
                    postFinal.GetComponent<Text>().text = "Final score: " + pointsGained + " (First place!)";
                    leaderboardObj.SetActive(true);
                    leaderboardTextObj.SetActive(true);
                    leaderboardTextObj.GetComponent<Text>().text = "Congratulations! You are rank 1 on the leaderboard!\n" +
    "Please enter your name below to have your name added.";
                    return;
                }
            case 2:
                {
                    postFinal.GetComponent<Text>().text = "Final score: " + pointsGained + " (Second place!)";
                    leaderboardObj.SetActive(true);
                    leaderboardTextObj.SetActive(true);
                    leaderboardTextObj.GetComponent<Text>().text = "Congratulations! You are rank 2 on the leaderboard!\n" +
    "Please enter your name below to have your name added.";
                    return;
                }
            case 3:
                {
                    postFinal.GetComponent<Text>().text = "Final score: " + pointsGained + " (Third place!)";
                    leaderboardObj.SetActive(true);
                    leaderboardTextObj.SetActive(true);
                    leaderboardTextObj.GetComponent<Text>().text = "Congratulations! You are rank 3 on the leaderboard!\n" +
    "Please enter your name below to have your name added.";
                    return;
                }
            case 4:
                {
                    postFinal.GetComponent<Text>().text = "Final score: " + pointsGained + " (Fourth place!)";
                    leaderboardObj.SetActive(true);
                    leaderboardTextObj.SetActive(true);
                    leaderboardTextObj.GetComponent<Text>().text = "Congratulations! You are rank 4 on the leaderboard!\n" +
    "Please enter your name below to have your name added.";
                    return;
                }
            case 5:
                {
                    postFinal.GetComponent<Text>().text = "Final score: " + pointsGained + " (Fifth place!)";
                    leaderboardObj.SetActive(true);
                    leaderboardTextObj.SetActive(true);
                    leaderboardTextObj.GetComponent<Text>().text = "Congratulations! You are rank 5 on the leaderboard!\n" +
                        "Please enter your name below to have your name added.";
                    return;
                }
            case 6:
                {
                    postFinal.GetComponent<Text>().text = "You are in the tutorial.";
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
        lockPauseMenu = true;
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
        p = powerController.RandomAvailablePower();
        losePower2 = p.ID;
        while (losePower1 == losePower2)
        {
            p = powerController.RandomAvailablePower();
            losePower2 = p.ID;
        }
        PowerDrain2.GetComponent<Image>().sprite = sprites[losePower2];
        powerDesc2.GetComponent<Text>().text = p.PowerDescription;
        powerName2.GetComponent<Text>().text = p.PowerName;

    }

    public void PowerDrainScreen(int power1ToDrain, int power2ToDrain) //Will be called by the Progression handler when the player completes a wave.
    {
        Cursor.lockState = CursorLockMode.None;
        lockPauseMenu = true;
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
        PowerBehaviour.Power p = powerController.getSpecificPower(power1ToDrain);
        PowerDrain1.GetComponent<Image>().sprite = sprites[power1ToDrain];
        powerName1.GetComponent<Text>().text = p.PowerName;
        powerDesc1.GetComponent<Text>().text = p.PowerDescription;
        p = powerController.getSpecificPower(power2ToDrain);
        losePower2 = p.ID;
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
            inPauseMenu = false;
            if (TutorialStage > 11)
            {
                isPaused = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            return;
        }
        inPauseMenu = false;
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
        fireWarning.SetActive(true);
        return;
        
    }
    public void DisableFireWarning()
    {
        fireWarning.SetActive(false);
    }
    void FixedUpdate()
    {
        if (SceneManager.GetActiveScene().name != "Game")
        {
            return;
        }


    }
    //Used when the game moves to another scene.
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "UI Scale Testing")
        {
            LoadHighscores();
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
        int highScore = PlayerPrefs.GetInt("EasyFirstPlaceScore");
        Debug.Log("Easy highscore: " + highScore);
        if (highScore > 0)
        {
            if (easyScore != null)
            {
                easyScore.text = "Highscore: " + highScore;
            }

        }
        highScore = PlayerPrefs.GetInt("NormalFirstPlaceScore");
        Debug.Log("Normal highscore: " + highScore);
        if (highScore > 0)
        {
            if (normalScore != null)
            {
                normalScore.text = "Highscore: " + highScore;
            }

        }
        highScore = PlayerPrefs.GetInt("HardFirstPlaceScore");
        Debug.Log("Hard highscore: " + highScore);
        if (highScore > 0)
        {
            if (hardScore != null)
            {
                hardScore.text = "Highscore: " + highScore;
            }

        }
        highScore = PlayerPrefs.GetInt("ExpertFirstPlaceScore");
        Debug.Log("Expert highscore: " + highScore);
        if (highScore > 0)
        {
            if (expertScore != null)
            {
                expertScore.text = "Highscore: " + highScore;
            }
        }

    }
    public int storeHighscores(int difficulty) //Pass in difficulty, find relevant highscore and complete the check.
    {
        switch (difficulty)
        {
            case 1:
            {
                    int EasyFifthPlace = PlayerPrefs.GetInt("EasyFifthPlaceScore", 0);
                    if (EasyFifthPlace > pointsGained)
                    {
                        return 0; //Score lower than fifth place. End check here.
                    }
                    else
                    {
                        int EasyFourthPlace = PlayerPrefs.GetInt("EasyFourthPlaceScore", 0);
                        if (EasyFourthPlace > pointsGained)
                        {
                            //Fifth place achieved
                            PlayerPrefs.SetInt("EasyFifthPlaceScore", (int)pointsGained);
                            return 5;
                        }
                        else
                        {
                            int EasyThirdPlace = PlayerPrefs.GetInt("EasyThirdPlaceScore", 0);
                            if (EasyThirdPlace > pointsGained)
                            {
                                //fourth place achieved
                                PlayerPrefs.SetInt("EasyFifthPlaceScore", EasyFourthPlace);
                                PlayerPrefs.SetInt("EasyFourthPlaceScore", (int)pointsGained);
                                return 4;
                            }
                            else
                            {
                                int EasySecondPlace = PlayerPrefs.GetInt("EasySecondPlaceScore", 0);
                                if (EasySecondPlace > pointsGained)
                                {
                                    //third place achieved
                                    PlayerPrefs.SetInt("EasyFifthPlaceScore", EasyFourthPlace);
                                    PlayerPrefs.SetInt("EasyFourthPlaceScore", EasyThirdPlace);
                                    PlayerPrefs.SetInt("EasyThirdPlaceScore", (int)pointsGained);
                                    return 3;

                                }
                                else
                                {
                                    int EasyFirstPlace = PlayerPrefs.GetInt("EasyFirstPlaceScore", 0);
                                    if (EasyFirstPlace > pointsGained)
                                    {
                                        //second place achieved
                                        PlayerPrefs.SetInt("EasyFifthPlaceScore", EasyFourthPlace);
                                        PlayerPrefs.SetInt("EasyFourthPlaceScore", EasyThirdPlace);
                                        PlayerPrefs.SetInt("EasyThirdPlaceScore", EasySecondPlace);
                                        PlayerPrefs.SetInt("EasySecondPlaceScore", (int)pointsGained);
                                        return 2;
                                    }
                                    else
                                    {
                                        //first place achieved
                                        PlayerPrefs.SetInt("EasyFifthPlaceScore", EasyFourthPlace);
                                        PlayerPrefs.SetInt("EasyFourthPlaceScore", EasyThirdPlace);
                                        PlayerPrefs.SetInt("EasyThirdPlaceScore", EasySecondPlace);
                                        PlayerPrefs.SetInt("EasySecondPlaceScore", EasyFirstPlace);
                                        PlayerPrefs.SetInt("EasyFirstPlaceScore", (int)pointsGained);
                                        return 1;
                                    }
                                }
                            }
                        }
                    }
            }
            case 2:
                {
                    int NormalFifthPlace = PlayerPrefs.GetInt("NormalFifthPlaceScore", 0);
                    if (NormalFifthPlace > pointsGained)
                    {
                        return 0; //Score lower than fifth place. End check here.
                    }
                    else
                    {
                        int NormalFourthPlace = PlayerPrefs.GetInt("NormalFourthPlaceScore", 0);
                        if (NormalFourthPlace > pointsGained)
                        {
                            //Fifth place achieved
                            PlayerPrefs.SetInt("NormalFifthPlaceScore", (int)pointsGained);
                            return 5;
                        }
                        else
                        {
                            int NormalThirdPlace = PlayerPrefs.GetInt("NormalThirdPlaceScore", 0);
                            if (NormalThirdPlace > pointsGained)
                            {
                                //fourth place achieved
                                PlayerPrefs.SetInt("NormalFifthPlaceScore", NormalFourthPlace);
                                PlayerPrefs.SetInt("NormalFourthPlaceScore", (int)pointsGained);
                                return 4;
                            }
                            else
                            {
                                int NormalSecondPlace = PlayerPrefs.GetInt("NormalSecondPlaceScore", 0);
                                if (NormalSecondPlace > pointsGained)
                                {
                                    //third place achieved
                                    PlayerPrefs.SetInt("NormalFifthPlaceScore", NormalFourthPlace);
                                    PlayerPrefs.SetInt("NormalFourthPlaceScore", NormalThirdPlace);
                                    PlayerPrefs.SetInt("NormalThirdPlaceScore", (int)pointsGained);
                                    return 3;

                                }
                                else
                                {
                                    int NormalFirstPlace = PlayerPrefs.GetInt("NormalFirstPlaceScore", 0);
                                    if (NormalFirstPlace > pointsGained)
                                    {
                                        //second place achieved
                                        PlayerPrefs.SetInt("NormalFifthPlaceScore", NormalFourthPlace);
                                        PlayerPrefs.SetInt("NormalFourthPlaceScore", NormalThirdPlace);
                                        PlayerPrefs.SetInt("NormalThirdPlaceScore", NormalSecondPlace);
                                        PlayerPrefs.SetInt("NormalSecondPlaceScore", (int)pointsGained);
                                        return 2;
                                    }
                                    else
                                    {
                                        //first place achieved
                                        PlayerPrefs.SetInt("NormalFifthPlaceScore", NormalFourthPlace);
                                        PlayerPrefs.SetInt("NormalFourthPlaceScore", NormalThirdPlace);
                                        PlayerPrefs.SetInt("NormalThirdPlaceScore", NormalSecondPlace);
                                        PlayerPrefs.SetInt("NormalSecondPlaceScore", NormalFirstPlace);
                                        PlayerPrefs.SetInt("NormalFirstPlaceScore", (int)pointsGained);
                                        return 1;
                                    }
                                }
                            }
                        }
                    }
                }
            case 3:
                {
                    int HardFifthPlace = PlayerPrefs.GetInt("HardFifthPlaceScore", 0);
                    if (HardFifthPlace > pointsGained)
                    {
                        return 0; //Score lower than fifth place. End check here.
                    }
                    else
                    {
                        int HardFourthPlace = PlayerPrefs.GetInt("HardFourthPlaceScore", 0);
                        if (HardFourthPlace > pointsGained)
                        {
                            //Fifth place achieved
                            PlayerPrefs.SetInt("HardFifthPlaceScore", (int)pointsGained);
                            return 5;
                        }
                        else
                        {
                            int HardThirdPlace = PlayerPrefs.GetInt("HardThirdPlaceScore", 0);
                            if (HardThirdPlace > pointsGained)
                            {
                                //fourth place achieved
                                PlayerPrefs.SetInt("HardFifthPlaceScore", HardFourthPlace);
                                PlayerPrefs.SetInt("HardFourthPlaceScore", (int)pointsGained);
                                return 4;
                            }
                            else
                            {
                                int HardSecondPlace = PlayerPrefs.GetInt("HardSecondPlaceScore", 0);
                                if (HardSecondPlace > pointsGained)
                                {
                                    //third place achieved
                                    PlayerPrefs.SetInt("HardFifthPlaceScore", HardFourthPlace);
                                    PlayerPrefs.SetInt("HardFourthPlaceScore", HardThirdPlace);
                                    PlayerPrefs.SetInt("HardThirdPlaceScore", (int)pointsGained);
                                    return 3;

                                }
                                else
                                {
                                    int HardFirstPlace = PlayerPrefs.GetInt("HardFirstPlaceScore", 0);
                                    if (HardFirstPlace > pointsGained)
                                    {
                                        //second place achieved
                                        PlayerPrefs.SetInt("HardFifthPlaceScore", HardFourthPlace);
                                        PlayerPrefs.SetInt("HardFourthPlaceScore", HardThirdPlace);
                                        PlayerPrefs.SetInt("HardThirdPlaceScore", HardSecondPlace);
                                        PlayerPrefs.SetInt("HardSecondPlaceScore", (int)pointsGained);
                                        return 2;
                                    }
                                    else
                                    {
                                        //first place achieved
                                        PlayerPrefs.SetInt("HardFifthPlaceScore", HardFourthPlace);
                                        PlayerPrefs.SetInt("HardFourthPlaceScore", HardThirdPlace);
                                        PlayerPrefs.SetInt("HardThirdPlaceScore", HardSecondPlace);
                                        PlayerPrefs.SetInt("HardSecondPlaceScore", HardFirstPlace);
                                        PlayerPrefs.SetInt("HardFirstPlaceScore", (int)pointsGained);
                                        return 1;
                                    }
                                }
                            }
                        }
                    }
                }
            case 4:
                int ExpertFifthPlace = PlayerPrefs.GetInt("ExpertFifthPlaceScore", 0);
                if (ExpertFifthPlace > pointsGained)
                {
                    return 0; //Score lower than fifth place. End check here.
                }
                else
                {
                    int ExpertFourthPlace = PlayerPrefs.GetInt("ExpertFourthPlaceScore", 0);
                    if (ExpertFourthPlace > pointsGained)
                    {
                        //Fifth place achieved
                        PlayerPrefs.SetInt("ExpertFifthPlaceScore", (int)pointsGained);
                        return 5;
                    }
                    else
                    {
                        int ExpertThirdPlace = PlayerPrefs.GetInt("ExpertThirdPlaceScore", 0);
                        if (ExpertThirdPlace > pointsGained)
                        {
                            //fourth place achieved
                            PlayerPrefs.SetInt("ExpertFifthPlaceScore", ExpertFourthPlace);
                            PlayerPrefs.SetInt("ExpertFourthPlaceScore", (int)pointsGained);
                            return 4;
                        }
                        else
                        {
                            int ExpertSecondPlace = PlayerPrefs.GetInt("ExpertSecondPlaceScore", 0);
                            if (ExpertSecondPlace > pointsGained)
                            {
                                //third place achieved
                                PlayerPrefs.SetInt("ExpertFifthPlaceScore", ExpertFourthPlace);
                                PlayerPrefs.SetInt("ExpertFourthPlaceScore", ExpertThirdPlace);
                                PlayerPrefs.SetInt("ExpertThirdPlaceScore", (int)pointsGained);
                                return 3;

                            }
                            else
                            {
                                int ExpertFirstPlace = PlayerPrefs.GetInt("ExpertFirstPlaceScore", 0);
                                if (ExpertFirstPlace > pointsGained)
                                {
                                    //second place achieved
                                    PlayerPrefs.SetInt("ExpertFifthPlaceScore", ExpertFourthPlace);
                                    PlayerPrefs.SetInt("ExpertFourthPlaceScore", ExpertThirdPlace);
                                    PlayerPrefs.SetInt("ExpertThirdPlaceScore", ExpertSecondPlace);
                                    PlayerPrefs.SetInt("ExpertSecondPlaceScore", (int)pointsGained);
                                    return 2;
                                }
                                else
                                {
                                    //first place achieved
                                    PlayerPrefs.SetInt("ExpertFifthPlaceScore", ExpertFourthPlace);
                                    PlayerPrefs.SetInt("ExpertFourthPlaceScore", ExpertThirdPlace);
                                    PlayerPrefs.SetInt("ExpertThirdPlaceScore", ExpertSecondPlace);
                                    PlayerPrefs.SetInt("ExpertSecondPlaceScore", ExpertFirstPlace);
                                    PlayerPrefs.SetInt("ExpertFirstPlaceScore", (int)pointsGained);
                                    return 1;
                                } 
                            }
                        }
                    }
                }

        }
        return 0;
    }



    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "UI Scale Testing")
        {
            if (PlayerPrefs.GetInt("TutorialCompleteStatus") != 1)
            {
                SceneManager.LoadScene("Game");
            }
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
            LoadHighscores();
        }
        if (SceneManager.GetActiveScene().name == "Game") //Ensure player is in main game when checking for pause
        {
            if (PlayerPrefs.GetInt("TutorialCompleteStatus") != 1 && TutorialStage == 0)
            {
                TutorialStage = 1;
            }
            if (Input.GetKeyDown(KeyCode.F12))
            {
                PowerDrainScreen();//Testing.
            }

            if (Input.GetKeyDown(KeyCode.Escape) == true)
            {
                if (lockPauseMenu == true)
                {
                    return;
                }
                if (isPaused == false || inPauseMenu == false)
                {
                    inPauseMenu = true;
                    isPaused = true;
                    Debug.Log("Escape pressed, game paused!");
                    GamePausedFunction();
                    Cursor.lockState = CursorLockMode.None;
                }
                else
                {
                    inPauseMenu = false;
                    isPaused = false;
                    Debug.Log("Game resumed!");
                    GameUnpausedFunction();
                    Cursor.lockState = CursorLockMode.Locked;
                    return;

                }
            }
            if (TutorialStage > 0)
            {
                if (TutorialStage == 1)
                {
                    DisableUIForTutorial();
                    Tutorial();
                }
                if (TutorialStage == 2 && Input.GetMouseButton(0) && inPauseMenu == false)
                {
                    TutorialStage = 3;
                    Tutorial();
                }
                if (TutorialStage == 4 && Input.GetMouseButton(0) && inPauseMenu == false)
                {
                    TutorialStage = 5;
                    Tutorial();
                }
                if (TutorialStage == 6 && Input.GetMouseButton(0) && inPauseMenu == false)
                {
                    TutorialStage = 7;
                    Tutorial();
                }
                if (TutorialStage == 8 && Input.GetMouseButton(0) && inPauseMenu == false)
                {
                    TutorialStage = 9;
                    Tutorial();
                }
                if (TutorialStage == 10 && Input.GetMouseButton(0) && inPauseMenu == false)
                {
                    TutorialStage = 11;
                    Tutorial();
                    isPaused = false;
                }
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
            if (isPaused == false)
            {
                ShowAllPowersInGame();
            }
            else
            {
                
                if (Input.GetKeyDown(KeyCode.R) == true && buttonpressed == false)
                {
                    
                    buttonpressed = true;
                }
                else
                {
                    buttonpressed = false;
                }
            }
            if (health != null)
            {
                getUpdatePlayerHP();
            }

        }





    }
}
