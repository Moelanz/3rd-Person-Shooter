using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cover : MonoBehaviour 
{
	[SerializeField] Collider trigger;

	PlayerCover playerCover;

	void OnTriggerEnter(Collider other)
	{
		if (!IsLocalPlayer (other))
			return;

		playerCover.SetPlayerCoverAllowed (true);
	}

	void OnTriggerExit(Collider other)
	{
		if (!IsLocalPlayer (other))
			return;

		playerCover.SetPlayerCoverAllowed (false);
	}

	private bool IsLocalPlayer(Collider other)
	{
		if (other.tag != "Player")
			return false;

		//We are not the local player
		if (other.GetComponent<Player> () != GameManager.Instance.LocalPlayer)
			return false;

		playerCover = GameManager.Instance.LocalPlayer.GetComponent<PlayerCover> ();
		return true;
	}
}
