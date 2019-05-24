using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoCounter : MonoBehaviour 
{
	[SerializeField] Text textAmmo;
	[SerializeField] Text textWeapon;

	PlayerShoot playerShoot;
	WeaponReloader reloader;

	void Awake()
	{
		GameManager.Instance.OnLocalPlayerJoined += HandleOnLocalPlayerJoined;
		//HandleOnAmmoChange ();
	}

	void Update()
	{
		
	}

	void HandleOnLocalPlayerJoined(Player player)
	{
		playerShoot = player.PlayerShoot;
		playerShoot.OnWeaponSwitch += HandleOnWeaponSwitch;

		reloader = playerShoot.ActiveWeapon.reloader;
		reloader.OnAmmoChange += HandleOnAmmoChange;
		HandleOnAmmoChange ();
	}

	void HandleOnWeaponSwitch(Shooter activeWeapon)
	{
		reloader = activeWeapon.reloader;
		reloader.OnAmmoChange += HandleOnAmmoChange;
		HandleOnAmmoChange ();
	}

	void HandleOnAmmoChange()
	{
		int amountInInventory = reloader.RoundsRemainingInInventory;
		int amountInClip = reloader.RoundsRemainingInClip;

        if(textAmmo != null)
		    textAmmo.text = string.Format("{0}/{1}", amountInClip, amountInInventory);

		if(textWeapon != null)
			textWeapon.text = playerShoot.ActiveWeapon.name;
	}
}
