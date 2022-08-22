using UnityEngine;

public class CrouchDetector : MonoBehaviour
{
    CapsuleCollider capsuleCollider;
    public static bool canStandUp = true;
    RaycastHit hit;
    float rayDistance = 1f;

    void Start() {
        capsuleCollider = transform.root.GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update() {
        // if (Physics.SphereCast(
        // transform.position + capsuleCollider.center + Vector3.up * -capsuleCollider.height * 0.7F,
        // (transform.position + capsuleCollider.center + Vector3.up * -capsuleCollider.height * 0.7F) + Vector3.up * capsuleCollider.height,
        // capsuleCollider.radius,
        // transform.up,
        // out hit, 
        // rayDistance))
        // {
        if (Physics.SphereCast(transform.position, 1, transform.up, out hit, rayDistance))
        {
            canStandUp = false; 
        }
        else
        {
            canStandUp = true;
        }
    }
}
