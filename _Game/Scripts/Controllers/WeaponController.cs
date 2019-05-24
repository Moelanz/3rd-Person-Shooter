using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;

public class WeaponController : MonoBehaviour 
{
	[SerializeField] float weaponSwitchTime;

	[HideInInspector] public bool canFire;
	[HideInInspector] public Shooter[] weapons;

	int currentWeaponIndex;
	Transform weaponsContainer;

	public event System.Action<Shooter> OnWeaponSwitch;

	Shooter m_ActiveWeapon;
	public Shooter ActiveWeapon
	{
		get
		{
			return m_ActiveWeapon;
		}
	}

    void Awake()
	{
		weaponsContainer = transform.Find ("Weapons");
		weapons = weaponsContainer.GetComponentsInChildren<Shooter> ();

		if (weapons.Length > 0)
			Equip (0);	
	}

    public void RefreshWeapons()
    {
        weapons = weaponsContainer.GetComponentsInChildren<Shooter> ();

        if (weapons.Length > 0)
			Equip (currentWeaponIndex);

        ActiveWeapon.reloader.HandleOnAmmoChanged();
    }

    public void DropAllGuns(bool dropAmmo)
    {
        foreach (Shooter weapon in weapons)
        {
            if(dropAmmo)
                weapon.reloader.DropAllAmmo();

            DropGun(weapon);
        }
    }

    void DropGun(Shooter weapon)
    {
        GameObject weaponGo = weapon.gunDropPrefab;

        if(weaponGo == null)
        {
            Destroy(weapon.gameObject);
            return;
        }

        //Set the ammo for the dropped gun
        weaponGo.GetComponent<WeaponPickup>().NewWeaponPickup(weapon.reloader.weaponType, weapon.reloader.RoundsRemainingInClip);
        //Get random position
        Vector3 dropPos = new Vector3(transform.position.x + Random.Range(-2, 2), transform.position.y, transform.position.z + Random.Range(-2, 2));
        //Instantiate the dropped ammo
        weaponGo = Instantiate(weaponGo, dropPos.DetectGround(), transform.rotation);
        //Destroy after 30sec
        Destroy(weaponGo, 45f);

        Destroy(weapon.gameObject); //Destroy gun from inventory
    }

	void DeactivateWeapons()
	{
		for (int i = 0; i < weapons.Length; i++) 
		{
			weapons [i].transform.SetParent (weaponsContainer);
			weapons [i].gameObject.SetActive (false);
		}
	}

	internal void SwitchWeapon(int direction)
	{
		canFire = false;
		currentWeaponIndex += direction;

		if (currentWeaponIndex > weapons.Length - 1)
			currentWeaponIndex = 0;

		if (currentWeaponIndex < 0)
			currentWeaponIndex = weapons.Length - 1;

		GameManager.Instance.Timer.Add (() => {
			Equip(currentWeaponIndex);
		}, weaponSwitchTime);
	}

	internal void Equip(int index)
	{
		DeactivateWeapons ();
		canFire = true;

		m_ActiveWeapon = weapons [index];
		m_ActiveWeapon.Equip ();

		weapons [index].gameObject.SetActive (true);

        if (OnWeaponSwitch != null)
			OnWeaponSwitch (m_ActiveWeapon);
	}
}
