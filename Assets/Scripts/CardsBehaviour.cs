using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class CardsBehaviour : MonoBehaviour
{
    [Header("Arrays")]
    [SerializeField] private PlayableUnitBehaviour[] playableUnits;
    [SerializeField] private StatsCard[] statsCard;

    public Text[] targetName;
    public RectTransform[] maskBar;
    public RectTransform[] backgroundBar;

    [SerializeField] private float[] startingHealth;
    private float maxWidth;
    private float[] newWidth;

    public void Init()
    {
        for (int i = 0; i < playableUnits.Length; i++)
        {
            statsCard[i].selectedTarget = playableUnits[i].gameObject;

            targetName[i].text = statsCard[i].selectedTarget.gameObject.GetComponent<Characters>().characterName;
            startingHealth[i] = statsCard[i].selectedTarget.gameObject.GetComponent<Characters>().startingHitPoints;

            maxWidth = maskBar[i].sizeDelta.x;
        }
    }

    void Update()
    {
        for (int i = 0; i < statsCard.Length; i++)
        {
            UpdateBar(statsCard[i].selectedTarget.gameObject.GetComponent<Characters>().hitPoints);
        }
    }

    public void UpdateBar(float newLife)
    {
        for (int i = 0; i < statsCard.Length; i++)
        {
            newWidth[i] = (maxWidth * newLife) / startingHealth[i];
            maskBar[i].sizeDelta = new Vector2(newWidth[i], maskBar[i].sizeDelta.y);
            Debug.Log("Hello"); 
        }
    }
}
