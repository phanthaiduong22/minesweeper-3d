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
	public int nBombs;
	public int randomProportion; // >= 2; Smaller number will be harder;
	public int[] dx = { -1, -1, -1, 0, 0, 0, 1, 1, 1, -1, -1, -1, 0, 0, 0, 1, 1, 1, -1, -1, -1, 0, 0, 0, 1, 1, 1 };
	public int[] dy = { -1, 0, 1, -1, 0, 1, -1, 0, 1, -1, 0, 1, -1, 0, 1, -1, 0, 1, -1, 0, 1, -1, 0, 1, -1, 0, 1 };
	public int[] dz = { -1, -1, -1, -1, -1, -1, -1, -1, -1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
	Cube[,,] cubesArray = new Cube[50, 50, 50];

	bool firstClick;
	void Start()
	{
		firstClick = false;
		// GameObject gameClone = Instantiate(cube);
		// cam = Camera.main;
		switch (GameValues.Difficulty)
		{
			case GameValues.Difficulties.Easy:
				rows = 5;
				cols = 5;
				heights = 5;
				nBombs = 5;
				break;
			case GameValues.Difficulties.Medium:
				rows = 10;
				cols = 10;
				heights = 10;
				nBombs = 30;
				break;
			case GameValues.Difficulties.Hard:
				rows = 20;
				cols = 20;
				heights = 20;
				nBombs = 60;
				break;
		}
		randomProportion = 10;
		CreateCubes(rows, cols, heights);

	}

	// Update is called once per frame
	void Update()
	{
		CheckClicked();
		if (Cube.GetFlags() == Cube.GetCorrect() && Cube.GetFlags() == nBombs)
        {
			print("Win");
        }
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
					//ActivateNearbyCubes(i, j, k);
					if (cubesArray[i, j, k].isClicked == 1)
						ActivateCube(new Vector3Int(i, j, k));
				}
			}
		}
	}

	void ActivateAllBombs()
    {
		for (int i = 0; i < rows; ++i)
		{
			for (int j = 0; j < cols; ++j)
			{
				for (int k = 0; k < heights; ++k)
				{
					if (cubesArray[i, j, k].isBomb == 1)
                    {
						cubesArray[i, j, k].DisplayBomb();
                    }

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

		if (cubesArray[x, y, z].isClicked == 3)
		{
			print("gameover from gameplay");
			ActivateAllBombs();
			FindObjectOfType<AudioManager>().Play("Lose");
			cubesArray[x, y, z].isClicked = -1;
			gameOverScreen.SetUp();
		}
		if (cubesArray[x, y, z].isClicked == 1)
		{
			cubesArray[x, y, z].isClicked = -1;
			if (cubesArray[x, y, z].isClicked == 1)
			{

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
					//SetBomb(cubeCloneComponent);
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

	void SetUpMatch(Vector3Int position)
    {
		HashSet<Vector3Int> notBombs = new HashSet<Vector3Int>();
		notBombs.Add(position);
		for (int i = 0; i < 27; ++i)
        {
			int x = position.x + dx[i];
			int y = position.y + dy[i];
			int z = position.z + dz[i];
			if (isValid(x, y, z) && (dx[i] != 0 || dy[i] != 0 || dz[i] != 0))
            {
				notBombs.Add(new Vector3Int(x, y, z));
            }
        }
		int temp = nBombs;
		while (temp != 0)
        {
			int x = Random.Range(0, rows);
			int y = Random.Range(0, cols);
			int z = Random.Range(0, heights);
			if (!notBombs.Contains(new Vector3Int(x, y, z)) && cubesArray[x, y, z].isBomb != 1)
            {
				cubesArray[x, y, z].isBomb = 1;
				temp--;
            }
        }
		CalculateAllCubes();
    }

	void CalculateAllCubes()
    {
		for (int i = 0; i < rows; ++i)
		{
			for (int j = 0; j < cols; ++j)
			{
				for (int k = 0; k < heights; ++k)
				{
					if (cubesArray[i, j, k].isBomb == 0)
					{
						int counter = CountNearbyBombs(i, j, k);
						cubesArray[i, j, k].SetBombs(counter);
					}

				}
			}
		}
	}

	void ActivateCube(Vector3Int position)
    {
		if (!firstClick)
        {
			firstClick = true;
			SetUpMatch(position);
        }
		Cube cube = cubesArray[position.x, position.y, position.z];
		if (cube.isClicked != -1 && cube.isClicked != 2)
        {
			if (cube.isBomb == 1)
            {
				ActivateAllBombs();
				FindObjectOfType<AudioManager>().Play("Lose");
				gameOverScreen.SetUp();
			}
			else
            {
				cube.Display();
				FindObjectOfType<AudioManager>().Play("Tick");
				if (cube.GetBombs() == 0)
                {
					ActivateAround(position);
                }
            }
        }
    }

	void ActivateAround(Vector3Int pos)
    {
		for (int i = 0; i < 27; ++i)
        {
			int x = pos.x + dx[i];
			int y = pos.y + dy[i];
			int z = pos.z + dz[i];
			if (isValid(x, y, z) && (dx[i] != 0 || dy[i] != 0 || dz[i] != 0))
            {
				ActivateCube(new Vector3Int(x, y, z));
            }
		}
    }
}
