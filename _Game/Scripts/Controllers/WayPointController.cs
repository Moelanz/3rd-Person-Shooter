using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointController : MonoBehaviour 
{
	WayPoint[] waypoints;

	int currentWaypointIndex = -1;

	public event System.Action<WayPoint> OnWayPointChanged;

	void Awake()
	{
		waypoints = GetComponentsInChildren<WayPoint> ();
	}

	public void SetNextWayPoint()
	{
		currentWaypointIndex++;

		if (currentWaypointIndex == waypoints.Length)
			currentWaypointIndex = 0;

		if (OnWayPointChanged != null)
			OnWayPointChanged (waypoints[currentWaypointIndex]);
	}

	private WayPoint[] GetWayPoints()
	{
		return GetComponentsInChildren<WayPoint> ();
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;

		Vector3 previousWaypoint = Vector3.zero;

		foreach (WayPoint waypoint in GetWayPoints()) 
		{
			Vector3 waypointPos = waypoint.transform.position;
			Gizmos.DrawSphere (waypoint.transform.position, .2f);

			if (previousWaypoint != Vector3.zero)
				Gizmos.DrawLine (previousWaypoint, waypointPos);

			previousWaypoint = waypointPos;
		}
	}
}
