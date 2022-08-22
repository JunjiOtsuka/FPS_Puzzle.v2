using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UICanvasManager : MonoBehaviour
{
    public EventSystem _events; 
    public GameObject parentOfKeybinds;
    public List<GameObject> UIList;
    public static float loadedKeybinds;
    bool KeybindsLoaded = false;

    void OnEnable()
    {
        UIEnableButtons("UI:Character");
        if (loadedKeybinds >= parentOfKeybinds.transform.childCount)
        {
            DisableOptionUI();
        }
    }

    public void UIEnableButtons(string EnableGameObject)
    {
        foreach(GameObject UI in UIList) 
        {
            if (UI.name == EnableGameObject) 
            {
                UI.SetActive(true);
            }
        }
    }

    public void DisableOptionUI()
    {
        foreach(GameObject UI in UIList) 
        {
            UI.SetActive(false);
        }
        PlayerActionMapManager.ToggleActionMap(InputManager.inputActions.Player, "Player");
        InputManager.inputActions.Player.Enable();
        InputManager.inputActions.UI.Disable();
    }

    // void OnDisable()
    // {
    //     PlayerActionMapManager.ToggleActionMap(InputManager.inputActions.Player, "Player");
    //     InputManager.inputActions.Player.Enable();
    // }
}
