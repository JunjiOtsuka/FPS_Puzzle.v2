using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    //Modify Wave
    public string m_WaveNumber;
    public List<GameObject> NewWave;

    [Serializable]
    public class WaveDictionary : SerializableDictionary<string, List<GameObject>>{}

    public WaveDictionary WaveTable;
    public List<GameObject> CurrentWave = new List<GameObject>();

    [HideInInspector]
    public AISpawner AISpawner;
    [HideInInspector]
    public GameObject GoblinPrefab;
    [HideInInspector]
    public GameObject TrollPrefab;
    [HideInInspector]
    public GameObject OgrePrefab;
    [HideInInspector]
    public GameObject prefab;
    public CooldownManager m_CooldownManager = new CooldownManager();

    // Update is called once per frame
    void Update()
    {
        SetSpawnerPrefab();
    }
    
    void SetSpawnerPrefab()
    {
        if (CurrentWave.Count == 0) return;

        m_CooldownManager.SetCDTimer(1.0f);
        m_CooldownManager.StartCDTimer(m_CooldownManager);

        if (m_CooldownManager.bCDEnd())
        {
            prefab = CurrentWave.First();
            CurrentWave.RemoveAt(0);
            SpawnMonster();
            m_CooldownManager.ResetCDTimer();
        }
    }

    void SpawnMonster()
    {
        AISpawner.SpawnMonster(prefab);
    }

    public void AddGoblin()
    {
        NewWave.Add(GoblinPrefab);
    }

    public void AddTroll()
    {
        NewWave.Add(TrollPrefab);
    }

    public void AddOgre()
    {
        NewWave.Add(OgrePrefab);
    }

    // public void CreateWave()
    // {
    //     List<GameObject> NewWave = new List<GameObject>();
    // }

    //Add
    public void AddWave()
    {
        if (WaveTable.ContainsKey("Wave" + m_WaveNumber)) {
            Debug.Log("Contains key");
            return;
        }
        if (m_WaveNumber == null) return;
        Debug.Log("Added");

        var CopyNewWave = NewWave.ToList();

        WaveTable.Add("Wave" + m_WaveNumber, CopyNewWave);
        m_WaveNumber = "";
        NewWave.Clear();
    }

    //Get
    public void GetWave()
    {
        if (!WaveTable.ContainsKey("Wave" + m_WaveNumber)) {
            Debug.Log("Error: Doesn't contains key");
            return;
        }
        CurrentWave = WaveTable["Wave" + m_WaveNumber];
        Debug.Log(CurrentWave[0].name);
    }

    //Remove
    public void RemoveWave()
    {
        if (!WaveTable.ContainsKey("Wave" + m_WaveNumber)) {
            Debug.Log("Error: Doesn't contains key");
            return;
        }
        Debug.Log("Removed");
        WaveTable.Remove("Wave" + m_WaveNumber);
    }
}
