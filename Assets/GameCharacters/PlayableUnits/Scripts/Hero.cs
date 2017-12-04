using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : PlayableUnitBehaviour
{
    float selectTarget;
    float currentSelectTarget;
    bool isDoingSkill = true;

    [SerializeField]
    float skillRadius;
    [SerializeField]
    Transform skillCircle;
    float maxDistance = Mathf.Infinity;

    LayerMask mask; 

    void Update()
    {
        if (isDoingSkill)
        {
            SkillUpdate();
            return;
        }

        UnitUpdate();
    }

    void SkillUpdate()
    {
        Vector3 center = Camera.main.ScreenToWorldPoint(InputManager.mousePosition);
        skillCircle.position = new Vector3 (center.x, skillCircle.position.y, center.y);

        Ray ray = Camera.main.ScreenPointToRay(InputManager.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, maxDistance, mask))
        {
            skillCircle.position = new Vector3(center.x, 0, center.z); 
        }
    }

    void SelectTarget()
    {
        currentSelectTarget -= Time.deltaTime;
        if (currentSelectTarget < 0)
        {
            AttackUpdate();
        }
    }
}
