using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPhase : MonoBehaviour
{
    public virtual void InternalStart()
    {
    }

    public virtual void InternalUpdate()
    {
    }

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
        // 2/4 ataque de garras
        // 3/4 caen bolas del aire
    }
}
