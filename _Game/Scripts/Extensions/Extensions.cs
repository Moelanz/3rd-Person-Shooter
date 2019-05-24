using System;
using System.Collections.Generic;
using UnityEngine;

public class MoreExtensions : MonoBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="type">SpawnPoint Enum SpawnType</param>
    /// <returns>Array of valid spawnpoints</returns>
	public static SpawnPoint[] GetSpawnPoints(SpawnPoint.SpawnType type)
    {
        SpawnPoint[] spawnPoints = FindObjectsOfType<SpawnPoint>();
        List<SpawnPoint> validSpawns = new List<SpawnPoint>();

        foreach (SpawnPoint point in spawnPoints)
        {
            if(point.spawnType == type)
                validSpawns.Add(point);
        }

        return validSpawns.ToArray();
    }
}


