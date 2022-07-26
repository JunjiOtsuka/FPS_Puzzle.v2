using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UICanvasEnable : MonoBehaviour
{
    public EventSystem _events; 
    public GameObject firstSelected;

    void OnEnable()
    {
        _events.SetSelectedGameObject(firstSelected);
        firstSelected.GetComponent<Button>().Select();
    }
}
