using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Networking;

public class Projectile : MonoBehaviour
{
    //Public variables

    //SerializeField variables
    [SerializeField]
    float raycastLength = 5f;
    [SerializeField]
    double damagePower = 5f;
    //Private variables
    RaycastHit hit;
    GameObject player;
    Animator ifEnemy;
    const int JUSTICE_INT_CHANCE = 125; //1.25% chance of triggering.
    const int RAIN_INT_CHANCE = 100; //1% chance of triggering.
    const float TIME_TO_DESTROY = 3.5f;
    float timer = 0.0f;
    UIController isPaused;
    // Start is called before the first frame update
    void Start()
    {
        isPaused = GameObject.FindGameObjectWithTag("UIHandler").GetComponent<UIController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPaused.GetIsPaused()) //Only increment timer if the game is not paused!
        {
            timer = timer + Time.deltaTime;
            if (timer > TIME_TO_DESTROY)
            {
                Destroy(gameObject);
            }
        }    
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        if (Physics.SphereCast(transform.position, 0.25f, forward, out hit, 1.5f))
        {
            if (hit.collider.gameObject.CompareTag("ProjectileEnemy") || hit.collider.gameObject.CompareTag("SwordEnemy") || hit.collider.gameObject.CompareTag("FinalBoss"))
            {
                if (gameObject.tag == "EnemyProjectile")
                {
                    return;
                }
                var rand = new System.Random();
                int RNGRoll = rand.Next(0, 9999); //Roll a number between 0 and 999.


                if (RNGRoll > RAIN_INT_CHANCE && RNGRoll < RAIN_INT_CHANCE * 2) //Mysterious power doubles the chance of the environmental effects occuring.
                {
                    if (GameObject.FindWithTag("PowerHandler").GetComponent<PowerBehaviour>().powerHandler[11].GetPowerActive() == true)
                    {
                        RNGRoll = 1; //change RNG to pass.
                    }
                }

                if (RNGRoll < RAIN_INT_CHANCE) //0 to 24 gives a 2.5% chance when damaging.
                {
                    UIController ui = GameObject.FindWithTag("UIHandler").GetComponent<UIController>();

                    PowerBehaviour powers = GameObject.FindWithTag("PowerHandler").GetComponent<PowerBehaviour>();
                    if (powers.powerHandler[12].GetPowerActive() == true)
                    {
                        ui.ShowPowerActivatedMessage(1);
                        powers.DisableAllFires();
                    }
                }

                RNGRoll = rand.Next(0, 9999); //Roll 10,000 numbers. Chance is a constant.

                if (RNGRoll > JUSTICE_INT_CHANCE && RNGRoll < JUSTICE_INT_CHANCE * 2)
                {
                    if (GameObject.FindWithTag("PowerHandler").GetComponent<PowerBehaviour>().powerHandler[11].GetPowerActive() == true)
                    {
                        RNGRoll = 1; //change RNG to pass.
                    }
                }
                if (RNGRoll < JUSTICE_INT_CHANCE)
                {
                    //Justice rains from above. 2.5% chance of occuring when damage is dealt.
                    PowerBehaviour powers = GameObject.FindWithTag("PowerHandler").GetComponent<PowerBehaviour>();
                    if (powers.powerHandler[13].GetPowerActive() == true)
                    {
                        UIController ui = GameObject.FindWithTag("UIHandler").GetComponent<UIController>();

                        GameObject[] projectileEnemies = GameObject.FindGameObjectsWithTag("ProjectileEnemy");
                        GameObject[] swordEnemies = GameObject.FindGameObjectsWithTag("SwordEnemy");
                        GameObject FB = GameObject.FindGameObjectWithTag("FinalBoss");
                        if (FB != null)
                        {
                            JusticeSpawn projectiles = FB .GetComponentInChildren<JusticeSpawn>();
                            if (projectiles != null)
                            {
                                ui.ShowPowerActivatedMessage(2);
                                projectiles.FirePowerEffect();
                            }
                        }
                        foreach (GameObject obj in projectileEnemies)
                        {
                            JusticeSpawn projectiles = obj.GetComponentInChildren<JusticeSpawn>();
                            if (projectiles != null)
                            {
                                ui.ShowPowerActivatedMessage(2);
                                projectiles.FirePowerEffect();
                            }

                        }
                        foreach (GameObject obj in swordEnemies)
                        {
                            JusticeSpawn projectiles = obj.GetComponentInChildren<JusticeSpawn>();
                            if (projectiles != null)
                            {
                                projectiles.FirePowerEffect();
                            }
                        }
                    }
                }
                double damageCalc;
                Player player = GameObject.Find("Player").GetComponent<Player>();
                Enemy enemy = hit.collider.gameObject.GetComponent<Enemy>();
                damageCalc = player.GetWeaponPower() * damagePower; //Needs modification to check difficulty.
                Debug.Log("Enemy Damaged!");
                if (hit.collider.gameObject.GetComponent<Enemy>() != null)
                {
                    hit.collider.gameObject.GetComponent<Enemy>().takeDamage(damageCalc);
                }
                if (hit.collider.gameObject.CompareTag("FinalBoss"))
                {
                    hit.collider.gameObject.GetComponent<FinalBoss>().DealDamage((float)damageCalc);
                }
                Destroy(this.gameObject);


            }
            else if (hit.collider.gameObject.CompareTag("Player"))
            {
                Destroy(this.gameObject);
                Player player = hit.collider.gameObject.GetComponent<Player>();
                player.takeDamage(damagePower);
            }
            else if (hit.collider.gameObject.CompareTag("FireDamager"))
            {
                if (gameObject.gameObject.CompareTag("EnemyProjectile"))
                {
                    return;
                }
                else
                {
                    Destroy(this.gameObject);
                    PowerBehaviour power = GameObject.Find("PowerHandler").GetComponent<PowerBehaviour>();
                    //When the trident model is integrated, check if the weapon is the trident, and NOT the other staff.
                    if (power.powerHandler[10].GetPowerActive()) //Check if the power is active
                    {
                        hit.collider.gameObject.GetComponent<FireCollision>().temporarilyDisableFire();
                    }
                }

            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Projectile collision: " + collision.gameObject.name);
    }
}
