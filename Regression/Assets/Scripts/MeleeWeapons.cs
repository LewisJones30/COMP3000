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
        forward = collisionDetection.transform.TransformDirection(Vector3.forward);
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
