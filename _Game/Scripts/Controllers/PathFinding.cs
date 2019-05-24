using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PathFinding : MonoBehaviour 
{
	[HideInInspector] public NavMeshAgent agent;

	[SerializeField] float distanceRemaingTreshold;

	bool m_destinationReached;
	bool destinationReached
	{
		get 
		{
			return m_destinationReached;
		}
		set
		{
			m_destinationReached = value;

			if (m_destinationReached) 
			{
				if (OnDestinationReached != null)
					OnDestinationReached ();
			}
		}
	}

	public event System.Action OnDestinationReached;

	void Awake()
	{
		agent = GetComponent<NavMeshAgent> ();
	}

	void Update()
	{
		if (destinationReached || !agent.hasPath)
			return;

		if (agent.remainingDistance < distanceRemaingTreshold)
			destinationReached = true;
	}

	public void SetTarget(Vector3 target)
	{
		agent.SetDestination (target);
		destinationReached = false;
	}
}
