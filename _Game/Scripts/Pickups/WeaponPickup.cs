using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : PickupItem
{
	[SerializeField] EWeaponType weaponType;
	[SerializeField] float respawnTime;
	[SerializeField] int ammoInClip;
    [SerializeField] bool useRespawn;
    [SerializeField] GameObject weaponPrefab;

    public void NewWeaponPickup(EWeaponType type, int ammoInClip)
    {
        this.weaponType = type;
        this.ammoInClip = ammoInClip;
        this.useRespawn = false;
    }

	public override void OnPickup(Player player)
	{
        //Add weapon to the players weaponlist
        GameObject weaponGO = Instantiate(weaponPrefab, player.transform.position, player.transform.rotation);
        weaponGO.transform.SetParent(player.transform.Find("Weapons"));

        //Give the amount still in clip
        WeaponReloader reloaderGO = weaponGO.GetComponent<WeaponReloader>();
        reloaderGO.RoundsRemainingInClip = ammoInClip;

        //Destroy or respawn the object
        if(useRespawn)
            GameManager.Instance.Respawner.Despawn (gameObject, respawnTime);
        else
            Destroy(gameObject);

        //Refresh weapons, you just added a gun to the list
		player.PlayerShoot.RefreshWeapons();
	}
}
