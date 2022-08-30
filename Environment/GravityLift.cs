using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityLift : MonoBehaviour
{
    public float liftForce = 20f;

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player"))
        {
            var rb = other.gameObject.GetComponent<Rigidbody>();
            rb.useGravity = false;
            LiftStateManager.liftState = LiftStateManager.LiftState.ONLIFT;
        }
    }

    void OnTriggerStay(Collider other) {
        if (other.gameObject.CompareTag("Player"))
        {
            var rb = other.gameObject.GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * liftForce, ForceMode.Acceleration);
            LiftStateManager.liftState = LiftStateManager.LiftState.ONLIFT;
        }
    }
    void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Player"))
        {
            var rb = other.gameObject.GetComponent<Rigidbody>();
            rb.useGravity = true;
            LiftStateManager.liftState = LiftStateManager.LiftState.NONE;
        }
    }
}
