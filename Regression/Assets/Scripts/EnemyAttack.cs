using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    GameObject projectile;
    [SerializeField]
    public double attackSpeed = 5f;
    double storedAS;
    GameObject enemyParent;
    UIController ispauseCheck;
    Animator isEnemy;
    void Start()
    {
        ispauseCheck = GameObject.Find("UIHandler").GetComponent<UIController>();
        storedAS = attackSpeed;
        enemyParent = this.transform.parent.gameObject;
        if (this.gameObject.tag == "ProjectileEnemy" || this.gameObject.tag == "SwordEnemy")
        {
            isEnemy = GetComponentInParent<Animator>();
        }
    }

    // Update is called once per frame

    private void FixedUpdate()
    {
        if (ispauseCheck.isPaused == false)
        {
            attackSpeed = attackSpeed - Time.deltaTime;
            if (attackSpeed <= 0)
            {
                //Attack player
                if (this.tag == "ProjectileEnemy")
                {
                    attackSpeed = storedAS + 1.2f;
                    //StartCoroutine("attackAnim");
                    isEnemy.SetTrigger("Attack");
                    Invoke("spawnProjectile", 1.25f);
                  

                }
                else if (this.tag == "SwordEnemy")
                {

                    attackSpeed = storedAS;
                }
            }
        }
    }
    void spawnProjectile()
    {
        GameObject projectileShot;
        projectileShot = Instantiate(projectile, transform.position, transform.rotation);
        projectileShot.transform.position = new Vector3(projectileShot.transform.position.x, projectileShot.transform.position.y, projectileShot.transform.position.z);
        projectileShot.transform.rotation = enemyParent.transform.rotation;
        attackSpeed = storedAS;
        Debug.Log("Anim triggered!");
        isEnemy.SetTrigger("StopAttack");
    }
    IEnumerator attackAnim()
    {
        isEnemy.SetTrigger("Attack");
        yield return new WaitForSeconds(1.25f);
        GameObject projectileShot;
        bool projectileMade = false;
        while (projectileMade == false)
        {
            projectileMade = true;
            projectileShot = Instantiate(projectile, transform.position, transform.rotation);
            projectileShot.transform.position = new Vector3(projectileShot.transform.position.x, projectileShot.transform.position.y, projectileShot.transform.position.z);
            projectileShot.transform.rotation = enemyParent.transform.rotation;

        }

        attackSpeed = storedAS;
        Debug.Log("Anim triggered!");
        isEnemy.SetTrigger("StopAttack");
    }
}
