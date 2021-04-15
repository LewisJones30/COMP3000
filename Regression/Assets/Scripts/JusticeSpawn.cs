using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JusticeSpawn : MonoBehaviour
{
    //Public variables
    //SerializeField variables
    [SerializeField]
    GameObject DamagingProjectile;
    //Private variables
    PowerBehaviour powerCheck;


    //Script notes
    //This should be applied to a child object for every enemy.
    //The specified height for this object is 19.67 in the Y direction, 0 for X/Z.
    //The height is different for the final boss due to the size!
    void Start()
    {
        powerCheck = GameObject.Find("PowerHandler").GetComponent<PowerBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(transform.parent); 
    }



    public void FirePowerEffect()
    {
        if (powerCheck.powerHandler[13].GetPowerActive())
        {
            StartCoroutine("CometDamage");
        }
    }
    IEnumerator CometDamage()
    {

        GameObject projectile;
        projectile = Instantiate(DamagingProjectile, transform.position + new Vector3(1, 0, 0), transform.rotation);
        if (gameObject.tag == "FinalBoss")
        {
            FinalBoss fb = transform.parent.GetComponent<FinalBoss>();
            yield return new WaitForSeconds(0.75f);
            fb.DealDamage(25);
        }
        else
        {
            Enemy enemyScript = transform.parent.GetComponent<Enemy>();
            yield return new WaitForSeconds(0.75f);
            enemyScript.takeDamage(25);
        }

    }
}
