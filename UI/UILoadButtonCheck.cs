using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILoadButtonCheck : MonoBehaviour
{
    public GameObject loadButton;
    PlayerData data;
    // Start is called before the first frame update
    void Start()
    {
        data = SaveAndLoad.LoadData();
        SaveFileChecker();
    }

    private void SaveFileChecker()
    {
        if (data == null) loadButton.SetActive(false);
    }
}
