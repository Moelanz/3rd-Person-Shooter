using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;

[RequireComponent(typeof(EnemyPlayer))]
public class EnemyShoot : WeaponController 
{
	[SerializeField] float shootingSpeed;
	[SerializeField] float burstDurationMin;
	[SerializeField] float burstDurationMax;

	EnemyPlayer enemy;
	bool shootFire;

	void Start()
	{
		enemy = GetComponent<EnemyPlayer> ();
		enemy.OnTargetSelected += OnTargetSelected;
	}

	void Update()
	{
		if (!shootFire || !canFire || !enemy.EnemyHealth.isAlive)
			return;

		ActiveWeapon.Fire ();
	}

	void OnTargetSelected (Player target)
	{
		ActiveWeapon.aimTarget = target.transform;
		ActiveWeapon.aimTargetOffset = Vector3.up * 1.5f;

		StartBurst ();
	}

	void CrouchState()
	{
		bool takeCover = Random.Range (0, 3) == 0;

		if (!takeCover)
			return;

		float distanceToTarget = Vector3.Distance (transform.position, ActiveWeapon.aimTarget.position);
		if (distanceToTarget > 15) 
		{
			enemy.GetComponent<EnemyAnimation> ().isCrouched = true;
		}
	}

	void StartBurst()
	{
		if (!enemy.EnemyHealth.isAlive && !CanSeeTarget())
			return;

		CheckForRealoed ();
		shootFire = true;

		GameManager.Instance.Timer.Add(EndBurst, Random.Range(burstDurationMin, burstDurationMax));
	}

	void EndBurst()
	{
		shootFire = false;

		if (!enemy.EnemyHealth.isAlive)
			return;

		CheckForRealoed();

		if(CanSeeTarget())
			GameManager.Instance.Timer.Add (StartBurst, shootingSpeed);
	}

	bool CanSeeTarget()
	{
		if (!transform.IsInLineOfSight (ActiveWeapon.aimTarget.position, 90, enemy.scanner.mask, Vector3.up)) 
		{
			//Clear target
			enemy.ClearTargetAndScan();
			return false;
		}


		return true;
	}

	void CheckForRealoed()
	{
		if (ActiveWeapon.reloader.RoundsRemainingInClip == 0)
			ActiveWeapon.Reload ();

		CrouchState ();
	}
}
