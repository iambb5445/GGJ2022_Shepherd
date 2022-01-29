using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabTarget : MonoBehaviour
{
    [SerializeField]
    GameObject highlight;

    Transform holdPoint = null;

    public void underTarget()
    {
        highlight.SetActive(true);
    }

    public void notUnderTarget()
    {
        highlight.SetActive(false);
    }

    public void grabed(Transform holdPoint)
    {
        this.holdPoint = holdPoint;
    }

    public void released()
    {
        this.holdPoint = null;
    }

    void Start()
    {
    }

    void Update()
    {
        if (holdPoint != null)
        {
            transform.position = holdPoint.position;
        }
    }
}
