using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Destructable : MonoBehaviour 
{
	[SerializeField] float hitPoints;
    [SerializeField] public int killScore;
    [SerializeField] public int hitScore;
  
	public event System.Action OnDeath;
	public event System.Action OnDamageReceived;

	public float damageTaken;

    public float maxHitPoints
    {
        get
        {
            return hitPoints;
        }
    }

	public float hitPointsRemaining
	{
		get 
		{
			return hitPoints - damageTaken;
		}
	}

	public bool isAlive 
	{
		get
		{
			return hitPointsRemaining > 0;
		}
	}

	public virtual void Die()
	{
		if (OnDeath != null)
			OnDeath ();
	}

    public virtual void Heal(float amount)
    {
        if (!isAlive)
			return;

        damageTaken -= amount;

        if(damageTaken < 0)
            damageTaken = 0;
    }

    //Take normal damage
	public virtual void TakeDamage(float amount)
	{
		if (!isAlive)
			return;

        damageTaken += amount;
        
		if (OnDamageReceived != null)
			OnDamageReceived ();

		if (hitPointsRemaining <= 0)
			Die ();
	}

	public void Reset()
	{
		damageTaken = 0;
	}
}
