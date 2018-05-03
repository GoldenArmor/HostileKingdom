using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class PopupText : MonoBehaviour, IPooledObject
{
    [SerializeField]
    Animation anim; 
    [SerializeField]
    Text text;

    float totalDamage; 
    
    public void PooledAwake()
    {
        gameObject.SetActive(true); 
    }

    public void PooledStart()
    {
    }

    void Update()
    {
        if (!anim.isPlaying || transform.parent == null)
        {
            totalDamage = 0;
            anim.Stop(); 
            transform.SetParent(null); 
            gameObject.SetActive(false); 
        }
    }

    public void SetTextAnim(float damage)
    {
        if(!anim.isPlaying)
        {
            anim.Play();
        }

        if (damage > 0)
        {
            text.text = ((int)totalDamage + (int)damage).ToString();
        }
        else
        {
            text.text = (totalDamage + damage).ToString();
        }
        totalDamage += damage; 
    }

    public bool IsActive()
    {
        return gameObject.activeSelf; 
    }
}
