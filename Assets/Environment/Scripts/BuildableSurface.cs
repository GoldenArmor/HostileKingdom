using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildableSurface : MonoBehaviour
{
    [SerializeField]
    Renderer meshRenderer;

    [Header("Building")]
    public Transform buildingPoint;
    bool canBuild;

    [Header("Color")]
    [HideInInspector]
    public Color hoverColor;
    Color startColor; 

    public bool isSelected;

    public Color MyStart()
    {
        canBuild = true; 
        startColor = meshRenderer.material.color;
        return startColor; 
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
}
