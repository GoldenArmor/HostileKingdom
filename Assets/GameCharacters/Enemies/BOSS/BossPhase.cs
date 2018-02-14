using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPhase : MonoBehaviour
{
    [HideInInspector] public enum AttackBehaviour { First, Second, Third }
    public AttackBehaviour behaviour;

    [SerializeField]
    protected Boss boss; 
    
    protected bool canAttack;
    [SerializeField]
    protected float cooldown;
    float setCooldown; 

    public virtual void InternalStart()
    {
        setCooldown = cooldown; 
    }

    public virtual void InternalUpdate()
    {
        if (canAttack)
        {
            if (RandomBehaviour() == 0)
            {
                SetFirst();
            }
            else if (RandomBehaviour() == 1)
            {
                SetSecond();
            }
            else
            {
                SetThird();
            }
        }
        else if (!canAttack)
        {
            cooldown -= Time.deltaTime;

            switch (behaviour)
            {
                case AttackBehaviour.First:
                    Firstbehaviour();
                    break;
                case AttackBehaviour.Second:
                    SecondBehaviour();
                    break;
                case AttackBehaviour.Third:
                    ThirdBehaviour();
                    break;
                default:
                    break;
            }
        }
    }

    #region Updates
    protected virtual void Firstbehaviour()
    {
        // 1/4 ataque en area. 
        // 2/4 escudo
        // 3/4 spawn esqueletos
        // 4/4 muerte
    }

    protected virtual void SecondBehaviour()
    {
        // 1/4 barrido de fuego
        // 2/4 volar 
        // 3/4 barrido de fuego
    }

    protected virtual void ThirdBehaviour()
    {
        // 1/4 barrido de fuego
        // 2/4 ataque de garras
        // 3/4 caen bolas del aire
    }
    #endregion

    #region Sets
    protected virtual void SetFirst()
    {
        // 1/4 ataque en area. 
        // 2/4 escudo
        // 3/4 spawn esqueletos
        // 4/4 muerte
        behaviour = AttackBehaviour.First;
        canAttack = false;
        cooldown = setCooldown;
    }

    protected virtual void SetSecond()
    {
        // 1/4 barrido de fuego
        // 2/4 volar 
        // 3/4 barrido de fuego
        behaviour = AttackBehaviour.Second;
        canAttack = false;
        cooldown = setCooldown;
    }

    protected virtual void SetThird()
    {
        // 1/4 barrido de fuego
        // 2/4 ataque de garras
        // 3/4 caen bolas del aire
        behaviour = AttackBehaviour.Third;
        canAttack = false;
        cooldown = setCooldown;
    }
    #endregion

    protected virtual int RandomBehaviour()
    {
        return Random.Range(0,3); 
    }
}
