using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;


public class CharacterController : MonoBehaviour, IMouse, IPointerEnterHandler, IPointerExitHandler
{
	private bool stepOn = true;
	public int blood = 5;
	private int blood_sum;

	private NavMeshAgent mMeshAgent;

	private Brick mPreviousBrick;

	private Brick mCurrentBrick;

	public GameOverController GameOver;

	public HealthBarController HealthBar;

	Color color;


	// Start is called before the first frame update
	void Start()
	{
		blood_sum = blood;
		GetComponent<Renderer>().material.SetColor("_Color", Color.green);
		mMeshAgent = GetComponent<NavMeshAgent>();

		color = GetComponent<Renderer>().material.color;
	}

	// Update is called once per frame
	void Update()
	{
		if (blood == 0)
		{
			GameOver.Setup();
		}
		// else
		// {
		// 	if (Input.GetMouseButtonDown(0))
		// 	{
		// 		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		// 		RaycastHit hit;
		// 		if (Physics.Raycast(ray, out hit))
		// 		{
		// 			GameObject hitObject = hit.transform.gameObject;
		// 			Brick brick = hitObject.GetComponent<Brick>();
		// 			if (brick != null)
		// 			{
		// 				mMeshAgent.SetDestination(hit.transform.position);
		// 			}
		// 		}
		// 	}

		// 	DetectMine();
		// }
	}


	public void OnRightMouseDown(InputAction.CallbackContext context)
	{
		print("OnRightMouseDown");

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
		print("OnLeftMouseDown");
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit))
		{
			GameObject hitObject = hit.transform.gameObject;
			Brick brick = hitObject.GetComponent<Brick>();
			if (brick != null)
			{
				mMeshAgent.SetDestination(hit.transform.position);
			}
		}
		DetectMine();
	}
	public void OnLeftMouseDrag(InputAction.CallbackContext context)
	{

	}

	private void ColorChange(int value)
	{
		Color[] colors = { Color.red, Color.magenta, Color.yellow, Color.blue, Color.green };
		// Color[] colors = {Color.red};
		if (value > 0)
		{
			GetComponent<Renderer>().material.SetColor("_Color", colors[value]);
		}
	}
	private void DetectMine()
	{
		Ray ray = new Ray(transform.position, -transform.up);
		RaycastHit hit;
		if (Physics.SphereCast(ray, 0.2f, out hit))
		{
			GameObject hitObject = hit.transform.gameObject;
			Brick brick = hitObject.GetComponent<Brick>();
			if (brick != null)
			{
				brick.ShowSecret();

				if (brick.mine && mPreviousBrick != null && stepOn)
				{
					mMeshAgent.SetDestination(mPreviousBrick.transform.position);
					blood -= 1;
					stepOn = false;
					// ColorChange(blood);
					HealthBar.updateHealth((float)blood / (float)blood_sum);
				}
				else
				if (!brick.mine)
				{
					stepOn = true;
				}

				if (brick != mCurrentBrick)
				{
					mPreviousBrick = mCurrentBrick;
					mCurrentBrick = brick;
				}
			}
		}
	}
	// private void OnMouseEnter()
	// {
	//     GetComponent<Renderer>().material.SetColor("_Color", Color.red);
	// }
	// private void OnMouseExit()
	// {
	//     GetComponent<Renderer>().material.SetColor("_Color", Color.green);
	// }

	public void OnPointerEnter(PointerEventData eventData)
	{
		GetComponent<Renderer>().material.color = new Color(color.r, color.g, color.b, color.a / 2);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		GetComponent<Renderer>().material.color = color;
	}
}
