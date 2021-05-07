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
    //Public 

    bool buttonpressed = false;
    public PowerBehaviour powerController;
    public GameObject activePowers, healthObj, UIWaveComplete;
    public int intChosenDifficulty = 0; //Used for the start button. Not using getter/setter as it is a basic int that doesn't require special protection.

    [Tooltip("This array should be the same length as the number of powers.")]
    [SerializeField]
    Sprite[] sprites;
    [SerializeField]
    GameObject[] spriteUIElements;
    [SerializeField]
    GameObject PowerDrain1, PowerDrain2, powerName1, powerName2, powerDesc1, powerDesc2;
    [SerializeField]
    GameObject summaryDifficulty, summaryPoints, summaryWave;
    Text health;
    float pointsGained;
    [SerializeField]
    Text easyScore, normalScore, hardScore, expertScore, satanicScore, currentScore;
    [SerializeField]
    GameObject postObj, postWave, postDifficulty, postFinal, postPowersRemaining, postPowersDrained, HPBar;
    double currentHPercentage;
    [Space(10)]
    [SerializeField]
    GameObject fireWarning;
    [SerializeField]
    GameObject expertDrainPowerImage, expertDrainPowerNameText, expertDrainPowerNameText2; //Expert difficulty drain
    [Space(5)]
    [SerializeField]
    Text firstPlace, secondPlace, thirdPlace, fourthPlace, fifthPlace, titleText; 
    [SerializeField]
    GameObject leaderboardObj, leaderboardTextObj;
    [Space(5)]
    [SerializeField]
    GameObject UIStunnedMessage;
    [SerializeField]
    Text bonusPoints1, bonusPoints2;
    [SerializeField]
    Image progressBar;
    [SerializeField]
    AudioClip losePowerMusic, finalBossMusic;
    [SerializeField]
    TutorialHandler TutorialStatus;
    [SerializeField]
    Text chooseDifficultyDescriptions, chosenDifficulty;
    [SerializeField]
    Image blackoutImage;
    [SerializeField]
    Text deathOrCompletionText;

    //Non-SerializeField variables.
    bool isPaused = false;
    Player player;
    bool waveCompletePause = false;
    float escapePushCooldown = 0.5f;
    public int losePower1, losePower2; //Storage for losing powers
    //UI Controller tracking stages.
    bool inPauseMenu = false;
    bool lockPauseMenu = false; //Used while certain windows are open to ensure pause menu will NOT load.
    bool playerDead = false; //Used in an overload to override the timescale when the player dies.
    Progression p;
    GameObject audioController;
    const string EasyDescription = "You will start with all powers.\nYou start with double health.\nYou deal 1.25x more damage.\nYou take 0.75x damage.\n" +
        "You are able to choose between two powers to lose each round.\nYou are able to choose between the staff and sword between each round.\nOverall, you will recieve 0.25x points.";
    const string NormalDescription = "You will start with all powers.\nYou start with regular health.\nYou deal normal damage.\nYou take normal damage.\n" +
        "You are able to choose between two powers to lose each round.\nYou are able to choose between the staff and sword between each round.\nOverall, you will recieve 1x points.";
    const string HardDescription = "You will start with one less power.\nYou start with 0.75x health.\nYou deal 0.75x damage.\nYou take 1.25x more damage.\n" +
        "You are unable to choose a power to lose each round. It is chosen automatically.\nYou are able to choose between the staff and sword between each round.\nOverall, you will recieve 1.5x points.";
    const string ExpertDescription = "You will start with two less powers.\nYou start with 0.5x health.\nYou deal 0.5x damage.\nYou take 1.5x damage.\n" +
        "You are unable to choose a power to lose each round. It is chosen automatically.\nYour weapon is chosen automatically each round.\nOverall, you will recieve 2.5x points.";
    bool blackout = false;
   

    // Start is called before the first frame update
    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        if (SceneManager.GetActiveScene().name == "Game") //Check if the scene is "GetActiveScene". If it is, run below.
        {
            if (expertDrainPowerImage != null) //Set these to false. The reason they are true beforehand is to ensure that Unity can link to them properly.
            {
                expertDrainPowerImage.SetActive(false);
                expertDrainPowerNameText.SetActive(false);
                expertDrainPowerNameText2.SetActive(false);
            }
            if (UIStunnedMessage != null) //Same reason as above.
            {
                UIStunnedMessage.SetActive(false);
            }
            if (deathOrCompletionText != null)
            {
                deathOrCompletionText.gameObject.SetActive(false);
            }
            WeaponSelection();
        }
        else if (SceneManager.GetActiveScene().name == "UI Scale Testing")
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
            else if (obj.name == "PointsText")
            {
                currentScore = obj.GetComponent<Text>();
            }
        }
        //Find GameObject section.
        powerController = GameObject.Find("PowerHandler").GetComponent<PowerBehaviour>();
        player = GameObject.Find("Player").GetComponent<Player>();
        UIWaveComplete = GameObject.Find("WaveCompleteText");
        p = GameObject.Find("ProgressionHandler").GetComponent<Progression>();
        TutorialStatus = GameObject.FindWithTag("TutorialHandler").GetComponent<TutorialHandler>();
        //Get component section

        if (leaderboardObj != null)
        {
            leaderboardObj.SetActive(false);
        }
        if (UIStunnedMessage != null)
        {
            UIStunnedMessage.SetActive(false);
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

        Cursor.lockState = CursorLockMode.None;

        foreach (GameObject obj in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[]) //This is used to search all inactive objects.
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
            HPBarMethod(health, player.GetMaxHP());
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
    //Methods called by PowerBehaviour to draw certain UI elements and redraw others
    public void LostPowerMessage(int PowerDrainedID)
    {
        StartCoroutine("ShowMessage", PowerDrainedID);


    }

    string formatLostPowerMessage(int PowerDrainedID)
    {
        //This method returns a formatted string used when a power is drained at the end of a wave.
        switch (PowerDrainedID)
        {
            case 0:
                {
                    return "You are vulnerable to death!";
                }
            case 1:
                {
                    return "You no longer deal double damage to enemies.";
                }
            case 2:
                {
                    return "Your maximum health has been halved.";
                }
            case 3:
                {
                    return "Your health no longer regenerates!";
                }
            case 4:
                {
                    return "You no longer deal more damage the lower your health.";
                }
            case 5:
                {
                    return "You will now be damaged by fire.";
                }
            case 6:
                {
                    return "Your staff will now attack at a reduced rate.";
                }
            case 7:
                {
                    return "The curse has lifted from enemies.\n" +
                        "They are now their original size.";
                }
            case 8:
                {
                    return "The mace no longer deals 50% more damage."; 
                }
            case 9:
                {
                    return "You no longer take 50% less damage.";
                }
            case 10:
                {
                    return "Your staff no longer extinguishes fires.";
                }
            case 11:
                {
                    return "The mysterious effect drains away...";
                }
            case 12:
                {
                    return "The rain dance will no longer occur...";
                }
            case 13:
                {
                    return "Projectiles will no longer fall from the sky.";
                }
            default:
                {
                    return "An error has occurred.";
                }
        }
    }

    //This method is called by LostPowerMessage. 
    IEnumerator ShowMessage(int PowerDrainedID)
    {
        if (expertDrainPowerImage.activeInHierarchy == true || expertDrainPowerNameText.activeInHierarchy == true || expertDrainPowerNameText2.activeInHierarchy == true)
        {
            yield return new WaitForSeconds(1.5f);
        }
        expertDrainPowerImage.SetActive(true);
        expertDrainPowerNameText.SetActive(true);
        expertDrainPowerNameText2.SetActive(true);
        expertDrainPowerImage.transform.localPosition = new Vector3(0f, 0f, 0f);
        expertDrainPowerNameText.transform.localPosition = new Vector3(0f, -114f, 0f);
        expertDrainPowerNameText2.transform.localPosition = new Vector3(0f, -194f, 0f);
        Animation imageAnim = expertDrainPowerImage.GetComponent<Animation>();
        Animation text1Anim = expertDrainPowerNameText.GetComponent<Animation>();
        Animation text2Anim = expertDrainPowerNameText2.GetComponent<Animation>();
        if (PowerDrainedID == 255)
        {
            expertDrainPowerNameText.GetComponent<Text>().text = "All powers drained!";
            text1Anim.Play("ExpertText1Anim");
            yield return new WaitForSeconds(4);
            text1Anim.Play("ExpertText1Disappear");
        }
        else
        {
            expertDrainPowerImage.GetComponent<Image>().sprite = sprites[PowerDrainedID];
            expertDrainPowerNameText.GetComponent<Text>().text = powerController.powerHandler[PowerDrainedID].GetPowerName();
            expertDrainPowerNameText2.GetComponent<Text>().text = formatLostPowerMessage(PowerDrainedID);
            imageAnim.Play("ExpertImageAppear");
            text1Anim.Play("ExpertText1Anim");
            text2Anim.Play("ExpertTextAnim2");
            ShowAllPowersInGame(); //Redraw powers for the user on the UI. Removes the redundant power.

            yield return new WaitForSeconds(4);
            imageAnim.Play("ExpertImageDisappear");
            text1Anim.Play("ExpertText1Disappear");
            text2Anim.Play("ExpertText2Anim");
            yield return new WaitForSeconds(1f);
            expertDrainPowerImage.transform.localPosition = new Vector3(556.9971f, 0f, 0f);
            expertDrainPowerNameText.transform.localPosition = new Vector3(560.9915f, -114f, 0f);
            expertDrainPowerNameText2.transform.localPosition = new Vector3(556f, -194f, 0f);
        }
        


        //Check if the player's GetWeaponChoice is true or false.
        //If true, run progression handler.

        switch (PlayerPrefs.GetInt("DifficultyChosen"))
        {
            case 1:
                {
                    if (player.GetEasyWeaponChoice() == false)
                    {
                        int i = player.SpawnRandomWeapon();
                        if (i == 1)
                        {
                            expertDrainPowerImage.GetComponent<Image>().sprite = sprites[6];
                            expertDrainPowerNameText.GetComponent<Text>().text = "You are now using the staff!";
                            expertDrainPowerImage.transform.localPosition = new Vector3(0f, 0f, 0f);
                            expertDrainPowerNameText.transform.localPosition = new Vector3(0f, -114f, 0f);
                            expertDrainPowerNameText2.transform.localPosition = new Vector3(0f, -194f, 0f);
                            imageAnim.Play("ExpertImageAppear");
                            text1Anim.Play("ExpertText1Anim");
                            yield return new WaitForSeconds(2f);
                            imageAnim.Play("ExpertImageDisappear");
                            text1Anim.Play("ExpertText1Disappear");
                            yield return new WaitForSeconds(2f);
                            expertDrainPowerImage.transform.localPosition = new Vector3(556.9971f, 0f, 0f);
                            expertDrainPowerNameText.transform.localPosition = new Vector3(560.9915f, -114f, 0f);
                            expertDrainPowerNameText2.transform.localPosition = new Vector3(556f, -194f, 0f);
                        }
                        else if (i == 0)
                        {
                            expertDrainPowerImage.GetComponent<Image>().sprite = sprites[8];
                            expertDrainPowerNameText.GetComponent<Text>().text = "You are now using the sword!";
                            expertDrainPowerImage.transform.localPosition = new Vector3(0f, 0f, 0f);
                            expertDrainPowerNameText.transform.localPosition = new Vector3(0f, -114f, 0f);
                            expertDrainPowerNameText2.transform.localPosition = new Vector3(0f, -194f, 0f);
                            imageAnim.Play("ExpertImageAppear");
                            text1Anim.Play("ExpertText1Anim");
                            yield return new WaitForSeconds(2f);
                            imageAnim.Play("ExpertImageDisappear");
                            text1Anim.Play("ExpertText1Disappear");
                            yield return new WaitForSeconds(2f);
                            expertDrainPowerImage.transform.localPosition = new Vector3(556.9971f, 0f, 0f);
                            expertDrainPowerNameText.transform.localPosition = new Vector3(560.9915f, -114f, 0f);
                            expertDrainPowerNameText2.transform.localPosition = new Vector3(556f, -194f, 0f);
                        }
                    }
                    else
                    {
                        WeaponSelection();
                        yield break;
                    }
                    break;
                }
            case 2:
                {
                    if (player.GetNormalWeaponChoice() == false)
                    {
                        int i = player.SpawnRandomWeapon();
                        if (i == 1)
                        {
                            expertDrainPowerImage.GetComponent<Image>().sprite = sprites[6];
                            expertDrainPowerNameText.GetComponent<Text>().text = "You are now using the staff!";
                            expertDrainPowerImage.transform.localPosition = new Vector3(0f, 0f, 0f);
                            expertDrainPowerNameText.transform.localPosition = new Vector3(0f, -114f, 0f);
                            expertDrainPowerNameText2.transform.localPosition = new Vector3(0f, -194f, 0f);
                            imageAnim.Play("ExpertImageAppear");
                            text1Anim.Play("ExpertText1Anim");
                            yield return new WaitForSeconds(2f);
                            imageAnim.Play("ExpertImageDisappear");
                            text1Anim.Play("ExpertText1Disappear");
                            yield return new WaitForSeconds(2f);
                            expertDrainPowerImage.transform.localPosition = new Vector3(556.9971f, 0f, 0f);
                            expertDrainPowerNameText.transform.localPosition = new Vector3(560.9915f, -114f, 0f);
                            expertDrainPowerNameText2.transform.localPosition = new Vector3(556f, -194f, 0f);
                        }
                        else if (i == 0)
                        {
                            expertDrainPowerImage.GetComponent<Image>().sprite = sprites[8];
                            expertDrainPowerNameText.GetComponent<Text>().text = "You are now using the sword!";
                            expertDrainPowerImage.transform.localPosition = new Vector3(0f, 0f, 0f);
                            expertDrainPowerNameText.transform.localPosition = new Vector3(0f, -114f, 0f);
                            expertDrainPowerNameText2.transform.localPosition = new Vector3(0f, -194f, 0f);
                            imageAnim.Play("ExpertImageAppear");
                            text1Anim.Play("ExpertText1Anim");
                            yield return new WaitForSeconds(2f);
                            imageAnim.Play("ExpertImageDisappear");
                            text1Anim.Play("ExpertText1Disappear");
                            yield return new WaitForSeconds(2f);
                            expertDrainPowerImage.transform.localPosition = new Vector3(556.9971f, 0f, 0f);
                            expertDrainPowerNameText.transform.localPosition = new Vector3(560.9915f, -114f, 0f);
                            expertDrainPowerNameText2.transform.localPosition = new Vector3(556f, -194f, 0f);
                        }
                    }
                    else
                    {
                        WeaponSelection();
                        yield break;
                    }
                    break;
                }
            case 3:
                {
                    if (player.GetHardWeaponChoice() == false)
                    {
                        int i = player.SpawnRandomWeapon();
                        if (i == 1)
                        {
                            expertDrainPowerImage.GetComponent<Image>().sprite = sprites[6];
                            expertDrainPowerNameText.GetComponent<Text>().text = "You are now using the staff!";
                            expertDrainPowerImage.transform.localPosition = new Vector3(0f, 0f, 0f);
                            expertDrainPowerNameText.transform.localPosition = new Vector3(0f, -114f, 0f);
                            expertDrainPowerNameText2.transform.localPosition = new Vector3(0f, -194f, 0f);
                            imageAnim.Play("ExpertImageAppear");
                            text1Anim.Play("ExpertText1Anim");
                            yield return new WaitForSeconds(2f);
                            imageAnim.Play("ExpertImageDisappear");
                            text1Anim.Play("ExpertText1Disappear");
                            yield return new WaitForSeconds(2f);
                            expertDrainPowerImage.transform.localPosition = new Vector3(556.9971f, 0f, 0f);
                            expertDrainPowerNameText.transform.localPosition = new Vector3(560.9915f, -114f, 0f);
                            expertDrainPowerNameText2.transform.localPosition = new Vector3(556f, -194f, 0f);
                        }
                        else if (i == 0)
                        {
                            expertDrainPowerImage.GetComponent<Image>().sprite = sprites[8];
                            expertDrainPowerNameText.GetComponent<Text>().text = "You are now using the sword!";
                            expertDrainPowerImage.transform.localPosition = new Vector3(0f, 0f, 0f);
                            expertDrainPowerNameText.transform.localPosition = new Vector3(0f, -114f, 0f);
                            expertDrainPowerNameText2.transform.localPosition = new Vector3(0f, -194f, 0f);
                            imageAnim.Play("ExpertImageAppear");
                            text1Anim.Play("ExpertText1Anim");
                            yield return new WaitForSeconds(2f);
                            imageAnim.Play("ExpertImageDisappear");
                            text1Anim.Play("ExpertText1Disappear");
                            yield return new WaitForSeconds(2f);
                            expertDrainPowerImage.transform.localPosition = new Vector3(556.9971f, 0f, 0f);
                            expertDrainPowerNameText.transform.localPosition = new Vector3(560.9915f, -114f, 0f);
                            expertDrainPowerNameText2.transform.localPosition = new Vector3(556f, -194f, 0f);
                        }
                    }
                    else
                    {
                        WeaponSelection();
                        yield break;
                    }
                    break;
                }
            case 4:
                {
                    if (player.GetExpertWeaponChoice() == false)
                    {
                        int i = player.SpawnRandomWeapon();
                        if (i == 1)
                        {
                            expertDrainPowerImage.GetComponent<Image>().sprite = sprites[6];
                            expertDrainPowerNameText.GetComponent<Text>().text = "You are now using the staff!";
                            expertDrainPowerImage.transform.localPosition = new Vector3(0f, 0f, 0f);
                            expertDrainPowerNameText.transform.localPosition = new Vector3(0f, -114f, 0f);
                            expertDrainPowerNameText2.transform.localPosition = new Vector3(0f, -194f, 0f);
                            imageAnim.Play("ExpertImageAppear");
                            text1Anim.Play("ExpertText1Anim");
                            yield return new WaitForSeconds(2f);
                            imageAnim.Play("ExpertImageDisappear");
                            text1Anim.Play("ExpertText1Disappear");
                            yield return new WaitForSeconds(2f);
                            expertDrainPowerImage.transform.localPosition = new Vector3(556.9971f, 0f, 0f);
                            expertDrainPowerNameText.transform.localPosition = new Vector3(560.9915f, -114f, 0f);
                            expertDrainPowerNameText2.transform.localPosition = new Vector3(556f, -194f, 0f);
                        }
                        else if (i == 0)
                        {
                            expertDrainPowerImage.GetComponent<Image>().sprite = sprites[8];
                            expertDrainPowerNameText.GetComponent<Text>().text = "You are now using the sword!";
                            expertDrainPowerImage.transform.localPosition = new Vector3(0f, 0f, 0f);
                            expertDrainPowerNameText.transform.localPosition = new Vector3(0f, -114f, 0f);
                            expertDrainPowerNameText2.transform.localPosition = new Vector3(0f, -194f, 0f);
                            imageAnim.Play("ExpertImageAppear");
                            text1Anim.Play("ExpertText1Anim");
                            yield return new WaitForSeconds(2f);
                            imageAnim.Play("ExpertImageDisappear");
                            text1Anim.Play("ExpertText1Disappear");
                            yield return new WaitForSeconds(2f);
                            expertDrainPowerImage.transform.localPosition = new Vector3(556.9971f, 0f, 0f);
                            expertDrainPowerNameText.transform.localPosition = new Vector3(560.9915f, -114f, 0f);
                            expertDrainPowerNameText2.transform.localPosition = new Vector3(556f, -194f, 0f);
                        }
                    }
                    else
                    {
                        WeaponSelection();
                        yield break;

                    }
                    break;
                }
            default:
                {
                    break;
                }
        }
        Progression p = GameObject.FindGameObjectWithTag("ProgressionHandler").GetComponent<Progression>();
        expertDrainPowerNameText.GetComponent<Text>().text = "Wave " + p.GetCurrentWave();
        text1Anim.Play("ExpertText1Anim");
        yield return new WaitForSeconds(3f);
        text1Anim.Play("ExpertText1Disappear");
        yield return new WaitForSeconds(1f);
        expertDrainPowerImage.transform.localPosition = new Vector3(556.9971f, 0f, 0f);
        expertDrainPowerNameText.transform.localPosition = new Vector3(560.9915f, -114f, 0f);
        expertDrainPowerNameText2.transform.localPosition = new Vector3(556f, -194f, 0f);
        p.SetWaveComplete(false);
        returnToMainGame();

    }

    public void CallLeaderboards(int difficulty)
    {
        LoadLeaderboard(difficulty);
    }

    public void ShowPowerActivatedMessage(int ID)
    {
        StartCoroutine("ShowMessage1", ID);
    }
    IEnumerator ShowMessage1(int ID)
    {
        if (expertDrainPowerImage.activeInHierarchy == true || expertDrainPowerNameText.activeInHierarchy == true || expertDrainPowerNameText2.activeInHierarchy == true)
        {
            yield return new WaitForSeconds(1.5f);
        }
        expertDrainPowerImage.SetActive(true);
        expertDrainPowerNameText.SetActive(true);
        expertDrainPowerNameText2.SetActive(true);
        Animation imageAnim = expertDrainPowerImage.GetComponent<Animation>();
        Animation text1Anim = expertDrainPowerNameText.GetComponent<Animation>();
        Animation text2Anim = expertDrainPowerNameText2.GetComponent<Animation>();
        expertDrainPowerImage.GetComponent<Image>().sprite = sprites[ID + 11];
        expertDrainPowerNameText.GetComponent<Text>().text = powerController.powerHandler[ID+11].GetPowerName();
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
     * If there is no name, default is "Anonymous".
     * Note there is no name filtering system in place at this time.
     * 
     */

    void LoadLeaderboard(int DifficultyID)
    {
        int firstPoints, secondPoints, thirdPoints, fourthPoints, fifthPoints;
        switch (DifficultyID)
        {
            case 1: //EASY
                {
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
                break;
            case 2: //NORMAL.
                {
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
                break;
            case 3: //HARD
                {
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
                    break;
                }
            case 4: //EXPERT.
                {
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
                    break;
                }
            default:
                {
                    break;
                }
        }
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
        waveCompletePause = true;
        yield return new WaitForSeconds(2f);
        GameObject.Find("BackgroundPowerDrain").SetActive(false);
        //Switch depending on which difficulty they are on.
        switch (PlayerPrefs.GetInt("DifficultyChosen"))
        {
            case 1:
                {
                    if (player.GetEasyPowerDrain() == true)
                    {
                        PowerDrainScreen();
                        break;
                    }
                    else
                    {

                        powerController.LosePowerHard();
                        break;
                    }
                   
                }
            case 2:
                {
                    if (player.GetNormalPowerDrain() == true)
                    {
                        PowerDrainScreen();
                        break;
                    }
                    else
                    {
                        powerController.LosePowerHard();
                        break;
                    }
                }
            case 3:
                {
                    if (player.GetHardPowerDrain() == true)
                    {
                        PowerDrainScreen();
                        break;
                    }
                    else
                    {
                        powerController.LosePowerHard();
                        break;
                    }
                }
            case 4:
                {
                    if (player.GetExpertPowerDrain() == true)
                    {
                        PowerDrainScreen();
                        break;
                    }
                    else
                    {
                        powerController.LosePowerHard();
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
        isPaused = true;
    }
    public void GameCompleteText()
    {
        StartCoroutine("GameCompleteRoutine");
    }
    IEnumerator GameCompleteRoutine()
    {
        blackout = true;
        yield return new WaitForSeconds(2.5f);
        isPaused = true;
        storeHighscores(powerController.difficultyLevel);
        GameSummaryScreen(false);
        Cursor.lockState = CursorLockMode.None;
        yield return new WaitForEndOfFrame();
    }
    void FadeBlackout(Image image)
    {
        if (image.color.a >= 1)
        {
            return;
        }
        float alphaValue = image.color.a;
        alphaValue = alphaValue + Time.deltaTime;
        image.color = new Color(1, 1, 1, alphaValue);

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
            if (obj.CompareTag("RemoveWhenResumed"))
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
        PowerBehaviour p = powerController.GetComponent<PowerBehaviour>();
        Image j;
        for (int i = 0; i < 14; i++)
        {
            if (!p.powerHandler[i].GetPowerActive()) //Check if the power is false.
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
        if (TutorialStatus.GetTutorialStage() > 0)
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

    public void GameSummaryScreen(bool playerDied)
    {
        Cursor.lockState = CursorLockMode.None;
        postObj.SetActive(true);
        currentScore.gameObject.SetActive(false); //Set the currentscore obj to false
        //Get current wave ID.
        GameObject progression = GameObject.Find("ProgressionHandler");
        Progression progObj = progression.GetComponent<Progression>();
        deathOrCompletionText.gameObject.SetActive(true);
        if (playerDied)
        {
            GameObject.FindWithTag("MusicHandler").GetComponent<AudioController>().PlayMusic(1);
            postWave.GetComponent<Text>().text = "Died on wave: " + progObj.GetCurrentWave();
            deathOrCompletionText.text = "You have perished to the devil's forces...";

        }
        else
        {
            postWave.GetComponent<Text>().text = "All waves successfully completed!";
            deathOrCompletionText.text = "You have successfully defended aginst the devil's forces!";
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
        Progression progressionCheck = GameObject.Find("ProgressionHandler").GetComponent<Progression>();
        Cursor.lockState = CursorLockMode.None;
        lockPauseMenu = true;
        if (audioController == null)
        {
            audioController = GameObject.FindGameObjectWithTag("MusicHandler");
        }
        audioController.GetComponent<AudioController>().PlayMusic(1);
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
        losePower1 = p.GetID();
        PowerDrain1.GetComponent<Image>().sprite = sprites[losePower1];
        powerName1.GetComponent<Text>().text = p.GetPowerName();
        powerDesc1.GetComponent<Text>().text = FormatPowerDescription(losePower1);
        switch (p.PowerStrength)
        {
            case 3:
                {
                    bonusPoints1.text = "Bonus points on offer: " + ((progressionCheck.GetMaximumWaves() - progressionCheck.GetCurrentWave()) * 500);
                    break;
                }
            case 2:
                {
                    bonusPoints1.text = "Bonus points on offer: " + ((progressionCheck.GetMaximumWaves() - progressionCheck.GetCurrentWave()) * 250);
                    break;
                }
            case 1:
                {
                    bonusPoints1.text = "No bonus points available.";
                    break;
                }
            default:
                {
                    bonusPoints1.text = "";
                    break;
                }
        }
        p = powerController.RandomAvailablePower();
        losePower2 = p.GetID();
        while (losePower1 == losePower2)
        {
            p = powerController.RandomAvailablePower();
            losePower2 = p.GetID();
        }
        PowerDrain2.GetComponent<Image>().sprite = sprites[losePower2];
        powerDesc2.GetComponent<Text>().text = FormatPowerDescription(losePower2);
        powerName2.GetComponent<Text>().text = p.GetPowerName();
        switch (p.PowerStrength)
        {
            case 3:
                {
                    bonusPoints2.text = "Bonus points on offer: " + ((progressionCheck.GetMaximumWaves() - progressionCheck.GetCurrentWave()) * 500);
                    break;
                }
            case 2:
                {
                    bonusPoints2.text = "Bonus points on offer: " + ((progressionCheck.GetMaximumWaves() - progressionCheck.GetCurrentWave()) * 250);
                    break;
                }
            case 1:
                {
                    bonusPoints2.text = "No bonus points available.";
                    break;
                }
            default:
                {
                    bonusPoints2.text = "";
                    break;
                }
        }

    }

    public void PowerDrainScreen(int power1ToDrain, int power2ToDrain) //Will be called by the Progression handler when the player completes a wave.
    {
        Cursor.lockState = CursorLockMode.None;
        lockPauseMenu = true;
        if (audioController == null)
        {
            audioController = GameObject.FindGameObjectWithTag("MusicHandler");
        }
        audioController.GetComponent<AudioController>().PlayMusic(1);
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
        powerName1.GetComponent<Text>().text = p.GetPowerName();
        powerDesc1.GetComponent<Text>().text = FormatPowerDescription(losePower1);
        p = powerController.getSpecificPower(power2ToDrain);
        losePower2 = p.GetID();
        PowerDrain2.GetComponent<Image>().sprite = sprites[losePower2];
        powerDesc2.GetComponent<Text>().text = FormatPowerDescription(losePower2);
        powerName2.GetComponent<Text>().text = p.GetPowerName();

    }

    string FormatPowerDescription(int powerID)
    {
        string formattedString = "";

        switch (powerID)
        {
            case 0:
                {
                    formattedString = "You are unable to die!";
                    return formattedString;
                }
            case 1:
                {
                    formattedString = "All damage dealt \nis doubled against enemies.";
                    return formattedString;
                }
            case 2:
                {
                    formattedString = "Your health is doubled.";
                    return formattedString;
                }
            case 3:
                {
                    formattedString = "Your health regenerates\nat a rate of 2.5% per second.";
                    return formattedString;
                }
            case 4:
                {
                    formattedString = "As you lose health,\nyou will deal an increasing amount\nof damage proportional to health lost.";
                    return formattedString;
                }
            case 5:
                {
                    formattedString = "You will not be damaged by fire.";
                    return formattedString;
                }
            case 6:
                {
                    formattedString = "Magical weapons attack 33% faster.";
                    return formattedString;
                }
            case 7:
                {
                    formattedString = "Your enemies have been cursed.\nThey are now 50% bigger.\nA certain enemy is not affected by this power.";
                    return formattedString;
                }
            case 8:
                {
                    formattedString = "Swords deal more damage.";
                    return formattedString;
                }
            case 9:
                {
                    formattedString = "You take 50% less damage.";
                    return formattedString;
                }
            case 10:
                {
                    formattedString = "A certain weapon can now \nextinguish fires temporarily.";
                    return formattedString;
                }
            case 11:
                {
                    formattedString = "???";
                    return formattedString;
                }
            case 12:
                {
                    formattedString = "You have a small chance of all fires\nbeing temporarily extinguished\nfor 45 seconds.";
                    return formattedString;
                }
            case 13:
                {
                    formattedString = "Occasionally, projectiles will fall\nfrom the sky and damage\n all enemies.";
                    return formattedString;
                }
            default:
                {
                    formattedString = "";
                    Debug.LogError("An error has occurred loading power description.");
                    return formattedString;
                }
        }

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
        if (TutorialStatus == null)
        {
            TutorialStatus = GameObject.FindWithTag("TutorialHandler").GetComponent<TutorialHandler>();
        }
        if (TutorialStatus.GetTutorialStage() > 0)
        {
            GetGameObjectsGame();
            GameUnpausedFunction();
            currentScore.text = "Points: " + pointsGained;
            ShowAllPowersInGame();
            inPauseMenu = false;
            if (TutorialStatus.GetTutorialStage() > 11)
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
            if (powerController.powerHandler[i].GetPowerActive())
            {
                spriteUIElements[ObjsFilled].GetComponent<Image>().sprite = sprites[i];
                spriteUIElements[ObjsFilled].GetComponent<Image>().enabled = true;
                ObjsFilled = ObjsFilled + 1;
            }
        }
    }

    public void EnableStunWarning()
    {
        UIStunnedMessage.SetActive(true);
    }
    public void DisableStunWarning()
    {
        UIStunnedMessage.SetActive(false);
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
            //LoadLeaderboard(1);
        }


    }

    //Points modifiers======================================
    public void AddPoints(float pointsToAdd)
    {
        pointsToAdd = pointsToAdd * player.GetPointsModifier();
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

    void ProgressUpdate()
    {
        //if (progressBar != null && progressBar.enabled)
        //{
        //    progressBar.fillAmount = p.GetProgression();
        //}

    }

    //Called by the button caller when a difficulty is chosen.
    public void ChosenDifficulty(int chosenDifficultyInt)
    {
        switch (chosenDifficultyInt)
        {
            case 1:
                {
                    chooseDifficultyDescriptions.text = EasyDescription;
                    chosenDifficulty.text = "Selected Difficulty: Easy";
                    intChosenDifficulty = 1;
                    return;
                }
            case 2:
                {
                    chooseDifficultyDescriptions.text = NormalDescription;
                    chosenDifficulty.text = "Selected Difficulty: Normal";
                    intChosenDifficulty = 2;
                    return;
                }
            case 3:
                {
                    chooseDifficultyDescriptions.text = HardDescription;
                    chosenDifficulty.text = "Selected Difficulty: Hard";
                    intChosenDifficulty = 3;
                    return;
                }
            case 4:
                {
                    chooseDifficultyDescriptions.text = ExpertDescription;
                    chosenDifficulty.text = "Selected Difficulty: Expert";
                    intChosenDifficulty = 4;
                    return;
                }
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "UI Scale Testing")
        {
            Time.timeScale = 1.0f; //Set timescale to 1.
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
        }
        else
        {
            {
                if (blackout)
                {
                    FadeBlackout(blackoutImage);
                }
                if (Input.GetKeyDown(KeyCode.F12))
                {
                    GameCompleteText();
                }
                if (isPaused == true)
                {
                    if ((TutorialStatus != null && TutorialStatus.GetTutorialStage() > 0) || playerDead || waveCompletePause)
                    {
                        Time.timeScale = 1.0f;
                    }
                    else
                    {
                        Time.timeScale = 0.0f;
                    }
                }
                else
                {
                    Time.timeScale = 1.0f;
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
                        AudioSource[] objs = FindObjectsOfType<AudioSource>();
                        foreach (AudioSource obj in objs)
                        {
                            if (obj.tag == "MusicHandler")
                            {

                            }
                            else
                            {
                                if (obj.clip != null)
                                {
                                    obj.Pause();
                                }
                            }
                        }
                        Debug.Log("Escape pressed, game paused!");
                        GamePausedFunction();
                        Cursor.lockState = CursorLockMode.None;
                    }
                    else
                    {
                        inPauseMenu = false;
                        isPaused = false;
                        AudioSource[] objs = FindObjectsOfType<AudioSource>();
                        foreach (AudioSource obj in objs)
                        {
                            if (obj.tag == "MusicHandler")
                            {

                            }
                            else
                            {
                                obj.UnPause();
                            }
                        }
                        Debug.Log("Game resumed!");
                        GameUnpausedFunction();
                        Cursor.lockState = CursorLockMode.Locked;
                        return;

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
                ProgressUpdate();
            }
        }
       
    }
            

  

    public void SetPauseMenuLock(bool value)
    {
        lockPauseMenu = value;
    }
    //OVERRIDE, USED BY PLAYER SCRIPT. USE SETISPAUSED(bool value) INSTEAD.
    public void SetIsPaused(bool value, bool playerDeadVal) 
    {
        isPaused = value;
        playerDead = playerDeadVal;
    }
    public bool GetInPauseMenu()
    {
        return inPauseMenu;
    }
    public bool GetIsPaused()
    {
        return isPaused;
    }
    public void SetIsPaused(bool value)
    {
        isPaused = value;
    }

}
