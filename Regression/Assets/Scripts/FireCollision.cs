using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCollision : MonoBehaviour
{
    GameObject player;
    Player playerScript;
    bool AllFiresActive = true;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        if (player == null)
        {
            Debug.Log("ERROR (Player cannot be found by fire OBJ).");
        }
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
    public void temporarilyDisableFire()
    {
        Debug.Log("Fire temporarily extinguished!");
        StartCoroutine("WaterExinguish");
    }
    IEnumerator WaterExinguish()
    {
        var emission = this.gameObject.GetComponent<ParticleSystem>().emission;
        emission.enabled = false;
        this.gameObject.GetComponent<BoxCollider>().enabled = false;
        yield return new WaitForSeconds(20);
        Debug.Log("Fire re-enabled!");
        this.gameObject.GetComponent<BoxCollider>().enabled = true;
        emission.enabled = true;
    }


}
