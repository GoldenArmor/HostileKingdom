using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnScalePingPong : MonoBehaviour
{
    float currentTime;
    [SerializeField]
    Vector3 initialValue;
    [SerializeField]
    Vector3 finalValue;
    [SerializeField]
    float durationTime;

    bool isSpawning; 

    Vector3 deltaValue;

    public void ResetEasing()
    {
        Vector3 ini = finalValue;
        finalValue = initialValue;
        initialValue = ini;
        deltaValue = finalValue - initialValue;

        currentTime = 0;
    }

    public void InstantEnd()
    {
        transform.localScale = finalValue;

        currentTime = durationTime;
    }

    void Start ()
    {
        currentTime = durationTime;

        transform.localScale = initialValue;

        Vector3 ini = finalValue;
        finalValue = initialValue;
        initialValue = ini;
        deltaValue = finalValue - initialValue;
    }

	void Update ()
    {
        if (currentTime < durationTime)
        {
            DoEasing();
        }
    }

    void DoEasing()
    {
        transform.localScale = new Vector3(
        Easing.QuadEaseOut(currentTime, initialValue.x, deltaValue.x, durationTime),
        Easing.QuadEaseOut(currentTime, initialValue.y, deltaValue.y, durationTime),
        Easing.QuadEaseOut(currentTime, initialValue.z, deltaValue.z, durationTime));

        currentTime += Time.deltaTime; 

        if (currentTime >= durationTime)
        {
            transform.localScale = finalValue;

            currentTime = durationTime;
        }
    }
}
