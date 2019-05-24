using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vaulting : MonoBehaviour
{
    PlayerVaulting playerVaulting;

    void OnTriggerEnter(Collider other)
	{
		if (!IsLocalPlayer (other))
			return;

		playerVaulting.SetPlayerVaultAllowed (true);
        Physics.IgnoreLayerCollision(9,0, true);
	}

	void OnTriggerExit(Collider other)
	{
		if (!IsLocalPlayer (other))
			return;

		playerVaulting.SetPlayerVaultAllowed (false);
        Physics.IgnoreLayerCollision(9,0, false);
	}

	private bool IsLocalPlayer(Collider other)
	{
		if (other.tag != "Player")
			return false;

		//We are not the local player
		if (other.GetComponent<Player> () != GameManager.Instance.LocalPlayer)
			return false;

		playerVaulting = GameManager.Instance.LocalPlayer.GetComponent<PlayerVaulting> ();
		return true;
	}
}
