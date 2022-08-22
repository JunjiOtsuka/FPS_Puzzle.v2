using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIController : MonoBehaviour
{
    public static InputAction UINavigate;
    public static InputAction UIExit;
    public static PlayerInput _PlayerInput;
    public UICanvasManager _UICanvasManager;

    private void Awake()
    {
        _PlayerInput = GetComponent<PlayerInput>();   

        //movement related variables
        UIExit = InputManager.inputActions.UI.Exit;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerActionMapManager.playerInput.currentActionMap.name != "UI") 
        {
            return;
        }
        if (UIExit.WasPerformedThisFrame())
        {
            _UICanvasManager.DisableOptionUI();
        }
    }
}
