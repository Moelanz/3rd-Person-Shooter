using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PathFinding))]
[RequireComponent(typeof(EnemyPlayer))]
public class EnemyPatrol : MonoBehaviour 
{
	[SerializeField] WayPointController waypointController;
	[SerializeField] float waitTimeMin;
	[SerializeField] float waitTimeMax;

	PathFinding pathFinder;

	private EnemyPlayer m_EnemyPlayer;
	public EnemyPlayer EnemyPlayer
	{
		get
		{
			if (m_EnemyPlayer == null)
				m_EnemyPlayer = GetComponent<EnemyPlayer> ();
			return m_EnemyPlayer;
		}
	}

	void Awake()
	{
		pathFinder = GetComponent<PathFinding> ();

		EnemyPlayer.EnemyHealth.OnDeath += OnDeath;
		EnemyPlayer.OnTargetSelected += OnTargetSelected;
	}

	void OnTargetSelected (Player target)
	{
        if (pathFinder.agent.isActiveAndEnabled)
            pathFinder.agent.isStopped = true;
	}

	void OnEnable()
	{
		pathFinder.OnDestinationReached += PathFinder_OnDestinationReached;
		waypointController.OnWayPointChanged += WaypointController_OnWayPointChanged;
	}

	void OnDisable()
	{
		pathFinder.OnDestinationReached -= PathFinder_OnDestinationReached;
		waypointController.OnWayPointChanged -= WaypointController_OnWayPointChanged;
	}

	void OnDeath ()
	{
        if (pathFinder.agent.isActiveAndEnabled)
            pathFinder.agent.isStopped = true;
	}

	void Start()
	{
		waypointController.SetNextWayPoint ();
	}

	void PathFinder_OnDestinationReached ()
	{
		GameManager.Instance.Timer.Add (waypointController.SetNextWayPoint, Random.Range (waitTimeMin, waitTimeMax));
	}

	void WaypointController_OnWayPointChanged (WayPoint waypoint)
	{
		pathFinder.SetTarget (waypoint.transform.position);
	}
}
