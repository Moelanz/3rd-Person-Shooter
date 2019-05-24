using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PathFinding))]
[RequireComponent(typeof(EnemyPlayer))]
public class EnemyAnimation : MonoBehaviour 
{
	[SerializeField] Animator animator;

	Vector3 lastPos;
	PathFinding pathFinder;
	EnemyPlayer enemy;

	private bool m_isCrouched;
	public bool isCrouched
	{
		get 
		{
			return m_isCrouched;
		}
		set 
		{
			m_isCrouched = true;
			GameManager.Instance.Timer.Add (CheckIsSaveToStandup, Random.Range(20, 25));
		}
	}

	void Awake()
	{
		pathFinder = GetComponent<PathFinding> ();
		enemy = GetComponent<EnemyPlayer> ();
	}

	void Update()
	{
		float velocity = (transform.position - lastPos).magnitude / Time.deltaTime;
		lastPos = transform.position;

		animator.SetBool ("IsWalking", enemy.EnemyState.mode == EnemyState.Mode.AWARE);
		animator.SetFloat("Vertical", velocity / pathFinder.agent.speed);

		animator.SetBool ("IsAiming", enemy.EnemyState.mode == EnemyState.Mode.AWARE);
		animator.SetBool ("IsCrouched", isCrouched);
	}

	void CheckIsSaveToStandup()
	{
		bool isUnaware = enemy.EnemyState.mode == EnemyState.Mode.UNAWARE;

		if (isUnaware)
			isCrouched = false;
	}
}
