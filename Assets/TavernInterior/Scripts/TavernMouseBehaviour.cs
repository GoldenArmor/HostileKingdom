using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TavernMouseBehaviour : MonoBehaviour
{
    public LayerMask mask; //Máscara que se aplica al rayo para detectar una capa determinada de objetos. 
    private RaycastHit hit; //Creamos un RaycastHit que nos devolverá la información del objeto con el que el rayo colisiona.
    private float maxDistance = Mathf.Infinity;

    public GameObject selectedCard; 
	
	void Update ()
    {
        if (selectedCard != null)
        {
            selectedCard.gameObject.GetComponent<TavernFigureBehaviour>().isSelected = true;
        }
    }

    public void CardSelection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //Creamos un rayo que va a la dirección del Mouse desde la cámara y la traducimos a una posición en el mundo. 

        if (selectedCard == null) //Si no hay ninguna unidad seleccionada.
        {
            if (Physics.Raycast(ray, out hit, maxDistance, mask, QueryTriggerInteraction.Ignore)) //Si el rayo colisiona con algo.
            {
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("TavernCard")) //Si el objeto con el que el rayo ha impactado está en la capa de "Unit".
                {
                    selectedCard = hit.transform.gameObject; //La unidad seleccionada (selectedUnit) pasa a ser el GameObject de dicha colisión.
                }
            }
        }

        else//En cualquier otro caso.
        {
            if (Physics.Raycast(ray, out hit, maxDistance, mask, QueryTriggerInteraction.Ignore)) //Si el rayo colisiona con algo.
            {
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("TavernCard")) //Si el objeto con el que el rayo ha impactado está en la capa de "Unit".
                {
                    selectedCard.gameObject.GetComponent<TavernFigureBehaviour>().isSelected = false;
                    selectedCard = null; //Se deselecciona la actual unidad seleccionada.
                    selectedCard = hit.transform.gameObject; //La unidad seleccionada (selectedUnit) pasa a ser el GameObject de dicha colisión.
                }
            }
        }
    }

}
