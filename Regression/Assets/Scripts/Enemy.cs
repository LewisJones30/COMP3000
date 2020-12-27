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
    RaycastHit hit;
    double attackTime;
    bool stopMoving = false;
    Animator enemyAnimations;
    UIController pauseCheck;
    Progression progressionController;
    Rigidbody i;

    // Start is called before the first frame update
    void Start()
    {
        enemyAnimations = this.GetComponent<Animator>();
        attackTime = this.gameObject.GetComponentInChildren<EnemyAttack>().attackSpeed;
        attackSpeed = attackTime;
        pauseCheck = GameObject.Find("UIHandler").GetComponent<UIController>();
        progressionController = GameObject.Find("ProgressionHandler").GetComponent<Progression>();
        i = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //Face player if in a specific aggression range.
        transform.LookAt(GameObject.Find("Player").transform); //This code needs to be changed to use the aggression range.
        
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Debug.DrawRay(transform.position, forward * 10, Color.green);
        var hits = Physics.OverlapSphere(transform.position, 0.5f);
        if (stopMoving == false)
        {
            i.AddRelativeForce(0.2f * new Vector3(0f, 0f, 1f), ForceMode.Force);
            Vector3 newVelocity = i.velocity.normalized;
            newVelocity *= 1.5f;
            i.velocity = newVelocity;
            transform.position = new Vector3(transform.position.x, 1f, transform.position.z);
        }
        else
        {
            i.velocity = Vector3.zero;
            i.angularVelocity = Vector3.zero;
            transform.position = new Vector3(transform.position.x, 1f, transform.position.z);
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
                enemyAnimations.enabled = true;
            }
        }

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        if (Physics.Raycast(transform.position, forward, out hit, raycastLength))
        {

            if (hit.collider.gameObject.name == "Player")
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

        if (stopMoving == false)
        {
            transform.position = (transform.forward / 25 + transform.position);
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
            //Grant points
            progressionController.enemyKilled();
            Destroy(this.gameObject); //Kill enemy
        }
    }
    IEnumerator flashDamaged()
    {
        
        //transform.Find("ghoul").gameObject.GetComponent<Renderer>().material.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        //transform.Find("ghoul").gameObject.GetComponent<Renderer>().material.color = Color.white;


    }
}
