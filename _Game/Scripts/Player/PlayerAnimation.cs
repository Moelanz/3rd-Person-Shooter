using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour 
{
	private Animator animator;


	private PlayerAim m_PlayerAim;
	private PlayerAim PlayerAim
	{
		get
		{
			if (m_PlayerAim == null)
				m_PlayerAim = GameManager.Instance.LocalPlayer.playerAim;
			return m_PlayerAim;
		}
	}

	void Awake()
	{
		animator = GetComponentInChildren<Animator> ();
	}



	void Update()
	{
		if (GameManager.Instance.isPaused)
			return;

		animator.SetFloat ("Vertical", GameManager.Instance.InputController.vertical);
		animator.SetFloat ("Horizontal", GameManager.Instance.InputController.horizontal);

		animator.SetBool ("IsJumping", (GameManager.Instance.LocalPlayer.PlayerState.moveState == PlayerState.EMoveState.JUMPING));

		animator.SetBool ("IsWalking", GameManager.Instance.InputController.isWalking);
		animator.SetBool ("IsSprinting", GameManager.Instance.InputController.isSprinting);
		animator.SetBool ("IsCrouched", GameManager.Instance.InputController.isCrouched);

		animator.SetFloat ("AimAngle", PlayerAim.GetAngle());

		if(GameManager.Instance.LocalPlayer.PlayerState.moveState != PlayerState.EMoveState.SPRINTING)
			animator.SetBool ("IsAiming", GameManager.Instance.LocalPlayer.PlayerState.weaponState == PlayerState.EWeaponState.AIMING || GameManager.Instance.LocalPlayer.PlayerState.weaponState == PlayerState.EWeaponState.AIMED_FIRING);

		animator.SetBool ("IsInCover", GameManager.Instance.LocalPlayer.PlayerState.moveState == PlayerState.EMoveState.COVER);
        animator.SetBool ("IsVaulting", GameManager.Instance.LocalPlayer.PlayerState.moveState == PlayerState.EMoveState.VAULTING);
	}
}
