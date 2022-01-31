using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Duality : MonoBehaviour
{
    [SerializeField]
    GameObject day;
    [SerializeField]
    GameObject night;

    SheepAI daySheep;
    SheepAI nightSheep;
    void Start()
    {
        day = Instantiate(day, transform);
        day.transform.position = transform.position;
        day.transform.rotation = transform.rotation;
        daySheep = day.GetComponent<SheepAI>();
        if (night != null)
        {
            night = Instantiate(night, transform);
            night.transform.position = transform.position;
            night.transform.rotation = transform.rotation;
            nightSheep = night.GetComponent<SheepAI>();
            night.SetActive(false);
        }
        if (daySheep != null)
        {
            SheepAI.sheepList.Add(day);
        }
        if (nightSheep != null)
        {
            SheepAI.sheepList.Add(night);
        }
    }
    void Update()
    {
        if ((daySheep != null && daySheep.isDead) || (nightSheep != null && nightSheep.isDead))
        {
            day.SetActive(false);
            night.SetActive(false);
            gameObject.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && night != null)
        {
            night.transform.position = day.transform.position;
            night.transform.rotation = day.transform.rotation;
            day.SetActive(false);
            night.SetActive(true);
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) && night != null)
        {
            day.transform.position = night.transform.position;
            day.transform.rotation = night.transform.rotation;
            day.SetActive(true);
            night.SetActive(false);
        }
    }
}
