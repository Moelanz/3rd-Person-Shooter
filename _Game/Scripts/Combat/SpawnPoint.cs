using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour 
{
    public enum SpawnType
    {
        PLAYER,
        NPC
    }

    [SerializeField] Color gizmoColor = Color.blue;
    [SerializeField] public SpawnType spawnType;

	void OnDrawGizmos()
	{
		Gizmos.color = gizmoColor;
		Gizmos.matrix = transform.localToWorldMatrix;
		Gizmos.DrawWireCube (Vector3.zero + Vector3.up * 1, Vector3.one + Vector3.up * 1);
	}
}
