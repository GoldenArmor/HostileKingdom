using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnScale : MonoBehaviour
{
    float currentTime;
    [SerializeField]
    Vector3 initialValue;
    [SerializeField]
    Vector3 finalValue;
    [SerializeField]
    float durationTime;

    Vector3 deltaValue;

    public virtual void ResetEasing()
    {
        transform.localScale = Vector3.zero; 
        currentTime = 0;
    }

    void Start ()
    {
        currentTime = 0;
        deltaValue = finalValue - initialValue;

        transform.localScale = initialValue;
    }

	void Update ()
    {
        if (currentTime < durationTime)
        {
            DoEasing();
        }

        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    currentTime = 0; 
        //}
    }

    void DoEasing()
    {
        transform.localScale = new Vector3(
        Easing.ElasticEaseOut(currentTime, initialValue.x, deltaValue.x, durationTime),
        Easing.ElasticEaseOut(currentTime, initialValue.y, deltaValue.y, durationTime),
        Easing.ElasticEaseOut(currentTime, initialValue.z, deltaValue.z, durationTime));

        currentTime += Time.deltaTime; 

        if (currentTime >= durationTime)
        {
            transform.localScale = finalValue;

            currentTime = durationTime; 
        }
    }
}
