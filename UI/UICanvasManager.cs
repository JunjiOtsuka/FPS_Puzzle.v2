using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UICanvasManager : MonoBehaviour
{
    public EventSystem _events; 
    public List<GameObject> UIList;

    void OnEnable()
    {
        foreach(GameObject UI in UIList) 
        {
            UI.SetActive(false);
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
}
