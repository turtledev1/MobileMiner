using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelManager : Singleton<LevelManager>
{
	public GameObject dirt;

	GameObject[,] level;
	IBlock[,] blocks;
	bool[,] accessible;

	int width;
	int height;
	int depthMultiplicator;
	int currentDepth;
	int maxDepth;
	public Text depthIndicator;

	void Start()
	{
		width = 7;
		height = 100;
		depthMultiplicator = 0;
		currentDepth = 0;
		maxDepth = 0;

		BuildLevel();

		CameraManager.Instance.ToggleMoving(true);
	}

	void Update()
	{
		if(-(Mathf.RoundToInt(Camera.main.transform.position.y)) > currentDepth)
			ChangedRow();
	}

	//When the camera is one row deeper
	void ChangedRow()
	{
		int depth = -(Mathf.RoundToInt(Camera.main.transform.position.y));
		currentDepth = depth;
			
		if(depth % 100 == 50)
			depthMultiplicator++;

		//Start creating more rows after we reached depth50
		if(depth >= 50)
		{
			int row = (depth + 50) % 100;
			DeleteRow(row);
			SpawnRow(row);
		}
	}

	//Build the 100 first blocks
	void BuildLevel()
	{
		level = new GameObject[width, height];
		blocks = new IBlock[width, height];
		accessible = new bool[width, height];

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
		for(int y = 2; y < height; y++)
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

			//Air
			if(value < 0.5f)
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

	//Delete an entire row
	void DeleteRow(int y)
	{
		for(int x = 0; x < width; x++)
		{
			DestroyBlock(x, y);
		}
	}

	//Destroy one block
	public void DestroyBlock(int x, int y)
	{
		Destroy(level[x, y]);
		blocks[x, y] = null;
	}

	//Mine a block and update the accessibility of the adjacent blocks
	public void MineBlock(int x, int y)
	{
		DestroyBlock(x, y);
		UpdateAccessible(x, y);
	}

	//When the player click in the screen
	public void Click(Vector3 playerPosition, Vector3 clickPosition)
	{
		int[] playerPos = {(int)playerPosition.x, -(int)playerPosition.y};
		int[] clickPos = {(int)clickPosition.x, -(int)clickPosition.y};

		//If the block is accessible
		if(accessible[clickPos[0], clickPos[1] % 100])
		{
			MoveAndMine(clickPos[0], clickPos[1] % 100);
		}
		else
		{
			//If we clicked deeper than the player, find the deepest accessible block within the same column, if none, check the next column
			if(clickPos[1] > playerPos[1])
			{
				//Clicked to the left
				if(clickPos[0] < playerPos[0])
				{
					for(int x = clickPos[0]; x < playerPos[0]; x++)
						for(int y = clickPos[1] - 1; y >= playerPos[1]; y--)
						{
							if(accessible[x, y % 100])
							{
								MoveAndMine(x, y % 100);
								return;
							}
						}
				}
				//Clicked to the right
				else if(clickPos[0] > playerPos[0])
				{
					for(int x = clickPos[0]; x > playerPos[0]; x--)
						for(int y = clickPos[1] - 1; y >= playerPos[1]; y--)
						{
							if(accessible[x, y % 100])
							{
								MoveAndMine(x, y % 100);
								return;
							}
						}
				}
				//Clicked under
				else
				{
					for(int y = clickPos[1] - 1; y > playerPos[1]; y--)
					{
						if(accessible[playerPos[0], y % 100])
						{
							MoveAndMine(playerPos[0], y % 100);
							return;
						}
					}
				}
			}
			//If we clicked over the player, find the nearest accessible block within the same row
			else
			{
				//Clicked to the left
				if(clickPos[0] < playerPos[0])
				{
					for(int x = clickPos[0]; x < playerPos[0]; x++)
						if(accessible[x, clickPos[1] % 100])
						{
							MoveAndMine(x, clickPos[1] % 100);
							return;
						}
				}
				//Clicked to the right
				else if(clickPos[0] > playerPos[0])
				{
					for(int x = clickPos[0]; x > playerPos[0]; x--)
						if(accessible[x, clickPos[1] % 100])
						{
							MoveAndMine(x, clickPos[1] % 100);
							return;
						}
				}
				//Clicked right over
				else
				{
					MoveAndMine(clickPos[0], playerPos[1] % 100 - 1);
					return;
				}
			}
		}
	}

	void MoveAndMine(int x, int y)
	{
		//If it's air, just move there
		if(blocks[x, y] == null)
		{
			Vector3 newPos;
			if(y < (currentDepth + 50) % 100)
				newPos = new Vector3(x, -y - depthMultiplicator * 100, 0);
			else
				newPos = new Vector3(x, -y - (depthMultiplicator - 1) * 100, 0);
			PlayerManager.Instance.MoveTo(newPos);
		}
		//If it's a block, move next to it and mine it
		else
		{
			int[] pos = GetAdjacentAirBlock(x, y);

			Vector3 newPos;
			if(pos[1] < (currentDepth + 50) % 100)
			{
				newPos = new Vector3(pos[0], -pos[1] - depthMultiplicator * 100, 0);
			}
			else
				newPos = new Vector3(pos[0], -pos[1] - (depthMultiplicator - 1) * 100, 0);
			PlayerManager.Instance.MoveTo(newPos);
			blocks[x, y].Mine();
		}
	}

	//Update the accessibility of the blocks
	void UpdateAccessible(int x, int y)
	{
		//If it's an air block, make all surrounding blocks accessible
		if(blocks[x, y] == null)
		{
			accessible[x, y] = true;
			if(y > maxDepth)
			{
				depthIndicator.text = "" + y;
				maxDepth = y;
			}
				
			//Left
			if(x != 0)
			{
				if(!accessible[x - 1, y])
					UpdateAccessible(x - 1, y);
			}

			//Right
			if(x != width - 1)
			{
				if(!accessible[x + 1, y])
					UpdateAccessible(x + 1, y);
			}

			//Up
			if(y != 0)
			{
				if(!accessible[x, y - 1])
					UpdateAccessible(x, y - 1);
			}
			else
			{
				if(!accessible[x, height - 1])
					UpdateAccessible(x, height - 1);
			}

			//Down
			if(y != height - 1)
			{
				if(!accessible[x, y + 1])
					UpdateAccessible(x, y + 1);
			}
			else
			{
				if(!accessible[x, 0])
					UpdateAccessible(x, 0);
			}
		}
		//If it's a solid block, just make this one accessible
		else
		{
			accessible[x, y] = true;
		}
	}

	int[] GetAdjacentAirBlock(int x, int y)
	{
		int[] pos = new int[2];

		//Under
		if(y != height - 1)
		{
			if(blocks[x, y + 1] == null && accessible[x, y + 1])
			{
				pos[0] = x;
				pos[1] = y + 1;
				return pos;
			}
		}
		else
		{
			if(blocks[x, 0] == null && accessible[x, 0])
			{
				pos[0] = x;
				pos[1] = 0;
				return pos;
			}
		}

		//Left
		if(x != 0)
		{
			if(blocks[x - 1, y] == null && accessible[x - 1, y])
			{
				pos[0] = x - 1;
				pos[1] = y;
				return pos;
			}
		}

		//Right
		if(x != 6)
		{
			if(blocks[x + 1, y] == null && accessible[x + 1, y])
			{
				pos[0] = x + 1;
				pos[1] = y;
				return pos;
			}
		}

		//Top
		pos[0] = x;
		if(y != 0)
			pos[1] = y - 1;
		else
			pos[1] = height - 1;
		return pos;
	}
}
