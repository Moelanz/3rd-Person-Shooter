using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerState))]
[RequireComponent(typeof(PlayerHealth))]
public class Player : MonoBehaviour 
{
	[System.Serializable]
	public class MouseInput
	{
		public Vector2 damping;
		public Vector2 sensitivity;
		public bool lockMouse;
	}

	[Header("Moving")]
	[SerializeField] Character characterSettings;
	[SerializeField] MouseInput mouseControl;
	[SerializeField] float minMoveTreshold;

	[Header("")]
	public PlayerAim playerAim;
    public Container inventory;

	[Header("Audio")]
	[SerializeField] AudioController footStep;

	Vector3 lastPos;
	Vector3 movedir = Vector3.zero;

    //Falldamage
    float airTime;
    float fallDamage;

	private CharacterController m_moveController;
	public CharacterController Movecontroller
	{
		get 
		{
			if (m_moveController == null)
			{
				m_moveController = GetComponent<CharacterController> ();
			}
			return m_moveController;
		}
	}

	private PlayerShoot m_PlayerShoot;
	public PlayerShoot PlayerShoot 
	{
		get
		{
			if (m_PlayerShoot == null)
				m_PlayerShoot = GetComponent<PlayerShoot> ();

			return m_PlayerShoot;
		}
	}

	private PlayerState m_PlayerState;
	public PlayerState PlayerState
	{
		get
		{
			if (m_PlayerState == null)
				m_PlayerState = GetComponent<PlayerState> ();

			return m_PlayerState;
		}
	}

	private PlayerHealth m_PlayerHealth;
	public PlayerHealth PlayerHealth
	{
		get
		{
			if (m_PlayerHealth == null)
				m_PlayerHealth = GetComponent<PlayerHealth> ();

			return m_PlayerHealth;
		}
	}

    private PlayerScore m_PlayerScore;
	public PlayerScore PlayerScore
	{
		get
		{
			if (m_PlayerScore == null)
				m_PlayerScore = GetComponent<PlayerScore> ();

			return m_PlayerScore;
		}
	}

    private InputController playerInput;
	Vector2 mouseInput;

	// Use this for initialization
	void Awake () 
	{
        //Fall damage calculated on the max health
        fallDamage = PlayerHealth.maxHitPoints / 2;

		GameManager.Instance.LocalPlayer = this;
		playerInput = GameManager.Instance.InputController;

		if (mouseControl.lockMouse) 
		{
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
		}
	}

	// Update is called once per frame
	void Update () 
	{
		if (!PlayerHealth.isAlive || GameManager.Instance.isPaused)
			return;
		
		Move ();
		LookAround ();
        CheckForFallDamage();
	}

    void CheckForFallDamage()
    {
        if(!Movecontroller.isGrounded)
            airTime += Time.deltaTime;

        if(Movecontroller.isGrounded)
        {
            if(airTime > 1f)
                PlayerHealth.TakeDamage(fallDamage * airTime);

            airTime = 0;
        }
    }

	void Move()
	{
		float moveSpeed = characterSettings.runSpeed;

		if (playerInput.isSprinting)
			moveSpeed = characterSettings.sprintSpeed;

		if (playerInput.isWalking)
			moveSpeed = characterSettings.walkSpeed;

		if (playerInput.isCrouched)
			moveSpeed = characterSettings.crouchSpeed;

		if (PlayerState.moveState == PlayerState.EMoveState.COVER)
			moveSpeed = characterSettings.walkSpeed;

		if (Movecontroller.isGrounded) 
		{
			movedir = new Vector3 (playerInput.horizontal, 0, playerInput.vertical);
			movedir = transform.TransformDirection (movedir);
			movedir *= moveSpeed;

			if (GameManager.Instance.InputController.isJumping && Movecontroller.isGrounded && PlayerState.moveState != PlayerState.EMoveState.CROUCHING)
				movedir.y = characterSettings.jumpSpeed;
		}

		movedir.y -= characterSettings.gravity * Time.deltaTime;
		Movecontroller.Move (movedir * Time.deltaTime);

		if (Vector3.Distance(transform.position, lastPos) > minMoveTreshold && Movecontroller.isGrounded)
			footStep.Play ();
	
		lastPos = transform.position;
	}

	void LookAround()
	{
		mouseInput.x = Mathf.Lerp(mouseInput.x, playerInput.mouseInput.x, 1f / mouseControl.damping.x);
		mouseInput.y = Mathf.Lerp(mouseInput.y, playerInput.mouseInput.y, 1f / mouseControl.damping.y);

		transform.Rotate (Vector3.up * mouseInput.x * mouseControl.sensitivity.x);

		playerAim.SetRotation (mouseInput.y * mouseControl.sensitivity.y);
	}
}
