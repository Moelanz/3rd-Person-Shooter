using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour 
{
    [SerializeField] string itemName;
    [SerializeField] bool pressKeyToPickup;

    private bool isInTrigger;

	void OnTriggerEnter(Collider collider)
	{
        Player player = collider.GetComponentInParent<Player>();
        if(player == null)
            return;

        if(!pressKeyToPickup)
		    PickUp (player);

        isInTrigger = true;
	}

    void OnTriggerStay(Collider collider)
    {
        Player player = collider.GetComponentInParent<Player>();
        if(player == null)
            return;

        if(pressKeyToPickup && GameManager.Instance.InputController.action)
		    PickUp (player);
    }

    void OnTriggerExit(Collider collider)
    {
        Player player = collider.GetComponentInParent<Player>();
        if(player == null)
            return;

        isInTrigger = false;
    }

    void OnGUI()
    {
        if(isInTrigger)
            GUI.Label(new Rect((Screen.width - 250) / 2, (Screen.height - 300) / 2, 250, 300), "Press \"F\" to pickup " + itemName);
    }

	public virtual void OnPickup(Player player)
	{
		
	}

	void PickUp(Player player)
	{
		OnPickup (player);
	}
}
