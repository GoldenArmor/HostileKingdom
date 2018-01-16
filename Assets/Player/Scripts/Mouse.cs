using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mouse : MonoBehaviour
{
    [Header("Unit Selection")]
    public PlayableUnitBehaviour selectedUnit; //GameObject seleccionado para actualizar.
    public List<PlayableUnitBehaviour> selectedUnits = new List<PlayableUnitBehaviour>(); //Lista de GameObjects para una selección multiple.
    public List<PlayableUnitBehaviour> selectableUnits = new List<PlayableUnitBehaviour>(); //Lista de GameObjects para saber cuantas hay en pantalla.
    public LayerMask mask; //Máscara que se aplica al rayo para detectar una capa determinada de objetos. 
    RaycastHit hit; //Creamos un RaycastHit que nos devolverá la información del objeto con el que el rayo colisiona.
    float maxDistance = Mathf.Infinity; //Máxima distancia que puede recorrer el rayo lanzado des de la cámara. 
    public Vector3 mousePosition; 

    [Header("Drag Selection")]
    public bool isDragging; //Comprobar si estamos pulsando y arrastrando el ratón. 
    public Image selectionBox;
    Vector2 selectionBoxOrigin;
    Rect selectionRect;
    public bool multipleUnitSelection = false;

    [HideInInspector]
    public bool isDoingSkill; 

    Camera mainCamera;

    [SerializeField]
    ButtonManager buttonManager; 

    void Start()
    {
        mainCamera = Camera.main;
        for (int i = 0; i < selectableUnits.Count; i++)
        {
            selectableUnits[i].mouse = this;
        }
    }

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

        if (selectableUnits.Count == 0)
        {
            buttonManager.ChangeToLost();
        }
     }

    #region DragSelection
    void DragUpdate()
    {
        if (mousePosition.x < selectionBoxOrigin.x)
        {
            selectionRect.xMin = mousePosition.x;
            selectionRect.xMax = selectionBoxOrigin.x;
        }
        else
        {
            selectionRect.xMin = selectionBoxOrigin.x;
            selectionRect.xMax = mousePosition.x;
        }

        if (mousePosition.y < selectionBoxOrigin.y)
        {
            selectionRect.yMin = mousePosition.y;
            selectionRect.yMax = selectionBoxOrigin.y;
        }
        else
        {
            selectionRect.yMin = selectionBoxOrigin.y;
            selectionRect.yMax = mousePosition.y;
        }

        selectionBox.rectTransform.offsetMin = selectionRect.min;
        selectionBox.rectTransform.offsetMax = selectionRect.max;
    }

    public void MouseButtonUp()
    {
        selectionBox.rectTransform.sizeDelta = Vector2.zero;
        isDragging = false;
        multipleUnitSelection = false; 
        
        for (int i = 0; i < selectableUnits.Count; i++)
        {
            if (selectionRect.Contains(mainCamera.WorldToScreenPoint(selectableUnits[i].transform.position)))
            {
                if (selectableUnits[i].isSelected)
                    return;

                selectedUnits.Add(selectableUnits[i]);
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

    public void ClickState() 
    {
        Ray ray = mainCamera.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out hit, maxDistance, mask, QueryTriggerInteraction.Ignore))
        {
            selectionBoxOrigin = mousePosition;
            selectionRect = new Rect();

            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("PlayableUnit"))
            {
                if (multipleUnitSelection)
                {
                    MultipleUnitSelection();
                    return;
                }
                if (!isDoingSkill)
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
            }
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                if (!multipleUnitSelection && !isDoingSkill)
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
        }
        isDoingSkill = false; 
    }

    void MultipleUnitSelection()
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

    void SelectedUnitClear()
    {
        selectedUnit.isSelected = false;
        selectedUnit = null;
    }
}
