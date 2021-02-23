using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject.Asteroids;

public class MagicWeapon : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Default time between shots.\nNote that some difficulties and powers will change this.")]
    double shootingCooldown = 3f;
    UIController isPauseCheck; //Check if paused.
    [SerializeField]
    GameObject projectile;
    private double dupeCD; //Original cooldown, so shooting cooldown can return to that value.
    [SerializeField]
    GameObject UITempWeaponText;
    Text text;
    [SerializeField]
    GameObject Powerhandler;
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
        if (PowerBehaviour.powerHandler[6].PowerActive == true)
        {
            //Reduce the cooldown of shooting by 50% with power 6.
            shootingCooldown = shootingCooldown / 2f;
            dupeCD = shootingCooldown;
            powerModified = true;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (isPauseCheck.isPaused == false) //Game is active.
        {
            shootingCooldown = shootingCooldown - Time.deltaTime;

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
                        GameObject projectileShot;
                        projectileShot = Instantiate(projectile, transform.position, transform.rotation);
                        projectileShot.transform.position = new Vector3(projectileShot.transform.position.x, projectileShot.transform.position.y, projectileShot.transform.position.z);
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
                    text.text = "Weapon recharging...";
                }

            }
        }
        else //Game paused.
        {
            
        }
    }
    void checkPower()
    {
        if (PowerBehaviour.powerHandler[6].PowerAvailable == false && powerModified == true)
        {
            //Reset the effect.
            dupeCD = dupeCD * 1.5f;
            shootingCooldown = dupeCD;
            powerModified = false; //Power is no longer in effect!
        }
    }
}
