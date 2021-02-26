using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBoss : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    float currentHP;
    [SerializeField]
    readonly float MAX_HP = 2000; //Maximum health of the golem
    [SerializeField]
    float DEFAULT_ATTACK_SPEED = 5f; //Frequency the boss uses an attack.
    [SerializeField]
    [Tooltip("This is displayed for debug purposes.")]
    float attackSpeedTimer;
    [SerializeField]
    GameObject projectile1;
    [SerializeField]
    GameObject projectile2;
    [SerializeField]
    GameObject projectile3;
    UIController ui;
    [SerializeField]
    GameObject projectileSpawner;
    GameObject playerLook;
    Rigidbody i;
    int CURRENT_ATTACK_CYCLE;
    // Update is called once per frame
    void Update()
    {
        transform.LookAt(playerLook.transform);
        projectileSpawner.transform.LookAt(playerLook.transform);
        if (ui.isPaused == true)
        {
            return;
        }
        attackSpeedTimer = attackSpeedTimer - Time.deltaTime;
        if (attackSpeedTimer <= 0)
        {
            CURRENT_ATTACK_CYCLE = CURRENT_ATTACK_CYCLE + 1; //Iterate the cycle
            //Planned attack cycle:
            //Projectile -> Projectile -> Projectile
            //50/50 - Spawn a minion or charge at the player
            //Projectile -> Projectile -> Projectile
            //Charge at the player
            //Projectile x5
            //Spawn a mega minion
            //Projectile x3
            //Cycle from beginning
            switch(CURRENT_ATTACK_CYCLE)
            {
                case 1:
                    {
                        Instantiate(projectile1, transform.Find("RockSpawner").position, transform.rotation);

                        attackSpeedTimer = DEFAULT_ATTACK_SPEED;
                        return;
                    }
                case 2:
                    {
                        Instantiate(projectile1, transform.Find("RockSpawner").position, transform.rotation);
                        attackSpeedTimer = DEFAULT_ATTACK_SPEED;
                        return;
                    }
                case 3:
                    {
                        Instantiate(projectile1, transform.Find("RockSpawner").position, transform.rotation);
                        attackSpeedTimer = DEFAULT_ATTACK_SPEED;
                        return;
                    }
                case 4:
                    {
                        //Spawn minion or charge at player
                        attackSpeedTimer = DEFAULT_ATTACK_SPEED;
                        return;
                    }
                case 5:
                    {
                        Instantiate(projectile1, transform.Find("RockSpawner").position, transform.rotation);
                        attackSpeedTimer = DEFAULT_ATTACK_SPEED;
                        return;
                    }
                case 6:
                    {
                        Instantiate(projectile1, transform.Find("RockSpawner").position, transform.rotation);
                        attackSpeedTimer = DEFAULT_ATTACK_SPEED;
                        return;
                    }
                case 7:
                    {
                        Instantiate(projectile1, transform.Find("RockSpawner").position, transform.rotation);
                        attackSpeedTimer = DEFAULT_ATTACK_SPEED;
                        return;
                    }
                case 8:
                    {
                        //Charge at the player
                        attackSpeedTimer = DEFAULT_ATTACK_SPEED;
                        return;
                    }
                case 9:
                    {
                        Instantiate(projectile1, transform.Find("RockSpawner").position, transform.rotation);
                        attackSpeedTimer = DEFAULT_ATTACK_SPEED;
                        return;
                    }
                case 10:
                    {
                        Instantiate(projectile1, transform.Find("RockSpawner").position, transform.rotation);
                        attackSpeedTimer = DEFAULT_ATTACK_SPEED;
                        return;
                    }
                case 11:
                    {
                        Instantiate(projectile1, transform.Find("RockSpawner").position, transform.rotation);
                        attackSpeedTimer = DEFAULT_ATTACK_SPEED;
                        return;
                    }
                case 12:
                    {
                        Instantiate(projectile1, transform.Find("RockSpawner").position, transform.rotation);
                        attackSpeedTimer = DEFAULT_ATTACK_SPEED;
                        return;
                    }
                case 13:
                    {
                        Instantiate(projectile1, transform.Find("RockSpawner").position, transform.rotation);
                        attackSpeedTimer = DEFAULT_ATTACK_SPEED;
                        return;
                    }
                case 14:
                    {
                        //Spawn a mega minion
                        attackSpeedTimer = DEFAULT_ATTACK_SPEED;
                        return;
                    }
                case 15:
                    {
                        Instantiate(projectile1, transform.Find("RockSpawner").position, transform.rotation);
                        attackSpeedTimer = DEFAULT_ATTACK_SPEED;
                        return;
                    }
                case 16:
                    {
                        Instantiate(projectile1, transform.Find("RockSpawner").position, transform.rotation);
                        attackSpeedTimer = DEFAULT_ATTACK_SPEED;
                        return;
                    }
                case 17:
                    {
                        Instantiate(projectile1, transform.Find("RockSpawner").position, transform.rotation);
                        attackSpeedTimer = DEFAULT_ATTACK_SPEED;
                        return;
                    }
            }

        }

    }
    void Start()
    {
        attackSpeedTimer = DEFAULT_ATTACK_SPEED;
        currentHP = MAX_HP;
        ui = GameObject.Find("UIHandler").GetComponent<UIController>();
        playerLook = GameObject.Find("playerLook");
        i = GetComponent<Rigidbody>();
    }

    void ChargeAtPlayer()
    {
        i.velocity = transform.forward * 40; //Charge at player for 2-3 seconds.
        //Play walking animation.
        //If the player takes damage in the 2 seconds, stop charging immediately, golem will stop attacking for an extra 10 seconds
        //Damage the player for 45 damage, stun them for 5 seconds too.
    }

    float healthRemainingPercentage() //Used for the UI health bar, when the boss is active.
    {
        float returnValue;
        returnValue = currentHP / MAX_HP;
        return returnValue;
    }
    public float getHPRemaining() //Public call to the healthRemaining
    {
        return healthRemainingPercentage();
    }
    void TakeDamage(float damageDealt)
    {
        currentHP = currentHP - damageDealt;
        StartCoroutine("DamageFlash");
        if (currentHP <= 0)
        {
            //death sequence
            Destroy(this.gameObject);
        }
    }
    public void DealDamage(float damageToDeal) //External call to deal damage to final boss
    {
        TakeDamage(damageToDeal);
    }
    IEnumerator DamageFlash()
    {
        this.gameObject.GetComponentInChildren<Renderer>().material.color = Color.red;
        //transform.Find("ghoul").gameObject.GetComponent<Renderer>().material.color = Color.red;
        yield return new WaitForSeconds(0.4f);
        this.gameObject.GetComponentInChildren<Renderer>().material.color = Color.white;
    }
}
