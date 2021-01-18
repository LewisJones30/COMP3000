using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Networking;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    float raycastLength = 5f;
    RaycastHit hit;
    GameObject player;
    [SerializeField]
    double damagePower = 5f;
    Animator ifEnemy;
    // Start is called before the first frame update
    void Start()
    {
        if (this.gameObject.tag == "ProjectileEnemy" || this.gameObject.tag == "SwordEnemy")
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        if (Physics.SphereCast(transform.position, 0.25f, forward, out hit, 1))
        {
            if (hit.collider.gameObject.tag == "ProjectileEnemy" || hit.collider.gameObject.tag == "SwordEnemy")
            {
                var rand = new System.Random();
                int RNGRoll = rand.Next(0, 999); //Roll a number between 0 and 999.
                if (RNGRoll < 25) //0 to 24 gives a 2.5% chance when damaging.
                {
                    //The player has a 2.5% chance of extinguishing all fires temporarily, if the power is available.
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
                        foreach (GameObject obj in projectileEnemies)
                        {
                            JusticeSpawn projectiles = obj.GetComponentInChildren<JusticeSpawn>();
                            projectiles.FirePowerEffect();
                        }
                        foreach (GameObject obj in swordEnemies)
                        {
                            JusticeSpawn projectiles = obj.GetComponentInChildren<JusticeSpawn>();
                            projectiles.FirePowerEffect();
                        }
                    }
                }
                if (RNGRoll < 10)
                {
                    //All powers are temporarily re-enabled!
                }
                double damageCalc;
                Player player = GameObject.Find("Player").GetComponent<Player>();
                Enemy enemy = hit.collider.gameObject.GetComponent<Enemy>();
                damageCalc = player.weaponPower * damagePower; //Needs modification to check difficulty.
                Debug.Log("Enemy Damaged!");
                hit.collider.gameObject.GetComponent<Enemy>().takeDamage(damageCalc);
                Destroy(this.gameObject);


            }
            else if (hit.collider.gameObject.name == "Player")
            {
                Destroy(this.gameObject);
                Player player = hit.collider.gameObject.GetComponent<Player>();
                player.takeDamage(damagePower);
            }
            else if (hit.collider.gameObject.tag == "FireDamager")
            {
                Destroy(this.gameObject);
                if (this.gameObject.gameObject.tag == "ProjectileEnemy")
                {
                    return;
                }
                else
                {
                    PowerBehaviour power = GameObject.Find("PowerHandler").GetComponent<PowerBehaviour>();
                    //When the trident model is integrated, check if the weapon is the trident, and NOT the other staff.
                    if (power.powerHandler[10].PowerActive == true) //Check if the power is active
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
