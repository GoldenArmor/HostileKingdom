using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : MonoBehaviour
{
    [Header("Skill")]
    [SerializeField]
    LayerMask skillMask;
    [SerializeField]
    float skillCircleRadius;

    Collider[] hitColliders;

    public bool canDoSkill;

    float cooldown = 10;

    void Update()
    {
        cooldown -= Time.deltaTime;
        if (cooldown <= 0)
        {
            canDoSkill = true;
        }
    }

    public void SkillUpdate()
    {
        if (canDoSkill)
        {
            hitColliders = Physics.OverlapSphere(transform.position, skillCircleRadius, skillMask);
            for (int i = 0; i < hitColliders.Length; i++)
            {
                PlayableUnitBehaviour allyTarget = hitColliders[i].GetComponent<PlayableUnitBehaviour>();
                allyTarget.Heal(allyTarget.hitPoints * 0.1f);
            }
            hitColliders = null;
            canDoSkill = false;
            cooldown = 10;
        }
    }
}
