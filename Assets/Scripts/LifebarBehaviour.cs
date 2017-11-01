using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifebarBehaviour : MonoBehaviour
{
    public RectTransform maskBar;
    public RectTransform currentLifeBar;
    private MouseBehaviour mouse;
    public GameObject selectedUnit;
    public bool isSelected = false; 

    [SerializeField] private float startingHealth;

    private float maxWidth;
    private float newWidth;

    void Start()
    {
        mouse = GameObject.FindGameObjectWithTag("Player").GetComponent<MouseBehaviour>();
    }

    void Update()
    {
        if (isSelected == true)
        {
            if (mouse.selectedUnit != null) selectedUnit = mouse.selectedUnit;
            else selectedUnit = mouse.selectedUnits[0];

            if (selectedUnit.tag == "Enemy") Init(selectedUnit.gameObject.GetComponent<EnemyBehaviour>().hitPoints);
            if (selectedUnit.tag == "PlayableUnit") Init(selectedUnit.gameObject.GetComponent<PlayableUnitBehaviour>().hitPoints);  
        }
    }

    void Init(float life)
    {
        startingHealth = life;
        maxWidth = maskBar.sizeDelta.x;  //Coge la width(x del Vector2) del objeto MaskBar para determinar el valor de maxWidth
        UpdateBar(startingHealth);
    }

    public void UpdateBar(float newLife)
    {
        float newWidth = (maxWidth * newLife) / startingHealth;
        currentLifeBar.sizeDelta = new Vector2(newWidth, currentLifeBar.sizeDelta.y);
    }
}
