using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVaulting : MonoBehaviour 
{
	[SerializeField] int numberOfRays;

	private bool canVault;
	private bool isVaulting;
	private RaycastHit closestHit;
    private Transform startPos;
    private Collider[] bodyColliders;

	bool isAiming
	{
		get 
		{
			return GameManager.Instance.LocalPlayer.PlayerState.weaponState == PlayerState.EWeaponState.AIMING || 
				GameManager.Instance.LocalPlayer.PlayerState.weaponState == PlayerState.EWeaponState.AIMED_FIRING;
		}
	}

    void Awake()
    {
        bodyColliders = GetComponentsInChildren<Collider>();
    }

	void Update()
	{			
		if (!canVault)
        {
            if (GameManager.Instance.InputController.isJumping) 
                GameManager.Instance.EventBus.RaiseEvent ("canJump");
            return;
        }

		if (GameManager.Instance.InputController.isJumping) 
		{
            GameManager.Instance.EventBus.RaiseEvent ("canNotJump");
			StartVault ();
		}

        if(isVaulting)
        {
            if(Vector3.Distance(startPos.position, transform.position) > .5)
            {
                VaultToggle ();
                return;
            }
            transform.position = Vector3.Lerp(transform.position, transform.forward, .2f * Time.deltaTime);
        }
	}

    void TriggerBodyColliders()
    {
        /*foreach (Collider collider in bodyColliders)
        {
            collider.isTrigger = !collider.isTrigger;
        }*/
    }

	public void SetPlayerVaultAllowed (bool value)
	{
		canVault = value;

		if(!canVault && isVaulting)
			VaultToggle ();
	}

	void StartVault()
	{
		FindWallAroundPlayer ();

		if (closestHit.distance == 0)
			return;

		VaultToggle ();
	}

	void VaultToggle()
	{
		isVaulting = !isVaulting;
        //TriggerBodyColliders();
		GameManager.Instance.EventBus.RaiseEvent ("Vaulting");
        startPos = transform;
		//transform.rotation = Quaternion.LookRotation (closestHit.normal) * Quaternion.Euler (0, 180f, 0);
	}

	private void FindWallAroundPlayer()
	{
		closestHit = new RaycastHit ();
		float angleStep = 360 / numberOfRays;

		for (int i = 0; i < numberOfRays; i++) 
		{
			Quaternion angle = Quaternion.AngleAxis (i * angleStep, transform.up);
			CheckClosestPoint (angle);
		}

		//Debug.DrawLine (transform.position + Vector3 * .3f, closestHit.point, Color.magenta, .5f);
	}

	private void CheckClosestPoint(Quaternion angle)
	{
		RaycastHit hit;
		if(Physics.Raycast(transform.position + Vector3.up * .3f, angle * Vector3.forward, out hit, 5f))
		{
			if (closestHit.distance == 0 || hit.distance < closestHit.distance)
				closestHit = hit;
		}
	}
}
