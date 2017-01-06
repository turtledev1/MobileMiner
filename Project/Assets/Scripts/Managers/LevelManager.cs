using UnityEngine;
using System.Collections;

public class LevelManager : Singleton<LevelManager>
{
	public GameObject dirt;

	GameObject[,] level;
	IBlock[,] blocks;
	bool[,] accessible;

	int width;
	int depth;
	int depthMultiplicator;
	int currentDepth;

	void Start()
	{
		width = 7;
		depth = 100;
		depthMultiplicator = 0;
		currentDepth = 0;

		BuildLevel();

		CameraManager.Instance.ToggleMoving(true);
	}

	void Update()
	{
		if(-(int)Camera.main.transform.position.y > currentDepth)
			ChangedRow();
	}

	//When the camera is one row deeper
	void ChangedRow()
	{
		int depth = -(int)Camera.main.transform.position.y;
		currentDepth = depth;

		if(depth % 100 == 50)
			depthMultiplicator++;

		if(depth >= 50)
		{
			int row = (depth + 50) % 100;
			DeleteRow(row);
			SpawnRow(row);
		}

		Debug.Log(depthMultiplicator);
	}

	//Build the 100 first blocks
	void BuildLevel()
	{
		level = new GameObject[width, depth];
		blocks = new IBlock[width, depth];
		accessible = new bool[width, depth];

		//First row all air
		for(int x = 0; x < width; x++)
		{
			level[x, 0] = null;
			blocks[x, 0] = null;
			accessible[x, 0] = true;
		}
		//Second row all dirt
		for(int x = 0; x < width; x++)
		{
			level[x, 1] = (GameObject)Instantiate(dirt, new Vector3(x, -1, 0), Quaternion.identity);
			blocks[x, 1] = level[x, 1].GetComponent<Dirt>();
			accessible[x, 1] = true;
		}
		//The rest of the level
		for(int y = 2; y < depth; y++)
		{
			SpawnRow(y);
		}
	}

	//Spawn a row of blocks
	void SpawnRow(int y)
	{
		for(int x = 0; x < width; x++)
		{
			float value = Random.value;

			if(value < 0.01f)
			{
				level[x, y] = null;
				blocks[x, y] = null;
				accessible[x, y] = false;
			}
			//Dirt
			else
			{
				level[x, y] = (GameObject)Instantiate(dirt, new Vector3(x, -y - (depthMultiplicator * 100), 0), Quaternion.identity);
				blocks[x, y] = level[x, y].GetComponent<Dirt>();
				accessible[x, y] = false;
			}
		}
	}

	void DeleteRow(int y)
	{
		for(int x = 0; x < width; x++)
		{
			Destroy(level[x, y]);
		}
	}

	public void DestroyBlock(int x, int y)
	{
		Debug.Log("Destroyed block (" + x + ", " + y + ")");
		blocks[x, y] = null;
		Debug.Log(level[x, y]);
	}

	public IBlock GetNearestAccessibleBlock(Vector3 playerPosition, Vector3 clickPosition)
	{
		int[] playerPos = {(int)playerPosition.x, -(int)playerPosition.y};
		int[] clickPos = {(int)clickPosition.x, -(int)clickPosition.y};

		return blocks[clickPos[0], clickPos[1]];
	}

	bool IsAccessible(int x, int y)
	{
		return accessible[x, y];
	}
}
