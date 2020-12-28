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
    RaycastHit hit;
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

        rb.AddRelativeForce(new Vector3(X * movementSpeed, 0, Z * movementSpeed), ForceMode.Force);
        Vector3 newVelocity = rb.velocity.normalized;
        newVelocity *= 1.5f;
       rb.velocity = newVelocity;
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        if (Physics.Raycast(transform.position, forward, out hit, 2))
        {
            if (hit.transform.gameObject.layer == 10)
            {
                rb.angularVelocity = Vector3.zero;
                rb.velocity = Vector3.zero;
            }
        }
        //rb.velocity = (transform.right * X + transform.forward * Z) * movementSpeed;
        if (X + Z == 0)
        {
            rb.angularVelocity = Vector3.zero;
            rb.velocity = Vector3.zero;
        }
    }

}

