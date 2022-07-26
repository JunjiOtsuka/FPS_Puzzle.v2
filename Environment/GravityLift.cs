using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityLift : MonoBehaviour
{
    public float liftForce = 20f;

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("upon enter");
            var rb = other.gameObject.GetComponent<Rigidbody>();
            rb.useGravity = false;
        }
    }

    void OnTriggerStay(Collider other) {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("upon stay");
            var rb = other.gameObject.GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * liftForce, ForceMode.Acceleration);
        }
    }
    void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("upon exit");
            var rb = other.gameObject.GetComponent<Rigidbody>();
            rb.useGravity = true;
        }
    }
}
