using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapButton : MonoBehaviour
{
    private string buildIndex;

    public void EnterTavern()
    {
        buildIndex = "Tavern_screen";
        SceneManager.LoadScene(buildIndex); 
    }

    public void ExitTavern()
    {
        buildIndex = "Marc_Pruebas";
        SceneManager.LoadScene(buildIndex);
    }
}
