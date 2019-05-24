using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Shooter))]
public class WeaponRecoil : MonoBehaviour 
{
	[System.Serializable]
	public struct Layer
	{
		public AnimationCurve curve;
		public Vector3 direction;
	}

	[SerializeField] Layer[] layers;
	[SerializeField] float recoilSpeed;
	[SerializeField] float recoilCooldown;
	[SerializeField] float strength;

	float nextRecoilCooldown;
	float recoilActiveTime;

	Shooter m_Shooter;
	Shooter Shooter
	{
		get 
		{
			if(m_Shooter == null)
				m_Shooter = GetComponent<Shooter> ();
			return m_Shooter;
		}
	}

	Crosshair m_Crosshair;
	Crosshair Crosshair
	{
		get 
		{
			if (m_Crosshair == null)
				m_Crosshair = GameManager.Instance.LocalPlayer.playerAim.GetComponentInChildren<Crosshair> ();
			return m_Crosshair;
		}
	}

	void Update()
	{
		if (nextRecoilCooldown > Time.time)
		{
			//Holding the fire button
			recoilActiveTime += Time.deltaTime;
			float percentage = GetPercentage ();
			Vector3 recoilAmount = Vector3.zero;

			for (int i = 0; i < layers.Length; i++) 
				recoilAmount += layers [i].direction * layers [i].curve.Evaluate(percentage);

			this.Shooter.aimTargetOffset = Vector3.Lerp (Shooter.aimTargetOffset, Shooter.aimTargetOffset + recoilAmount, strength * Time.deltaTime);
			this.Crosshair.ApplyScale (percentage * Random.Range(strength * 7, strength * 9));
		} 
		else
		{
			//Not holding the fire button
			recoilActiveTime -= Time.deltaTime;
			if (recoilActiveTime < 0)
				recoilActiveTime = 0;

			this.Crosshair.ApplyScale (GetPercentage ());

			if (recoilActiveTime == 0) 
			{
				this.Shooter.aimTargetOffset = Vector3.zero;
				this.Crosshair.ApplyScale (0);
			}
		}
	}

	public void Activate()
	{
		nextRecoilCooldown = Time.time + recoilCooldown;
	}

	float GetPercentage()
	{
		return Mathf.Clamp01 (recoilActiveTime / recoilSpeed);
	}
}
