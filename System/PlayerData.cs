using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerData
{
    public static PlayerData data;
    public string SavedScene;
    public float masterVolume;
    public float musicVolume;
    public float sfxVolume;

    public PlayerData()
    {
    }

    // public PlayerData(string currentScene)
    // {
    //     this.SavedScene = MySceneManager.currentScene;
    // }

    // public PlayerData(Dictionary<string, float> dict)
    // {
    //     this.masterVolume = dict["MasterVolume"];
    //     this.musicVolume = dict["MusicVolume"];
    //     this.sfxVolume = dict["SFXVolume"];
    // }
}
