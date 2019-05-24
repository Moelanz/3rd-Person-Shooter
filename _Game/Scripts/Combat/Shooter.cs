using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour 
{
	[SerializeField] float rateOfFire;
	[SerializeField] Projectile projectile;
	[SerializeField] Transform hand;
	[SerializeField] AudioController audioReload;
	[SerializeField] AudioController audioFire;
	[SerializeField] GameObject casing;
    [SerializeField] public GameObject gunDropPrefab;

	public Transform aimTarget;
	public Vector3 aimTargetOffset;

	[HideInInspector] public WeaponReloader reloader;
	private ParticleSystem muzzleParticleSystem;

	float nextFireAllowed;
	public bool canFire;
	Transform muzzle;
	Transform casingSpawn;

	private WeaponRecoil m_WeaponRecoil;
	private WeaponRecoil WeaponRecoil
	{
		get 
		{
			if (m_WeaponRecoil == null)
				m_WeaponRecoil = GetComponent<WeaponRecoil> ();
			return m_WeaponRecoil;
		}
	}

	public void Equip()
	{
		transform.SetParent (hand);
		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.identity;
	}

	void Awake()
	{
		muzzle = transform.Find("Model/Muzzle");
		casingSpawn = transform.Find("Model/CasingSpawn");
		reloader = GetComponent<WeaponReloader> ();
		muzzleParticleSystem = muzzle.GetComponent<ParticleSystem> ();
	}

	public void Reload()
	{
		if (reloader == null)
			return;
		
		reloader.Reload ();
		audioReload.Play ();
	}

	public virtual void Fire()
	{
		canFire = false;

		if (Time.time < nextFireAllowed)
			return;

		if (reloader != null) 
		{
			if (reloader.IsReloading)
				return;

			if (reloader.RoundsRemainingInClip == 0)
				return;

			reloader.TakeFromClip (1);
		}

		bool isLocalPlayer = aimTarget == null;
		nextFireAllowed = Time.time + rateOfFire;

		if(!isLocalPlayer)
			muzzle.LookAt (aimTarget.position + aimTargetOffset);

		// instantiate the projectile
		Projectile newBullet = (Projectile) Instantiate(projectile, muzzle.position, muzzle.rotation);
		if (isLocalPlayer) 
		{
			Ray ray = Camera.main.ViewportPointToRay(new Vector3(.5f, .5f, 0));
			RaycastHit hit;
			Vector3 targetPosition = ray.GetPoint (500);

			if(Physics.Raycast(ray, out hit))
				targetPosition = hit.point;	

            newBullet.owner = GameManager.Instance.LocalPlayer;
			newBullet.transform.LookAt (targetPosition + aimTargetOffset);
		}

		if (this.WeaponRecoil)
			this.WeaponRecoil.Activate ();

		if (casingSpawn != null)
			CasingSpawn ();

		FireEffect ();
		audioFire.Play ();
		canFire = true;
	}

	void FireEffect()
	{
		if (muzzleParticleSystem == null)
			return;

		muzzleParticleSystem.Play ();
	}

	void CasingSpawn () {
		//Spawn a casing at every casing spawnpoint
		Instantiate (casing, casingSpawn.transform.position, casingSpawn.transform.rotation);
	}
}
