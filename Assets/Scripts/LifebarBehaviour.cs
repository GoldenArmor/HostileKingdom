using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifebarBehaviour : MonoBehaviour
{
    public RectTransform maskBar;
    public RectTransform currentLifeBar;
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
        if (isSelected == false)
        {
            if (mouse.selectedUnit != null)
            {
                selectedTarget = mouse.selectedUnit;
                Init(selectedTarget.gameObject.GetComponent<Characters>().hitPoints);
            }
        }
        if (mouse.selectedUnit == null)
        {
            selectedTarget = null;
            isSelected = false; 
        }
        if (selectedTarget != null)
        {
            selectedTarget = mouse.selectedUnit;
            UpdateBar(selectedTarget.gameObject.GetComponent<Characters>().hitPoints);
        }
    }

    void Init(float life)
    {
        startingHealth = life;
        maxWidth = maskBar.sizeDelta.x;  //Coge la width(x del Vector2) del objeto MaskBar para determinar el valor de maxWidth
        UpdateBar(startingHealth);
        isSelected = true; 
    }

    public void UpdateBar(float newLife)
    {
        float newWidth = (maxWidth * newLife) / startingHealth;
        currentLifeBar.sizeDelta = new Vector2(newWidth, currentLifeBar.sizeDelta.y);
    }
}
