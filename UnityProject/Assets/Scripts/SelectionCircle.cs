using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionCircle : MonoBehaviour
{
    public PlayableUnitBehaviour unit;
    public SpriteRenderer activeTexture;
    private Vector3 maxScale; 

	void Start ()
    {
        activeTexture = this.gameObject.GetComponent<SpriteRenderer>();

        unit = GetComponentInParent<PlayableUnitBehaviour>();

        maxScale = activeTexture.transform.localScale;
	}
	
	void Update ()
    {
        if (unit.isSelected)
        {
            activeTexture.enabled = true;
        }
        if (unit.isSelected == false)
        {
            activeTexture.enabled = false;
        }
    }

    void FixedUpdate()
    {
        if (unit.isSelected)
        {
            activeTexture.transform.Rotate(Vector3.forward);
            if (activeTexture.transform.localScale.x < maxScale.x) activeTexture.transform.localScale += new Vector3(Mathf.Lerp(0, maxScale.x, 0.3f), Mathf.Lerp(0, maxScale.y, 0.3f), maxScale.z);
            if (activeTexture.transform.localScale.x >= maxScale.x) activeTexture.transform.localScale = maxScale;
        }
        if (unit.isSelected == false)
        {
            activeTexture.transform.localScale = Vector3.zero;
        }
    }
}
