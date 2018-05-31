using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chicken : Enemy
{
    bool startDieCounter; 
    float dieCounter = 1.5f;

    public override void PooledStart()
    {
        dieCounter = 1.5f; 
        base.PooledStart();
    }

    public override void SetDead()
    {
        if (!isDead)
        {
            die = true;
            EnemyWaveManager.enemiesAlive--;
            Player.money += moneyValue;
            audioPlayer.PlaySFX(0);
            base.SetDead();
            startDieCounter = true; 
        }
        else
        {
            DeadUpdate();
        }
        base.SetDead();
    }

    protected override void MyUpdate()
    {
        if (startDieCounter)
        {
            dieCounter -= Time.deltaTime;
            if (dieCounter <= 0)
            {
                startDieCounter = false;
                gameObject.SetActive(false);
            }
        }
        base.MyUpdate();
    }
}
