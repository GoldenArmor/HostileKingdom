using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mouse : MonoBehaviour
{
    [Header("Buildable Surface Selection")]
    public BuildableSurface selectedSurface; //GameObject seleccionado para actualizar.
    public List<GameObject> selectableSurfaces = new List<GameObject>(); //Lista de GameObjects para saber cuantas hay en pantalla.
    public LayerMask mask; //Máscara que se aplica al rayo para detectar una capa determinada de objetos. 
    RaycastHit hit; //Creamos un RaycastHit que nos devolverá la información del objeto con el que el rayo colisiona.
    float maxDistance = Mathf.Infinity; //Máxima distancia que puede recorrer el rayo lanzado des de la cámara. 
    public Vector3 mousePosition;

    [Header("Color")]
    [SerializeField]
    Color hoverColor;
    [SerializeField]
    Color startColor;
    BuildableSurface colorizedSurface; 

    [Header("Construction")]
    [SerializeField]
    CanvasManager constructionCanvas;
    [SerializeField]
    CanvasManager sellingCanvas; 

    Camera mainCamera;

    void Start()
    {
        BuildableSurface[] selectableList = FindObjectsOfType<BuildableSurface>();
        for (int i = 0; i < selectableList.Length; i++)
        {
            startColor = selectableList[i].MyStart(); 
            selectableList[i].ChangeColor(startColor);
            selectableList[i].hoverColor = hoverColor; 
            selectableSurfaces.Add(selectableList[i].gameObject);
        }
        mainCamera = Camera.main;
    }

    void Update()
    {
        //Ray ray = mainCamera.ScreenPointToRay(mousePosition);
        //if (Physics.Raycast(ray, out hit, maxDistance, mask, QueryTriggerInteraction.UseGlobal))
        //{
        //    if (hit.transform.CompareTag("BuildableSurface"))
        //    {
        //        colorizedSurface = hit.transform.GetComponent<BuildableSurface>();
        //        colorizedSurface.ChangeColor(hoverColor);
        //    }

        //    else
        //    {
        //        if (colorizedSurface != null && !colorizedSurface.isSelected)
        //        {
        //            colorizedSurface.ChangeColor(startColor);
        //            colorizedSurface = null; 
        //        }
        //    }

        //}
    }

    public void ClickState() 
    {
        Ray ray = mainCamera.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out hit, maxDistance, mask, QueryTriggerInteraction.UseGlobal))
        {
            if (hit.transform.CompareTag("BuildableSurface"))
            {
                if (selectedSurface == null)
                {
                    SelectBuildableSurface();
                    return; 
                }
                if (hit.transform.gameObject != selectedSurface.gameObject)
                {
                    ClearSelectedSurface();
                    SelectBuildableSurface();
                } 
            }
            if (hit.transform.CompareTag("Ground"))
            {
                if (selectedSurface != null)
                {
                    ClearSelectedSurface();
                }            
            }
        }
    }

    void SelectBuildableSurface()
    {
        selectedSurface = hit.transform.GetComponent<BuildableSurface>();
        if (selectedSurface.isBuilding)
        {
            selectedSurface = null;
            return; 
        }
        selectedSurface.isSelected = true;
        selectedSurface.ChangeColor(hoverColor);

        if (selectedSurface.CanBuild())
        {
            constructionCanvas.Initialize(new Vector3(selectedSurface.transform.position.x, 25, selectedSurface.transform.position.z));
        }
        else
        {
            sellingCanvas.Initialize(new Vector3(selectedSurface.transform.position.x, 25, selectedSurface.transform.position.z));
        }
    }

    void ClearSelectedSurface()
    {
        if (selectedSurface.CanBuild())
        {
            constructionCanvas.Hide();
        }
        else
        { 
            sellingCanvas.Hide(); 
        }
        selectedSurface.isSelected = false;
        selectedSurface.ChangeColor(startColor);  
        selectedSurface = null;
    }

    public void Construct(GameObject turret) 
    {
        selectedSurface.BeginConstruct(turret);
        constructionCanvas.Hide();
        selectedSurface.isSelected = false;
        selectedSurface.ChangeColor(startColor);
        selectedSurface = null; 
    }

    public void Sell()
    {
        selectedSurface.isSelected = false;
        selectedSurface.ChangeColor(startColor);
        selectedSurface.SellTurret();
        selectedSurface = null; 
        sellingCanvas.Hide(); 
    }
}
