using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionCircle : MonoBehaviour
{
    PlayableUnitBehaviour unit;
    ParticleSystem activeTexture;
    Vector3 maxScale;

    void Start()
    {
        activeTexture = GetComponent<ParticleSystem>();

        unit = GetComponentInParent<PlayableUnitBehaviour>();

        maxScale = activeTexture.transform.localScale;
    }

    void Update()
    {
        if (unit.isSelected)
        {
            activeTexture.Play();
        }
        if (unit.isSelected == false)
        {
            activeTexture.Stop();
            activeTexture.Clear(); 
        }
    }

    void FixedUpdate()
    {
        if (unit.isSelected)
        {
            if (activeTexture.transform.localScale.x < maxScale.x) activeTexture.transform.localScale += new Vector3(Mathf.Lerp(0, maxScale.x, 0.3f), Mathf.Lerp(0, maxScale.y, 0.3f), maxScale.z);
            if (activeTexture.transform.localScale.x >= maxScale.x) activeTexture.transform.localScale = maxScale;
        }
        else activeTexture.transform.localScale = Vector3.zero;
    }
}
