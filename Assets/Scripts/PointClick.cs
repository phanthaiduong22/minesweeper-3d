using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PointClick : MonoBehaviour, IMouse
{
    public float sensitivity = .5f;

    Vector3 angles;
    Vector3 position;
    Mouse mouse;

    PlayerInput playerInput;
    InputAction drag;


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

        drag = map.FindAction("Drag", true);

        angles = transform.localEulerAngles;
        position = transform.localPosition;
        mouse = Mouse.current;
    }

    public void OnLeftClickStarted(InputAction.CallbackContext context)
    {
        RaycastHit hit;
        if (mouse.clickCount.ReadValue() == 1)
        {
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
        }
    }

    public void OnLeftClickCanceled(InputAction.CallbackContext context)
    {
        drag.performed -= currentDragger.OnLeftMouseDrag;
        currentDragger.OnLeftMouseUp(context);
    }

    public void OnRightClickStarted(InputAction.CallbackContext context)
    {
        RaycastHit hit;
        if (mouse.clickCount.ReadValue() == 1)
        {
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
        }
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

        Transform center = transform.Find("Center");
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
        Transform center = transform.Find("Center");
        Transform cam = center.Find("Camera");
        Vector3 pos = cam.localPosition;
        pos.x -= delta.x;
        pos.y -= delta.y;
        cam.localPosition = pos;
    }
}