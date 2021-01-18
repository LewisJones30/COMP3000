using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapons : MonoBehaviour
{
    [SerializeField]
    double cooldown = 1.5f;
    double duplicateCD;
    [SerializeField]
    [Tooltip("The amount of damage this weapon deals, before modifiers are applied.")]
    double damageDealt = 20f;
    UIController isPausedCheck;
    Animator anim;
    RaycastHit hit;
    [SerializeField]
    GameObject collisionDetection;
    Vector3 forward;
    Player playerScript;
    // Start is called before the first frame update
    void Start()
    {
        isPausedCheck = GameObject.Find("UIHandler").GetComponent<UIController>(); //Get component for checking for pausing
        anim = this.gameObject.GetComponent<Animator>();
        duplicateCD = cooldown;
        playerScript = GameObject.Find("Player").GetComponent<Player>();
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
                    StartCoroutine("animCode");
                    cooldown = duplicateCD;
                    if (Physics.Raycast(transform.parent.position, transform.forward, out hit, 2.5f))
                    {
                        Debug.Log("Sword hit " + hit.collider.gameObject.name);
                        if (hit.collider.gameObject.tag == "ProjectileEnemy" || hit.collider.gameObject.tag == "SwordEnemy")
                        {
                            GameObject obj = hit.collider.gameObject;
                            Enemy enemyScript = hit.collider.gameObject.GetComponent<Enemy>();
                            PowerBehaviour power = GameObject.Find("PowerHandler").GetComponent<PowerBehaviour>();
                            var rand = new System.Random();
                            int RNGRoll = rand.Next(0, 999);
                            if (RNGRoll < 25)
                            {
                                PowerBehaviour powers = GameObject.Find("PowerHandler").GetComponent<PowerBehaviour>();
                                if (powers.powerHandler[12].PowerActive == true)
                                {
                                    powers.DisableAllFires();
                                }
                            }
                            RNGRoll = rand.Next(0, 999); //Roll another number between 0 and 999.
                            if (RNGRoll < 25)
                            {
                                //Justice rains from above. 2.5% chance of occuring when damage is dealt.
                                PowerBehaviour powers = GameObject.Find("PowerHandler").GetComponent<PowerBehaviour>();
                                if (powers.powerHandler[13].PowerActive == true)
                                {
                                    GameObject[] projectileEnemies = GameObject.FindGameObjectsWithTag("ProjectileEnemy");
                                    GameObject[] swordEnemies = GameObject.FindGameObjectsWithTag("SwordEnemy");
                                    foreach (GameObject obj1 in projectileEnemies)
                                    {
                                        JusticeSpawn projectiles = obj1.GetComponentInChildren<JusticeSpawn>();
                                        if (projectiles != null)
                                        {
                                            projectiles.FirePowerEffect();
                                        }

                                    }
                                    foreach (GameObject obj1 in swordEnemies)
                                    {
                                        JusticeSpawn projectiles = obj1.GetComponentInChildren<JusticeSpawn>();
                                        if (projectiles != null)
                                        {
                                            projectiles.FirePowerEffect();
                                        }
                                    }
                                }
                            }
                            RNGRoll = rand.Next(0, 999);
                            if (RNGRoll < 10)
                            {
                                //All powers are temporarily re-enabled!
                            }
                            double damageCalc;
                            double bonusDamagePower;
                            if (power.powerHandler[8].PowerAvailable == true)
                            {
                                bonusDamagePower = 1.5f;
                            }
                            else
                            {
                                bonusDamagePower = 1f;
                            }
                            damageCalc = damageDealt * bonusDamagePower; //Additional calculation here if in Expert difficulty, to reduce damage.
                            enemyScript.takeDamage(damageCalc);
                        }
                    }
                    }
            }
        }
        else
        {

        }
    }
    IEnumerator animCode()
    {
        anim.SetBool("AttackAnim", true);
        yield return new WaitForSeconds(1);
        anim.SetBool("AttackAnim", false);
    }
}
