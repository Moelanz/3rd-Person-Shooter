using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : PickupItem
{
	[SerializeField] EWeaponType weaponType;
	[SerializeField] float respawnTime;
	[SerializeField] int amount;
    [SerializeField] bool useRespawn;

    public void NewAmmoPickup(EWeaponType type, int amount)
    {
        this.weaponType = type;
        this.amount = amount;
        this.useRespawn = false;
    }

	public override void OnPickup(Player player)
	{
		Container playerInventory = player.GetComponentInChildren<Container> ();

        if(useRespawn)
            GameManager.Instance.Respawner.Despawn (gameObject, respawnTime);
        else
            Destroy(gameObject);

		playerInventory.Put(weaponType.ToString(), amount);

		player.PlayerShoot.ActiveWeapon.reloader.HandleOnAmmoChanged ();
	}
}
