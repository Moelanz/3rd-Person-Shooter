using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinCondition : MonoBehaviour
{
	[SerializeField] Destructable[] Targets;

	int targetDestroyCounter;

	void Start()
	{
		for (int i = 0; i < Targets.Length; i++)
		{
			Targets[i].OnDeath += OnDeath;
		}
	}

	void OnDeath ()
	{
		targetDestroyCounter++;

		if (targetDestroyCounter == Targets.Length)
			GameManager.Instance.EventBus.RaiseEvent ("GameWin");
	}
}
