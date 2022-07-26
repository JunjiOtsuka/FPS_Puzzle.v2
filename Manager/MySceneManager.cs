using UnityEngine;
using UnityEngine.SceneManagement;

public class MySceneManager : MonoBehaviour
{
    public GameObject playerPrefab;
    Transform playerSpawner;
    public static GameObject player;

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        playerSpawner = GameObject.Find("PlayerSpawner").GetComponent<Transform>();
        player = Instantiate(playerPrefab, playerSpawner.position, playerSpawner.rotation);
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
