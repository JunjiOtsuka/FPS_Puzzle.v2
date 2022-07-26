using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChanger : MonoBehaviour
{
    public GameObject loadingScreen;

    public string sceneToLoad;

    GameObject player;

    /*Loading Bar Slider*/
    Slider slider;

    void Awake()
    {
        slider = loadingScreen.GetComponentInChildren<Slider>();
    }

    void Start()
    {
        player = GameObject.Find("Player");
    }

    public void OnPlayButtonClicked()
    {
        StartCoroutine(LoadYourAsyncScene(sceneToLoad));
    }

    public void OnQuitButtonClicked()
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

    IEnumerator LoadYourAsyncScene(string sceneToLoad)
    {
        // Set the current Scene to be able to unload it later
        Scene currentScene = SceneManager.GetActiveScene();

        //Set the current Scene Name variable to keep track of where the player came from

        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.
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
