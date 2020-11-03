using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour
{

    Rigidbody player;
    public float movementSpeed = 15; //Multiplier for movement speed
    public float rotationSpeed = 10; //Multiplier for rotation speed
    public Rigidbody rb;
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        //transform.position = new Vector3(transform.position.x, 5, transform.position.z);
        //Movement using WASD
        if (Input.GetKey(KeyCode.W))
        {
            //rb.AddRelativeForce(new Vector3(0.0f, 0.0f, movementSpeed));
            rb.velocity = transform.forward * movementSpeed;
            
            

        }
        else if (Input.GetKey(KeyCode.S))
        {
            //rb.AddRelativeForce(new Vector3(0.0f, 0.0f, movementSpeed * -1)); //multiplied by -1 to go opposite direction
            rb.velocity = transform.forward * -movementSpeed;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            //rb.AddRelativeForce(new Vector3(movementSpeed * -1, 0.0f, 0.0f));
            rb.velocity = transform.right * -movementSpeed;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            //rb.AddRelativeForce(new Vector3(movementSpeed, 0.0f, 0.0f));
            rb.velocity = transform.right * movementSpeed;
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
    }

}

