#define USE_NEW_INPUT_SYSTEM

using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : Singleton<InputManager>
{
    private AutoGen_PlayerInputActions playerInputActions;
    
    protected override void Awake()
    {
        base.Awake();
        
        playerInputActions = new AutoGen_PlayerInputActions();
        playerInputActions.Player.Enable();
    }
    
    public Vector2 GetMouseScreenPosition()
    {
#if USE_NEW_INPUT_SYSTEM
        return Mouse.current.position.ReadValue();
#else
        return Input.mousePosition;
#endif
    }

    public bool WasPrimaryActionPerformedThisFrame()
    {
#if USE_NEW_INPUT_SYSTEM
        return playerInputActions.Player.PrimaryAction.WasPerformedThisFrame();
#else
        return Input.GetMouseButtonDown(0);
#endif
    }
    
    public bool WasSecondaryActionPerformedThisFrame()
    {
#if USE_NEW_INPUT_SYSTEM
        return playerInputActions.Player.SecondaryAction.WasPerformedThisFrame();
#else
        return Input.GetMouseButtonDown(1);
#endif
    }
}
