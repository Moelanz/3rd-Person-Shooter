using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState : MonoBehaviour 
{
	public enum Mode
	{
		AWARE,
		UNAWARE
	}

	private Mode m_Mode;
	public Mode mode
	{
		get 
		{
			return m_Mode;
		}
		set
		{
			if (m_Mode == value)
				return;

			m_Mode = value;

			if (OnModeChanged != null)
				OnModeChanged (m_Mode);
		}
	}

	public event System.Action<Mode> OnModeChanged;

	void Awake()
	{
		mode = Mode.UNAWARE;
	}
}
