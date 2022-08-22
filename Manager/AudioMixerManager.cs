using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioMixerManager : MonoBehaviour
{
    public static float masterVolume { get; private set; }
    public static float musicVolume { get; private set; }
    public static float sfxVolume { get; private set; }
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;
    [SerializeField] private AudioMixer audioMixer;
    public static Dictionary<string, float> audioData = new Dictionary<string, float>();
    public PlayerData playerData;
    SaveAndLoad _SaveAndLoad;

    void Start()
    {
        LoadVolumeSettings();
        UpdateVolumeUI();
        //Adds a listener to the main slider and invokes a method when the value changes.
        masterSlider.onValueChanged.AddListener(delegate {ValueChangeCheck("MasterVolume"); });
        musicSlider.onValueChanged.AddListener(delegate {ValueChangeCheck("MusicVolume"); });
        sfxSlider.onValueChanged.AddListener(delegate {ValueChangeCheck("SFXVolume"); });
        _SaveAndLoad = GameObject.Find("SaveLoadData").GetComponent<SaveAndLoad>();
    }

    // Invoked when the value of the slider changes.
    public void ValueChangeCheck(string mixerName)
    {
        if (mixerName == "MasterVolume") SetVolume(mixerName, masterSlider.value);
        if (mixerName == "MusicVolume") SetVolume(mixerName, musicSlider.value);
        if (mixerName == "SFXVolume") SetVolume(mixerName, sfxSlider.value);
    }

    public void SetVolume(string mixerName, float sliderValue)
    {
        audioMixer.SetFloat(mixerName, Mathf.Log10(sliderValue) * 20);
        SaveVolumeSettings();
    }

    public void SaveVolumeSettings()
    {
        if (SaveAndLoad.playerData == null) return;
        
        SetFloat("MasterVolume", masterSlider.value);
        SetFloat("MusicVolume", musicSlider.value);
        SetFloat("SFXVolume", sfxSlider.value);

        //Save to SaveAndLoad script
        SaveAndLoad.playerData.masterVolume = GetFloat("MasterVolume", 0.5f);
        SaveAndLoad.playerData.musicVolume = GetFloat("MusicVolume", 0.5f);
        SaveAndLoad.playerData.sfxVolume = GetFloat("SFXVolume", 0.5f);
    }

    public void LoadVolumeSettings()
    {
        PlayerData data = SaveAndLoad.LoadData();

        if (data == null) return;

        Debug.Log(data);
        Debug.Log(audioMixer);

        //Loading data from _SaveAndLoad script
        audioMixer.SetFloat("MasterVolume",  Mathf.Log10(data.masterVolume) * 20);
        audioMixer.SetFloat("MusicVolume",  Mathf.Log10(data.musicVolume) * 20);
        audioMixer.SetFloat("SFXVolume",  Mathf.Log10(data.sfxVolume) * 20);
        // audioMixer.SetFloat("MasterVolume",  Mathf.Log10(GetFloat("MasterVolume", 0.5f)) * 20);
        // audioMixer.SetFloat("MusicVolume",  Mathf.Log10(GetFloat("MusicVolume", 0.5f)) * 20);
        // audioMixer.SetFloat("SFXVolume",  Mathf.Log10(GetFloat("SFXVolume", 0.5f)) * 20);
    }

    //without this function audioMixer is set to default volume
    public void UpdateVolumeUI()
    {
        PlayerData data = SaveAndLoad.LoadData();
        if (data == null) return;

        masterSlider.value = data.masterVolume;
        musicSlider.value = data.musicVolume;
        sfxSlider.value = data.sfxVolume;
        // masterSlider.value = GetFloat("MasterVolume", 0.5f);
        // musicSlider.value = GetFloat("MusicVolume", 0.5f);
        // sfxSlider.value = GetFloat("SFXVolume", 0.5f);
    }

    public static void SetFloat(string key, float value)
    {
        if (!audioData.ContainsKey(key)) audioData.Add(key, value);
        
        audioData[key] = value;
    }

    public static float GetFloat(string key, float? value)
    {
        return !audioData.ContainsKey(key) ? (float)value : audioData[key];
    }

    void OnDisable()
    {
        // PlayerData.data.audioData = audioData;
        SaveVolumeSettings();
    }
}
