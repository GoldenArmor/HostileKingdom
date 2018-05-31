using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class PopupText : MonoBehaviour, IPooledObject
{
    [SerializeField]
    Animator anim;  
    [SerializeField]
    Text text;

    float totalDamage;
    bool isBeingDamaged;
    bool beginEndCounter; 
    float endCounter;
    [SerializeField]
    Animation fallText;
    [SerializeField]
    Animation scaledText; 
    int triggerHashValue;

    void Awake()
    {
        //anim = GetComponentInChildren<Animator>() as Animator; 
        //triggerHashValue = Animator.StringToHash("EndDamage");
        //anim.enabled = true; 
    }

    public void PooledAwake()
    {
        gameObject.SetActive(true); 
    }

    public void PooledStart()
    {
        //anim = GetComponentInChildren<Animator>() as Animator;
        //triggerHashValue = Animator.StringToHash("EndDamage");
        //anim.enabled = true;
        scaledText.Play(); 
        endCounter = 1.5f;  
    }

    void Update()
    {
        if (beginEndCounter || transform.parent == null)
        {
            endCounter -= Time.deltaTime; 
            if (endCounter <= 0)
            {
                totalDamage = 0;
                beginEndCounter = false;
                gameObject.SetActive(false);
            }
        }
    }

    public void SetDamage(float damage)
    {
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

    public void ClearDamage()
    {
        Vector3 oldPosition = transform.parent.position; 
        transform.SetParent(null);
        transform.position = oldPosition; 
        //transform.localScale = Vector3.one; 
        //anim.SetTrigger(triggerHashValue);
        scaledText.Stop();
        fallText.Play(); 
        beginEndCounter = true;
    }

    public bool IsActive()
    {
        return gameObject.activeSelf; 
    }
}
