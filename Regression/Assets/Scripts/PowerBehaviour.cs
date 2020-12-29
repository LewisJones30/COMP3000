using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PowerBehaviour : MonoBehaviour //THE GAMEOBJECT THAT THIS SCRIPT IS ATTACHED TO IS CREATED WHEN THE GAME IS LOADED, AND IS ALWAYS KEPT BETWEEN SCENES.
{

    public int difficultyLevel;
    public Power[] powerHandler = new Power[20];
    UIController redrawCurrentPowers;
    Player player;
    public bool gameStarted = false;
    // Start is called before the first frame update
    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        InitialisePowers();
        if (SceneManager.GetActiveScene().name == "Game")
        {
            difficultyLevel = PlayerPrefs.GetInt("DifficultyChosen");
            LoadDifficulty();
            player = GameObject.Find("Player").GetComponent<Player>();

            redrawCurrentPowers = GameObject.Find("UIHandler").GetComponent<UIController>();

            

        }



    }
    void LoadDifficulty()
    {
        if (difficultyLevel == 1)
        {
            StartGameEasy();
        }
        else if (difficultyLevel == 2)
        {
            StartGameNormal();
        }
        else if (difficultyLevel == 3)
        {
            StartGameHard();
        }
        else if (difficultyLevel == 4)
        {
            StartGameExpert();
        }
        else if (difficultyLevel == 5)
        {
            StartGameExpert();
        }
        else
        {
            Debug.Log("Error: DifficultyLevel is not defined correctly!");

        }
    }
    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name != "Game")
        {
            return;
        }
        if (player == null)
        {
            player = GameObject.Find("Player").GetComponent<Player>();
        }

    }
    public void FindObjects()
    {
        redrawCurrentPowers = GameObject.Find("UIHandler").GetComponent<UIController>();
        player = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Player>();
    }
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        redrawCurrentPowers = GameObject.Find("UIHandler").GetComponent<UIController>();
        player = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Player>();
       
    }
    void StartPower(int numbPowers) //Pass in from difficulty how many to enable
    {
        //As not all slots are filled, this reduces to make sure.
        if (numbPowers > 10)
        {
            numbPowers = 10;
        }
        for (int i = 0; i < numbPowers; i++)
        {
            bool enabled = false;
            while (enabled == false)
            {
                Power chosenPower = RandomPower();
                if (chosenPower.PowerAvailable == true && chosenPower.PowerActive == false)
                {
                    chosenPower.PowerActive = true;
                    chosenPower.PowerStartedActive = true;
                    Debug.Log(chosenPower.PowerName + " has been enabled!");
                    enabled = true;
                }
            }
        }
        if (player != null)
        {
            player.ModifyPlayer();
        }
        else
        {
            player = GameObject.Find("Player").GetComponent<Player>();
            player.ModifyPlayer();
        }

    }
    public string ModifierText()
    {
        string ModifierText = "";
        for (int i = 0; i < powerHandler.Length; i++)
        {
            if (powerHandler[i].PowerActive == true)
            {
                ModifierText = ModifierText + "• " + powerHandler[i].PowerName + "\n";
            }
        }
        return ModifierText;
    }
    //====================================This section is the difficulty controller. This is executed when the player presses what difficulty they wish to run. ==================================
    //Still requires a trigger to change the scene. Needs to keep this object persistent in the next scene as well.
    //No scene has been added yet.
    //These methods are currently public as the UI will need to be able to send these triggers.
    public void StartGameEasy()
    {
        difficultyLevel = 1;
        StartPower(5); //Start with all available powers.
    }
    public void StartGameNormal()
    {
        difficultyLevel = 2;
        StartPower(19); //Start with all available powers.
    }
    public void StartGameHard()
    {
        difficultyLevel = 3;
        StartPower(19); //Start with all available powers.
    }
    public void StartGameExpert()
    {
        difficultyLevel = 4;
        StartPower(14); //Start with 5 less powers.
    }
    public void StartGameSatanic()
    {
        difficultyLevel = 5;
        StartPower(14); //Start with 5 less powers.
    }
    public void StartGameTutorial()
    {
        if (gameStarted == true)
        {
            return;
        }
        difficultyLevel = 6;
        powerHandler[0].PowerAvailable = true;
        StartPower(20); //Start with all powers. 
    }
    public void EndGameTutorial() //Public to allow UI to trigger this
    {
        powerHandler[0].PowerAvailable = false;
        resetPowers();
    }
    public void EndGame() //Public to allow UI to trigger this
    {
        resetPowers();
    }
    Power RandomPower() //Used for initialising the game, as well as when the game takes powers away from the player.
    {
            System.Random r = new System.Random();
            int powerNumber = r.Next(0, 19); //Within the 20 values.
            var currentPower = powerHandler[powerNumber];
            return currentPower;

    }
    void resetPowers() //Code to reset powers when the game ends
    {
        //Failsafe incase a player somehow manages to trigger EndGame instead of EndGameTutorial within the tutorial.
        if (powerHandler[0].PowerAvailable == true)
        {
            powerHandler[0].PowerAvailable = false;
        }
        for (int i = 0; i < powerHandler.Length; i++)
        {
            powerHandler[i].PowerActive = false;
        }
    }

    void losePowerEasyNormal() //The player is able to choose which power they want to lose.
    {
     
    }
    //In hard and higher, the game will choose a power to disable immediately for you.
    public void losePowerHard() //Returns which power was drained for use in UI.
    {
        bool powerDrained = false;
        int drainedPowerID = 255;
        int cycledAttempts = 0;
        System.Random r = new System.Random();
        while (powerDrained == false)
        {
            if (cycledAttempts > 1000)
            {
                Debug.Log("ERROR: NO POWERS LEFT TO DRAIN.");
                break;
            }
            int powerNumber = r.Next(0, 19);
            if (powerHandler[powerNumber].PowerActive == true)
            {
                powerHandler[powerNumber].PowerActive = false; //Disable the power
                powerDrained = true;
                drainedPowerID = powerNumber;
            }
            cycledAttempts = cycledAttempts + 1;
        }
        redrawCurrentPowers.LostPowerMessage(drainedPowerID);
        
    }

    public void loseAllPowers() //Temporary method. Used for first build.
    {
        for (int i = 0; i < powerHandler.Length; i++)
        {
            powerHandler[i].PowerActive = false;
        }
        player = GameObject.Find("Player").GetComponent<Player>();
        player.PowerLost();
        
    }
    //=======================================================================Initialise each power====================================================================
    void InitialisePowers() //Fill each value of the powers in code here manually.
    {
        //Power slot 1
        Power slot1 = new Power();
        slot1.ID = 0;
        slot1.PowerName = "Immune to death.";
        slot1.PowerDescription = "You are unable to die with this power enabled!";
        slot1.PowerAvailable = false; //ONLY ENABLED WITH THE TUTORIAL, OTHERWISE THIS MUST BE LEFT FALSE.
        slot1.PowerActive = false;
        slot1.PowerStartedActive = false;
        //Power slot 2
        Power slot2 = new Power();
        slot2.ID = 1;
        slot2.PowerName = "Will of the Gods";
        slot2.PowerDescription = "All damage dealt is doubled against enemies.";
        slot2.PowerAvailable = true;
        slot2.PowerActive = false;
        slot2.PowerStartedActive = false;

        //Power slot 3
        Power slot3 = new Power();
        slot3.ID = 2;
        slot3.PowerName = "Not quite immortality";
        slot3.PowerDescription = "Your health is doubled.";
        slot3.PowerAvailable = true;
        slot3.PowerActive = false;
        slot3.PowerStartedActive = false;

        //Power slot 4
        Power slot4 = new Power();
        slot4.ID = 3;
        slot4.PowerName = "I don't need healing, i'll do it myself";
        slot4.PowerDescription = "Your health regenerates at 2.5% per second.";
        slot4.PowerAvailable = true;
        slot4.PowerActive = false;
        slot4.PowerStartedActive = false;

        //Power slot 5
        Power slot5 = new Power();
        slot5.ID = 4;
        slot5.PowerName = "Playing with fire";
        slot5.PowerDescription = "As you lose health, you will deal an increasing amount of damage proportional to health lost.";
        slot5.PowerAvailable = true;
        slot5.PowerActive = false;
        slot5.PowerStartedActive = false;

        //Power slot 6
        Power slot6 = new Power();
        slot6.ID = 5;
        slot6.PowerName = "Imbued with fire";
        slot6.PowerDescription = "You will not be damaged by fire.";
        slot6.PowerAvailable = true;
        slot6.PowerActive = false;
        slot6.PowerStartedActive = false;

        //Power slot 7
        Power slot7 = new Power();
        slot7.ID = 6;
        slot7.PowerName = "Wizard's Words";
        slot7.PowerDescription = "Magical weapons attack faster.";
        slot7.PowerAvailable = true;
        slot7.PowerActive = false;
        slot7.PowerStartedActive = false;

        //Power slot 8
        Power slot8 = new Power();
        slot8.ID = 7;
        slot8.PowerName = "Skilled Archer";
        slot8.PowerDescription = "Arrow based weapons reload faster.";
        slot8.PowerAvailable = true;
        slot8.PowerActive = false;
        slot8.PowerStartedActive = false;

        //Power slot 9
        Power slot9 = new Power();
        slot9.ID = 8;
        slot9.PowerName = "Warrior Spirit";
        slot9.PowerDescription = "Swords deal more damage and penetrate armour better.";
        slot9.PowerAvailable = true;
        slot9.PowerActive = false;
        slot9.PowerStartedActive = false;

        //Power slot 10
        Power slot10 = new Power();
        slot10.ID = 9;
        slot10.PowerName = "Iron Maiden";
        slot10.PowerDescription = "You take 50% less damage.";
        slot10.PowerAvailable = true;
        slot10.PowerActive = false;
        slot10.PowerStartedActive = false;

        //Power slot 11
        Power slot11 = new Power();
        slot11.ID = 10;
        slot11.PowerName = "Posiedon's Blessing";
        slot11.PowerDescription = "A certain weapon can now extinguish fires temporarily.";
        slot11.PowerAvailable = true;
        slot11.PowerActive = false;
        slot11.PowerStartedActive = false;

        //Power slot 12
        Power slot12 = new Power();
        slot12.ID = 11;
        slot12.PowerName = "???";
        slot12.PowerDescription = "???";
        slot12.PowerAvailable = true;
        slot12.PowerActive = false;
        slot12.PowerStartedActive = false;

        //Power slot 13
        Power slot13 = new Power();
        slot13.ID = 12;
        slot13.PowerName = "null";
        slot13.PowerDescription = "null";
        slot13.PowerAvailable = false;
        slot13.PowerActive = false;
        slot13.PowerStartedActive = false;

        //Power slot 14
        Power slot14 = new Power();
        slot14.ID = 13;
        slot14.PowerName = "null";
        slot14.PowerDescription = "null";
        slot14.PowerAvailable = false;
        slot14.PowerActive = false;
        slot14.PowerStartedActive = false;

        //Power slot 15
        Power slot15 = new Power();
        slot15.ID = 14;
        slot15.PowerName = "null";
        slot15.PowerDescription = "null";
        slot15.PowerAvailable = false;
        slot15.PowerActive = false;
        slot15.PowerStartedActive = false;

        //Power slot 16
        Power slot16 = new Power();
        slot16.ID = 15;
        slot16.PowerName = "null";
        slot16.PowerDescription = "null";
        slot16.PowerAvailable = false;
        slot16.PowerActive = false;
        slot16.PowerStartedActive = false;

        //Power slot 17
        Power slot17 = new Power();
        slot17.ID = 16;
        slot17.PowerName = "null";
        slot17.PowerDescription = "null";
        slot17.PowerAvailable = false;
        slot17.PowerActive = false;
        slot17.PowerStartedActive = false;

        //Power slot 18
        Power slot18 = new Power();
        slot18.ID = 17;
        slot18.PowerName = "null";
        slot18.PowerDescription = "null";
        slot18.PowerAvailable = false;
        slot18.PowerActive = false;
        slot18.PowerStartedActive = false;

        //Power slot 19
        Power slot19 = new Power();
        slot19.ID = 18;
        slot19.PowerName = "null";
        slot19.PowerDescription = "null";
        slot19.PowerAvailable = false;
        slot19.PowerActive = false;
        slot19.PowerStartedActive = false;

        //Power slot 20
        Power slot20 = new Power();
        slot20.ID = 19;
        slot20.PowerName = "null";
        slot20.PowerDescription = "null";
        slot20.PowerAvailable = false;
        slot20.PowerActive = false;
        slot20.PowerStartedActive = false;

        //Fill the array

        //Powers 1 to 10
        powerHandler[0] = slot1;
        powerHandler[1] = slot2;
        powerHandler[2] = slot3;
        powerHandler[3] = slot4;
        powerHandler[4] = slot5;
        powerHandler[5] = slot6;
        powerHandler[6] = slot7;
        powerHandler[7] = slot8;
        powerHandler[8] = slot9;
        powerHandler[9] = slot10;
        //Powers 11 to 20

        powerHandler[10] = slot11;
        powerHandler[11] = slot12;
        powerHandler[12] = slot13;
        powerHandler[13] = slot14;
        powerHandler[14] = slot15;
        powerHandler[15] = slot16;
        powerHandler[16] = slot17;
        powerHandler[17] = slot18;
        powerHandler[18] = slot19;
        powerHandler[19] = slot20;

    }



    public class Power
    {
        public int ID { get; set; } //ID of the power between 1 and 20
        public string PowerName { get; set; } //Name of the power
        public string PowerDescription { get; set; } //Description of what this power does
        public bool PowerActive { get; set; } //Whether the power is active or not. This will start as default of all false.
        public bool PowerStartedActive { get; set; } //This is a check to see if the boolean was active at the start of the game. Whenever a power is drained, it is a check to ensure the player has the correct stats.
        public bool PowerAvailable { get; set; } //Modifier to show if this power is actually ready to be used. E.G tutorial will be false outside of tutorial. 
                                                 //This also applies to any empty slots with default/null values.
    }
}
