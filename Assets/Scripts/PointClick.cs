using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PointClick : MonoBehaviour, IMouse
{
	public float sensitivity = .1f;

	Vector3 angles;
	Mouse mouse;

	PlayerInput playerInput;
	InputAction drag;
	Transform center;
	Transform cam;

	IMouse currentDragger;

	void Start()
	{
		playerInput = GetComponent<PlayerInput>();

		InputActionMap map = playerInput.currentActionMap;

		InputAction rightClick = map.FindAction("RightClick", true);
		rightClick.started += OnRightClickStarted;
		rightClick.canceled += OnRightClickCanceled;

		InputAction leftClick = map.FindAction("LeftClick", true);
		leftClick.started += OnLeftClickStarted;
		leftClick.canceled += OnLeftClickCanceled;

		InputAction scroll = map.FindAction("Scroll", true);
		scroll.started += OnMouseScroll;

		drag = map.FindAction("Drag", true);

		angles = transform.localEulerAngles;
		center = transform.Find("Center");
		cam = center.Find("Camera");
		mouse = Mouse.current;
	}

	public void OnLeftClickStarted(InputAction.CallbackContext context)
	{
		RaycastHit hit;
		// if (mouse.clickCount.ReadValue() == 1)
		// {
		currentDragger = this;

		if (Physics.Raycast(
				Camera.main.ScreenPointToRay(
					Mouse.current.position.ReadValue()), out hit))
		{
			IMouse dragger = hit.collider.gameObject.GetComponent<IMouse>();

			if (dragger != null)
			{
				currentDragger = dragger;
			}
		}
		currentDragger.OnLeftMouseDown(context);
		drag.performed += currentDragger.OnLeftMouseDrag;
		// }
	}

	public void OnLeftClickCanceled(InputAction.CallbackContext context)
	{
		drag.performed -= currentDragger.OnLeftMouseDrag;
		currentDragger.OnLeftMouseUp(context);
	}

	public void OnRightClickStarted(InputAction.CallbackContext context)
	{
		RaycastHit hit;
		// if (mouse.clickCount.ReadValue() == 1)
		// {
		currentDragger = this;

		if (Physics.Raycast(
				Camera.main.ScreenPointToRay(
					Mouse.current.position.ReadValue()), out hit))
		{
			IMouse dragger = hit.collider.gameObject.GetComponent<IMouse>();

			if (dragger != null)
			{
				currentDragger = dragger;
			}
		}

		currentDragger.OnRightMouseDown(context);
		drag.performed += currentDragger.OnRightMouseDrag;
		// }
	}

	public void OnRightClickCanceled(InputAction.CallbackContext context)
	{
		drag.performed -= currentDragger.OnRightMouseDrag;
		currentDragger.OnRightMouseUp(context);
	}

	public void OnRightMouseDrag(InputAction.CallbackContext context)
	{
		Vector2 delta = sensitivity * context.ReadValue<Vector2>();

		angles.x += delta.y;
		angles.y += delta.x;

		center = transform.Find("Center");
		center.localEulerAngles = angles;
	}

	public void OnRightMouseDown(InputAction.CallbackContext context)
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

	}

	public void OnLeftMouseDrag(InputAction.CallbackContext context)
	{
		Vector2 delta = sensitivity * context.ReadValue<Vector2>();
		Vector3 pos = cam.localPosition;
		pos.x -= delta.x;
		pos.y -= delta.y;
		cam.localPosition = pos;
	}

	public void OnMouseScroll(InputAction.CallbackContext context)
	{
		float delta = context.ReadValue<float>();
		if (delta > 0)
		{
			delta = -1f;
		}
		else if (delta < 0)
		{
			delta = 1f;
		}
		Camera camera = cam.GetComponent<Camera>();
		float fov = camera.fieldOfView;
		float target = Mathf.Clamp(fov + delta * 10f, 30f, 90f);
		camera.fieldOfView = Mathf.Lerp(fov, target, 10f * Time.deltaTime);
	}
}