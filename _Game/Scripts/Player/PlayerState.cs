using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour 
{
	public enum EMoveState 
	{
		WALKING,
		RUNNING,
		SPRINTING,
		CROUCHING,
		JUMPING,
		COVER,
        VAULTING
	}

	public enum EWeaponState 
	{
		IDLE,
		FIRING,
		AIMING,
		AIMED_FIRING
	}

	public EMoveState moveState;
	public EWeaponState weaponState;

	bool isInCover = false;
    bool isVaulting = false;

	private InputController m_InputController;
	public InputController InputController
	{
		get
		{
			if (m_InputController == null)
				m_InputController = GameManager.Instance.InputController;
			return m_InputController;
		}
	}

	void Awake()
	{
		GameManager.Instance.EventBus.AddListener ("CoverToggle", ToggleCover);
        GameManager.Instance.EventBus.AddListener("Vaulting", ToggleVault);
	}

	void Update()
	{
		SetMoveState ();
		SetWeaponState ();
	}

	void ToggleCover()
	{
		isInCover = !isInCover;
	}

    void ToggleVault()
	{
		isVaulting = !isVaulting;
	}

	void SetWeaponState()
	{
		weaponState = EWeaponState.IDLE;

		if (InputController.fire1)
			weaponState = EWeaponState.FIRING;

		if (InputController.fire2)
			weaponState = EWeaponState.AIMING;

		if (InputController.fire1 && InputController.fire2)
			weaponState = EWeaponState.AIMED_FIRING;
	}

	void SetMoveState()
	{
		moveState = EMoveState.RUNNING;

		if (!GameManager.Instance.LocalPlayer.Movecontroller.isGrounded && InputController.isJumping)
			moveState = EMoveState.JUMPING;

		if (InputController.isSprinting)
			moveState = EMoveState.SPRINTING;

		if (InputController.isWalking)
			moveState = EMoveState.WALKING;

		if (InputController.isCrouched)
			moveState = EMoveState.CROUCHING;

		if (isInCover)
			moveState = EMoveState.COVER;

        if (isVaulting)
			moveState = EMoveState.VAULTING;
	}
}
