using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Zombie))]
public class ZombieAnimation : MonoBehaviour
{
    [SerializeField] Animator animator;

    Zombie zombie;
    Vector3 lastPos;

	// Use this for initialization
	void Awake ()
    {
		zombie = GetComponent<Zombie> ();
        zombie.OnAttack += OnAttack;
        zombie.ZombieHealth.OnDeath += OnDeath;
	}

    private void OnDeath()
    {
        animator.SetTrigger("Death");
    }

    private void OnAttack()
    {
         animator.SetTrigger("Melee");
    }

    // Update is called once per frame
    void Update ()
    {
		float velocity = (transform.position - lastPos).magnitude / Time.deltaTime;
		lastPos = transform.position;
        if(zombie.agent.isStopped) velocity = 0;

		animator.SetBool ("IsWalking", true);
		animator.SetFloat("Vertical", velocity / zombie.agent.speed);
	}
}
