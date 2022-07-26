using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapFollowCam : MonoBehaviour
{
	public GameObject player;

	// void Awake()
	// {
    //     player = GameObject.Find("PlayerNewInput");
	// }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3 (player.transform.position.x, player.transform.position.y + 100, player.transform.position.z);
    }
}
