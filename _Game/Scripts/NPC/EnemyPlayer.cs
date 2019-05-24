using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(PathFinding))]
[RequireComponent(typeof(EnemyHealth))]
[RequireComponent(typeof(EnemyState))]
public class EnemyPlayer : MonoBehaviour 
{
	[SerializeField] Character npcSettings;
	[SerializeField] public Scanner scanner;

	PathFinding pathFinder;
	Player target;
	List<Player> myTargets;

	public event System.Action<Player> OnTargetSelected;

	private EnemyHealth m_EnemyHealth;
	public EnemyHealth EnemyHealth
	{
		get
		{
			if (m_EnemyHealth == null)
				m_EnemyHealth = GetComponent<EnemyHealth> ();
			return m_EnemyHealth;
		}
	}

	private EnemyState m_EnemyState;
	public EnemyState EnemyState
	{
		get
		{
			if (m_EnemyState == null)
				m_EnemyState = GetComponent<EnemyState> ();
			return m_EnemyState;
		}
	}

	void Start()
	{
		pathFinder = GetComponent<PathFinding> ();
		pathFinder.agent.speed = npcSettings.walkSpeed;

		scanner.OnScanReady += Scanner_OnScanReady;
		Scanner_OnScanReady ();

		EnemyHealth.OnDeath += OnDeath;
		EnemyState.OnModeChanged += OnModeChanged;
	}

	void Update()
	{
		if (target == null || !EnemyHealth.isAlive)
			return;

		transform.LookAt (target.transform.position);
	}

	void OnModeChanged (EnemyState.Mode state)
	{
		if(state == EnemyState.Mode.AWARE)
			pathFinder.agent.speed = npcSettings.runSpeed;
	}

	void OnDeath ()
	{
		
	}

	void Scanner_OnScanReady ()
	{
		if (scanner == null)
			return;
		
		if (target != null)
			return;

		myTargets = scanner.ScanForTargets<Player> ();

		if (myTargets.Count == 1)
			target = myTargets [0];
		else
			SelectedClosestTarget ();

		if (target != null) 
		{
			if (OnTargetSelected != null) 
			{
				this.EnemyState.mode = EnemyState.Mode.AWARE;
				OnTargetSelected (target);
			}


		}
	}

	void SetDestination ()
	{
		pathFinder.SetTarget (target.transform.position);
	}

	void SelectedClosestTarget()
	{
		if (scanner == null)
			return;
		
		float closestTarget = scanner.scanRange;

		foreach (Player player in myTargets) 
		{
			if (Vector3.Distance (transform.position, player.transform.position) < closestTarget)
				target = player;
		}
	}

	void CheckEaseWeapon()
	{
		//Check If we can ease our weapon (stop aming)
		if (target != null)
			return;

		this.EnemyState.mode = EnemyState.Mode.UNAWARE;
	}

	void CheckContinuePatrol()
	{
		// Check if we can continue our patrol
		if (target != null)
			return;

        pathFinder.agent.isStopped = false;
	}

	public void ClearTargetAndScan ()
	{
		target = null;

		GameManager.Instance.Timer.Add (CheckEaseWeapon, Random.Range(3, 5));
		GameManager.Instance.Timer.Add (CheckContinuePatrol, Random.Range(12, 15));

		Scanner_OnScanReady ();
	}
}
