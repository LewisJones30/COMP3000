using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    //Public variables

    //SerializeField variables
    [SerializeField]
    GameObject projectile;
    [SerializeField]
    public double attackSpeed = 5f;
    [SerializeField]
    int MeleeRaycastLength = 2;
    [SerializeField]
    RaycastHit hit;
    [SerializeField]
    AudioClip MagicAttack, MeleeAttack;



    //Non-Serializefield private variables
    double storedAS;
    GameObject enemyParent;
    UIController ispauseCheck;
    Animator isEnemy;
    void Start()
    {
        ispauseCheck = GameObject.Find("UIHandler").GetComponent<UIController>();
        storedAS = attackSpeed;
        enemyParent = this.transform.parent.gameObject;
        if (this.gameObject.tag == "ProjectileEnemy" || this.gameObject.tag == "SwordEnemy")
        {
            isEnemy = GetComponentInParent<Animator>();
        }
    }

    // Update is called once per frame

    private void FixedUpdate()
    {
        if (!ispauseCheck.GetIsPaused())
        {
            attackSpeed = attackSpeed - Time.deltaTime;
            if (attackSpeed <= 0)
            {
                //Attack player
                if (this.tag == "ProjectileEnemy")
                {
                    float dist = Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("MainCamera").transform.position);
                    float volume;
                    volume = 1 - (dist / 70);
                    if (volume < 0)
                    {
                        volume = 0;
                    }
                    GetComponent<AudioSource>().PlayOneShot(MagicAttack, volume);
                    attackSpeed = storedAS + 1.2f;
                    isEnemy.SetTrigger("Attack");
                    Invoke("SpawnProjectile", 1.25f);
                  

                }
                else if (this.tag == "SwordEnemy")
                {
                    
                    attackSpeed = storedAS;
                    MeleeSwipe();

                }
            }
        }
    }
    void SpawnProjectile()
    {

        GameObject projectileShot;
        projectileShot = Instantiate(projectile, transform.position, transform.rotation);
        projectileShot.transform.position = new Vector3(projectileShot.transform.position.x, projectileShot.transform.position.y, projectileShot.transform.position.z);
        attackSpeed = storedAS;
        Debug.Log("Anim triggered!");
        isEnemy.SetTrigger("StopAttack");
    }
    void MeleeSwipe()
    {

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        if (Physics.Raycast(transform.position, forward, out hit, MeleeRaycastLength))
        {
            if (hit.transform.gameObject.name == "Player")
            {
                float dist = Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("MainCamera").transform.position);
                float volume;
                if (dist > 10)
                {
                    volume = 0;
                }
                else
                {
                    volume = 1 - (dist / 10);
                }
                GetComponent<AudioSource>().PlayOneShot(MagicAttack, volume);
                //isEnemy.Rebind();
                isEnemy.SetTrigger("Attack");
                hit.transform.gameObject.GetComponent<Player>().takeDamage(25);
                Debug.Log("Player damaged by melee attack!");
            }
        }
        isEnemy.SetTrigger("StopAttack");

    }
}
