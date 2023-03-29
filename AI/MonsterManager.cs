using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    public List<GameObject> prefabs = new List<GameObject>();
    private List<Stats> stats = new List<Stats>();
    
    // Start is called before the first frame update
    void Start()
    {
        //interate prefabs list
        foreach (GameObject prefab in prefabs)
        {
            //get all prefab stats in stats list
            stats.Add(prefab.GetComponent<Stats>());
        }
        //iterate through stats list
        foreach (Stats stat in stats)
        {
            //set monster data in dictionary
            stat.SetMonsterData();
        }
    }
}
