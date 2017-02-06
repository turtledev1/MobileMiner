using UnityEngine;
using System.Collections;

public class CameraManager : Singleton<CameraManager>
{
	float movement;
	bool move = false;

	GameObject player;

	public GameObject backgroundPrefab;
	public GameObject gameOver;

	void Awake()
	{
		player = GameObject.FindGameObjectWithTag("Player");
	}

	void Start()
	{
		movement = 0.001f;
	}

	void FixedUpdate()
	{
		if(move)
		{
			//Move the camera down
			Vector3 newPos = transform.position;
			newPos.y -= movement;
			transform.position = newPos;

			//Accelerate the movement
			movement += 0.00001f;

			//Check if the player is dead
			if(player.transform.position.y > transform.position.y + 6.8f)
			{
				move = false;
				gameOver.SetActive(true);
				PlayerManager.Instance.Death();
			}
		}
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		Vector3 pos = new Vector3(3, col.transform.position.y - 15, 1);
		Instantiate(backgroundPrefab, pos, Quaternion.identity);
		

		Destroy(col.gameObject);
	}

	public void ToggleMoving(bool m)
	{
		move = m;
	}
}
