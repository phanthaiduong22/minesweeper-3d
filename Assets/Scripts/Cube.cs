using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// There are four state of cube:
// isClick = -1: Showed cube ->  not clickable
// isClick = 0: Hasn't showed cube -> clickable to showed
// isClick = 1: Cube is showing (run ActivateNearbyBombs func)
// isClick = 2: Cube is flaged -> not clickable, need to remove flag to return isClick = 0;

public class Cube : MonoBehaviour, IMouse
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
		// transform.Rotate(new Vector3(90f, 90f, 90f));


	}
	// public GamePlay GamePlay;

	void Update()
	{
		renderer = GetComponent<Renderer>();
		// transform.Rotate(Vector3.down, rotationRate * deltaTime);

	}
	private void ClickCube(GameObject gameObject)
	{
		Cube cube = gameObject.GetComponent<Cube>();
		if (cube.isClicked == 0)
		{
			if (cube.isBomb == 1)
			{
				// GameOver
				print("Game Over!!! Haha");
				cube.isClicked = 1;

			}
			else
			{
				// Open nearby boxes;
				cube.isClicked = 1;
			}
		}

	}

	private void FlagCube(GameObject gameObject)
	{
		Cube cube = gameObject.GetComponent<Cube>();
		if (cube.isClicked == 0)
		{
			cube.isClicked = 2;
		}
		else if (cube.isClicked == 2)
		{
			cube.isClicked = 0;
		}


	}
	private void OnMouseEnter()
	{
		// if (isClicked == 0)
		// {
		// 	if (isBomb == 1)
		// 	{
		// 		renderer.material.color = Color.red;
		// 	}
		// 	else
		// 	{
		// 		renderer.material.color = Color.green;
		// 	}
		// }

		// Print Logs
		PrintState();
		// Hover animation
		if (isClicked == 0)
			renderer.material.color = Color.grey;

	}

	private void OnMouseExit()
	{
		if (isClicked == 0)
		{
			renderer.material.color = Color.white;
		}
		else if (isClicked == 2)
		{
			renderer.material.color = Color.yellow;
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

	public void OnRightMouseDown(InputAction.CallbackContext context)
	{
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

		if (Physics.Raycast(ray, out hit, 100.0f))
		{
			if (hit.transform != null)
			{
				FlagCube(hit.transform.gameObject);
			}
		}
	}

	public void OnRightMouseDrag(InputAction.CallbackContext context)
	{

	}

	public void OnRightMouseUp(InputAction.CallbackContext context)
	{

	}

	public void OnLeftMouseUp(InputAction.CallbackContext context)
	{

	}

	public void OnLeftMouseDown(InputAction.CallbackContext context)
	{
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

		if (Physics.Raycast(ray, out hit, 100.0f))
		{
			if (hit.transform != null)
			{
				ClickCube(hit.transform.gameObject);
			}
		}
	}

	public void OnLeftMouseDrag(InputAction.CallbackContext context)
	{

	}
}
