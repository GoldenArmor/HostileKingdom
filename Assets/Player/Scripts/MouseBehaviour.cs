using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseBehaviour : MonoBehaviour
{
    [Header("Unit Selection")]
    public PlayableUnitBehaviour selectedUnit; //GameObject seleccionado para actualizar.
    public List<PlayableUnitBehaviour> selectedUnits = new List<PlayableUnitBehaviour>(); //Lista de GameObjects para una selección multiple.
    public List<PlayableUnitBehaviour> unitsOnScreenSpace = new List<PlayableUnitBehaviour>(); //Lista de GameObjects para saber cuantas hay en pantalla.
    public LayerMask mask; //Máscara que se aplica al rayo para detectar una capa determinada de objetos. 
    private RaycastHit hit; //Creamos un RaycastHit que nos devolverá la información del objeto con el que el rayo colisiona.
    private float maxDistance = Mathf.Infinity; //Máxima distancia que puede recorrer el rayo lanzado des de la cámara. 

    [Header("Drag Selection")]
    public bool isDragging; //Comprobar si estamos pulsando y arrastrando el ratón. 
    public Image selectionBox;
    private Vector2 selectionBoxOrigin;
    private Rect selectionRect;

    void Update()
    {
        if (isDragging) //Si estamos pulsando y arrastrando el ratón.
        {
            DragUpdate();
        }

        if (selectedUnit != null)
        {
            selectedUnit.isSelected = true;
        }
        if (selectedUnits.Count > 0)
        {
            for (int i = 0; i < selectedUnits.Count; i++)
            {
                selectedUnits[i].isSelected = true;
            }
        }
    }

    #region DragSelection
    void DragUpdate()
    {
        if (Input.mousePosition.x < selectionBoxOrigin.x)
        {
            selectionRect.xMin = Input.mousePosition.x;
            selectionRect.xMax = selectionBoxOrigin.x;
        }
        else
        {
            selectionRect.xMin = selectionBoxOrigin.x;
            selectionRect.xMax = Input.mousePosition.x;
        }

        if (Input.mousePosition.y < selectionBoxOrigin.y)
        {
            selectionRect.yMin = Input.mousePosition.y;
            selectionRect.yMax = selectionBoxOrigin.y;
        }
        else
        {
            selectionRect.yMin = selectionBoxOrigin.y;
            selectionRect.yMax = Input.mousePosition.y;
        }

        selectionBox.rectTransform.offsetMin = selectionRect.min;
        selectionBox.rectTransform.offsetMax = selectionRect.max;
    }

    public void MouseButtonPressed()
    {
        if (Physics.Raycast(Raycast(), out hit, maxDistance, mask, QueryTriggerInteraction.Ignore))
        {
            if (hit.transform.gameObject.layer != LayerMask.NameToLayer("PlayableUnit"))
            {
                isDragging = true;
            }
        }    
    }

    public void MouseButtonUp()
    {
        selectionBox.rectTransform.sizeDelta = Vector2.zero;
        isDragging = false;
        
        for (int i = 0; i < unitsOnScreenSpace.Count; i++)
        {
            if (selectionRect.Contains(Camera.main.WorldToScreenPoint(unitsOnScreenSpace[i].transform.position)))
            {
                if (unitsOnScreenSpace[i].GetComponent<PlayableUnitBehaviour>().isSelected)
                    return;

                selectedUnits.Add(unitsOnScreenSpace[i]);
            }
        }
    }

    public bool UnitWithinScreenSpace(Vector2 unitScreenPosition)
    {
        if ((unitScreenPosition.x < Screen.width && unitScreenPosition.y < Screen.height) && 
            (unitScreenPosition.x > 0f && unitScreenPosition.y > 0f))
            return true;
        else
            return false;
    }

    private bool CheckMouseDrag()
    {
        if (selectionRect.x - 1 >= selectionBoxOrigin.x || selectionRect.y - 1 >= selectionBoxOrigin.y ||
            selectionRect.x < selectionBoxOrigin.x - 1 || selectionRect.y < selectionBoxOrigin.y - 1) return true;
        else return false; 
    }
    #endregion

    public void OneUnitSelection() 
    {
        Ray ray = Raycast(); 

        if (Physics.Raycast(ray, out hit, maxDistance, mask, QueryTriggerInteraction.Ignore))
        {
            Debug.Log(hit.transform.name);

            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("PlayableUnit"))
            {
                if (selectedUnit == null)
                {
                    for (int i = 0; i < selectedUnits.Count; i++)
                    {
                        selectedUnits[i].isSelected = false;
                    }
                    selectedUnits.Clear();
                    selectedUnit = hit.transform.GetComponent<PlayableUnitBehaviour>();
                }
                else
                    SelectedUnitClear();
                    selectedUnit = hit.transform.GetComponent<PlayableUnitBehaviour>();
            }
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                if (selectedUnit == null)
                {
                    for (int i = 0; i < selectedUnits.Count; i++)
                    {
                        selectedUnits[i].isSelected = false;
                    }
                    selectedUnits.Clear();
                }
                else
                    SelectedUnitClear();
            }
        }
        
        selectionBoxOrigin = Input.mousePosition;
        selectionRect = new Rect();
    }

    public void MultipleUnitSelection()
    {
        if (Physics.Raycast(Raycast(), out hit, maxDistance, mask, QueryTriggerInteraction.Ignore))
        {
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("PlayableUnit"))
            {
                if (selectedUnit != null)
                {
                    selectedUnits.Add(selectedUnit);
                    SelectedUnitClear();
                }
                if (hit.transform.GetComponent<PlayableUnitBehaviour>().isSelected)
                    return;

                selectedUnits.Add(hit.transform.GetComponent<PlayableUnitBehaviour>());
            }
        }

        selectionBoxOrigin = Input.mousePosition;
        selectionRect = new Rect();
    }

    Ray Raycast()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        return ray;
    }

    void SelectedUnitClear()
    {
        selectedUnit.isSelected = false;
        selectedUnit = null;
    }
}
