using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapons : MonoBehaviour
{



    //Public Variables

    //SerializeField variables
    [SerializeField]
    double cooldown = 1f;
    [SerializeField]
    [Tooltip("The amount of damage this weapon deals, before modifiers are applied.")]
    double damageDealt;
    [SerializeField]
    GameObject collisionDetection;

    //Private variables
    double duplicateCD;
    UIController isPausedCheck;
    Animator anim;
    RaycastHit hit;
    Vector3 forward;
    Player playerScript;
    const float SWORD_ATTACK_DISTANCE = 12.5f;
    const int JUSTICE_POWER_CHANCE = 125;
    const int RAIN_POWER_CHANCE = 100;
    PowerBehaviour power;
    bool powerModifierApplied = false;

    // Start is called before the first frame update
    void Start()
    {
        isPausedCheck = GameObject.Find("UIHandler").GetComponent<UIController>(); //Get component for checking for pausing
        anim = this.gameObject.GetComponent<Animator>();
        duplicateCD = cooldown;
        playerScript = GameObject.Find("Player").GetComponent<Player>();
        power = GameObject.Find("PowerHandler").GetComponent<PowerBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        checkPower();
        if (!isPausedCheck.GetIsPaused())
        {
            cooldown = cooldown - Time.deltaTime;
            if (cooldown <= 0)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    //Melee Attack
                    StartCoroutine("animCode");
                    GameObject.FindGameObjectWithTag("MusicHandler").GetComponent<AudioController>().PlaySoundEffect(7); //Case 7 is Melee Attack.
                    cooldown = duplicateCD;
                    if (Physics.Raycast(transform.parent.position, transform.forward, out hit, SWORD_ATTACK_DISTANCE))
                    {
                        Debug.Log("RNG rolled!");
                        if (hit.collider.gameObject.CompareTag("ProjectileEnemy") || hit.collider.gameObject.CompareTag("SwordEnemy") || hit.collider.gameObject.CompareTag("FinalBoss"))
                        {
                            GameObject obj = hit.collider.gameObject;
                            Enemy enemyScript = hit.collider.gameObject.GetComponent<Enemy>();
                            PowerBehaviour powers = GameObject.FindWithTag("PowerHandler").GetComponent<PowerBehaviour>();
                            var rand = new System.Random();
                            Player player = GameObject.FindWithTag("MainCamera").GetComponent<Player>();
                            int RNGRoll = rand.Next(0, 9999);
                            if (RNGRoll > RAIN_POWER_CHANCE && RNGRoll < RAIN_POWER_CHANCE * 2 && player.GetRainAvailable())
                            {
                                if (powers.powerHandler[11].GetPowerActive() == true)
                                {
                                    RNGRoll = 1;
                                }
                            }
                            if (RNGRoll < RAIN_POWER_CHANCE)
                            {
                                if (powers.powerHandler[12].GetPowerActive() == true)
                                {
                                    isPausedCheck.ShowPowerActivatedMessage(1);
                                    powers.DisableAllFires();
                                    player.SetRainAvailable(false);
                                }
                            }
                            RNGRoll = rand.Next(0, 9999); //Roll another number between 0 and 999.
                            if (RNGRoll > JUSTICE_POWER_CHANCE && RNGRoll < JUSTICE_POWER_CHANCE * 2 && player.GetJusticeAvailable())
                            {
                                if (powers.powerHandler[11].GetPowerActive() == true)
                                {
                                    RNGRoll = 1;
                                }
                            }
                            if (RNGRoll < JUSTICE_POWER_CHANCE && player.GetJusticeAvailable())
                            {
                                //Justice rains from above. 2.5% chance of occuring when damage is dealt.

                                if (powers.powerHandler[13].GetPowerActive() == true)
                                {
                                    GameObject[] projectileEnemies = GameObject.FindGameObjectsWithTag("ProjectileEnemy");
                                    GameObject[] swordEnemies = GameObject.FindGameObjectsWithTag("SwordEnemy");
                                    foreach (GameObject obj1 in projectileEnemies)
                                    {
                                        JusticeSpawn projectiles = obj1.GetComponentInChildren<JusticeSpawn>();
                                        if (projectiles != null)
                                        {
                                            isPausedCheck.ShowPowerActivatedMessage(2);
                                            projectiles.FirePowerEffect();
                                            player.SetJusticeAvailable(false);
                                        }

                                    }
                                    foreach (GameObject obj1 in swordEnemies)
                                    {
                                        JusticeSpawn projectiles = obj1.GetComponentInChildren<JusticeSpawn>();
                                        if (projectiles != null)
                                        {
                                            projectiles.FirePowerEffect();
                                            player.SetJusticeAvailable(false);
                                        }
                                    }
                                }
                            }
                            double damageCalc;
                            double bonusDamagePower;
                            if (power.powerHandler[8].GetPowerAvailable() == true)
                            {
                                bonusDamagePower = 1.5f;
                            }
                            else
                            {
                                bonusDamagePower = 1f;
                            }
                            damageCalc = damageDealt * bonusDamagePower; //Additional calculation here if in Expert difficulty, to reduce damage.
                            if (hit.collider.gameObject.CompareTag("FinalBoss"))
                            {
                                FinalBoss fbScript = hit.collider.gameObject.GetComponent<FinalBoss>();
                                fbScript.DealDamage((float)damageCalc);
                            }
                            if (enemyScript != null)
                            {
                                enemyScript.takeDamage(damageCalc);
                            }
                        }
                    }
                    }
            }
        }
        else
        {

        }
    }

    void checkPower()
    {
        if (power.powerHandler[5].GetPowerActive() == true && powerModifierApplied == false)
        {
            powerModifierApplied = true;
            damageDealt = damageDealt * 1.25f;
        }
        if (power.powerHandler[5].GetPowerActive() == false && powerModifierApplied == true)
        {
            powerModifierApplied = false;
            damageDealt = damageDealt / 1.25f;
        }
    }
    IEnumerator animCode()
    {
        anim.SetBool("AttackAnim", true);
        yield return new WaitForSeconds(1);
        anim.SetBool("AttackAnim", false);
    }
}
