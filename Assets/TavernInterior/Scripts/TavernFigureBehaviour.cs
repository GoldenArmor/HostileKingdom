using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TavernFigureBehaviour : MonoBehaviour
{
    [SerializeField]
    Transform figure;
    [HideInInspector]
    public bool isSelected = false; 
    Vector3 maxScale;

    void Start()
    {
        maxScale = figure.transform.localScale;
    }

    void FixedUpdate()
    {
        if (isSelected == true)
        {
            figure.gameObject.SetActive(true);
            figure.transform.Rotate(Vector3.up);
            if (figure.transform.localScale.x < maxScale.x) figure.transform.localScale += new Vector3(Mathf.Lerp(0, maxScale.x, 0.025f), Mathf.Lerp(0, maxScale.y, 0.025f), Mathf.Lerp(0, maxScale.z, 0.025f));
            if (figure.transform.localScale.x >= maxScale.x) figure.transform.localScale = maxScale;
        }
        else figure.transform.localScale = Vector3.zero;
    }
}
