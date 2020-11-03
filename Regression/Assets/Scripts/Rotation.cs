using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI;

public class Rotation : MonoBehaviour
{
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        //transform.Rotate(Vector3.right * Time.deltaTime * Input.GetAxis("Mouse Y") * speed);
        //transform.Rotate(Vector3.up * Time.deltaTime * Input.GetAxis("Mouse X") * speed);
        if (Input.GetAxis("Mouse X") < 0)
            transform.Rotate((Vector3.up) * speed);
        if (Input.GetAxis("Mouse X") > 0)
            transform.Rotate((Vector3.up) * -speed);
        //if (Input.GetAxis("Mouse Y") < 0)
        //    transform.Rotate((Vector3.right) * speed);
        //if (Input.GetAxis("Mouse Y") > 0)
        //    transform.Rotate((Vector3.right) * -speed);

        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
    }
}
