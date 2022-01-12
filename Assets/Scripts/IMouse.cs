using UnityEngine.InputSystem;

public interface IMouse
{
    void OnRightMouseDown(InputAction.CallbackContext context);
    void OnRightMouseDrag(InputAction.CallbackContext context);
    void OnRightMouseUp(InputAction.CallbackContext context);
    void OnLeftMouseUp(InputAction.CallbackContext context);
    void OnLeftMouseDown(InputAction.CallbackContext context);
    void OnLeftMouseDrag(InputAction.CallbackContext context);
}
