using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;

public class WeaponReloader : MonoBehaviour 
{
	[SerializeField] int maxAmmo;
	[SerializeField] float reloadTime;
	[SerializeField] int clipSize;
	[SerializeField] Container inventory;
	[SerializeField] public EWeaponType weaponType;
    [SerializeField] public GameObject ammoPickupPrefab;

	int shotFiredInClip;
	bool isReloading;
	System.Guid containerItemId;

	public event System.Action OnAmmoChange;

	void Awake()
	{
        if(inventory == null)
            inventory = GameManager.Instance.LocalPlayer.inventory;

        containerItemId = inventory.ItemExists(weaponType.ToString());

        if (containerItemId == System.Guid.Empty)
		    containerItemId = inventory.Add (weaponType.ToString(), maxAmmo);
    }

	public int RoundsRemainingInClip
	{
		get
		{
			return clipSize - shotFiredInClip;
		}
        set
        {
            shotFiredInClip -= value;

            if(shotFiredInClip < 0)
                shotFiredInClip = 0;
        }
	}

	public int RoundsRemainingInInventory
	{
		get
		{
			return inventory.GetAmountRemaining(containerItemId);
		}
	}

	public bool IsReloading
	{
		get 
		{
			return isReloading;
		}
	}

    public void DropAllAmmo()
    {
        if(RoundsRemainingInInventory == 0)
            return;

        if(ammoPickupPrefab == null)
        {
            inventory.TakeFromContainer(containerItemId, RoundsRemainingInInventory);
            return;
        }

        //Set the ammo for the dropped ammo
        ammoPickupPrefab.GetComponent<AmmoPickup>().NewAmmoPickup(weaponType, RoundsRemainingInInventory);
        //Get random position
        Vector3 dropPos = new Vector3(transform.position.x + Random.Range(-2, 2), transform.position.y, transform.position.z + Random.Range(-2, 2));
        //Instantiate the dropped ammo
        ammoPickupPrefab = Instantiate(ammoPickupPrefab, dropPos.DetectGround(), transform.rotation);
        //Destroy after 30sec
        Destroy(ammoPickupPrefab, 45f);

        inventory.TakeFromContainer(containerItemId, RoundsRemainingInInventory);
    }

	public void Reload()
	{
		if (isReloading)
			return;

		isReloading = true;
		int amountFromInventory = inventory.TakeFromContainer (containerItemId, clipSize - RoundsRemainingInClip);

		GameManager.Instance.Timer.Add (() => {  ExecuteReload(amountFromInventory); },  reloadTime);
	}

	private void ExecuteReload(int amount)
	{
		isReloading = false;
		shotFiredInClip -= amount;

		HandleOnAmmoChanged();
	}

	public void TakeFromClip(int amount)
	{
		shotFiredInClip += amount;

		HandleOnAmmoChanged();
	}

	public void HandleOnAmmoChanged()
	{
		if (OnAmmoChange != null)
			OnAmmoChange ();
	}
}
