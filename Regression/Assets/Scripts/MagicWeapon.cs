﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject.Asteroids;

public class MagicWeapon : MonoBehaviour
{
    //Public variables

    //SerializeField variables
    [SerializeField]
    [Tooltip("Default time between shots.\nNote that some difficulties and powers will change this.")]
    double shootingCooldown = 1f;
    [SerializeField]
    GameObject projectile;
    [SerializeField]
    GameObject Powerhandler;
    [SerializeField]
    GameObject UITempWeaponText;

    //Private variables
    UIController isPauseCheck; //Check if paused.
    private double dupeCD; //Original cooldown, so shooting cooldown can return to that value.
    Text text;
    PowerBehaviour PowerBehaviour;
    bool powerModified = false; //Boolean so that the power is not endlessly applied.
    // Start is called before the first frame update
    void Start()
    {
        dupeCD = shootingCooldown;
        isPauseCheck = GameObject.Find("UIHandler").GetComponent<UIController>();
        //Obtain any powers that modify.
        text = UITempWeaponText.GetComponent<Text>();
        PowerBehaviour = GameObject.Find("PowerHandler").GetComponent<PowerBehaviour>();
        if (PowerBehaviour.powerHandler[6].GetPowerActive() == true)
        {
            //Reduce the cooldown of shooting by 1/3 with power 6.
            shootingCooldown = shootingCooldown / 1.5f;
            dupeCD = shootingCooldown;
            powerModified = true;
        }

    }

    private void FixedUpdate()
    {
        if (PowerBehaviour.powerHandler[6].GetPowerActive()  && !powerModified)
        {
            //Reduce the cooldown of shooting by half with power 6.
            shootingCooldown = shootingCooldown / 2f;
            dupeCD = shootingCooldown;
            powerModified = true;
        }
        if (!isPauseCheck.GetIsPaused()) //Game is active.
        {
            shootingCooldown = shootingCooldown - Time.deltaTime;
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckPower();
        if (!isPauseCheck.GetIsPaused())
        {
            if (shootingCooldown <= 0)
            {
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                {
                    return;
                }
                text.enabled = false;
                if (Input.GetMouseButtonDown(0))
                {
                    //Shoot projectile
                    //Check which weapon this script is bound to, and then shoot the specific projectile.
                    if (this.gameObject.name == "Trident")
                    {
                        //Shoot AOE projectiles
                    }
                    else
                    {
                        StartCoroutine("ShootProjectile");
                        shootingCooldown = dupeCD;
                        //Shoot magic blast projectile.
                    }
                }
            }
            else
            {
                if (GameObject.Find("Mace1Clone"))
                {
                    text.gameObject.SetActive(false);
                }
                else
                {
                    text.enabled = true;
                    text.text = "Weapon Recharging...";

                }

            }
        }
        else //Game paused.
        {
            
        }
    }

    IEnumerator ShootProjectile()
    {
        GameObject.FindGameObjectWithTag("MusicHandler").GetComponent<AudioController>().PlaySoundEffect(6);
        GameObject.FindGameObjectWithTag("StaffOBJ").GetComponent<Animator>().SetBool("isAttacking", true);
        yield return new WaitForSeconds(0.33f);
        GameObject projectileShot;
        projectileShot = Instantiate(projectile, transform.position, transform.rotation);
        projectileShot.transform.position = new Vector3(projectileShot.transform.position.x, projectileShot.transform.position.y, projectileShot.transform.position.z);
        yield return new WaitForSeconds(0.67f);
        GameObject.FindGameObjectWithTag("StaffOBJ").GetComponent<Animator>().SetBool("isAttacking", false);
    }
    void CheckPower()
    {
        if (PowerBehaviour.powerHandler[6].GetPowerActive() == false && PowerBehaviour.powerHandler[6].GetPowerStartedActive() == true && powerModified == true)
        {
            //Reset the effect.
            dupeCD = dupeCD * 2f;
            shootingCooldown = dupeCD;
            powerModified = false; //Power is no longer in effect!
        }
    }
}
