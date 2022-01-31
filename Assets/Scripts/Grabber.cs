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
    GrabTarget grabbed = null;
    void Update()
    {
        if (!GameManager.isNight)
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
                grabbed = grabTarget;
            }
            else if (Input.GetKeyUp(KeyCode.Mouse0) && grabbed != null)
            {
                grabbed.release();
                if (grabTarget != null)
                {
                    grabTarget.underTarget();
                }
                grabbed = null;
            }
        }
        else if (GameManager.nightTrigger && grabbed != null)
        {
            grabbed.release();
            grabbed = null;
        }
    }
}
