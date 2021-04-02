using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalBoss : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    float currentHP;
    [SerializeField]
    readonly float MAX_HP = 1500; //Maximum health of the golem
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
    [SerializeField]
    GameObject BossHPBar, BossHPBorder, BossName, GolemChargeText; //Variables relating to 
    GameObject UI;
    bool enragedState = false;
    // Update is called once per frame
    Animator animator;





    void FixedUpdate()
    {
        if (UI == null)
        {
            UI = GameObject.Find("UIHandler");
        }
        if (UI != null)
        {
            if (ui.isPaused)
            {
                BossHPBar.SetActive(false);
                BossHPBorder.SetActive(false); 
                BossName.SetActive(false);
            }
            else
            {
                BossHPBar.SetActive(true);
                BossHPBorder.SetActive(true);
                BossName.SetActive(true);
            }
            if (Input.GetKeyDown(KeyCode.F7))
            {
                StartCoroutine("GolemDeath");
            }
        }
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
        healthRemainingPercentage();
        if (attackSpeedTimer <= 0)
        {

            CURRENT_ATTACK_CYCLE = CURRENT_ATTACK_CYCLE + 1; //Iterate the cycle
            //Planned attack cycle:
            //Projectile -> Projectile -> Projectile
            //Spawn minions
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
                        StartCoroutine("GolemThrowProjectile");
                        attackSpeedTimer = DEFAULT_ATTACK_SPEED;
                        return;
                    }
                case 2:
                    {
                        StartCoroutine("GolemThrowProjectile");
                        attackSpeedTimer = DEFAULT_ATTACK_SPEED;
                        return;
                    }
                case 3:
                    {
                        StartCoroutine("GolemThrowProjectile");
                        attackSpeedTimer = DEFAULT_ATTACK_SPEED;
                        return;
                    }
                case 4:
                    {
                        if (enragedState)
                        {
                            StartCoroutine("GolemThrowProjectile");
                        }
                        else
                        {
                            StartCoroutine("GolemRoar");
                            MinionSpawn();
                        }                        
                        attackSpeedTimer = DEFAULT_ATTACK_SPEED;
                        return;
                    }
                case 5:
                    {
                        StartCoroutine("GolemThrowProjectile");
                        attackSpeedTimer = DEFAULT_ATTACK_SPEED;
                        return;
                    }
                case 6:
                    {
                        StartCoroutine("GolemThrowProjectile");
                        attackSpeedTimer = DEFAULT_ATTACK_SPEED;
                        return;
                    }
                case 7:
                    {
                        StartCoroutine("GolemThrowProjectile");
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
                        StartCoroutine("GolemThrowProjectile");
                        attackSpeedTimer = DEFAULT_ATTACK_SPEED;
                        return;
                    }
                case 10:
                    {
                        StartCoroutine("GolemThrowProjectile");
                        attackSpeedTimer = DEFAULT_ATTACK_SPEED;
                        return;
                    }
                case 11:
                    {
                        StartCoroutine("GolemThrowProjectile");
                        attackSpeedTimer = DEFAULT_ATTACK_SPEED;
                        return;
                    }
                case 12:
                    {
                        StartCoroutine("GolemThrowProjectile");
                        attackSpeedTimer = DEFAULT_ATTACK_SPEED;
                        return;
                    }
                case 13:
                    {
                        StartCoroutine("GolemThrowProjectile");
                        attackSpeedTimer = DEFAULT_ATTACK_SPEED;
                        return;
                    }
                case 14:
                    {
                        //Spawn a mega minion
                        MinionSpawn();
                        StartCoroutine("GolemRoar");
                        attackSpeedTimer = DEFAULT_ATTACK_SPEED;
                        return;
                    }
                case 15:
                    {
                        StartCoroutine("GolemThrowProjectile");
                        attackSpeedTimer = DEFAULT_ATTACK_SPEED;
                        return;
                    }
                case 16:
                    {
                        StartCoroutine("GolemThrowProjectile");
                        attackSpeedTimer = DEFAULT_ATTACK_SPEED;
                        return;
                    }
                case 17:
                    {
                        StartCoroutine("GolemThrowProjectile");
                        attackSpeedTimer = DEFAULT_ATTACK_SPEED;
                        CURRENT_ATTACK_CYCLE = 0; //Reset cycle
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
        GolemChargeText.SetActive(true);
        GolemChargeText.GetComponent<Text>().text = "The golem spawns some minions!";
        int j = rand1.Next(0, 3);
        if (j == 0)
        {

            Instantiate(megaMinion, spawner2.transform.position, spawner2.transform.rotation);
            Instantiate(megaMinion, spawner1.transform.position, spawner2.transform.rotation);
            Invoke("DisableGolemText", 3f);
        }
        else
        {
            Instantiate(megaMinion, spawner2.transform.position, spawner2.transform.rotation);
            Instantiate(normalMinion, spawner1.transform.position, spawner2.transform.rotation);
            Invoke("DisableGolemText", 3f);
        }
    }
    void DisableGolemText()
    {
        if (GolemChargeText != null)
        {
            GolemChargeText.GetComponent<Text>().text = "The golem prepares to charge!";
            GolemChargeText.SetActive(false);
        }

    }
    void Start()
    {
        animator = GetComponent<Animator>();
        GameObject audioController;
        audioController = GameObject.FindGameObjectWithTag("MusicHandler");
        audioController.GetComponent<AudioController>().PlayFinalBossMusic();
        foreach (GameObject obj in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
        {
            if (obj.name == "BossHPBar")
            {
                BossHPBar = obj;
            }
            if (obj.name == "BossHPBorder")
            {
                BossHPBorder = obj;
            }
            if (obj.name == "BossName")
            {
                BossName = obj;
            }
            if (obj.name == "GolemCharge")
            {
                GolemChargeText = obj;
            }
        }
        if (BossHPBar != null)
        {
            BossHPBar.SetActive(true);
        }
        if (BossHPBorder != null)
        {
            BossHPBorder.SetActive(true);
        }
        if (BossName != null)
        {
            BossName.SetActive(true);
        }
        if (GolemChargeText != null)
        {
            GolemChargeText.SetActive(false);
        }
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
        GolemChargeText.SetActive(true);
        attackTimerPause = true;
        animator.SetBool("roaring", true);
        yield return new WaitForSecondsRealtime(3);
        animator.SetBool("roaring", false);
        lookAtPlayer = false;
        yield return new WaitForSecondsRealtime(1);
        GolemChargeText.SetActive(false);
        golemCharging = true;
        animator.SetBool("isCharging", true);
        i.velocity = transform.forward * 150;
        yield return new WaitForSecondsRealtime(4.5f);
        animator.SetBool("isCharging", false);
        golemCharging = false;
        i.velocity = Vector3.zero;
        i.angularVelocity = Vector3.zero;
        lookAtPlayer = true;
        attackTimerPause = false;
        
    }
    IEnumerator GolemThrowProjectile()
    {
        while (ui.isPaused)
        {

        }
        animator.SetBool("attacking", true);
        yield return new WaitForSecondsRealtime(0.5f);
        animator.SetBool("attacking", false);
        Instantiate(projectile1, transform.Find("RockSpawner").position, transform.rotation);

    }
    IEnumerator GolemRoar()
    {
        animator.SetBool("roaring", true);
        yield return new WaitForSecondsRealtime(0.5f);
        animator.SetBool("roaring", false);
    }
    IEnumerator GolemDeath()
    {
        animator.SetBool("isDead", true);
        yield return new WaitForSecondsRealtime(3f);
        Progression i = GameObject.Find("ProgressionHandler").GetComponent<Progression>();
        i.enemyKilled(false);
        GameObject.Find("UIHandler").GetComponent<UIController>().AddPoints(5000); //5000 point bonus for killing the final boss! (before multipliers, max of 10,000 in expert)
        Destroy(this.gameObject);

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player")
        {
            if (enragedState == true)
            {
                GameObject.Find("Player").GetComponent<Player>().takeDamage(100f);
            }
            else
            {
                GameObject.Find("Player").GetComponent<Player>().takeDamage(50f);
            }

        }
    }
    float healthRemainingPercentage() //Used for the UI health bar, when the boss is active.
    {
        float returnValue;
        returnValue = currentHP / MAX_HP;
        GameObject.FindGameObjectWithTag("BossHP").GetComponent<Image>().fillAmount = returnValue;
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
        GameObject.Find("UIHandler").GetComponent<UIController>().AddPoints(damageDealt);
        if (currentHP <= MAX_HP / 2 && enragedState == false)
        {
            enragedState = true;
            BossName.GetComponent<Text>().text = "The golem is ENRAGED!";
            DEFAULT_ATTACK_SPEED = DEFAULT_ATTACK_SPEED / 2f;
            StartCoroutine("GolemRoar");
            Invoke("GolemTextEnraged", 3f);
        }
        if (currentHP <= 0)
        {
            StartCoroutine("GolemDeath");
        }
    }
    void GolemTextEnraged()
    {
        if (BossName != null)
        {
            BossName.GetComponent<Text>().text = "Volcanic Golem Protector (Enraged)";
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
