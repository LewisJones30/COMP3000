using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapons : MonoBehaviour
{
    double cooldown = 1f;
    UIController isPausedCheck;
    // Start is called before the first frame update
    void Start()
    {
        isPausedCheck = GameObject.Find("UIHandler").GetComponent<UIController>(); //Get component for checking for pausing
    }

    // Update is called once per frame
    void Update()
    {
        if (isPausedCheck.isPaused == false)
        {
            cooldown = cooldown - Time.deltaTime;
            if (cooldown <= 0)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    //Melee Attack
                }
            }
        }
        else
        {

        }
    }
}
