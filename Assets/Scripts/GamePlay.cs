using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GamePlay : MonoBehaviour
{
	public GameObject cubeOriginal;


	public GameOverScreen gameOverScreen;
	// private Camera cam;
	// Start is called before the first frame update
	public int rows;
	public int cols;
	public int heights;
	public int randomProportion; // >= 2; Smaller number will be harder;
	public int[] dx = { -1, -1, -1, 0, 0, 0, 1, 1, 1, -1, -1, -1, 0, 0, 0, 1, 1, 1, -1, -1, -1, 0, 0, 0, 1, 1, 1 };
	public int[] dy = { -1, 0, 1, -1, 0, 1, -1, 0, 1, -1, 0, 1, -1, 0, 1, -1, 0, 1, -1, 0, 1, -1, 0, 1, -1, 0, 1 };
	public int[] dz = { -1, -1, -1, -1, -1, -1, -1, -1, -1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
	Cube[,,] cubesArray = new Cube[50, 50, 50];
	void Start()
	{
		// GameObject gameClone = Instantiate(cube);
		// cam = Camera.main;
		switch (GameValues.Difficulty)
		{
			case GameValues.Difficulties.Easy:
				rows = 5;
				cols = 5;
				heights = 5;
				break;
			case GameValues.Difficulties.Medium:
				rows = 10;
				cols = 10;
				heights = 10;
				break;
			case GameValues.Difficulties.Hard:
				rows = 20;
				cols = 20;
				heights = 20;
				break;
		}
		randomProportion = 10;
		CreateCubes(rows, cols, heights);

	}

	// Update is called once per frame
	void Update()
	{
		CheckClicked();
	}

	void CheckClicked()
	{
		for (int i = 0; i < rows; ++i)
		{
			for (int j = 0; j < cols; ++j)
			{
				for (int k = 0; k < heights; ++k)
				{
					// Activate nearby cubes
					ActivateNearbyCubes(i, j, k);

				}
			}
		}
	}

	bool isValid(int x, int y, int z)
	{
		if (x >= 0 && x < rows && y >= 0 && y < cols && z >= 0 && z < heights)
		{
			return true;
		}
		return false;
	}
	void ActivateNearbyCubes(int x, int y, int z)
	{
		if (cubesArray[x, y, z].isClicked == 1)
		{
			cubesArray[x, y, z].isClicked = -1;

			if (cubesArray[x, y, z].isBomb == 1)
			{
				FindObjectOfType<AudioManager>().Play("Lose");
				gameOverScreen.SetUp();
			}
			else
			{
				// Solution 0: Destroy Cube (deprecated)
				// cubesArray[x, y, z].DestroyCube();

				// Solution 1: scale down the Cube and scale up the Text
				int cntBombs = CountNearbyBombs(x, y, z);
				// print(cntBombs);
				cubesArray[x, y, z].DisplayBox(cntBombs);
				FindObjectOfType<AudioManager>().Play("Tick");
				for (int i = 0; i < 27; i++)
				{
					int x1 = x + dx[i], y1 = y + dy[i], z1 = z + dz[i];
					if (isValid(x1, y1, z1) && (dx[i] != 0 || dy[i] != 0 || dz[i] != 0) && cubesArray[x1, y1, z1].isClicked == 0 && cubesArray[x1, y1, z1].isBomb == 0 && CountNearbyBombs(x1, y1, z1) == 0)
					{
						cubesArray[x1, y1, z1].isClicked = 1;
						ActivateNearbyCubes(x1, y1, z1);
					}
				}
			}
		}
	}

	int CountNearbyBombs(int x, int y, int z) // calculate itself, but itself can not be a bomb
	{
		int ans = 0;
		for (int i = 0; i < 27; i++)
		{
			int x1 = x + dx[i], y1 = y + dy[i], z1 = z + dz[i];
			if (isValid(x1, y1, z1) && cubesArray[x1, y1, z1].isBomb == 1)
			{
				ans++;
			}
		}
		return ans;
	}
	void CreateCubes(int rows, int cols, int heights)
	{
		for (int i = 0; i < rows; i++)
		{
			for (int j = 0; j < cols; j++)
			{
				for (int k = 0; k < heights; k++)
				{
					GameObject cubeClone = Instantiate(cubeOriginal, new Vector3(i, j, k), cubeOriginal.transform.rotation);
					Cube cubeCloneComponent = cubeClone.GetComponent<Cube>();
					SetBomb(cubeCloneComponent);
					cubeCloneComponent.x = i;
					cubeCloneComponent.y = j;
					cubeCloneComponent.z = k;
					cubesArray[i, j, k] = cubeCloneComponent;
				}
			}
		}
	}

	void SetBomb(Cube cubeCloneComponent)
	{
		int isBomb = Random.Range(0, randomProportion);
		if (isBomb == 1)
		{
			cubeCloneComponent.isBomb = 1;
		}
		else
		{
			cubeCloneComponent.isBomb = 0;
		}
	}
}
