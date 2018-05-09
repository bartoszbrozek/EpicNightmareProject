using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    public Rigidbody rb;
    public float forcePower = 1000f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float actualForcePower = this.forcePower * Time.deltaTime;

        if (Input.GetKey("a"))
        {
            rb.AddForce(-actualForcePower, 0, 0);
        }
        else if (Input.GetKey("d"))
        {
            rb.AddForce(actualForcePower, 0, 0);
        }
        else if (Input.GetKey("w"))
        {
            rb.AddForce(0, 0, actualForcePower);
        }
        else if (Input.GetKey("s"))
        {
            rb.AddForce(0, 0, -actualForcePower);
        }
    }

}
