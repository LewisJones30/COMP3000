using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody rb;
    public float movementSpeedMultiplier = 1f;
    UIController isPauseCheck;
    void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody>();
        isPauseCheck = GameObject.Find("UIHandler").GetComponent<UIController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isPauseCheck.isPaused == true)
        {
            return;
        }
        if (Input.GetKey(KeyCode.W))
        {
            //this.gameObject
        }
        if (Input.GetKey(KeyCode.S))
        {

        }
        if (Input.GetKey(KeyCode.A))
        {

        }
        if (Input.GetKey(KeyCode.D))
        {

        }
    }
}
