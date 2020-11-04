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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        if (Physics.Raycast(transform.position, forward, out hit, raycastLength))
        {
            Debug.Log(hit.collider.gameObject.name);
            if (hit.collider.gameObject.tag == "ProjectileEnemy" || hit.collider.gameObject.tag == "SwordEnemy")
            {
                double damageCalc;
                Player player = GameObject.Find("Player").GetComponent<Player>();
                Enemy enemy = hit.collider.gameObject.GetComponent<Enemy>();
                damageCalc = player.weaponPower * damagePower; //Needs modification to check difficulty.
                Debug.Log("Enemy Damaged!");
                hit.collider.gameObject.GetComponent<Enemy>().takeDamage(damageCalc);
                Destroy(this.gameObject);
                
            }
        }
    }
}
