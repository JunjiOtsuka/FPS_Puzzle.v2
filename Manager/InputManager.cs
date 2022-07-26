using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static PlayerInputAction inputActions = new PlayerInputAction();
    public static event Action<InputActionMap> actionMapChange;

    public static void ToggleActionMap (InputActionMap actionMap) 
    {
        if (actionMap.enabled) return;

        inputActions.Disable();
        actionMapChange?.Invoke(actionMap);
        actionMap.Enable();
    } 
    
    private void OnEnable()
    {
        InputManager.inputActions.Enable();
    }

    private void OnDisable() 
    {
        InputManager.inputActions.Disable();
    }
}
