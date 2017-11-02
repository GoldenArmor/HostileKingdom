using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifebarBehaviour : MonoBehaviour
{
    public RectTransform maskBar;
    public RectTransform backgroundBar; 
    private MouseBehaviour mouse;
    public GameObject selectedTarget;
    [SerializeField] private bool isSelected = false;

    [SerializeField] private float startingHealth;

    private float maxWidth = 125f;
    private float newWidth;

    void Start()
    {
        mouse = GameObject.FindGameObjectWithTag("Player").GetComponent<MouseBehaviour>();
    }

    void Update()
    {
        if (mouse.selectedUnit != null)
        {
            if (maskBar.gameObject.activeInHierarchy == false)
            {
                maskBar.gameObject.SetActive(true);
                backgroundBar.gameObject.SetActive(true);
            }
            selectedTarget = mouse.selectedUnit;
            startingHealth = selectedTarget.gameObject.GetComponent<Characters>().startingHitPoints;
            UpdateBar(selectedTarget.gameObject.GetComponent<Characters>().hitPoints);
        }
        else
        {
            maskBar.gameObject.SetActive(false);
            backgroundBar.gameObject.SetActive(false); 
        }
    }

    public void UpdateBar(float newLife)
    {
        float newWidth = (maxWidth * newLife) / startingHealth;
        maskBar.sizeDelta = new Vector2(newWidth, maskBar.sizeDelta.y);
    }
}
