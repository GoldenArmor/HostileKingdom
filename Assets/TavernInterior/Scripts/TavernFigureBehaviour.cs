using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TavernFigureBehaviour : MonoBehaviour
{
    public GameObject figure;
    public bool isSelected = false; 

    void FixedUpdate()
    {
        if (isSelected == true)
        {
            figure.gameObject.SetActive(true);
            figure.transform.Rotate(Vector3.up);
        }
        else figure.gameObject.SetActive(false); 
    }
}
