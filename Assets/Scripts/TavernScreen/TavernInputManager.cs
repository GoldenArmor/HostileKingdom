using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TavernInputManager : MonoBehaviour
{
    public TavernMouseBehaviour mouse;
	
	void Update ()
    {
        if (Input.GetMouseButtonDown(0)) mouse.CardSelection(); 
    }
}
