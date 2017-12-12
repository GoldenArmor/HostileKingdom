﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

public class Hero : PlayableUnitBehaviour
{
    [Header("Skills")]
    [SerializeField]
    float selectTarget;
    float currentSelectTarget;
    public bool isDoingSkill;

    [SerializeField]
    Transform skillCircle;
    float maxDistanceHero = Mathf.Infinity;

    [SerializeField]
    LayerMask attackMask;
    [SerializeField]
    LayerMask circleMask;
    [SerializeField]
    float skillCircleRadius;

    Collider[] hitColliders;

    public bool isUpdatingCirclePosition;

    void Start()
    {
        UnitStart(); 
        currentSelectTarget = selectTarget; 
    }

    void Update()
    {
        if (isDoingSkill)
        {
            SkillUpdate();
            return;
        }
        if (isUpdatingCirclePosition)
        {
            skillCircle.GetComponent<SpriteRenderer>().enabled = true;
            CirclePositionUpdate();
        }
        else
        {
            skillCircle.GetComponent<SpriteRenderer>().enabled = false;
        }
        UnitUpdate();
    }

    void CirclePositionUpdate()
    {
        Ray ray = mainCamera.ScreenPointToRay(InputManager.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, maxDistanceHero, circleMask))
        {
            skillCircle.position = new Vector3(hit.point.x, skillCircle.position.y, hit.point.z); 
        }
    }

    void SelectTarget()
    {
        currentSelectTarget -= Time.deltaTime;
        if (currentSelectTarget < 0)
        {
            SkillUpdate();
        }
    }

    void SkillUpdate()
    {
        hitColliders = Physics.OverlapSphere(skillCircle.position, skillCircleRadius, attackMask);
        for (int i = 0; i < hitColliders.Length; i++)
        {
            hitColliders[i].GetComponent<EnemyBehaviour>().TakeDamage(attack*5);
            if (hitColliders[i].GetComponent<EnemyBehaviour>().hitPoints <= 0)
            {
                EnemyDies();
                return;
            }
        }
        hitColliders = null;
        currentSelectTarget = selectTarget;
        isDoingSkill = false;
        isUpdatingCirclePosition = false;
        Debug.Log("Skill Damage!");
    }

    void OnDrawGizmos()
    {
        Color newColor = Color.magenta;
        newColor.a = 0.2f;
        Gizmos.color = newColor;
        Gizmos.DrawSphere(skillCircle.position, skillCircleRadius);
    }
}
