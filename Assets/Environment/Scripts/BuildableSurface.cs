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
    GameObject constructionZone; 

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

    [Header("TurretPhases")]
    [SerializeField]
    GameObject[] archerTurretPhases;
    [SerializeField]
    GameObject[] warriorTurretPhases;

    public bool isSelected;

    [Header("Feedback")]
    [SerializeField]
    SpawnScalePingPong spawnScaleHighlight;
    [SerializeField]
    Color startColor;
    [HideInInspector]
    public Color hoverColor;

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
        if (currentTurret != null)
        {
            currentTurret.Select();
            spawnScaleHighlight.ResetEasing();
        }
    }

    public void UnselectTurret()
    {
        if (currentTurret != null)
        {
            currentTurret.Unselect();
            spawnScaleHighlight.ResetEasing(); 
        }
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
        canBuild = false;

        archerTurretPhases[2].SetActive(false);
        warriorTurretPhases[2].SetActive(false); 
    }

    public void UpdateConstruct(GameObject turret)
    {
        ChangeColor(startColor);
        isSelected = false; 

        currentConstructionCooldown += Time.deltaTime;

        UpdateConstructionBar();

        if (warriorTurret)
        {
            if (currentConstructionCooldown >= constructionCooldown / 3 && currentConstructionCooldown < constructionCooldown / 2)
            {
                warriorTurretPhases[0].SetActive(true);
                constructionZone.SetActive(false);
            }
            if (currentConstructionCooldown >= constructionCooldown / 2 && currentConstructionCooldown < constructionCooldown / 1.5f)
            {
                warriorTurretPhases[0].SetActive(false);
                warriorTurretPhases[1].SetActive(true);
            }
            if (currentConstructionCooldown >= constructionCooldown / 1.5f && currentConstructionCooldown < constructionCooldown)
            {
                warriorTurretPhases[1].SetActive(false);
                warriorTurretPhases[2].SetActive(true);
            }
        }
        else
        {
            if (currentConstructionCooldown >= constructionCooldown / 3 && currentConstructionCooldown < constructionCooldown / 2)
            {
                archerTurretPhases[0].SetActive(true);
                constructionZone.SetActive(false);
            }
            if (currentConstructionCooldown >= constructionCooldown / 2 && currentConstructionCooldown < constructionCooldown / 1.5f)
            {
                archerTurretPhases[0].SetActive(false);
                archerTurretPhases[1].SetActive(true);
            }
            if (currentConstructionCooldown >= constructionCooldown / 1.5f && currentConstructionCooldown < constructionCooldown)
            {
                archerTurretPhases[1].SetActive(false);
                archerTurretPhases[2].SetActive(true);
            }
        }

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
        spawnScaleHighlight.ResetEasing(); 
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
        if (warriorTurret)
        {
            Player.money += 100;
        }
        else Player.money += 70; 

        meshRenderers.Remove(currentTurret.gameObject.GetComponent<MeshRenderer>());
        currentTurret = null;
        turretToConstruct = null;
        constructionZone.SetActive(true);
        canBuild = true;
        UpdateConstructionBar(); 
    }
}
