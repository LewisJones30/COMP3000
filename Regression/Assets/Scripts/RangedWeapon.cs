using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeapon : MonoBehaviour
{
    double shootingCooldown = 1.5f;
    UIController isPauseCheck; //Check if paused.
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
                }
            }
        }
        else //Game paused.
        {

        }
    }
}
