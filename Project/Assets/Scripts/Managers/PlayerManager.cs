using UnityEngine;
using System.Collections;

public class PlayerManager : Singleton<PlayerManager>
{
	void Update()
	{
		//Click
		if(Input.GetButtonDown("Fire1"))
		{
			Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			mousePos.x = Mathf.Clamp((int)(mousePos.x + 0.5f), 0, 6);
			mousePos.y = Mathf.Clamp((int)(mousePos.y - 0.5f), -int.MaxValue, 0);
			mousePos.z = 0;

			LevelManager.Instance.Click(transform.position, mousePos);
		}
	}

	public void Death()
	{
		GetComponent<Renderer>().enabled = false;
		InventoryManager.Instance.ShowInventoryMenu();
	}

	public void MoveTo(Vector3 newPos)
	{
		transform.position = newPos;
	}
}
