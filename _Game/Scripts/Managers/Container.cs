using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Container : MonoBehaviour 
{
	private class ContainerItem
	{
		public System.Guid id;
		public string name;
		public int maximum;

		private int amountTaken;

		public ContainerItem()
		{
			id = System.Guid.NewGuid();
		}

		public int Remaining
		{
			get
			{
				return maximum - amountTaken;
			}
		}

		public int Get(int value)
		{
			if (amountTaken + value > maximum) 
			{
				int toMuch = (amountTaken + value) - maximum;
				amountTaken = maximum;
				return value - toMuch;
			}

			amountTaken += value;
			return value;
		}

		public void Set(int amount)
		{
			amountTaken -= amount;

			if (amountTaken < 0)
				amountTaken = 0;
		}
	}

	List<ContainerItem> items = new List<ContainerItem> ();
	public event System.Action OnContainerReady;

	void Awake()
	{
		/*if (OnContainerReady != null)
			OnContainerReady ();*/
	}

	public System.Guid Add(string name, int maximum)
	{
		items.Add (new ContainerItem {
			id = System.Guid.NewGuid(),
			maximum = maximum,
			name = name
		});

		return items.Last().id;
	}

	public void Put(string name, int amount)
	{
		var containerItem = items.Where (x => x.name == name).FirstOrDefault ();
		if (containerItem == null)
			return;

		containerItem.Set (amount);
	}

	public int TakeFromContainer(System.Guid id, int amount)
	{
		var containerItem = GetContainerItem (id);

		if (containerItem == null)
			return -1;

		return containerItem.Get (amount);
	}

	public int GetAmountRemaining(System.Guid id)
	{
        ContainerItem containerItem = GetContainerItem (id);

		if (containerItem == null)
			return -1;

		return containerItem.Remaining;
	}

    public System.Guid ItemExists(string name)
    {
        ContainerItem containerItem = GetContainerItemByName(name);

        if (containerItem == null)
            return System.Guid.Empty;

        return containerItem.id;
    }

	private ContainerItem GetContainerItem(System.Guid id)
	{
		return items.Where (x => x.id == id).FirstOrDefault ();
	}

    private ContainerItem GetContainerItemByName(string name)
    {
        return items.Where(x => x.name == name).FirstOrDefault();
    }
}
