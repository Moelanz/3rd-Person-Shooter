using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour 
{
	public float vertical;
	public float horizontal;
	public Vector2 mouseInput;

	public bool fire1;
	public bool fire2;
	public bool reload;

	public bool isWalking;
	public bool isSprinting;
	public bool isCrouched;
	public bool action;

	public bool isJumping;

	public bool mouseWheelUp;
	public bool mouseWheelDown;

	public bool escape;

	private void Update()
	{
		Walking ();

		mouseInput = new Vector2 (Input.GetAxisRaw ("Mouse X"), Input.GetAxisRaw ("Mouse Y"));
		mouseWheelUp = Input.GetAxis ("Mouse ScrollWheel") > 0;
		mouseWheelDown = Input.GetAxis ("Mouse ScrollWheel") < 0;

		fire1 = Input.GetButton ("Fire1"); //shooting
		fire2 = Input.GetButton ("Fire2"); //aiming
		reload = Input.GetKey (KeyCode.R);

		action = Input.GetKeyDown (KeyCode.F);
		isJumping = Input.GetKey (KeyCode.Space);
		escape = Input.GetKeyDown (KeyCode.Escape);
	}

	void Walking()
	{
		vertical = Input.GetAxis ("Vertical");
		horizontal = Input.GetAxis ("Horizontal");

        if(isJumping)
            isCrouched = false;

		if(Input.GetKeyDown(KeyCode.C))
		{
			isWalking = false;
			isSprinting = false;
			isCrouched = !isCrouched;
		}

		if(Input.GetKeyDown(KeyCode.LeftAlt))
		{
			isWalking = !isWalking;
			isSprinting = false;
			isCrouched = false;
		}

		if (Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift)) 
		{
			isWalking = false;
			isCrouched = false;
			isSprinting = true;
		} 
		else
		{
			isSprinting = false;
		}

		isSprinting = Input.GetKey (KeyCode.LeftShift);
	}
}
