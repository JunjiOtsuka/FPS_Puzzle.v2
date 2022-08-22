using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerActionMapManager : MonoBehaviour
{
    public static PlayerInput playerInput;
    public static event Action<InputActionMap> actionMapChange;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    public static void ToggleActionMap (InputActionMap actionMap, string nameOfActionMap) 
    {
        if (playerInput == null) return;

        playerInput.SwitchCurrentActionMap(nameOfActionMap);
        Debug.Log(playerInput.currentActionMap);

        if (actionMap.enabled) return;

        InputManager.inputActions.Disable();
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
