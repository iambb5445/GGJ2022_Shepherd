using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabber : MonoBehaviour
{
    [SerializeField]
    LayerMask grabLayer;
    [SerializeField]
    Transform holdPoint;

    GrabTarget lastGrabTarget = null;
    void Update()
    {
        GrabTarget grabTarget = null;
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 2, grabLayer))
        {
            if (raycastHit.collider != null)
            {
                grabTarget = raycastHit.transform.GetComponent<GrabTarget>();
            }
        }
        if (grabTarget != lastGrabTarget)
        {
            if (grabTarget != null)
            {
                grabTarget.underTarget();
            }
            if (lastGrabTarget != null)
            {
                lastGrabTarget.notUnderTarget();
            }
            lastGrabTarget = grabTarget;
        }
        if (Input.GetKeyDown(KeyCode.Mouse0) && grabTarget != null)
        {
            grabTarget.notUnderTarget();
            grabTarget.grab(holdPoint);
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0) && grabTarget != null)
        {
            grabTarget.release();
            grabTarget.underTarget();
        }
    }
}
