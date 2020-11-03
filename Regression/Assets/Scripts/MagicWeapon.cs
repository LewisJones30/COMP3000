using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject.Asteroids;

public class MagicWeapon : MonoBehaviour
{
    double shootingCooldown = 1.5f;
    UIController isPauseCheck; //Check if paused.
    [SerializeField]
    GameObject projectile;
    // Start is called before the first frame update
    void Start()
    {
        isPauseCheck = GameObject.Find("UIHandler").GetComponent<UIController>();
        //Obtain any powers that modify.

    }

    // Update is called once per frame
    void Update()
    {
        if (isPauseCheck.isPaused == false) //Game is active.
        {
            shootingCooldown = shootingCooldown - Time.deltaTime;
            if (shootingCooldown <= 0)
            {
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
                        //Shoot magic blast projectile.
                    }
                }
            }
        }
        else //Game paused.
        {

        }
    }
}
