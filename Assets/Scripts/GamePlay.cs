using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class GamePlay : MonoBehaviour
{
	public GameObject cubeOriginal;
	public GameOverScreen gameOverScreen;
	public Transform BombCounter;

	int rows;
	int cols;
	int heights;
	int nBombs;
	int[] dx = { -1, -1, -1, 0, 0, 0, 1, 1, 1, -1, -1, -1, 0, 0, 0, 1, 1, 1, -1, -1, -1, 0, 0, 0, 1, 1, 1 };
	int[] dy = { -1, 0, 1, -1, 0, 1, -1, 0, 1, -1, 0, 1, -1, 0, 1, -1, 0, 1, -1, 0, 1, -1, 0, 1, -1, 0, 1 };
	int[] dz = { -1, -1, -1, -1, -1, -1, -1, -1, -1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
	Cube[,,] cubesArray = new Cube[50, 50, 50];
	bool firstClick;
	Text bombCounter;
	int nFlags;

	void Start()
	{
		firstClick = false;
		switch (GameValues.Difficulty)
		{
			case GameValues.Difficulties.Easy:
				rows = 5;
				cols = 5;
				heights = 5;
				nBombs = 10;
				break;
			case GameValues.Difficulties.Medium:
				rows = 7;
				cols = 7;
				heights = 7;
				nBombs = 30;
				break;
			case GameValues.Difficulties.Hard:
				rows = 10;
				cols = 10;
				heights = 10;
				nBombs = 90;
				break;
		}
		CreateCubes(rows, cols, heights);
		bombCounter = BombCounter.GetComponent<Text>();
		bombCounter.text = nBombs.ToString();
		nFlags = 0;

	}

	void Update()
	{
		CheckClicked();
		if (Cube.GetFlags() != nFlags)
		{
			nFlags = Cube.GetFlags();
		}
		bombCounter.text = (nBombs - nFlags).ToString();
		if (nFlags == Cube.GetCorrect() && nFlags == nBombs && firstClick)
		{
			FindObjectOfType<AudioManager>().Play("Win");
			gameOverScreen.SetUp("YOU WIN!!!");
			firstClick = false;
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
					if (cubesArray[i, j, k].isClicked == 1)
					{
						ActivateCube(new Vector3Int(i, j, k));

					}
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
					cubeCloneComponent.x = i;
					cubeCloneComponent.y = j;
					cubeCloneComponent.z = k;
					cubesArray[i, j, k] = cubeCloneComponent;
				}
			}
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
				gameOverScreen.SetUp("GAME OVER");
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
