using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerShoot : WeaponController 
{
	void Update()
	{
		if (!GetComponent<Player>().PlayerHealth.isAlive || GameManager.Instance.isPaused)
			return;
		
		if (GameManager.Instance.InputController.mouseWheelDown)
			SwitchWeapon (1);

		if (GameManager.Instance.InputController.mouseWheelUp)
			SwitchWeapon (-1);

		if (GameManager.Instance.LocalPlayer.PlayerState.moveState == PlayerState.EMoveState.SPRINTING)
			return;

		if (!canFire)
			return;
		
		if (GameManager.Instance.InputController.fire1) 
		{
			ActiveWeapon.Fire ();
		}

		if (GameManager.Instance.InputController.reload) 
		{
			ActiveWeapon.Reload ();
		}
	}
}
