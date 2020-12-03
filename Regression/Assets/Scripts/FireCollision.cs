using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCollision : MonoBehaviour
{
    GameObject player;
    Player playerScript;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        if (player == null)
        {
            Debug.Log("ERROR (Player cannot be found by fire OBJ).");
        }
        playerScript = player.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            playerScript.playerOnFire = 1;
            Debug.Log("Fire!" + other.gameObject.name);
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            playerScript.playerOnFire = 2; 
            Debug.Log("No longer on fire! " + other.gameObject.name);
        }

    }
}
