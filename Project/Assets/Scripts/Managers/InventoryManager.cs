using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class InventoryManager : Singleton<InventoryManager>
{
	public GameObject inventoryMenu;

	Dictionary<string, int> inventory;

	void Awake()
	{
		inventory = new Dictionary<string, int>
		{
			{"Coal", 0},
			{"Bronze", 0},
			{"Iron", 0},
			{"Gold", 0},
			{"Diamond", 0}
		};
	}

	public void Add(string block)
	{
		inventoryMenu.transform.Find(block).GetComponent<Text>().text = block + ": " + ++inventory[block];
	}

	public void ShowInventoryMenu()
	{
		inventoryMenu.SetActive(true);
	}
}
