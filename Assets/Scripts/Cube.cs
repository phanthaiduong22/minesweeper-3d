using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

// There are four state of cube:
// isClick = -1: Showed cube ->  not clickable
// isClick = 0: Hasn't showed cube -> clickable to showed
// isClick = 1: Cube is showing (run ActivateNearbyBombs func)
// isClick = 2: Cube is flaged -> not clickable, need to remove flag to return isClick = 0;

public class Cube : MonoBehaviour, IMouse, IPointerEnterHandler, IPointerExitHandler
{
	private new Renderer renderer;
	// public GameObject gamePlay;
	public int isBomb;
	public int x;
	public int y;
	public int z;
	public int isClicked;
	public MeshRenderer meshRender;
	public CubeText cubeText;
	// public GameOverScreen gameOverScreen;

	Color color;
	void Start()
	{
		// this.GetComponent<MeshRenderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 0.1f);
		// gameObject.GetComponent<Renderer>().material.color.a = 0;
		renderer = GetComponent<Renderer>();
		color = renderer.material.color;
	}

	void Update()
	{

		// transform.Rotate(new Vector3(0f, 100f, 0f) * Time.deltaTime);
	}
	private void ClickCube(GameObject gameObject)
	{
		Cube cube = gameObject.GetComponent<Cube>();
		if (cube.isClicked == 0)
		{
			if (cube.isBomb == 1)
			{
				// GameOver
				cube.DisplayBomb();
				print("Game Over!!! Haha");
				cube.isClicked = 3;

				// gameOverScreen.SetUp();

			}
			else
			{
				// Open nearby boxes;
				cube.isClicked = 1;
			}
		}

	}

	private void DisplayBomb()
    {
		Transform mine = transform.Find("mine");
		SpriteRenderer mineRenderer = mine.GetComponent<SpriteRenderer>();
		mineRenderer.enabled = true;
		MeshRenderer mRenderer = GetComponent<MeshRenderer>();
		mRenderer.enabled = false;
		mine.transform.LookAt(Camera.main.transform.position);
		mine.transform.rotation = (Camera.main.transform.rotation);
	}

	private void FlagCube(GameObject gameObject)
	{
		Cube cube = gameObject.GetComponent<Cube>();
		if (cube.isClicked == 0)
		{
			cube.isClicked = 2;
			cube.FlagColor(true);
		}
		else if (cube.isClicked == 2)
		{
			cube.isClicked = 0;
			cube.FlagColor(false);
		}


	}

	public void DisplayBox(int cntBombs)
	{
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

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (isClicked == 0)
			renderer.material.color = new Color(color.r, color.g, color.b, color.a / 2);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (isClicked == 0)
			renderer.material.color = color;
	}

	public void FlagColor(bool flag)
    {
		if (flag)
        {
			renderer.material.color = Color.yellow;
        }
		else
        {
			renderer.material.color = color;
        }
    }
}
