using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour 
{
	[SerializeField] float speed;
	[SerializeField] float timeToLive;
	[SerializeField] float damage;

    [Header("Bullet Holes")]
	[SerializeField] Transform bulletHoleMetal;
    [SerializeField] Transform bulletHoleWood;
    [SerializeField] Transform bulletHoleConcrete;
    [SerializeField] Transform bulletHoleDirt;

    [HideInInspector] public Player owner;

	Vector3 destination;

	void Start()
	{
		Destroy (gameObject, timeToLive);
	}

	void Update()
	{
		if (IsDestinationReached ()) 
		{
			Destroy (gameObject);
			return;
		}

		transform.Translate (Vector3.forward * speed * Time.deltaTime);

		if (destination != Vector3.zero)
			return;

		RaycastHit hit;
		if (Physics.Raycast (transform.position, transform.forward, out hit, 5f)) 
		{
			CheckDestructable (hit);
		}
	}

	void CheckDestructable(RaycastHit hitInfo)
	{
        Destructable destructable = hitInfo.transform.GetComponent<Destructable> ();

		destination = hitInfo.point;

        //Check what we hit in the destructable
        string hit = hitInfo.collider.tag;

        //Draw a hole in object
        if (GetBulletHole(hit) != null)
        {
            Transform hole = (Transform)Instantiate(bulletHoleMetal, hitInfo.point + hitInfo.normal * .0015f, Quaternion.LookRotation(hitInfo.normal));
            hole.SetParent(hitInfo.transform);
            Destroy(hole.gameObject, 10);
        }

        //Has the receiver a health component?
		if (destructable == null)
			return;

        //Damage the receiver
		destructable.TakeDamage (damage * DamageMultiplier(hit));

        //Do we have player owner
        if(owner == null)
            return;

        //Apply hitscore to the playerscore
        owner.PlayerScore.AddScore(destructable.hitScore);

        //Destructable is death
        if(destructable.isAlive)
            return;

        //Apply killscore to the playerscore
        owner.PlayerScore.AddScore(destructable.killScore * ScoreMultiplier(hit));
	}

    Transform GetBulletHole(string tag)
    {
        Transform bulletHole = null;

        switch(tag)
        {
            default:
                break;

            case "Wood":
                bulletHole = bulletHoleWood;
                break;

            case "Metal":
                bulletHole = bulletHoleMetal;
                break;

            case "Concrete":
                bulletHole = bulletHoleConcrete;
                break;

              case "Dirt":
                bulletHole = bulletHoleDirt;
                break;
        }

        return bulletHole;
    }

	bool IsDestinationReached()
	{
		if (destination == Vector3.zero)
			return false;

		Vector3 directionToDestination = destination - transform.position;
		float dot = Vector3.Dot (directionToDestination, transform.forward);

		if (dot < 0)
			return true;

		return false;
	}

    int ScoreMultiplier(string tag)
    {
        int scoreMultiplier;

        switch(tag)
        {
            default:
                scoreMultiplier = 1;
                break;

            case "Head":
                scoreMultiplier = 2;
                break;
        }

        return scoreMultiplier;
    }

    float DamageMultiplier(string tag)
    {
        float damageMultiplier;

        switch(tag)
        {
            default:
                damageMultiplier = 1;
                break;

            case "Head":
                damageMultiplier = 2;
                break;
        }

        return damageMultiplier;
    }

	/*void OnTriggerEnter(Collider other)
	{
		var destructable = other.transform.GetComponent<Destructable> ();
		if (destructable == null)
			return;

		destructable.TakeDamage (damage);
	}*/
}
