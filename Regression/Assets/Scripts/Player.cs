using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    //Easy difficulty controls
    [HideInInspector]
    public double health = 100; //current health is controlled here. Public to allow UI access.
    [SerializeField]
    double maximumHealth = 100; //Base maximum hp.
    [Header("Easy")]
    [SerializeField]
    [Range(0.1f, 2f)]
    double EasyMaxHealthModifier = 1f;
    [SerializeField]
    [Range(0.1f, 2f)]
    double EasyWeaponPowerModifier = 1f;
    [SerializeField]
    [Range(0.1f, 2f)]
    double EasyDamageTakenModifier = 1f;
    [SerializeField]
    [Range(0.1f, 5f)]
    double EasyPointsModifier = 0.25f;
    [SerializeField]
    [Tooltip("Should the player choose between two powers to lose? \n" +
        "True = Yes. \n" +
        "False = Game chooses for the player.")]
    bool EasyPlayerChoosesPowerDrain = true;

    //Normal difficulty controls
    [Header("Normal")]
    [SerializeField]
    [Range(0.1f, 2f)]
    double NormalMaxHealthModifier = 1f;
    [SerializeField]
    [Range(0.1f, 2f)]
    double NormalWeaponPowerModifier = 1f;
    [SerializeField]
    [Range(0.1f, 2f)]
    double NormalDamageTakenModifier = 1f;
    [SerializeField]
    [Range(0.1f, 5f)]
    double NormalPointsModifier = 1f;
    [SerializeField]
    [Tooltip("Should the player choose between two powers to lose? \n" +
        "True = Yes. \n" +
        "False = Game chooses for the player.")]
    bool NormalPlayerChoosesPowerDrain = true;

    //Hard difficulty controls
    [Header("Hard")]
    [SerializeField]
    [Range(0.1f, 2f)]
    double HardMaxHealthModifier = 1f;
    [SerializeField]
    [Range(0.1f, 2f)]
    double HardWeaponPowerModifier = 1f;
    [SerializeField]
    [Range(0.1f, 2f)]
    double HardDamageTakenModifier = 1f;
    [SerializeField]
    [Range(0.1f, 5f)]
    double HardPointsModifier = 1f;
    [SerializeField]
    [Tooltip("Should the player choose between two powers to lose? \n" +
        "True = Yes. \n" +
        "False = Game chooses for the player.")]
    bool HardPlayerChoosesDrain = false;

    //Expert difficulty controls
    [Header("Expert")]
    [SerializeField]
    [Range(0.1f, 2f)]
    double ExpertMaxHealthModifier = 1f;
    [SerializeField]
    [Range(0.1f, 2f)]
    double ExpertWeaponPowerModifier = 1f;
    [SerializeField]
    [Range(0.1f, 2f)]
    double ExpertDamageTakenModifier = 1f;
    [SerializeField]
    [Range(0.1f, 5f)]
    double ExpertPointsModifier = 1f;
    [SerializeField]
    [Tooltip("Should the player choose between two powers to lose? \n" +
        "True = Yes. \n" +
        "False = Game chooses for the player.")]
    bool ExpertPlayerChoosesDrain = false;
    [Space(10)]

    //Public variables
    public GameObject deadtextObj;
    [HideInInspector]
    public double weaponPower = 1.0f; //Base weapon power, modified by difficulty.
    [HideInInspector]
    public double baseWeaponPower = 1.0f; //Used for power 5.
    [HideInInspector]
    public double damageTaken = 1.0f; //Player takes 1x damage. Modified by difficulty.
    [HideInInspector]
    public double pointsModifier = 1.0f; //Points gained.
    [HideInInspector]
    public double time = 0;


    bool tutorialActive = false; //Means player cannot die.
    bool isDead = false; //Player starts alive.
    bool[] powersLost = new bool[20];

    UIController isPausedCheck;
    PowerBehaviour powerController;
    Text deadtext;
    // Start is called before the first frame update
    void Start()
    {
        powerController = GameObject.Find("PowerHandler").GetComponent<PowerBehaviour>(); //Get the PowerBehaviour script.
        isPausedCheck = GameObject.Find("UIHandler").GetComponent<UIController>();
        deadtext = deadtextObj.GetComponent<Text>();
        deadtext.enabled = false;
        SpawnSpecificWeapon(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (isPausedCheck.isPaused == true)
        {
            //Code to pause the entire game.
        }    
        else if (powerController.powerHandler[3].PowerActive == true) //Starts time from when the player recieves the power.
        {
            time = time + Time.deltaTime;
            if (time >= 1)
            {
                heal(maximumHealth / 100);
                time = 0;
            }
        }
        if (isDead == true)
        {

        }
    }

    //Damage/healing effects
    public void takeDamage(double damageDealt)
    {
        health = health - (damageTaken * damageDealt); //Take away health based on the damage dealt. Iron maiden ability reduces this by 50%. Difficulties may also change this.
        if (health < 1)
        {
            if (tutorialActive == true) //Check if player is in tutorial
            {
                health = 1; //Health cannot go below 1 in tutorial.
            }
            else
            {
                isDead = true; //Set player as dead.
                health = 0;
                playerdiedScript(); //Invoke the player died script.
            }

        }
        else
        {
            if (powerController.powerHandler[4].PowerActive == true)
            {
                DharoksEffect(); //Recalculate the player's damage.
            }
        }
    }

    void heal(double healAmount)
    {
        if ((health + healAmount) > maximumHealth)
        {
            health = maximumHealth; //Ensure that the player does not go above maximum hp.

        }
        else
        {
            health = health + healAmount;
        }
    }
    void playerdiedScript()
    {
        StartCoroutine("routineDead");
    }
    IEnumerator routineDead()
    {
        deadtext.enabled = true;
        isPausedCheck.isPaused = true;
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("UI Scale Testing");
    }


    //Weapon instantiation
    public void SpawnSpecificWeapon(int weaponID)
    {
        switch (weaponID)
        {
            case 0:
                GameObject weapon;
                weapon = (GameObject)Instantiate(Resources.Load("Mace1"), transform.position, Quaternion.Euler(1.219f, 150f, 97.661f), this.gameObject.transform);
                weapon.transform.localPosition = (new Vector3(-0.026f, -0.094f, 0.146f));
                break;
        }
    }




//========================================================Power control
    public void ModifyPlayer() //This script will check all current modifiers, and update the player's abilities accordingly.
    {
        powerController = GameObject.Find("PowerHandler").GetComponent<PowerBehaviour>(); //Get the PowerBehaviour script.
        Debug.Log("ModifyPlayer triggered.");
        //Check difficulty - Easy will boost player's HP by 100.
        if (powerController.difficultyLevel == 1) //Easy
        {
            maximumHealth = maximumHealth * EasyMaxHealthModifier;
            weaponPower = weaponPower * EasyWeaponPowerModifier;
            damageTaken = damageTaken * EasyDamageTakenModifier;
            pointsModifier = pointsModifier * EasyPointsModifier; 
        }
        else if (powerController.difficultyLevel == 2) //Normal
        {
            maximumHealth = maximumHealth * NormalMaxHealthModifier;
            weaponPower = weaponPower * NormalWeaponPowerModifier;
            damageTaken = damageTaken * NormalDamageTakenModifier;
            pointsModifier = pointsModifier * NormalPointsModifier;
        }
        else if (powerController.difficultyLevel == 3)
        {
            maximumHealth = maximumHealth * HardMaxHealthModifier;
            weaponPower = weaponPower * HardWeaponPowerModifier;
            damageTaken = damageTaken * HardDamageTakenModifier;
            pointsModifier = pointsModifier * HardPointsModifier;
        }
        else if (powerController.difficultyLevel == 4) //Expert
        {
            maximumHealth = maximumHealth * ExpertMaxHealthModifier;
            weaponPower = weaponPower * ExpertWeaponPowerModifier;
            damageTaken = damageTaken * ExpertDamageTakenModifier;
            pointsModifier = pointsModifier * ExpertPointsModifier;
        }
        //else if (powerController.difficultyLevel == 5) //Satanic
        //{
        //    weaponPower = 0.5f; //Weapons are 50% effective.
        //    pointsModifier = 3.5f; //+250% points.
        //}
        //Check powers.
        //Slot 2 & 3 effect the player immediately.
        //Increase player's HP by 2x.
        if (powerController.powerHandler[1].PowerActive == true)
        {
            Debug.Log("Power 1 in effect!");
            weaponPower = weaponPower * 2f; //Weapons are twice as effective in terms of damage dealt.
            if (weaponPower > 2)
            {
                weaponPower = 2; 
            }    
        }
        //Power 3 check. Double maximum HP.
        if (powerController.powerHandler[2].PowerActive == true)
        {
            Debug.Log("Power 2 in effect!");
            maximumHealth = maximumHealth * 2f; //Health is doubled with this modifier
            health = maximumHealth;
        }
        //Power 10 check
        if (powerController.powerHandler[9].PowerActive == true)
        {
            damageTaken = damageTaken * 0.5f; //Damage taken is 50% lower.

        }
        baseWeaponPower = weaponPower; //Used for power 5 method.
    }
    public void PowerLost() //Called when the power has been lost.
    {
        /*
         * This method checks whether a power was active, is now not active and the power effect has not been removed.
         * powersLost[1] is a boolean array equivalent to the length of the powers array.
         * When a power effect has been drained, this is set to true. This means that the power effect is not taken multiple times.
         */
        if (powerController.powerHandler[1].PowerActive == false && powerController.powerHandler[1].PowerStartedActive == true)
        {
            if (powersLost[1] == false)
            {
                weaponPower = baseWeaponPower / 2;
                powersLost[1] = true;
            }
        }
        if (powerController.powerHandler[2].PowerActive == false && powerController.powerHandler[2].PowerStartedActive == true)
        {
            if (powersLost[2] == false)
            {
                maximumHealth = maximumHealth / 2;
                health = maximumHealth;
                powersLost[2] = true;
            }
        }
        //Power slot 3 does not need checking here.
        if (powerController.powerHandler[4].PowerActive == false && powerController.powerHandler[4].PowerStartedActive == true)
        {
            if (powersLost[4] == false)
            {
                weaponPower = baseWeaponPower; //Remove the dharoks effect.
                powersLost[4] = true;
            }
        }
        //Power slot 5,6,7,8,9 do not need checking here.
        if (powerController.powerHandler[10].PowerActive == false && powerController.powerHandler[10].PowerStartedActive == true)
        {
            if (powersLost[10] == false)
            {
                damageTaken = damageTaken * 2f;
                powersLost[10] = true;
            }
        }
        //The rest of power slots do not require checking here!
    }








    //Power 4 method
    //Power 5 method
    void DharoksEffect() //Lose health -> gain more power
    {
        weaponPower = baseWeaponPower;
        double percentageHPRemaining = (health / maximumHealth) * 100f;
        double powerIncrease = (100 - percentageHPRemaining) / 100; //At 10% hp, this returns 0.90.
        weaponPower = weaponPower * (1 + powerIncrease); //Maximum of 1.99x at 1% health.
    }
}
