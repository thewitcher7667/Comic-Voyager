using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public CharacterController Controller;
	public Transform GroundCheck;
	public Camera cam;

	Vector3 move;

	Vector3 Velocity;


	public LayerMask groundMask;

	float turnSmotthTime = 0.1f;
	float turnSmothVelocity;
	float x;
	float z;
	float MovingSpeed=20f;
	public float speed = 20f;
	public float sprintspeed = 40f;
	public float walkingspeed = 15f;
	float speedcarrier;
	public float gravity = -9.8f;
	public float mass = 1f;
	public float max_safe_falling_speed = -60f;
	public float max_falling_speed = -1000f;
	public float groundDistance = 0.5f;
	public float jumpheight = 3f;
	public float accrate;
	float accrate_carrier;
	float r1 = 1.5f;

	float x2 = 0;
	float z2 = 0;
	float x1 = 0;
	float z1 = 0;

	public bool Grounded;
	bool Landed;
	bool Slowed;
	bool Jumping;
	bool Sprinting;
	bool Walking;
	bool Crouched;
	bool Idle;
	bool Moving;



	void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;
		speedcarrier = speed;
		accrate_carrier = accrate;
		Velocity.y = gravity;
	}
	void Update()
	{
		x = Input.GetAxis("Horizontal");
		z = Input.GetAxis("Vertical");
		move = transform.right * x + transform.forward * z;

		//Camera
		Vector3 direction = new Vector3(x, 0f, z);
		if (direction.magnitude >= 0.1f)
		{
			float taretAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;
			float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, taretAngle, ref turnSmothVelocity, turnSmotthTime);
			transform.rotation = Quaternion.Euler(0f, angle, 0f);

			Vector3 movdir = Quaternion.Euler(0f, taretAngle, 0f) * Vector3.forward;
			Controller.Move(movdir.normalized * speed * Time.deltaTime);

		}
        else
        {
			Controller.Move(move * MovingSpeed * Time.deltaTime);
		}



		//PlayerMovement
		CheckMotion();
		MotionState();



		#region MovementSpeedControl
		if (Sprinting)
		{
			Accelerate(ref speed, sprintspeed, mass,ref r1);
			if (speed == sprintspeed) r1 = 1.5f;
		}
		else if (Walking)
		{
			Accelerate(ref speed, walkingspeed, mass, ref r1);
			if (speed == walkingspeed) r1 = 1.5f;
		}
		else if (Idle)
		{
			speed = speedcarrier;
			r1 = 1.5f;
		}

		#endregion
		//Check Landing
		if (Landed)
		{
			speed = speed / 3;
			StartCoroutine("Landing");
			Landed = false;
			Debug.Log("Fell with Speed: "+Velocity.y);
		}
		if (Slowed)
		{
			float rate = 2f;
			Accelerate(ref speed, speedcarrier, mass, ref rate);
		}

		//Gravity

		Accelerate(ref Velocity.y, max_falling_speed, mass, ref accrate);
		Controller.Move(Velocity * Time.deltaTime);

		if (Grounded && Velocity.y<0)
		{
			accrate = accrate_carrier;
			Velocity.y = -2f;
			Jumping = false;
		}

		if (Input.GetButtonDown("Jump") && Grounded && !Crouched)
		{
			Velocity.y = Mathf.Sqrt(jumpheight * -2 * gravity);
			Jumping = true;
		}
		else if (Input.GetButtonDown("Jump") && Crouched)
		{
			UnCrouch();
		}

		//SuperWeirdAbilitys
		//SwitchCursor();
		if (Input.GetKeyDown(KeyCode.T))
		{
			Velocity.y = 10f;
			accrate = accrate_carrier;
			Controller.Move(Velocity);
		}

		if (Input.GetKey(KeyCode.G))
		{
			Controller.Move(move*1.5f);
		}
	}
	
	void CheckMotion()
	{

		x1 = x2;
		z1 = z2;

		x2 = transform.position.x;
		z2 = transform.position.z;
		//Moving
		if (x1!=x2 || z1 != z2) Moving = true;

		else if (x1==x2 && z1 == z2) Moving = false;




		MovingSpeed = speed;

		Grounded = Physics.CheckSphere(GroundCheck.position, groundDistance, groundMask);

		if (Velocity.y<=max_safe_falling_speed && Grounded)
		{
			Landed = true;
		}
		if (!Grounded)
		{
			Jumping = false;
		}
		

	}

	void MotionState()
	{
		if (Input.GetKey(KeyCode.LeftShift) && Moving)
		{
			Sprinting = true;
			Walking = false;
			Idle = false;
		}
		else if(!Input.GetKey(KeyCode.LeftShift) && Moving)
		{
			Walking = true;
			Sprinting = false;
			Idle = false;
		}
		else if (!Moving)
		{
			Idle = true;
			Sprinting = false;
			Walking = false;
		}
		
		if (Input.GetKeyDown(KeyCode.LeftControl) && !Slowed)
		{
			if (!Crouched && Grounded)
			{
				speed = speed / 2.5f;				
				Crouched = true;
				Debug.Log("Crouched " + Crouched);
			}
			else if (Crouched)
			{
				speed = speedcarrier;
				Crouched = false;
				Debug.Log("Crouched " + Crouched);
			}
		}
		if (Crouched && !Grounded)
		{
			UnCrouch();
		}
		
	}
	void SwitchCursor()
	{
		if (Input.GetKeyDown(KeyCode.C) && Cursor.lockState == CursorLockMode.Locked)
			Cursor.lockState = CursorLockMode.None;
		else if (Input.GetKeyDown(KeyCode.C) && Cursor.lockState == CursorLockMode.None)
			Cursor.lockState = CursorLockMode.Locked;
	}
	void UnCrouch()
	{
		Crouched = false;
		speed = speedcarrier;
		Debug.Log("UnCrouched " + Crouched);
	}

	IEnumerator Landing()
	{
		//reset speed
		Slowed = true;
		yield return new WaitForSeconds(1f);
		speed = speedcarrier;
		Slowed = false;
		Debug.Log("SpeedBack");
	}
	void Accelerate(ref float speed, float max_speed,float mass,ref float accel_rate , float a1=1f)
	{
		if (Mathf.Abs(speed) < Mathf.Abs(max_speed))
		{
			accel_rate += accel_rate * a1 * Time.deltaTime;
			speed += (accel_rate * mass * Time.deltaTime);
		}
		else
		{
			speed = max_speed;
		}
	}

}
