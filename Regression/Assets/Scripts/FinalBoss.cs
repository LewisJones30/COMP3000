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
    bool lookAtPlayer = true;
    bool attackTimerPause = false;
    bool golemCharging = false;
    [SerializeField]
    GameObject megaMinion;
    [SerializeField]
    GameObject normalMinion;
    RaycastHit hit;
    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Debug.DrawRay(transform.position + (Vector3.up * 3), forward * 5, Color.white);
        if (Physics.SphereCast(transform.position + (Vector3.up * 3), 1.5f, forward, out hit, 10))
        {
            Debug.Log("GOLEM HIT: " + hit.collider.gameObject.name);
            if (hit.collider.gameObject.name == "Player")
            {
                if (golemCharging == true)
                {
                    //Damage
                    Player player = hit.collider.gameObject.GetComponent<Player>();
                    MovementScript stunned = player.GetComponent<MovementScript>();
                    stunned.StunPlayer(3f);
                    Invoke("DamagePlayerCharge", 3f);
                    i.velocity = Vector3.zero;
                    i.angularVelocity = Vector3.zero;
                    golemCharging = false;
                }
            }

        }
        if (lookAtPlayer == true)
        {
            transform.LookAt(playerLook.transform);
        }
        if (ui.isPaused == true)
        {
            return;
        }
        if (attackTimerPause == true)
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
                        //Instantiate(projectile1, transform.Find("RockSpawner").position, transform.rotation);
                        ChargeAtPlayer();
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
                        var rand = new System.Random();
                        int i = rand.Next(0, 1);
                        if (i == 0)
                        {
                            ChargeAtPlayer();
                        }
                        else
                        {
                            MinionSpawn();
                        }
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
                        ChargeAtPlayer();
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
                        MinionSpawn();
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

    void MinionSpawn()
    {
        //Spawn TWO minions. One is guaranteed to be a mega minion, the other is 50/50 regular or mega.
        var rand1 = new System.Random();
        GameObject spawner1 = GameObject.Find("FBSpawn1");
        GameObject spawner2 = GameObject.Find("FBSpawn2");
        int j = rand1.Next(0, 1);
        if (j == 0)
        {

            Instantiate(megaMinion, spawner2.transform.position, spawner2.transform.rotation);
            Instantiate(megaMinion, spawner1.transform.position, spawner2.transform.rotation);
        }
        else
        {
            Instantiate(megaMinion, spawner2.transform.position, spawner2.transform.rotation);
            Instantiate(normalMinion, spawner1.transform.position, spawner2.transform.rotation);
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
        StartCoroutine("GolemCharge");
        //Charge at player for 2-3 seconds.
        //Play walking animation.
        //If the player takes damage in the 2 seconds, stop charging immediately, golem will stop attacking for an extra 10 seconds
        //Damage the player for 45 damage, stun them for 5 seconds too.
    }
    IEnumerator GolemCharge()
    {

        //THE GOLEM IS ABOUT TO CHARGE AT YOU!
        //looks for 3 seconds, then stops, charges after another 3.
        Debug.Log("The golem is preparing to charge!");
        attackTimerPause = true;
        yield return new WaitForSecondsRealtime(3);
        lookAtPlayer = false;
        yield return new WaitForSecondsRealtime(1);
        golemCharging = true;
        i.velocity = transform.forward * 50;
        yield return new WaitForSecondsRealtime(4.5f);
        golemCharging = false;
        i.velocity = Vector3.zero;
        i.angularVelocity = Vector3.zero;
        lookAtPlayer = true;
        attackTimerPause = false;
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player")
        {
            GameObject.Find("Player").GetComponent<Player>().takeDamage(50f);
        }
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

    void DamagePlayerCharge()
   
    {
        Player player = hit.collider.gameObject.GetComponent<Player>();
        player.takeDamage(50f);
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
