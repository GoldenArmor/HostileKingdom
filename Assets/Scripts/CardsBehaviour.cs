using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardsBehaviour : MonoBehaviour
{
    [Header("Components")]
    public Text targetName;
    float maxWidth; // = 125f;
    float newWidth;
    [SerializeField]
    RectTransform maskBar;
    [SerializeField]
    RectTransform backgroundBar;
    public float startingHealth;


    public void MyStart()
    {
        maxWidth = maskBar.sizeDelta.x;
        UpdateLifeBar(startingHealth);
    }

    public void UpdateLifeBar(float newLife)
    {
        newWidth = (maxWidth * newLife) / startingHealth;
        maskBar.sizeDelta = new Vector2(newWidth, maskBar.sizeDelta.y);
        Debug.Log(maskBar.sizeDelta); 
    }
}
