using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    GameObject player;

    void Awake()
    {
        player = GameObject.Find("Player");
        spawnPlayerAtLocation();
    }

    void spawnPlayerAtLocation(){
        player.transform.position = transform.position;
        player.transform.rotation = transform.rotation;
    }
}
