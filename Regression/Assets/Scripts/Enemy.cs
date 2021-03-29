using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //Enemy variables
    [SerializeField]
    double health = 100f; //Can be modified for each enemy prefab in the prefab creation.
    [SerializeField]
    float ExpertOnlyDamageReduction = 0.1f;//(Set between 0 for 0% and 1 for 100%). As part of expert & satanic, this will reduce incoming damage by a certain amount.
    [SerializeField]
    [Tooltip("Assign a projectile prefab here.")]
    GameObject enemyProjectile; //If not a melee enemy, projectile can be set here.
    [SerializeField]
    [Tooltip("This is the amount of time between this enemy's attacks.")]
    double attackSpeed = 5f; //Set the enemy's attack speed.
    float storedAS; //Used for resetting the attack speed timer.
    [SerializeField]
    int pointsWhenKilled = 100;
    [SerializeField]
    [Tooltip("This shows the distance from the player this enemy will stop moving at.")]
    float raycastLength = 10f;
    [SerializeField]
    [Tooltip("Choose the movement speed of the enemy here. 1 is the default speed.")]
    [Range(0.1f, 5.0f)]
    float enemyMovementSpeed = 1f;
    [SerializeField]
    bool FinalBossMinion = false;
    RaycastHit hit;
    double attackTime;
    bool stopMoving = false;
    Animator enemyAnimations;
    UIController pauseCheck;
    Progression progressionController;
    [SerializeField]
    Rigidbody i;
    PowerBehaviour powerScaleCheck;
    const float INFLATED_POWER_SCALE = 1.5f;
    float startingScaleX, startingScaleY, startingScaleZ;
    float inflatedScaleX, inflatedScaleY, inflatedScaleZ;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        enemyAnimations = this.GetComponent<Animator>();
        attackTime = this.gameObject.GetComponentInChildren<EnemyAttack>().attackSpeed;
        attackSpeed = attackTime;
        pauseCheck = GameObject.Find("UIHandler").GetComponent<UIController>();
        progressionController = GameObject.Find("ProgressionHandler").GetComponent<Progression>();
        powerScaleCheck = GameObject.Find("PowerHandler").GetComponent<PowerBehaviour>();
        i = GetComponent<Rigidbody>();
        startingScaleX = transform.localScale.x;
        startingScaleY = transform.localScale.y;
        startingScaleZ = transform.localScale.z;
        inflatedScaleX = startingScaleX * INFLATED_POWER_SCALE;
        inflatedScaleY = startingScaleY * INFLATED_POWER_SCALE;
        inflatedScaleZ = startingScaleZ * INFLATED_POWER_SCALE;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //Face player if in a specific aggression range.
        transform.LookAt(GameObject.Find("playerLook").transform); //This code needs to be changed to use the aggression range.
        if (stopMoving == true)
        {
            i.Sleep();
        }
    }
    private void FixedUpdate()
    {
        if (stopMoving == false)
        {
            enemyAnimations.SetBool("isWalking", true);
        }
        else
        {
            enemyAnimations.SetBool("isWalking", false);
        }
        if (pauseCheck != null)
        {
            if (pauseCheck.isPaused == true)
            {
                stopMoving = true;
                enemyAnimations.enabled = false;
                return;
            }
            else
            {
                if (transform.localScale.z != inflatedScaleZ && powerScaleCheck.powerHandler[7].PowerActive == true)
                {
                    transform.localScale = new Vector3(inflatedScaleX, inflatedScaleY, inflatedScaleZ);

                }
                else
                {
                    if (powerScaleCheck.powerHandler[7].PowerActive == false)
                    {
                        transform.localScale = new Vector3(startingScaleX, startingScaleY, startingScaleZ);
                    }
                }
                enemyAnimations.enabled = true;
            }
        }

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        if (Physics.Raycast(transform.position, forward, out hit, raycastLength))
        {
            Debug.Log("Enemy raycast player check: " + hit.transform.gameObject.name);
            if (hit.collider.gameObject.name == "Player" || hit.collider.gameObject.tag == "SwordEnemy")
            {
                stopMoving = true;
                return;
            }
            else if (hit.collider.gameObject.GetComponent<Enemy>() != null)
            {
                stopMoving = true;
                return;
            }

        }

        else
        {
            stopMoving = false;
        }

        if (stopMoving == true)
        {
            i.velocity = Vector3.zero;
            i.angularVelocity = Vector3.zero;
            transform.position = new Vector3(transform.position.x, 2.5f, transform.position.z);

        }
        else
        {
            i.AddRelativeForce(new Vector3(0f, 0f, 1f), ForceMode.VelocityChange);
            Vector3 newVelocity = i.velocity.normalized;
            newVelocity *= 1.5f;
            i.velocity = newVelocity;
            transform.position = new Vector3(transform.position.x, 1.4f, transform.position.z);
        }
        attackTime = this.gameObject.GetComponentInChildren<EnemyAttack>().attackSpeed;
        if (attackTime <= 0 || attackTime > attackSpeed)
        {
            stopMoving = true;
        }
        else
        {
            stopMoving = false;
        }
    }
    public void takeDamage(double damageToTake)
    {
        health = health - damageToTake;
        StartCoroutine("flashDamaged");
        if (health <= 0)
        {
            StartCoroutine("EnemyKilled");
        }
    }
    IEnumerator EnemyKilled()
    {
        animator.SetBool("isDead", true);
        pauseCheck.AddPoints(pointsWhenKilled);
        yield return new WaitForSeconds(1f);
        if (pauseCheck.getTutorialStage() > 0)
        {
            pauseCheck.TutorialEnemyKilled();
        }

        progressionController.enemyKilled(FinalBossMinion);
        Destroy(this.gameObject); //Kill enemy
    }
    IEnumerator flashDamaged()
    {
        this.gameObject.GetComponentInChildren<Renderer>().material.color = Color.red;
        //transform.Find("ghoul").gameObject.GetComponent<Renderer>().material.color = Color.red;
        yield return new WaitForSeconds(0.25f);
        this.gameObject.GetComponentInChildren<Renderer>().material.color = Color.white;
        //transform.Find("ghoul").gameObject.GetComponent<Renderer>().material.color = Color.white;


    }
}
