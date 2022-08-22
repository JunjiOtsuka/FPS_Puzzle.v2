﻿using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
    public string nameOfNextLevel;
    Scene scene;

    void OnTriggerEnter(Collider other)
    {
        scene = SceneManager.GetActiveScene();
        StartCoroutine(LoadYourAsyncScene());
    }
    
    IEnumerator LoadYourAsyncScene()
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.

        // AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nameOfNextLevel);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene.buildIndex + 1);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
