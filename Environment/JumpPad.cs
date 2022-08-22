using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    public float bounce = 20f;

    void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Rigidbody>().AddForce(transform.up * bounce, ForceMode.VelocityChange);
        }
    }
}
