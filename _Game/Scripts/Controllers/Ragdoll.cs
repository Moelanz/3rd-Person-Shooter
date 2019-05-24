using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour 
{
	public Animator animator;
	private Rigidbody[] bodyparts;

	void Start()
	{
		bodyparts = transform.GetComponentsInChildren<Rigidbody> ();
		EnableRagdoll (false);
	}

	public void EnableRagdoll(bool value)
	{
		animator.enabled = !value;
		for (int i = 0; i < bodyparts.Length; i++) 
		{
			bodyparts [i].isKinematic = !value;
		}
	}
}
