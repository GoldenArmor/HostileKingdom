using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

public class Hero : PlayableUnitBehaviour
{
    [SerializeField]
    float selectTarget;
    float currentSelectTarget;
    bool isDoingSkill = true;

    [SerializeField]
    float skillRadius;
    [SerializeField]
    Transform skillCircle;
    float maxDistanceHero = Mathf.Infinity;

    [SerializeField]
    LayerMask heroMask;
    [SerializeField]
    float skillCircleRadius;

    Collider[] hitColliders;

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

        UnitUpdate();
    }

    void CirclePositionUpdate()
    {
        Ray ray = Camera.main.ScreenPointToRay(InputManager.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, maxDistanceHero, mask))
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
        hitColliders = Physics.OverlapSphere(skillCircle.position, skillCircleRadius, heroMask);
        for (int i = 0; i < hitColliders.Length; i++)
        {
            hitColliders[i].GetComponent<EnemyBehaviour>().TakeDamage(attack*5);
        }
        hitColliders = null;
        currentSelectTarget = selectTarget;
        isDoingSkill = false; 
    }
}
