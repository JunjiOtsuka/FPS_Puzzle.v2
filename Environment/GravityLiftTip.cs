using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityLiftTip : MonoBehaviour
{
    public float liftForce = 20f;

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player"))
        {
            var rb = other.gameObject.GetComponent<Rigidbody>();
            rb.useGravity = false;
            rb.constraints = RigidbodyConstraints.FreezePositionY;
        }
    }

    void OnTriggerStay(Collider other) {
        if (other.gameObject.CompareTag("Player"))
        {
            var rb = other.gameObject.GetComponent<Rigidbody>();
            rb.useGravity = false;
            rb.constraints = RigidbodyConstraints.FreezePositionY;
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Player"))
        {
            var rb = other.gameObject.GetComponent<Rigidbody>();
            rb.useGravity = true;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }
}
