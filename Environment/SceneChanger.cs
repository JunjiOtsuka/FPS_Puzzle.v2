using TMPro;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChanger : MonoBehaviour
{
    public GameObject loadingScreen;

    public string sceneToLoad;

    public TMP_InputField inputField;

    GameObject player;

    /*Loading Bar Slider*/
    Slider slider;
    [Header("Reset Game State")]
    [SerializeField] private Button restartButton;
    [Header("Quit Game")]
    [SerializeField] private Button quitButton;

    void Awake()
    {
        slider = loadingScreen.GetComponentInChildren<Slider>();
    }

    void Start()
    {
        player = GameObject.Find("Player");
        // _SaveAndLoad = GameObject.Find("SaveLoadData").GetComponent<SaveAndLoad>();
    }

    public void OnPlayButtonClicked()
    {
        StartCoroutine(LoadYourAsyncScene(sceneToLoad));
    }

    public void OnNewGameButton()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void OnLevelButton()
    {
        SceneManager.LoadScene("Level" + inputField.text);
    }


    public void OnSaveButton()
    {
        if (MySceneManager.currentScene == "MainMenu") return;
        SaveAndLoad.playerData.SavedScene = MySceneManager.currentScene;
        SaveAndLoad.SaveData();
    }

    public void OnLoadButton()
    {
        PlayerData data = SaveAndLoad.LoadData();
        Debug.Log(SaveAndLoad.data.SavedScene);
        SceneManager.LoadScene(data.SavedScene);
    }

    public void DoRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

        public void DoMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void DoQuit()
    {
        Application.Quit();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            StartCoroutine(LoadYourAsyncScene(sceneToLoad));
        }
    }

    public IEnumerator LoadYourAsyncScene(string sceneToLoad)
    {
        Scene currentScene = SceneManager.GetActiveScene();

        loadingScreen.SetActive(true);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            float loadPercentage = Mathf.Clamp01(asyncLoad.progress / 0.9f);

            slider.value = loadPercentage;

            yield return null;
        }

        loadingScreen.SetActive(false);

        SceneManager.MoveGameObjectToScene(player, SceneManager.GetSceneByName(sceneToLoad));
        
        if (currentScene.name != null)
        {
            SceneManager.UnloadSceneAsync(currentScene.name);
        }
    }
}
