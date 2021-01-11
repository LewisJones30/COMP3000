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
            Debug.Log(hit.collider.gameObject.name);
            if (hit.collider.gameObject.tag == "ProjectileEnemy" || hit.collider.gameObject.tag == "SwordEnemy")
            {
                Destroy(this.gameObject);
                double damageCalc;
                Player player = GameObject.Find("Player").GetComponent<Player>();
                Enemy enemy = hit.collider.gameObject.GetComponent<Enemy>();
                damageCalc = player.weaponPower * damagePower; //Needs modification to check difficulty.
                Debug.Log("Enemy Damaged!");
                hit.collider.gameObject.GetComponent<Enemy>().takeDamage(damageCalc);

                
            }
            else if (hit.collider.gameObject.name == "Player")
            {
                Destroy(this.gameObject);
                Player player = hit.collider.gameObject.GetComponent<Player>();
                player.takeDamage(damagePower);
            }
            else if (hit.collider.gameObject.tag == "FireWarning")
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
