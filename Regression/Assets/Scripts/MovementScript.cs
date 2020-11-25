using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour
{

    Rigidbody player;
    public float movementSpeed = 15; //Multiplier for movement speed
    public float rotationSpeed = 10; //Multiplier for rotation speed
    public Rigidbody rb;
    UIController ui;
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        ui = GameObject.Find("UIHandler").GetComponent<UIController>();
    }

    void FixedUpdate()
    {
        if (ui.isPaused == true)
        {
            return;
        }
        float X = Input.GetAxisRaw("Horizontal");
        float Z = Input.GetAxisRaw("Vertical");
        rb.velocity = (transform.right * X + transform.forward * Z) * movementSpeed;
    }

}

