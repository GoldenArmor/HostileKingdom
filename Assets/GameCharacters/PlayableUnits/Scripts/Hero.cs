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
