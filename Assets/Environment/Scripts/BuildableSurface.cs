using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class BuildableSurface : MonoBehaviour
{
    [SerializeField]
    Renderer meshRenderer;

    [Header("Building")]
    public Transform buildingPoint;
    bool canBuild;
    [HideInInspector]
    public bool isBuilding;
    [SerializeField]
    float constructionCooldown; 
    float currentConstructionCooldown;
    GameObject turretToConstruct;
    [SerializeField]
    Image constructionBar; 

    [Header("Color")]
    [HideInInspector]
    public Color hoverColor;
    Color startColor; 

    public bool isSelected;

    public Color MyStart()
    {
        canBuild = true; 
        startColor = meshRenderer.material.color;
        UpdateConstructionBar(); 
        return startColor; 
    }

    void Update()
    {
        if (isBuilding)
        {
            ConstructionCooldown(turretToConstruct); 
        }
    }

    public bool CanBuild()
    {
        if (canBuild)
        {
            canBuild = false; 
            return true; 
        }
        return canBuild; 
    }

    void OnMouseEnter()
    {
        if (!isSelected)
        {
            ChangeColor(hoverColor);
        }
    }

    void OnMouseExit()
    {
        if (!isSelected)
        {
            ChangeColor(startColor);
        }
    }

    public void ChangeColor(Color newColor)
    {
        meshRenderer.material.color = newColor; 
    }

    void Construct(GameObject turret)
    {
        if (!CanBuild())
        {
            Debug.Log("Can't build here");
            return;
        }

        ObjectPoolingManager.Instance.TurretPool.GetObject(turret, buildingPoint);
    }

    public void ConstructionCooldown(GameObject turret)
    {
        isBuilding = true;
        constructionBar.enabled = true;

        currentConstructionCooldown += Time.deltaTime;

        UpdateConstructionBar();

        turretToConstruct = turret;

        if (currentConstructionCooldown > constructionCooldown)
        { 
            isBuilding = false;
            currentConstructionCooldown = 0;
            constructionBar.enabled = false; 
            Construct(turretToConstruct);
        }
    }

    void UpdateConstructionBar()
    {
        constructionBar.fillAmount = currentConstructionCooldown / constructionCooldown;
    }

}
