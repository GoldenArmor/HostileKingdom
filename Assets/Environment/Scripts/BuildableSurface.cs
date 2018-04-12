using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildableSurface : MonoBehaviour
{
    [SerializeField]
    List<Renderer> meshRenderers = new List<Renderer>();

    [SerializeField]
    GameObject meshesGameObject; 

    public CanvasManager constructionBarCanvas; 

    [Header("Building")]
    public Transform buildingPoint;
    bool canBuild;
    [HideInInspector]
    public bool isBuilding;
    [SerializeField]
    float constructionCooldown; 
    float currentConstructionCooldown;
    GameObject turretToConstruct;
    public Turret currentTurret; 
    [SerializeField]
    Image constructionBar;

    bool warriorTurret; 

    [Header("Color")]
    [HideInInspector]
    public Color hoverColor;
    [SerializeField]
    Color startColor; 

    public bool isSelected;

    public void MyStart()
    {
        canBuild = true;
        UpdateConstructionBar(); 
    }

    void Update()
    {
        if (isBuilding)
        {
            UpdateConstruct(turretToConstruct); 
        }
    }

    public void SelectTurret()
    {
        if (currentTurret != null) currentTurret.Select(); 
    }

    public void UnselectTurret()
    {
        if (currentTurret != null) currentTurret.Unselect();
    }

    public bool CanBuild()
    {
        return canBuild; 
    }

    //void OnMouseEnter()
    //{
    //    if (!isSelected)
    //    {
    //        ChangeColor(hoverColor);
    //    }
    //}

    //void OnMouseExit()
    //{
    //    if (!isSelected)
    //    {
    //        ChangeColor(startColor);
    //    }
    //}

    public void ChangeColor(Color newColor)
    {
        for (int i = 0; i < meshRenderers.Count; i++)
        {
            meshRenderers[i].material.color = newColor;
        }
    }

    void Construct(GameObject turret)
    {
        if (!CanBuild())
        {
            //Debug.Log("Can't build here");
            return;
        }

        if(warriorTurret)
        {
            currentTurret = ObjectPoolingManager.WarriorTurretPool.GetObject(turret, buildingPoint);
        }
        else
        {
            currentTurret = ObjectPoolingManager.ArcherTurretPool.GetObject(turret, buildingPoint);
        }
        meshRenderers.Add(currentTurret.gameObject.GetComponent<MeshRenderer>());
        meshesGameObject.SetActive(false);
        canBuild = false;
    }

    public void UpdateConstruct(GameObject turret)
    {
        ChangeColor(startColor);
        isSelected = false; 

        currentConstructionCooldown += Time.deltaTime;

        UpdateConstructionBar();

        if (currentConstructionCooldown > constructionCooldown)
        {
            constructionBarCanvas.Hide(); 
            isBuilding = false;
            currentConstructionCooldown = 0;
            //constructionBar.enabled = false;
            Construct(turretToConstruct);         
        }
    }

    public void BeginConstruct(GameObject turret, bool isWarriorTurret)
    {
        isBuilding = true;
        //constructionBar.enabled = true;
        turretToConstruct = turret; 
        constructionBarCanvas.Initialize(constructionBarCanvas.myTransform.position);
        warriorTurret = isWarriorTurret; 
    }

    void UpdateConstructionBar()
    {
        constructionBar.fillAmount = currentConstructionCooldown / constructionCooldown;
    }

    public void SellTurret()
    {
        currentTurret.Sell();
        meshRenderers.Remove(currentTurret.gameObject.GetComponent<MeshRenderer>());
        currentTurret = null;
        turretToConstruct = null;
        meshesGameObject.SetActive(true);
        canBuild = true;
        UpdateConstructionBar(); 
    }
}
