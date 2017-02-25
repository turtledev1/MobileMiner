using UnityEngine;
using System.Collections;

public class Diamond : MonoBehaviour, IBlock
{
	int health;

	public void Start()
	{
		health = 2;
	}

	public void Mine()
	{
		health--;
		//GetComponent<Animator>().Play("DirtHit");
		if(health == 0)
		{
			LevelManager.Instance.MineBlock((int)transform.position.x, (-Mathf.RoundToInt(transform.position.y)) % 100);
			InventoryManager.Instance.Add(this.GetType().Name);
		}
	}
}
