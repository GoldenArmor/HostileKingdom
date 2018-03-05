using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class BuildableSurface : MonoBehaviour
{
    [SerializeField]
    Renderer meshRenderer;

    public CanvasManager canvasManager; 

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
            UpdateConstruct(turretToConstruct); 
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

    public void UpdateConstruct(GameObject turret)
    {
        currentConstructionCooldown += Time.deltaTime;

        UpdateConstructionBar();

        if (currentConstructionCooldown > constructionCooldown)
        {
            canvasManager.Hide(); 
            isBuilding = false;
            currentConstructionCooldown = 0;
            constructionBar.enabled = false; 
            Construct(turretToConstruct);
        }
    }

    public void BeginConstruct(GameObject turret)
    {
        isBuilding = true;
        constructionBar.enabled = true;
        turretToConstruct = turret; 
        canvasManager.Initialize(canvasManager.myTransform.position);
    }

    void UpdateConstructionBar()
    {
        constructionBar.fillAmount = currentConstructionCooldown / constructionCooldown;
    }

}
