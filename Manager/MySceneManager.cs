using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MySceneManager : MonoBehaviour
{
    public GameObject playerPrefab;
    Transform playerSpawner;
    public static GameObject player;
    public static string savedScene;
    public static string currentScene;
    // public static string currentSubScene;
    public PlayerData playerData;
    SceneChanger _SceneChanger;
    public AudioMixerManager _AudioMixerManager;
    PlayerData data;

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
        // data = SaveAndLoad.LoadData();
        // _AudioMixerManager = GameObject.Find("UI:Audio").GetComponent<AudioMixerManager>();
        if (SaveAndLoad.playerData == null) return;
        savedScene = SaveAndLoad.playerData.SavedScene;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //When in main menu do the following
        if (scene.name == "MainMenu") 
        {
            // Debug.Log(data.SavedScene);
            currentScene = savedScene;
        }
        //outside main menu do the following
        else
        {
            playerSpawner = GameObject.Find("PlayerSpawner").GetComponent<Transform>();
            player = Instantiate(playerPrefab, playerSpawner.position, playerSpawner.rotation);
            currentScene = scene.name;
        }
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
