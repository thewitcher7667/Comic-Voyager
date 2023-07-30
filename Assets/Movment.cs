using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movment : MonoBehaviour
{
    public CharacterController ck;
    public float speedConst  = 6f;
    public float sprint = 12f;

    float turnSmotthTime = 0.1f;
    float turnSmothVelocity;

    bool isGrounded;

    public Transform cam;

    public Transform sphereGround;


    public float fallingSpeed = 1500;
    LayerMask layerMask;
    // Start is called before the first frame update
    void Start()
    {
        layerMask = LayerMask.GetMask("Ground");
        isGrounded = Physics.CheckSphere(sphereGround.position,0.5f, layerMask);
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(sphereGround.position, 0.5f, layerMask);
        Debug.Log(layerMask);


        float speed;
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3 (x, 0f, z);

        if (Input.GetKey(KeyCode.LeftShift))
            speed = sprint;
        else
            speed = speedConst;

        Debug.Log(isGrounded);
        //Debug.Log(Input.GetKey(KeyCode.Space) && isGrounded);
        if (Input.GetKey(KeyCode.Space) && isGrounded)
        {
            ck.Move(new Vector3(0,4,0));
        }
        else
        {
            ck.Move(new Vector3(0,-1 * Time.deltaTime * Time.deltaTime * fallingSpeed, 0));

        }

        if (direction.magnitude >= 0.1f)
        {
            float taretAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, taretAngle, ref turnSmothVelocity, turnSmotthTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 movdir = Quaternion.Euler(0f, taretAngle, 0f) * Vector3.forward;
            ck.Move(movdir.normalized * speed * Time.deltaTime);
            
        }
    }
}
