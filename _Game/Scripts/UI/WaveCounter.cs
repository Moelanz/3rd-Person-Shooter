using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveCounter : MonoBehaviour
{
    [SerializeField] Text waveCount;

    ZombieSpawnManager spawnManager;

	// Use this for initialization
	void Awake ()
    {
		spawnManager = FindObjectOfType<ZombieSpawnManager>();

        spawnManager.OnWaveStart += OnWaveStart;
        spawnManager.OnWaveEnd += OnWaveEnd;

        InvokeRepeating("Blink", 0, 0.5f);
	}


    void Blink()
    {
        waveCount.gameObject.SetActive(!waveCount.gameObject.activeSelf);
    }

    private void OnWaveEnd()
    {
        InvokeRepeating("Blink", 0, 0.5f);
    }

    private void OnWaveStart(int wave)
    {
        CancelInvoke("Blink");
        waveCount.gameObject.SetActive(true);

        waveCount.text = wave.ToString();
    }
}
