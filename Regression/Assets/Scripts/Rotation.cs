using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI;

public class Rotation : MonoBehaviour
{
    //Public variables
    public float speed;
    public float sensitivityX = 1f;
    public float sensitivityY = 1f;
    //Private variables
    float rotationx = 0f;
    float rotationy = 0f;
    UIController isPauseCheck;
    void Start()
    {
        isPauseCheck = GameObject.Find("UIHandler").GetComponent<UIController>(); //For checking if game is paused.
    }

    // Update is called once per frame
    void Update()
    {

        if (isPauseCheck.isPaused == false)
        {
            rotationx += Input.GetAxis("Mouse X") * sensitivityX;
            rotationy += Input.GetAxis("Mouse Y") * sensitivityY;
           
        }
        rotationy = Mathf.Clamp(rotationy, -90, 90);
        transform.eulerAngles = new Vector3(-rotationy, rotationx, 0);
    }
}
