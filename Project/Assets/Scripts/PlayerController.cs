using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
	void Update()
	{
		//For PC
		if(Input.GetButtonDown("Fire1"))
		{
			Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			mousePos.x = Mathf.Clamp((int)(mousePos.x + 0.5f), 0, 7);
			mousePos.y = Mathf.Clamp((int)(mousePos.y - 0.5f), -int.MaxValue, 0);
			mousePos.z = 0;

			//Get the block
			IBlock block = LevelManager.Instance.GetNearestAccessibleBlock(transform.position, mousePos);
			if(block != null)
			{
				block.Mine();
			}
			else
				transform.position = mousePos;
		}

		/*//For mobile
		foreach(Touch touch in Input.touches)
		{
			if (touch.phase == TouchPhase.Began)
			{
				Vector3 mousePos = Camera.main.ScreenToWorldPoint(touch.position);
				mousePos.x = Mathf.Clamp((int)(mousePos.x + 0.5f), 0, 7);
				mousePos.y = Mathf.Clamp((int)(mousePos.y - 0.5f), -1000, 0);
				mousePos.z = 0;

				//Get the block
				IBlock block = LevelManager.Instance.GetNearestAccessibleBlock(transform.position, mousePos);
				if(block != null)
				{
					block.Mine();
				}
				else
					transform.position = mousePos;
			}
		}*/
	}
}
