using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PowerBehaviour : MonoBehaviour //THE GAMEOBJECT THAT THIS SCRIPT IS ATTACHED TO IS CREATED WHEN THE GAME IS LOADED, AND IS ALWAYS KEPT BETWEEN SCENES.
{
    //Public variables
    public int difficultyLevel;
    public Power[] powerHandler = new Power[20];
    public bool gameStarted = false;

    //Private variables
    UIController redrawCurrentPowers;
    Player player;
    bool AllFiresActive = true; //Used to ensure allfiresactive is not triggered multiple times.
    int powersDrainedCount = 0;
    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        InitialisePowers();
        if (SceneManager.GetActiveScene().name == "Game")
        {
            difficultyLevel = PlayerPrefs.GetInt("DifficultyChosen");
            LoadDifficulty();
            player = GameObject.FindWithTag("MainCamera").GetComponent<Player>();

            redrawCurrentPowers = GameObject.FindWithTag("UIHandler").GetComponent<UIController>();
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
            player = GameObject.FindWithTag("MainCamera").GetComponent<Player>();
        }
        if (player != null)
        {
            player.PowerLost();
        }

    }
    public void FindObjects()
    {
        redrawCurrentPowers = GameObject.FindWithTag("UIHandler").GetComponent<UIController>();
        player = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Player>();
    }
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        redrawCurrentPowers = GameObject.FindWithTag("UIHandler").GetComponent<UIController>();
        player = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Player>();
       
    }
    void StartPower(int numbPowers) //Pass in from difficulty how many to enable
    {
        //As not all slots are filled, this reduces to make sure.
        if (numbPowers > 13)
        {
            numbPowers = 13;
        }
        for (int i = 0; i < numbPowers; i++)
        {
            bool enabled = false;
            while (enabled == false)
            {
                Power chosenPower = RandomPower();
                if (chosenPower.GetPowerAvailable() && !chosenPower.GetPowerActive())
                {
                    chosenPower.SetPowerActive(true);
                    chosenPower.SetPowerStartedActive(true);
                    Debug.Log(chosenPower.GetPowerName() + " has been enabled!");
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
            player = GameObject.FindWithTag("MainCamera").GetComponent<Player>();
            player.ModifyPlayer();
        }

    }
    //Code to extinguish all fires immediately. Triggered by the player with a percentile chance.
    public void DisableAllFires()
    {
        if (AllFiresActive == true)
        {
            StartCoroutine("disableAllFiresTemporarily");
            AllFiresActive = false;
        }
    }
    IEnumerator disableAllFiresTemporarily()
    {
        GameObject[] allFireobjs = GameObject.FindGameObjectsWithTag("FireDamager");
        foreach (GameObject obj in allFireobjs)
        {
            //Cycle through all of the objects, disable all of the fire effects.
            var emission = obj.gameObject.GetComponent<ParticleSystem>().emission;
            emission.enabled = false;
            obj.gameObject.GetComponent<BoxCollider>().enabled = false;
        }
        GameObject rainEffect;
        SetRainEffect(true);
        Debug.Log("All fires disabled!");
        float timer = 0.0f;
        UIController ui = GameObject.FindGameObjectWithTag("UIHandler").GetComponent<UIController>();
        yield return new WaitForSeconds(45);
        SetRainEffect(false);
        foreach (GameObject obj in allFireobjs)
        {
            var emission = obj.gameObject.GetComponent<ParticleSystem>().emission;
            emission.enabled = true;
            obj.gameObject.GetComponent<BoxCollider>().enabled = true;
        }
        AllFiresActive = true;
        Debug.Log("All fires enabled!");
    }

    void SetRainEffect(bool value)
    {
        foreach (GameObject obj in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
        {
            if (obj.name == "RainEffect")
            {
                obj.SetActive(value);
            }
        }
    }
    //====================================This section is the difficulty controller. This is executed when the player presses what difficulty they wish to run. ==================================
    //Still requires a trigger to change the scene. Needs to keep this object persistent in the next scene as well.
    //No scene has been added yet.
    //These methods are currently public as the UI will need to be able to send these triggers.
    public void StartGameEasy()
    {
        difficultyLevel = 1;
        StartPower(13); //Start with all available powers.
    }
    public void StartGameNormal()
    {
        difficultyLevel = 2;
        StartPower(13); //Start with all available powers.
    }
    public void StartGameHard()
    {
        difficultyLevel = 3;
        StartPower(12); //Start with 1 less power.
    }
    public void StartGameExpert()
    {
        difficultyLevel = 4;
        StartPower(11); //Start with 2 less powers - Final boss player will have 1 power.
    }
    public void StartGameSatanic()
    {
        difficultyLevel = 5;
        StartPower(10); //Start with 5 less powers.
    }
    public void StartGameTutorial()
    {
        if (gameStarted == true)
        {
            return;
        }
        difficultyLevel = 6;
        powerHandler[0].SetPowerAvailable(true);
        StartPower(19); //Start with all powers. 
    }
    public void EndGameTutorial() //Public to allow UI to trigger this
    {
        powerHandler[0].SetPowerAvailable(false);
        resetPowers();
    }
    public void EndGame() //Public to allow UI to trigger this
    {
        resetPowers();
    }
    public Power RandomAvailablePower() //Used for initialising the game, as well as when the game takes powers away from the player.
    {
        System.Random r = new System.Random();  
        bool returned = false;
        Power returnValue;
        returnValue = powerHandler[19];
        while (returned == false)
        {
            int powerNumber = r.Next(0, 20); //Within the 20 values.
            if (powerHandler[powerNumber].GetPowerActive() == true)
            {
                returnValue = powerHandler[powerNumber];
                returned = true;
            }
        }
        return returnValue;

    }
    public Power RandomPower()
    {
        System.Random r = new System.Random();
        bool returned = false;
        Power returnValue;
        returnValue = powerHandler[19];
        while (returned == false)
        {
            int powerNumber = r.Next(0, 20); //Within the 20 values.
            if (powerHandler[powerNumber].GetPowerAvailable())
            {
                returnValue = powerHandler[powerNumber];
                returned = true;
            }
        }
        return returnValue;

    }
    public Power getSpecificPower(int powerID)
    {
        return powerHandler[powerID];
    }
    void resetPowers() //Code to reset powers when the game ends
    {
        //Failsafe incase a player somehow manages to trigger EndGame instead of EndGameTutorial within the tutorial.
        if (powerHandler[0].GetPowerAvailable())
        {
            powerHandler[0].SetPowerAvailable(false);
        }
        for (int i = 0; i < powerHandler.Length; i++)
        {
            powerHandler[i].SetPowerActive(false);
        }
        powersDrainedCount = 0; //Reset number of powers drained
    }

    //Called by Buttons
    public void LoseSpecificPower(int powerID)
    {
        //Set GetPowerActive() to false.
        powerHandler[powerID].SetPowerActive(false);
        powersDrainedCount = powersDrainedCount + 1;
    }


    //In hard and higher, the game will choose a power to disable immediately for you.
    public void LosePowerHard() //Returns which power was drained for use in UI.
    {
        bool powerDrained = false;
        int drainedPowerID = 255;
        int cycledAttempts = 0;
        System.Random r = new System.Random();
        while (powerDrained == false)
        {
            if (cycledAttempts > 1000) //The chance of not finding 1 in 10 powers being available after 1,000 attempts is extremely low, so this will never occur unless all powers are drained!
            {
                redrawCurrentPowers.LostPowerMessage(drainedPowerID);
            }
            int powerNumber = r.Next(0, 19);
            if (powerHandler[powerNumber].GetPowerActive() == true)
            {
                powerHandler[powerNumber].SetPowerActive(false); //Disable the power
                powerDrained = true;
                drainedPowerID = powerNumber;
            }
            cycledAttempts = cycledAttempts + 1;
        }
        redrawCurrentPowers.LostPowerMessage(drainedPowerID);
        powersDrainedCount = powersDrainedCount + 1;
    }

    public void LoseAllPowers() //Temporary method. Used for first build.
    {
        for (int i = 0; i < powerHandler.Length; i++)
        {
            powerHandler[i].SetPowerActive(false);
        }
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        player.PowerLost();
        redrawCurrentPowers.ShowAllPowersInGame();
        
    }
    public int GetPowersDrainedCount()
    {
        return powersDrainedCount;
    }
    //=======================================================================Initialise each power====================================================================
    void InitialisePowers() //Fill each value of the powers in code here manually.
    {
        //Power slot 1
        Power slot1 = new Power();
        slot1.SetID(0);
        slot1.SetPowerName("Immune to death.");
        slot1.SetPowerDescription("You are unable to die!");
        slot1.SetPowerAvailable(false); //ONLY ENABLED WITH THE TUTORIAL, OTHERWISE THIS MUST BE LEFT FALSE.
        slot1.SetPowerActive(false);
        slot1.SetPowerStartedActive(false);
        slot1.SetPowerStrength(0);
        //Power slot 2
        Power slot2 = new Power();
        slot2.SetID(1);
        slot2.SetPowerName("Will of the Gods");
        slot2.SetPowerDescription("All damage dealt is doubled against enemies.");
        slot2.SetPowerAvailable(true);
        slot2.SetPowerActive(false);
        slot2.SetPowerStartedActive(false);
        slot2.SetPowerStrength(3);

        //Power slot 3
        Power slot3 = new Power();
        slot3.SetID(2);
        slot3.SetPowerName("Not quite immortality");
        slot3.SetPowerDescription("Your health is doubled.");
        slot3.SetPowerAvailable(true);
        slot3.SetPowerActive(false);
        slot3.SetPowerStartedActive(false);
        slot3.SetPowerStrength(3);

        //Power slot 4
        Power slot4 = new Power();
        slot4.SetID(3);
        slot4.SetPowerName("I don't need healing, i'll do it myself");
        slot4.SetPowerDescription("Your health regenerates at 2.5% per second.");
        slot4.SetPowerAvailable(true);
        slot4.SetPowerActive(false);
        slot4.SetPowerStartedActive(false);
        slot4.SetPowerStrength(3);

        //Power slot 5
        Power slot5 = new Power();
        slot5.SetID(4);
        slot5.SetPowerName("Playing with fire");
        slot5.SetPowerDescription("As you lose health, you will deal an increasing amount of damage proportional to health lost.");
        slot5.SetPowerAvailable(true);
        slot5.SetPowerActive(false);
        slot5.SetPowerStartedActive(false);
        slot5.SetPowerStrength(3);

        //Power slot 6
        Power slot6 = new Power();
        slot6.SetID(5);
        slot6.SetPowerName("Imbued with fire");
        slot6.SetPowerDescription("You will not be damaged by fire.");
        slot6.SetPowerAvailable(true);
        slot6.SetPowerActive(false);
        slot6.SetPowerStartedActive(false);
        slot6.SetPowerStrength(2);

        //Power slot 7
        Power slot7 = new Power();
        slot7.SetID(6);
        slot7.SetPowerName("Wizard's Words");
        slot7.SetPowerDescription("Magical weapons attack faster.");
        slot7.SetPowerAvailable(true);
        slot7.SetPowerActive(false);
        slot7.SetPowerStartedActive(false);
        slot7.SetPowerStrength(2);

        //Power slot 8
        Power slot8 = new Power();
        slot8.SetID(7);
        slot8.SetPowerName("Inflation");
        slot8.SetPowerDescription("A curse has been placed on enemies. They are now 50% bigger, making them easier to kill. This does not work on certain enemies.");
        slot8.SetPowerAvailable(true);
        slot8.SetPowerActive(false);
        slot8.SetPowerStartedActive(false);
        slot8.SetPowerStrength(1);

        //Power slot 9
        Power slot9 = new Power();
        slot9.SetID(8);
        slot9.SetPowerName("Warrior Spirit");
        slot9.SetPowerDescription("Swords deal more damage and penetrate armour better.");
        slot9.SetPowerAvailable(true);
        slot9.SetPowerActive(false);
        slot9.SetPowerStartedActive(false);
        slot9.SetPowerStrength(2);

        //Power slot 10
        Power slot10 = new Power();
        slot10.SetID(9);
        slot10.SetPowerName("Iron Maiden");
        slot10.SetPowerDescription("You take 50% less damage.");
        slot10.SetPowerAvailable(true);
        slot10.SetPowerActive(false);
        slot10.SetPowerStartedActive(false);
        slot10.SetPowerStrength(3);

        //Power slot 11
        Power slot11 = new Power();
        slot11.SetID(10);
        slot11.SetPowerName("Posiedon's Blessing");
        slot11.SetPowerDescription("A certain weapon can now extinguish fires temporarily.");
        slot11.SetPowerAvailable(true);
        slot11.SetPowerActive(false);
        slot11.SetPowerStartedActive(false);
        slot11.SetPowerStrength(2);

        //Power slot 12
        Power slot12 = new Power();
        slot12.SetID(11);
        slot12.SetPowerName("???");
        slot12.SetPowerDescription("???");
        slot12.SetPowerAvailable(true);
        slot12.SetPowerActive(false);
        slot12.SetPowerStartedActive(false);
        slot12.PowerStrength = 1;

        //Power slot 13
        Power slot13 = new Power();
        slot13.SetID(12);
        slot13.SetPowerName("Rain Dance");
        slot13.SetPowerDescription("You have a small chance of all fires being temporarily extinguished for 45 seconds.");
        slot13.SetPowerAvailable(true);
        slot13.SetPowerActive(false);
        slot13.SetPowerStartedActive(false);
        slot13.SetPowerStrength(2);

        //Power slot 14
        Power slot14 = new Power();
        slot14.SetID(13);
        slot14.SetPowerName("Justice rains from above");
        slot14.SetPowerDescription("Occasionally, projectiles will fall from the sky and damage all of your enemies a medium amount of damage.");
        slot14.SetPowerAvailable(true);
        slot14.SetPowerActive(false);
        slot14.SetPowerStartedActive(false);
        slot14.SetPowerStrength(2);

        //Power slot 15
        Power slot15 = new Power();
        slot15.SetID(14);
        slot15.SetPowerName("null");
        slot15.SetPowerDescription("null");
        slot15.SetPowerAvailable(false);
        slot15.SetPowerActive(false);
        slot15.SetPowerStartedActive(false);

        //Power slot 16
        Power slot16 = new Power();
        slot16.SetID(15);
        slot16.SetPowerName("null");
        slot16.SetPowerDescription("null");
        slot16.SetPowerAvailable(false);
        slot16.SetPowerActive(false);
        slot16.SetPowerStartedActive(false);

        //Power slot 17
        Power slot17 = new Power();
        slot17.SetID(16);
        slot17.SetPowerName("null");
        slot17.SetPowerDescription("null");
        slot17.SetPowerAvailable(false);
        slot17.SetPowerActive(false);
        slot17.SetPowerStartedActive(false);

        //Power slot 18
        Power slot18 = new Power();
        slot18.SetID(17);
        slot18.SetPowerName("null");
        slot18.SetPowerDescription("null");
        slot18.SetPowerAvailable(false);
        slot18.SetPowerActive(false);
        slot18.SetPowerStartedActive(false);

        //Power slot 19
        Power slot19 = new Power();
        slot19.SetID(18);
        slot19.SetPowerName("null");
        slot19.SetPowerDescription("null");
        slot19.SetPowerAvailable(false);
        slot19.SetPowerActive(false);
        slot19.SetPowerStartedActive(false);

        //Power slot 20
        Power slot20 = new Power();
        slot20.SetID(19);
        slot20.SetPowerName("null");
        slot20.SetPowerDescription("null");
        slot20.SetPowerAvailable(false);
        slot20.SetPowerActive(false);
        slot20.SetPowerStartedActive(false);

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
        int ID { get; set; } //ID of the power between 1 and 20
        string PowerName { get; set; } //Name of the power
        string PowerDescription { get; set; } //Description of what this power does
        bool PowerActive { get; set; } //Whether the power is active or not. This will start as default of all false.
        bool PowerStartedActive { get; set; } //This is a check to see if the boolean was active at the start of the game. Whenever a power is drained, it is a check to ensure the player has the correct stats.
        bool PowerAvailable { get; set; } //Modifier to show if this power is actually ready to be used. E.G tutorial will be false outside of tutorial. 
                                                 //This also applies to any empty slots with default/null values
        public int PowerStrength { get; set; } //0 - N/A,  1 - Weak, 2 - Medium, 3 - Powerful.

        //Getters
        public int GetID()
        {
            return ID;
        }
        public string GetPowerName()
        {
            return PowerName;
        }
        public string GetPowerDescription()
        {
            return PowerDescription;
        }
        public bool GetPowerActive()
        {
            return PowerActive;
        }
        public bool GetPowerStartedActive()
        {
            return PowerStartedActive;
        }
        public bool GetPowerAvailable()
        {
            return PowerAvailable;
        }
        public int GetPowerStrength()
        {
            return PowerStrength;
        }
        //Setters

        public void SetID(int value)
        {
            ID = value;
        }
        public void SetPowerName(string value)
        {
            PowerName = value;
        }
        public void SetPowerDescription(string value)
        {
            PowerDescription = value;
        }
        public void SetPowerActive(bool value)
        {
            PowerActive = value;
        }
        public void SetPowerStartedActive(bool value)
        {
            PowerStartedActive = value;
        }
        public void SetPowerAvailable(bool value)
        {
            PowerAvailable = value;
        }
        public void SetPowerStrength(int value)
        {
            PowerStrength = value;
        }
    }
}
