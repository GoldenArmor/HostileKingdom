using System; 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    [SerializeField]
    ButtonManager buttonManager;

    public List<EnemyBehaviour> enemiesCount = new List<EnemyBehaviour>();

    void Update ()
    {
		if (enemiesCount.Count == 0)
        {
            buttonManager.ChangeToWin(); 
        }
	}
}
