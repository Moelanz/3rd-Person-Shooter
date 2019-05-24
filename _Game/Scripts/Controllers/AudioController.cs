using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioController : MonoBehaviour 
{
	[SerializeField] AudioClip[] clips;
	[SerializeField] float delay;

	bool canPlay;
	AudioSource source;

	void Start()
	{
		canPlay = true;
		source = GetComponent<AudioSource> ();
	}

	void Update()
	{
		
	}

	public void Play()
	{
		if (!canPlay)
			return;

		GameManager.Instance.Timer.Add (() => {
			canPlay = true;
		}, delay);

		canPlay = false;

		AudioClip clip = clips[Random.Range (0, clips.Length)];
		source.PlayOneShot (clip);
	}
}
