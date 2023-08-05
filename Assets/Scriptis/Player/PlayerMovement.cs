using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public CharacterController Controller;
	public Transform GroundCheck;
	public Camera cam;

	Vector3 moveH;
	Vector3 moveV;


	public LayerMask groundMask;

	float turnSmotthTime = 0.1f;
	float turnSmothVelocity;
	float x;
	float z;
	float MovingSpeed=20f;
	public float speed = 20f;
	public float sprintspeed = 40f;
	public float walkingspeed = 15f;
	float accel_start_speed;
	public float movement_accrate = 1.5f;
	float movement_accrate_carrier = 1.5f;
	float speedcarrier;
	public float gravity = -9.8f;
	public float mass = 1f;
	public float max_safe_falling_speed = -60f;
	public float max_falling_speed = -1000f;
	public float groundDistance = 0.5f;
	public float jumpheight = 3f;
	public float jump_factor = 3f;


	public float gravity_factor=0.05f;



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
		movement_accrate_carrier = movement_accrate;

		moveV.y = gravity;
	}
	void Update()
	{
		x = Input.GetAxis("Horizontal");
		z = Input.GetAxis("Vertical");
		moveH = transform.right * x + transform.forward * z;

		//Camera
		Vector3 direction = new Vector3(x, 0f, z);
		if (direction.magnitude >= 0.1f) // fix player rotation based on movement direction
		{
			float taretAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;
			float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, taretAngle, ref turnSmothVelocity, turnSmotthTime);
			transform.rotation = Quaternion.Euler(0f, angle, 0f);

			Vector3 movdir = Quaternion.Euler(0f, taretAngle, 0f) * Vector3.forward;
			Controller.Move(movdir.normalized * speed * Time.deltaTime);

		}
        else //moving
        {
			Controller.Move(moveH * MovingSpeed * Time.deltaTime);
		}



		//PlayerMovement
		CheckMotion();
		MotionState();



		#region MovementSpeedControl
		if (Sprinting)
		{
			Accelerate(ref speed, sprintspeed, ref movement_accrate ,mass);
			if (speed == sprintspeed) movement_accrate = movement_accrate_carrier;
		}
		else if (Walking)
		{
			Accelerate(ref speed, walkingspeed,ref movement_accrate, mass);
			if (speed == walkingspeed) movement_accrate = movement_accrate_carrier;
		}
		else if (Idle)
		{
			speed = speedcarrier;
			movement_accrate = movement_accrate_carrier;
		}

		#endregion
		//Check Landing
		if (Landed)
		{
			speed = speed / 3;
			StartCoroutine("Landing");
			Landed = false;
			Debug.Log("Fell with Speed: "+moveV.y);
		}
		if (Slowed)
		{
			float rate = 2f;
			Accelerate(ref speed, speedcarrier,  ref rate, mass);
		}

		//Gravity



		Controller.Move(moveV * Time.deltaTime);
		AccelByMass(ref moveV.y, accel_start_speed, max_falling_speed, mass, gravity);

		if (Grounded && moveV.y<0)
		{
			moveV.y = -2f;
			Jumping = false;
		}

		if (Input.GetButtonDown("Jump") && Grounded && !Crouched)
		{
			moveV.y = Mathf.Sqrt(jumpheight  * -1 * jump_factor * gravity);
			accel_start_speed = speed;
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
			moveV.y = 10f;
			Controller.Move(moveV);
		}

		if (Input.GetKey(KeyCode.G))
		{
			Controller.Move(moveH*1.5f);
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

		max_falling_speed = mass * gravity;

		Grounded = Physics.CheckSphere(GroundCheck.position, groundDistance, groundMask);

		if (moveV.y<=max_safe_falling_speed && Grounded)
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
	void Accelerate(ref float speed, float target_speed,ref float accel_rate, float mass=1) //Increases the magnitude of speed based on "accel_rate"
	{
		if (Mathf.Abs(speed) < Mathf.Abs(target_speed))
		{
			accel_rate += accel_rate * Time.deltaTime;
			speed += (accel_rate * mass * Time.deltaTime);
			Mathf.SmoothDamp(speed, target_speed,ref accel_rate, 2);
		}
		else
		{
			speed = target_speed;
		}
	}
	void AccelByTime(ref float actual_speed, float test_start_speed, float target_speed, float timer=1)
	{
		if (actual_speed == target_speed) return;
		bool UP;
		float speed_diff = target_speed - test_start_speed;
		if (speed_diff > 0) UP = true;
		else UP = false;


		actual_speed +=  (speed_diff * mass * Time.deltaTime)/timer;

		if (UP) { if (actual_speed > target_speed) actual_speed = target_speed; } //if speed exceeded maximum
		else { if (actual_speed < target_speed) actual_speed = target_speed; } //if speed exceeded minimum
		Debug.Log("test speed: " + actual_speed);

	}
	void AccelByMass(ref float actual_speed, float test_start_speed, float target_speed,float mass = 1,float gravity=1)
	{
		if (actual_speed == target_speed) return;
		bool UP;
		float speed_diff = target_speed - test_start_speed;
		if (speed_diff > 0) UP = true;
		else UP = false;

		float increasing = Mathf.Sign(speed_diff) * mass * Mathf.Abs(gravity) * gravity_factor * Time.deltaTime;
		float most_increasing = gravity * mass * Time.deltaTime;
		float least_increasing = gravity * Time.deltaTime;
		//actual_speed += Mathf.Sign(speed_diff) * mass * mass * Mathf.Abs(gravity) * gravity_factor * Time.deltaTime;
		actual_speed += Mathf.Clamp(increasing, most_increasing, least_increasing);


		if (UP) { if (actual_speed > target_speed) actual_speed = target_speed; } //if speed exceeded maximum
		else { if (actual_speed < target_speed) actual_speed = target_speed; } //if speed exceeded minimum
		Debug.Log("test speed: " + actual_speed);

	}


}
