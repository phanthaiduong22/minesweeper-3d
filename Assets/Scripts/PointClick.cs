using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class PointClick : MonoBehaviour, IMouse
{
	public float sensitivity = .001f;
	public Transform eventSystem;

	Vector3 angles;
	Mouse mouse;

	PlayerInput playerInput;
	InputAction drag;
	Transform center;
	Transform cam;
	EventSystem esystem;

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
		esystem = eventSystem.GetComponent<EventSystem>();
	}

	void OnDisable()
	{
		InputActionMap map = playerInput.currentActionMap;

		InputAction rightClick = map.FindAction("RightClick", true);
		rightClick.started -= OnRightClickStarted;
		rightClick.canceled -= OnRightClickCanceled;

		InputAction leftClick = map.FindAction("LeftClick", true);
		leftClick.started -= OnLeftClickStarted;
		leftClick.canceled -= OnLeftClickCanceled;

		InputAction scroll = map.FindAction("Scroll", true);
		scroll.started -= OnMouseScroll;
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
		if (esystem)
			esystem.enabled = false;
		drag.performed += currentDragger.OnLeftMouseDrag;
		// }
	}

	public void OnLeftClickCanceled(InputAction.CallbackContext context)
	{
		drag.performed -= currentDragger.OnLeftMouseDrag;
		if (esystem)
			esystem.enabled = true;
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
		if (esystem)
			esystem.enabled = false;
		drag.performed += currentDragger.OnRightMouseDrag;
		// }
	}

	public void OnRightClickCanceled(InputAction.CallbackContext context)
	{
		drag.performed -= currentDragger.OnRightMouseDrag;
		if (esystem)
			esystem.enabled = true;
		currentDragger.OnRightMouseUp(context);
	}

	public void OnRightMouseDrag(InputAction.CallbackContext context)
	{
		Vector2 delta = sensitivity * context.ReadValue<Vector2>();

		angles.x += delta.y;
		angles.y += delta.x;

		if (this)
		{
			center = transform.Find("Center");
			center.localEulerAngles = angles;
		}
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
		if (this)
		{
			Vector2 delta = sensitivity * context.ReadValue<Vector2>();
			Vector3 pos = cam.localPosition;
			pos.x = Mathf.Clamp(pos.x - delta.x, -10f, 10f);
			pos.y = Mathf.Clamp(pos.y - delta.y, -8f, 8f);
			//pos.x -= delta.x;
			//pos.y -= delta.y;
			cam.localPosition = pos;
		}

	}

	public void OnMouseScroll(InputAction.CallbackContext context)
	{
		if (this)
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
}