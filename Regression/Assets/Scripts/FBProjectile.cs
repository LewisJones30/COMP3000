using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FBProjectile : MonoBehaviour
{
    //Public variables


    //SerializeField variables
    [SerializeField]
    const int DAMAGE_DEALT_TO_PLAYER_DEFAULT = 25;
    //Non-SerializeField variables
    Rigidbody i;
    RaycastHit hit;

    void Start()
    {
        i = transform.gameObject.GetComponent<Rigidbody>();
        i.AddForce(transform.forward * 4000);
    }
    void Update()
    {
        if (transform.position.y < -5) //Immediately destroy if it falls through map at any point.
        {
            Destroy(this.gameObject);
        }

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        if (Physics.SphereCast(transform.position, 0.25f, forward, out hit, 1))
        {
            if (hit.collider.gameObject.name == "Player")
            {
                Destroy(this.gameObject);
                Player player = hit.collider.gameObject.GetComponent<Player>();
                MovementScript stunned = player.GetComponent<MovementScript>();
                stunned.StunPlayer(1.5f);
                player.takeDamage(DAMAGE_DEALT_TO_PLAYER_DEFAULT);
            }
        }
    }
}
