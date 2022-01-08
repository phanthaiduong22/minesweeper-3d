using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
	private Renderer renderer;
	// public GameObject gamePlay;
	public int isBomb;
	public int x;
	public int y;
	public int z;
	public int isClicked;
	public MeshRenderer meshRender;
	public CubeText cubeText;
	// Start is called before the first frame update
	void Start()
	{
		// this.GetComponent<MeshRenderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 0.1f);
		// gameObject.GetComponent<Renderer>().material.color.a = 0;
	}
	// public GamePlay GamePlay;


	// Update is called once per frame
	void Update()
	{
		renderer = GetComponent<Renderer>();
		// transform.Rotate(new Vector3(0f, 100f, 0f) * Time.deltaTime);
		if (Input.GetMouseButtonDown(0))
		{
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast(ray, out hit, 100.0f))
			{
				if (hit.transform != null)
				{
					ClickCube(hit.transform.gameObject);
				}
			}
		}
	}
	private void ClickCube(GameObject gameObject)
	{
		Cube cube = gameObject.GetComponent<Cube>();

		if (cube.isBomb == 1)
		{
			// GameOver
			print("Game Over!!! Haha");
		}
		else
		{
			// Open nearby boxes;
			cube.isClicked = 1;
		}
	}
	private void OnMouseEnter()
	{
		if (isClicked == 0)
		{
			if (isBomb == 1)
			{
				renderer.material.color = Color.red;
			}
			else
			{
				renderer.material.color = Color.green;
			}
		}

	}
	private void OnMouseExit()
	{
		if (isClicked == 0)
		{
			renderer.material.color = Color.white;
		}
	}
	public void DisplayBox(int cntBombs)
	{
		if (isBomb == 1)
		{
			renderer.material.color = Color.red;
		}
		else
		{
			renderer.material.color = Color.green;
		}
		// meshRender.enabled = false;
		transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
		cubeText.MeshRenderEnable();
		cubeText.ChangeTextMesh(cntBombs);
	}

	public void PrintState()
	{
		print("isBomb: " + isBomb + " | isClicked: " + isClicked + " | (x, y, z): " + "(" + x + "," + y + "," + z + ")");
	}

	public void DestroyCube()
	{
		Destroy(gameObject);
	}
}
