using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public double health = 100; //current health is controlled here. Public to allow UI access.
    [SerializeField]
    double maximumHealth = 100; //Base maximum hp.
    bool isDead = false; //Player starts alive.
    public double weaponPower = 1.0f; //Base weapon power, modified by difficulty.
    double baseWeaponPower = 1.0f; //Used for power 5.
    bool tutorialActive = false; //Means player cannot die.
    float damageTaken = 1.0f; //Player takes 1x damage. Modified by difficulty.
    float pointsModifier = 1.0f; //Points gained.
    UIController isPausedCheck;
    double time = 0;
    PowerBehaviour powerController;
    bool[] powersLost = new bool[20];

    // Start is called before the first frame update
    void Start()
    {
        powerController = GameObject.Find("PowerHandler").GetComponent<PowerBehaviour>(); //Get the PowerBehaviour script.
        isPausedCheck = GameObject.Find("UIHandler").GetComponent<UIController>();
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





//========================================================Power control
    public void ModifyPlayer() //This script will check all current modifiers, and update the player's abilities accordingly.
    {
        powerController = GameObject.Find("PowerHandler").GetComponent<PowerBehaviour>(); //Get the PowerBehaviour script.
        Debug.Log("ModifyPlayer triggered.");
        //Check difficulty - Easy will boost player's HP by 100.
        if (powerController.difficultyLevel == 1) //Easy
        {
            maximumHealth = maximumHealth * 1.5f;
            damageTaken = 0.5f;
            pointsModifier = 0.25f; 
        }
        else if (powerController.difficultyLevel == 3) //Hard
        {
            weaponPower = 0.9f; //Weapons are 90% effective
            pointsModifier = 1.5f;//+50% points.
        }
        else if (powerController.difficultyLevel == 4) //Expert
        {
            weaponPower = 0.75f; //Weapons are 75% effective
            pointsModifier = 2f; //+100% points.
        }
        else if (powerController.difficultyLevel == 5) //Satanic
        {
            weaponPower = 0.5f; //Weapons are 50% effective.
            pointsModifier = 3.5f; //+250% points.
        }
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
