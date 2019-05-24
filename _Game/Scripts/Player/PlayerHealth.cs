using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Extensions;

public class PlayerHealth : Destructable 
{
    [Header("Player")]
	[SerializeField] Ragdoll ragdoll;
	[SerializeField] float respawnTime;
    [SerializeField] bool useRespawn;

    [Header("Damage Screen")]
    [SerializeField] Image damageImage;
    [SerializeField] Color flashColour = new Color(1f, 0f, 0f, 0.1f);
    [SerializeField] bool flashAtLowHealth;
    [SerializeField] float flashBellowHealth;

    [Header("Health Regeneration Per Sec")]
    [SerializeField] bool useRegen;
    [SerializeField] float healthRegen;
    [SerializeField] float regenAfterTime;

    bool damaged;
    bool canRegen = true;
    float lastDamageTaken = 0;
    bool flashOn = false;

    void Update()
    {
        if (flashAtLowHealth && hitPointsRemaining < flashBellowHealth)
        {
            if (!flashOn)
            {
                flashOn = true;
                InvokeRepeating("ScreenFlash", 0, 0.5f);
            }
        }
        else
        {
            if (damaged)
                damageImage.color = flashColour;
            else
                damageImage.color = Color.Lerp(damageImage.color, Color.clear, 10f * Time.deltaTime);

            damaged = false;
            flashOn = false;
        }

        if(!useRegen)
            return;

        if (lastDamageTaken <= 0)
            Regen();
        else
            lastDamageTaken -= Time.deltaTime;
    }

    void ScreenFlash()
    {
        if(!flashOn)
        {
            CancelInvoke("ScreenFlash");
            return;
        }
        damageImage.color = damageImage.color == flashColour ? Color.clear : flashColour;
    }

    void RespawnAfterDeath()
    {
        SpawnAtNewSpawnPoint(); //Give a random spawn location
        Reset(); //Reset player health

        //Refresh ammo counter, you dropped all ammo in inventory
        GameManager.Instance.LocalPlayer.PlayerShoot.ActiveWeapon.reloader.HandleOnAmmoChanged();
    }

	void SpawnAtNewSpawnPoint()
	{
        SpawnPoint[] validSpawns = MoreExtensions.GetSpawnPoints(SpawnPoint.SpawnType.PLAYER);

        if(validSpawns.Length == 0)
            return;

		int spawnIndex = Random.Range (0, validSpawns.Length);
		transform.position = validSpawns [spawnIndex].transform.position;
		transform.rotation = validSpawns [spawnIndex].transform.rotation;

		ragdoll.EnableRagdoll (false);
	}

    void Regen()
    {
        if(!canRegen)
            return;

        if(damageTaken == 0)
            return;

        canRegen = false;
        Heal(healthRegen);

        GameManager.Instance.Timer.Add(() => { canRegen = true; }, 1);
    }

	public override void Die ()
	{
		base.Die ();
		ragdoll.EnableRagdoll (true);

        //GameManager.Instance.LocalPlayer.PlayerShoot.DropAllGuns(true); //Drop all guns and ammo on death

        if(useRespawn)
		    GameManager.Instance.Timer.Add (RespawnAfterDeath, respawnTime);
	}

    public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount);

        damaged = true;
        lastDamageTaken = regenAfterTime;
    }
}
