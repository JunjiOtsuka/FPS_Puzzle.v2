using UnityEngine;

public class GravityLiftHorizontal : MonoBehaviour
{
    public float liftForce = 20f;

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player"))
        {
            var rb = other.gameObject.GetComponent<Rigidbody>();
            rb.useGravity = false;
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.constraints = RigidbodyConstraints.FreezePositionY;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            LiftStateManager.liftState = LiftStateManager.LiftState.ONLIFT;
        }
    }

    void OnTriggerStay(Collider other) {
        if (other.gameObject.CompareTag("Player"))
        {
            var rb = other.gameObject.GetComponent<Rigidbody>();
            rb.useGravity = false;
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(transform.forward * liftForce + transform.up * 50f, ForceMode.Acceleration);
            rb.constraints = RigidbodyConstraints.FreezePositionY;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            LiftStateManager.liftState = LiftStateManager.LiftState.ONLIFT;
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Player"))
        {
            var rb = other.gameObject.GetComponent<Rigidbody>();
            rb.useGravity = true;
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, rb.velocity.z);
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            LiftStateManager.liftState = LiftStateManager.LiftState.NONE;
        }
    }
}
