using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleDetector : MonoBehaviour
{
    public LayerMask mask;
    public float rayDistance;
    public bool isGrappling;
    public RaycastHit hit;
    
    void Update()
    {
        Ray rayBottom = new Ray (transform.position, transform.forward);

        if (Physics.Raycast (rayBottom, out hit, rayDistance, mask)) {
            isGrappling = true;
            Debug.DrawLine(rayBottom.origin, hit.point, Color.red);
        } else {
            isGrappling = false;
            Debug.DrawLine(rayBottom.origin, rayBottom.origin + rayBottom.direction * rayDistance, Color.green);
        }
    }
}
