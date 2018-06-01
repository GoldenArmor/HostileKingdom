using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildableSurface : MonoBehaviour
{
    [SerializeField]
    List<Renderer> meshRenderers = new List<Renderer>();
    List<Renderer> turretRenderers = new List<Renderer>(); 

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
    [SerializeField]
    ParticleSystem constructionParticles; 

    bool mageTurret; 

    [Header("TurretPhases")]
    [SerializeField]
    GameObject[] archerTurretPhases;
    [SerializeField]
    GameObject[] mageTurretPhases;

    public bool isSelected;

    [Header("Feedback")]
    [SerializeField]
    SpawnScalePingPong spawnScaleHighlight;
    [SerializeField]
    Color startColor;
    [SerializeField]
    Color hoverColor;
    [SerializeField]
    Color turretStartColor;
    [SerializeField]
    Color turretHoverColor; 

    public void MyStart()
    {
        canBuild = true;
        UpdateConstructionBar();
        ChangeColor(startColor);
        //for (int i = 0; i < archerTurretPhases.Length; i++)
        //{
        //    archerTurretPhases[i].transform.position = buildingPoint.position;
        //    archerTurretPhases[i].transform.rotation = buildingPoint.rotation;

        //    warriorTurretPhases[i].transform.position = buildingPoint.position;
        //    warriorTurretPhases[i].transform.rotation = buildingPoint.rotation;
        //}
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
            ChangeTurretColor(turretHoverColor); 
        }
    }

    public void UnselectTurret()
    {
        if (currentTurret != null)
        {
            currentTurret.Unselect();
            spawnScaleHighlight.ResetEasing();
            ChangeTurretColor(turretStartColor); 
        }
    }

    public bool CanBuild()
    {
        return canBuild; 
    }

    void OnMouseEnter()
    {
        if (!isSelected)
        {
            ChangeColor(hoverColor);
            if (currentTurret != null)
            {
                ChangeTurretColor(turretHoverColor); 
            }
        }
    }

    void OnMouseExit()
    {
        if (!isSelected)
        {
            ChangeColor(startColor);
            if (currentTurret != null)
            {
                ChangeTurretColor(turretStartColor);
            }
        }
    }

    public void Select()
    {
        isSelected = true;
        ChangeColor(hoverColor); 
    }

    public void Unselect()
    {
        isSelected = false;
        ChangeColor(startColor);
    }

    public void ChangeColor(Color newColor)
    {
        for (int i = 0; i < meshRenderers.Count; i++)
        {
            meshRenderers[i].material.color = newColor;
        }
    }

    public void ChangeTurretColor(Color newColor)
    {
        for (int i = 0; i < turretRenderers.Count; i++)
        {
            turretRenderers[i].material.color = newColor;
        }
    }

    void Construct(GameObject turret)
    {
        if (!CanBuild())
        {
            //Debug.Log("Can't build here");
            return;
        }

        if(mageTurret)
        {
            currentTurret = ObjectPoolingManager.MageTurretPool.GetObject(turret, buildingPoint);
        }
        else
        {
            currentTurret = ObjectPoolingManager.ArcherTurretPool.GetObject(turret, buildingPoint);
        }
        turretRenderers.Add(currentTurret.gameObject.GetComponent<MeshRenderer>());
        canBuild = false;

        archerTurretPhases[2].SetActive(false);
        mageTurretPhases[2].SetActive(false); 
    }

    public void UpdateConstruct(GameObject turret)
    {
        Unselect();
        
        currentConstructionCooldown += Time.deltaTime;

        UpdateConstructionBar();

        if (mageTurret)
        {
            if (currentConstructionCooldown >= constructionCooldown / 3 && currentConstructionCooldown < constructionCooldown / 2)
            {
                mageTurretPhases[0].SetActive(true);
                constructionZone.SetActive(false);
            }
            if (currentConstructionCooldown >= constructionCooldown / 2 && currentConstructionCooldown < constructionCooldown / 1.5f)
            {
                mageTurretPhases[0].SetActive(false);
                mageTurretPhases[1].SetActive(true);
            }
            if (currentConstructionCooldown >= constructionCooldown / 1.5f && currentConstructionCooldown < constructionCooldown)
            {
                mageTurretPhases[1].SetActive(false);
                mageTurretPhases[2].SetActive(true);
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
            constructionParticles.Stop(); 
            currentConstructionCooldown = 0;
            //constructionBar.enabled = false;
            Construct(turretToConstruct);         
        }
    }

    public void BeginConstruct(GameObject turret, bool isWarriorTurret)
    {
        isBuilding = true;
        spawnScaleHighlight.ResetEasing();
        constructionParticles.Play(); 
        //constructionBar.enabled = true;
        turretToConstruct = turret; 
        constructionBarCanvas.Initialize(constructionBarCanvas.myTransform.position);
        mageTurret = isWarriorTurret; 
    }

    void UpdateConstructionBar()
    {
        constructionBar.fillAmount = currentConstructionCooldown / constructionCooldown;
    }

    public void SellTurret()
    {
        currentTurret.Sell();
        if (mageTurret)
        {
            Player.money += 100;
        }
        else Player.money += 70;

        ChangeColor(startColor); 
        turretRenderers.Remove(currentTurret.gameObject.GetComponent<MeshRenderer>());
        currentTurret = null;
        turretToConstruct = null;
        constructionZone.SetActive(true);
        canBuild = true;
        UpdateConstructionBar(); 
    }
}
