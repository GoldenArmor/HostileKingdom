using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildableSurface : MonoBehaviour
{
    [SerializeField]
    Renderer rend;

    [Header("Color")]
    [SerializeField]
    Color hoverColor;
    Color startColor;

    [Header("Building")]
    //[SerializeField]
    //GameObject buildingUI; 
    [SerializeField]
    GameObject turret;
    [SerializeField]
    Transform buildingPoint;
    bool hasBeenBuilded; 

    void Start()
    {
        startColor = rend.material.color; 
    }

    void OnMouseDown()
    {
        //if (turret != null)
        //{
        //    Debug.Log("Can't build here");
        //    return; 
        //}
        if (hasBeenBuilded == true)
        {
            Debug.Log("Can't build here");
            return;
        }

        //buildingUI.SetActive(true);

        ObjectPoolingManager.Instance.TurretPool.GetObject(turret, buildingPoint);
        hasBeenBuilded = true; 
    }

    void OnMouseEnter()
    {
        Debug.Log("Mouse Enter"); 
        rend.material.color = hoverColor;   
    }

    void OnMouseExit()
    {
        rend.material.color = startColor;     
    }
}
