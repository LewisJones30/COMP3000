using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    GameObject projectile;
    [SerializeField]
    double attackSpeed = 5f;
    double storedAS;
    GameObject enemyParent;
    void Start()
    {
        storedAS = attackSpeed;
        enemyParent = this.transform.parent.gameObject;
    }

    // Update is called once per frame

    private void FixedUpdate()
    {
        attackSpeed = attackSpeed - Time.deltaTime;
        if (attackSpeed <= 0)
        {
            //Attack player
            if (this.tag == "ProjectileEnemy")
            {
                GameObject projectileShot;
                projectileShot = Instantiate(projectile, transform.position, transform.rotation);
                projectileShot.transform.position = new Vector3(projectileShot.transform.position.x, projectileShot.transform.position.y, projectileShot.transform.position.z);
                projectileShot.transform.rotation = enemyParent.transform.rotation;
                attackSpeed = storedAS;
            }
            else if (this.tag == "SwordEnemy")
            {

                attackSpeed = storedAS;
            }
        }
    }
}
