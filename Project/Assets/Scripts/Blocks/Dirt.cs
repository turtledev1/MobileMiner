using UnityEngine;
using System.Collections;

public class Dirt : MonoBehaviour, IBlock
{
	int health;

	public void Start()
	{
		health = 2;
	}

	public void Mine()
	{
		health--;
		if(health == 0)
		{
			LevelManager.Instance.DestroyBlock((int)transform.position.x, -(int)transform.position.y);
			Destroy(gameObject);
		}
	}
}
