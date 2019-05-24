using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawnManager : MonoBehaviour
{
    [Header("Wave Settings")]
	public float spawnTime = 2f;
	public float waveDelay = 10f;
	public int wave = 1;
	public int maxSpawns = 20;

    [Header("Enemy's")] 
	public GameObject[] enemy;                // The enemy prefab to be spawned.
	public GameObject boss;

    private static int waveSpawned = 0;
    private static int waveKilled = 0;
    private float waveTime = 0;
    private bool waveStarted = false;
    private float _timer;
	private SpawnPoint[] spawnPoints;
    private List<SpawnPoint> spawnableSpawnPoint = new List<SpawnPoint> ();

    public event System.Action<int> OnWaveStart; 
    public event System.Action OnWaveEnd;

    void Start()
    {
        //Check for spawns
	    spawnPoints = FindSpawnPoints();
        CheckSpawnableSpawn ();

        if(spawnPoints.Length == 0)
            return;

        _timer = waveDelay;
		//waveFinishedText.gameObject.SetActive (true);
		//waveFinishedText.text = "Get ready for wave " + wave + "!";
		//wavePanel.gameObject.SetActive (false);
		//crossHair.gameObject.SetActive (false);
		//waveNumber.text = wave.ToString();

        GameManager.Instance.EventBus.AddListener ("ZombieDeath", ZombieKilled);

        StartCoroutine (SpawnWaves ());
    }

    IEnumerator SpawnWaves()
	{
		while (true) 
		{
			if (waveStarted) 
			{
				if (waveSpawned <= CalculateSpawnsPerWave()) 
				{
					if(waveSpawned - waveKilled <= maxSpawns) 
					{
						yield return new WaitForSeconds (CalculateSpawnTime());

						if (wave % 5 == 0 && waveSpawned >= CalculateSpawnsPerWave() && boss != null) 
						{
							SpawnBoss();
						} 
						else 
						{
							SpawnZombie();
						}

						waveSpawned++;
					}
					else
					{
						yield return null;
					}

				}
				else if(waveKilled >= waveSpawned)
				{
					NewWave ();
				}
				else 
				{
					yield return null;
				}
						
			} 
			else 
			{
				yield return new WaitForSeconds (waveDelay);
				waveStarted = true;
				if(OnWaveStart != null)
                    OnWaveStart(wave);
			}

			waveTime += 1 * Time.deltaTime;
		}
	}

    SpawnPoint[] FindSpawnPoints()
    {
        List<SpawnPoint> validSpawnPoints = new List<SpawnPoint>();
        SpawnPoint[] points = MoreExtensions.GetSpawnPoints(SpawnPoint.SpawnType.NPC);

        for (int i = 0; i < points.Length; i++)
        {
            validSpawnPoints.Add(points[i]);
        }

        return validSpawnPoints.ToArray();
    }

    public void CheckSpawnableSpawn()
	{
		if (spawnPoints.Length > 0) 
		{
			//Clear list and create new
			if(spawnableSpawnPoint.Count > 0)
				spawnableSpawnPoint.Clear();

			foreach(SpawnPoint spawnPoint in spawnPoints)
				//if (spawnPoint.CheckOpenBarrier ())
					spawnableSpawnPoint.Add (spawnPoint);
		}
	}

    void NewWave()
	{
		_timer = waveDelay;
		waveStarted = false;
		waveSpawned = 0;
		waveKilled = 0;
		wave++;
		
        if(OnWaveEnd != null)
            OnWaveEnd();
	}

    float CalculateSpawnTime()
	{
		if (wave <= 10) return spawnTime;
		return Mathf.Clamp(spawnTime / (wave / 10) ,0.5f ,spawnTime);
	}

    int CalculateSpawnsPerWave()
    {
        int starting = 0;
        if (wave == 1) starting = 2;

        double multiplier = wave * 0.15;
        return (int)(multiplier * maxSpawns) + starting;
    }
		
    void SpawnZombie()
	{
		int random = Random.Range (0, enemy.Length);
		Spawn (enemy[random]);
	}

    void SpawnBoss()
	{
		Spawn (boss);
	}

    public void Spawn(GameObject enemy)
	{
		//Get spawn position
		Transform spawn = GetRandomSpawn ();

		// Create an instance of the enemy prefab at the randomly selected spawn point's position and rotation.
		Instantiate (enemy, spawn.position, spawn.rotation);
	}

    Transform GetRandomSpawn()
	{
		List<SpawnPoint> spawnPointsClose = GetClosestSpawns();

		int random = Random.Range (0, spawnPointsClose.Count - 1);
		return spawnPointsClose[random].transform;
	}

    List<SpawnPoint> GetClosestSpawns()
	{
		int count = 0;
		int max = Mathf.Clamp (spawnableSpawnPoint.Count, 1, 5);

		List<SpawnPoint> closestList = new List<SpawnPoint> ();
		List<SpawnPoint> checkList = new List<SpawnPoint> ();
		checkList.AddRange(spawnableSpawnPoint);

		while (count <= max) 
		{
			SpawnPoint closest = null;
			float distance = Mathf.Infinity;
            GameObject[] gos = GameObject.FindGameObjectsWithTag("Player"); //Find all players in game
			Vector3 position = gos[Random.Range(0, gos.Length)].transform.position; //Random player

			foreach (SpawnPoint sp in checkList) {
				Vector3 diff = sp.transform.position - position;
				float curDistance = diff.sqrMagnitude;

				if (curDistance < distance) {
					closest = sp;
					distance = curDistance;
				}
			}

			checkList.Remove (closest);
			closestList.Add (closest);
			count++;
		}

		return closestList;
	}

    void ZombieKilled()
    {
        waveKilled++;
    }
}
