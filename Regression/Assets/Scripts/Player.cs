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

    // Start is called before the first frame update
    void Start()
    {
        powerController = GameObject.Find("PowerHandler").GetComponent<PowerBehaviour>(); //Get the PowerBehaviour script.
        isPausedCheck = GameObject.Find("UIHandler").GetComponent<UIController>();
        ModifyPlayer();
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
                heal(maximumHealth / 40);
                time = 0;
            }
        }

        

    }

    //Damage/healing effects

    void takeDamage(double damageDealt)
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
    void ModifyPlayer() //This script will check all current modifiers, and update the player's abilities accordingly.
    {
        //Check difficulty - Easy will boost player's HP by 100.
        if (powerController.difficultyLevel == 1) //Easy
        {
            health = health * 1.5f;
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
            weaponPower = weaponPower * 2f; //Weapons are twice as effective in terms of damage dealt.
        }
        else if (powerController.powerHandler[2].PowerActive == true)
        {
            maximumHealth = maximumHealth * 2f; //Health is doubled with this modifier
            health = maximumHealth;
        }
        else if (powerController.powerHandler[9].PowerActive == true)
        {
            damageTaken = 0.5f; //Damage taken is 50% lower.

        }
        baseWeaponPower = weaponPower; //Used for power 5 method.
    }
    //Power 4 method
    void regenHp()
    {
        if (powerController.powerHandler[3].PowerActive == true)
        {
            heal(maximumHealth / 40); //Run heal script for 2.5% of health.
        }
    }
    //Power 5 method
    void DharoksEffect() //Lose health -> gain more power
    {
        weaponPower = baseWeaponPower;
        double percentageHPRemaining = (health / maximumHealth) * 100f;
        double powerIncrease = (100 - percentageHPRemaining) / 100; //At 10% hp, this returns 0.90.
        weaponPower = weaponPower * (1 + powerIncrease); //Maximum of 1.99x at 1% health.
    }
}
