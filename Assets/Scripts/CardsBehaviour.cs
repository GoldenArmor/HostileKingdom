using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject[] playableUnitsAwake;
    [SerializeField] private GameObject[] statsCardsAwake;
    [SerializeField] private PlayableUnitBehaviour[] playableUnits;
    [SerializeField] private StatsCard[] statsCard;

    void Start()
    {
        playableUnitsAwake = GameObject.FindGameObjectsWithTag("PlayableUnit");
        statsCardsAwake = GameObject.FindGameObjectsWithTag("StatsCard");
        playableUnits = new PlayableUnitBehaviour[4];
        statsCard = new StatsCard[4];

        for (int i = 0; i < playableUnitsAwake.Length; i++)
        {
            playableUnits[i] = playableUnitsAwake[i].GetComponent<PlayableUnitBehaviour>();
            statsCard[i] = statsCardsAwake[i].GetComponent<StatsCard>();

            statsCard[i].cardNumber = i + 1;
            //if (playableUnits[i].cardNumber != statsCard[i].cardNumber) statsCard[i].selectedTarget = playableUnits[i].gameObject;
        }
        for (int i = 0; i < playableUnits.Length; i++)
        {
            
        }
    }

    void Update ()
    {

	}
}
