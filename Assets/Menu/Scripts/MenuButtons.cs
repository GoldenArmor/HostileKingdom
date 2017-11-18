using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButtons : MonoBehaviour
{
    private bool isActive = true; 

    public void StartGame()
    {
        this.gameObject.SetActive(!isActive); 
    }
}
