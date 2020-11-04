using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //Enemy variables
    [SerializeField]
    double health = 100f; //Can be modified for each enemy prefab in the prefab creation.
    [SerializeField]
    float ExpertOnlyDamageReduction = 0.1f;//(Set between 0 for 0% and 1 for 100%). As part of expert & satanic, this will reduce incoming damage by a certain amount.
    [SerializeField]
    GameObject enemyProjectile; //If not a melee enemy, projectile can be set here.
    [SerializeField]
    float attackSpeed = 5f; //Set the enemy's attack speed.
    float storedAS; //Used for resetting the attack speed timer.
    [SerializeField]
    int pointsWhenKilled = 100;
    [SerializeField]
    double aggressionRange; //Used for finding a range to attack the player.

    // Start is called before the first frame update
    void Start()
    {
        storedAS = attackSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        //Face player if in a specific aggression range.
        transform.LookAt(GameObject.Find("Player").transform); //This code needs to be changed to use the aggression range.
    }
    private void FixedUpdate()
    {
        attackSpeed = attackSpeed - Time.deltaTime;
        if (attackSpeed <= 0)
        {
            //Attack player
            if (this.tag == "ProjectileEnemy")
            {

                attackSpeed = storedAS;
            }
            else if (this.tag == "SwordEnemy")
            {

                attackSpeed = storedAS;
            }
        }
    }
    public void takeDamage(double damageToTake)
    {
        health = health - damageToTake;
        if (health <= 0)
        {
            //Grant points
            Destroy(this.gameObject); //Kill enemy
        }
    }
}
