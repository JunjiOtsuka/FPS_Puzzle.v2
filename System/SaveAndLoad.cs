using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveAndLoad : MonoBehaviour
{
    public static PlayerData playerData;
    public static PlayerData data;

    private static string path = "";
    private static string persistentPath = "";

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        playerData = new PlayerData();

        SetPaths();

        if (File.Exists(path))
        {
            LoadData();
        }
    }

    private static void SetPaths()
    {
        path = Application.dataPath + Path.AltDirectorySeparatorChar + "SaveData.json";
        persistentPath = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "SaveData.json";
    }

    public void UpdatePlayerData()
    {
        SaveData();
    }

    public static void SaveData()
    {
        string savePath = path; //saves in asset folder
        // string savePath = persistentPath;

        Debug.Log("Saving Data at " + savePath);
        string json = JsonUtility.ToJson(playerData);
        Debug.Log(json);

        using (StreamWriter writer = new StreamWriter(savePath))
        {
            writer.Write(json);
        }
    }

    public static PlayerData LoadData()
    {
        if (path == "") return null;
        
        using (StreamReader reader = new StreamReader(path))
        // using (StreamReader reader = new StreamReader(persistentPath))
        {
            string json = reader.ReadToEnd();

            data = JsonUtility.FromJson<PlayerData>(json);
            Debug.Log(data.masterVolume);
        }

        return data;
    }

    // void OnDisable()
    // {
    //     if (MySceneManager.currentScene == "MainMenu") return;
    //     SaveData();
    // }
}
