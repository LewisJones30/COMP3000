﻿using System.Collections;
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


        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        Vector3 back = transform.TransformDirection(Vector3.back);
        Vector3 left = transform.TransformDirection(Vector3.left);
        if (Physics.Raycast(transform.position, forward, out hit, 5))
        {
            Debug.Log("Player Forward hit: " + hit.transform.gameObject.tag);
            if (hit.transform.gameObject.tag == "SwordEnemy" || hit.transform.gameObject.tag == "ProjectileEnemy" || hit.transform.gameObject.tag == "Terrain")
            {
                rb.angularVelocity = Vector3.zero;
                rb.velocity = Vector3.zero;
                if (Z > 0)
                {
                    Z = 0;
                }
            }
        }
        if (Physics.Raycast(transform.position, left, out hit, 5))
        {
            Debug.Log("Player Forward hit: " + hit.transform.gameObject.tag);
            if (hit.transform.gameObject.tag == "SwordEnemy" || hit.transform.gameObject.tag == "ProjectileEnemy" || hit.transform.gameObject.tag == "Terrain")
            {
                rb.angularVelocity = Vector3.zero;
                rb.velocity = Vector3.zero;
                if (X < 0)
                {
                    X = 0;
                }
            }
        }
        if (Physics.Raycast(transform.position, right, out hit, 5))
        {
            Debug.Log("Player Forward hit: " + hit.transform.gameObject.tag);
            if (hit.transform.gameObject.tag == "SwordEnemy" || hit.transform.gameObject.tag == "ProjectileEnemy" || hit.transform.gameObject.tag == "Terrain")
            {
                rb.angularVelocity = Vector3.zero;
                rb.velocity = Vector3.zero;
                if (X > 0)
                {
                    X = 0;
                }
            }
        }
        if (Physics.Raycast(transform.position, back, out hit, 5))
        {
            Debug.Log("Player Forward hit: " + hit.transform.gameObject.tag);
            if (hit.transform.gameObject.tag == "SwordEnemy" || hit.transform.gameObject.tag == "ProjectileEnemy" || hit.transform.gameObject.tag == "Terrain")
            {
                rb.angularVelocity = Vector3.zero;
                rb.velocity = Vector3.zero;
                if (Z < 0)
                {
                    Z = 0;
                }
            }
        }
        rb.AddRelativeForce(new Vector3(X * movementSpeed, 0, Z * movementSpeed), ForceMode.Force);
        Vector3 newVelocity = rb.velocity.normalized;
        newVelocity *= 1.5f;
        rb.velocity = newVelocity;
        //rb.velocity = (transform.right * X + transform.forward * Z) * movementSpeed;
        if (X + Z == 0)
        {
            rb.angularVelocity = Vector3.zero;
            rb.velocity = Vector3.zero;
        }
    }

}

