using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasingList : BaseEasing
{
    void Start()
    {
        if(randomDelay) delayStart = Random.Range(minDelay, maxDelay);

        currentTime = durationTime;
        deltaValue = finalValue - initialValue; 
        
        switch(property)
        {
            case Property.POSITION:
                transform.localPosition = initialValue; 
                break;
            case Property.ROTATION:
                transform.localRotation = Quaternion.Euler(initialValue); 
                break;
            case Property.SCALE:
                transform.localScale = initialValue; 
                break;
            default:
                break;
        }
    }

    void Update()
    {
        if(currentTime >= durationTime)
        {
            currentTime = durationTime;
            if (animLoop == true)
            {
                currentTime = 0;
            }
        }
        //if(currentTime < durationTime && !cooldown.isActive)
        //{
        //    DoEasing();
        //}
    }

    protected override void DoEasing()
    {
        switch (type) 
        {
            case EasingType.LINEAR:
                easingValue = new Vector3(
                Easing.Linear(currentTime, initialValue.x, deltaValue.x, durationTime),
                Easing.Linear(currentTime, initialValue.y, deltaValue.y, durationTime),
                Easing.Linear(currentTime, initialValue.z, deltaValue.z, durationTime));
                break;
            case EasingType.EXPOOUT:
                easingValue = new Vector3(
                Easing.ExpoEaseOut(currentTime, initialValue.x, deltaValue.x, durationTime),
                Easing.ExpoEaseOut(currentTime, initialValue.y, deltaValue.y, durationTime),
                Easing.ExpoEaseOut(currentTime, initialValue.z, deltaValue.z, durationTime));
                break;
            case EasingType.EXPOIN:
                easingValue = new Vector3(
                Easing.ExpoEaseIn(currentTime, initialValue.x, deltaValue.x, durationTime),
                Easing.ExpoEaseIn(currentTime, initialValue.y, deltaValue.y, durationTime),
                Easing.ExpoEaseIn(currentTime, initialValue.z, deltaValue.z, durationTime));
                break;
            case EasingType.EXPOINOUT:
                easingValue = new Vector3(
                Easing.ExpoEaseInOut(currentTime, initialValue.x, deltaValue.x, durationTime),
                Easing.ExpoEaseInOut(currentTime, initialValue.y, deltaValue.y, durationTime),
                Easing.ExpoEaseInOut(currentTime, initialValue.z, deltaValue.z, durationTime));
                break;
            case EasingType.EXPOOUTIN:
                easingValue = new Vector3(
                Easing.ExpoEaseOutIn(currentTime, initialValue.x, deltaValue.x, durationTime),
                Easing.ExpoEaseOutIn(currentTime, initialValue.y, deltaValue.y, durationTime),
                Easing.ExpoEaseOutIn(currentTime, initialValue.z, deltaValue.z, durationTime));
                break;
            case EasingType.CIRCOUT:
                easingValue = new Vector3(
                Easing.CircEaseOut(currentTime, initialValue.x, deltaValue.x, durationTime),
                Easing.CircEaseOut(currentTime, initialValue.y, deltaValue.y, durationTime),
                Easing.CircEaseOut(currentTime, initialValue.z, deltaValue.z, durationTime));
                break;
            case EasingType.CIRCIN:
                easingValue = new Vector3(
                Easing.CircEaseIn(currentTime, initialValue.x, deltaValue.x, durationTime),
                Easing.CircEaseIn(currentTime, initialValue.y, deltaValue.y, durationTime),
                Easing.CircEaseIn(currentTime, initialValue.z, deltaValue.z, durationTime));
                break;
            case EasingType.CIRCINOUT:
                easingValue = new Vector3(
                Easing.CircEaseInOut(currentTime, initialValue.x, deltaValue.x, durationTime),
                Easing.CircEaseInOut(currentTime, initialValue.y, deltaValue.y, durationTime),
                Easing.CircEaseInOut(currentTime, initialValue.z, deltaValue.z, durationTime));
                break;
            case EasingType.CIRCOUTIN:
                easingValue = new Vector3(
                Easing.CircEaseOutIn(currentTime, initialValue.x, deltaValue.x, durationTime),
                Easing.CircEaseOutIn(currentTime, initialValue.y, deltaValue.y, durationTime),
                Easing.CircEaseOutIn(currentTime, initialValue.z, deltaValue.z, durationTime));
                break;
            case EasingType.QUADOUT:
                easingValue = new Vector3(
                Easing.QuadEaseOut(currentTime, initialValue.x, deltaValue.x, durationTime),
                Easing.QuadEaseOut(currentTime, initialValue.y, deltaValue.y, durationTime),
                Easing.QuadEaseOut(currentTime, initialValue.z, deltaValue.z, durationTime));
                break;
            case EasingType.QUADIN:
                easingValue = new Vector3(
                Easing.QuadEaseIn(currentTime, initialValue.x, deltaValue.x, durationTime),
                Easing.QuadEaseIn(currentTime, initialValue.y, deltaValue.y, durationTime),
                Easing.QuadEaseIn(currentTime, initialValue.z, deltaValue.z, durationTime));
                break;
            case EasingType.QUADINOUT:
                easingValue = new Vector3(
                Easing.QuadEaseInOut(currentTime, initialValue.x, deltaValue.x, durationTime),
                Easing.QuadEaseInOut(currentTime, initialValue.y, deltaValue.y, durationTime),
                Easing.QuadEaseInOut(currentTime, initialValue.z, deltaValue.z, durationTime));
                break;
            case EasingType.QUADOUTIN:
                easingValue = new Vector3(
                Easing.QuadEaseOutIn(currentTime, initialValue.x, deltaValue.x, durationTime),
                Easing.QuadEaseOutIn(currentTime, initialValue.y, deltaValue.y, durationTime),
                Easing.QuadEaseOutIn(currentTime, initialValue.z, deltaValue.z, durationTime));
                break;
            case EasingType.SINEOUT:
                easingValue = new Vector3(
                Easing.SineEaseOut(currentTime, initialValue.x, deltaValue.x, durationTime),
                Easing.SineEaseOut(currentTime, initialValue.y, deltaValue.y, durationTime),
                Easing.SineEaseOut(currentTime, initialValue.z, deltaValue.z, durationTime));
                break;
            case EasingType.SINEIN:
                easingValue = new Vector3(
                Easing.SineEaseIn(currentTime, initialValue.x, deltaValue.x, durationTime),
                Easing.SineEaseIn(currentTime, initialValue.y, deltaValue.y, durationTime),
                Easing.SineEaseIn(currentTime, initialValue.z, deltaValue.z, durationTime));
                break;
            case EasingType.SINEINOUT:
                easingValue = new Vector3(
                Easing.SineEaseInOut(currentTime, initialValue.x, deltaValue.x, durationTime),
                Easing.SineEaseInOut(currentTime, initialValue.y, deltaValue.y, durationTime),
                Easing.SineEaseInOut(currentTime, initialValue.z, deltaValue.z, durationTime));
                break;
            case EasingType.SINEOUTIN:
                easingValue = new Vector3(
                Easing.SineEaseOutIn(currentTime, initialValue.x, deltaValue.x, durationTime),
                Easing.SineEaseOutIn(currentTime, initialValue.y, deltaValue.y, durationTime),
                Easing.SineEaseOutIn(currentTime, initialValue.z, deltaValue.z, durationTime));
                break;
            case EasingType.CUBICOUT:
                easingValue = new Vector3(
                Easing.CubicEaseOut(currentTime, initialValue.x, deltaValue.x, durationTime),
                Easing.CubicEaseOut(currentTime, initialValue.y, deltaValue.y, durationTime),
                Easing.CubicEaseOut(currentTime, initialValue.z, deltaValue.z, durationTime));
                break;
            case EasingType.CUBICIN:
                easingValue = new Vector3(
                Easing.CubicEaseIn(currentTime, initialValue.x, deltaValue.x, durationTime),
                Easing.CubicEaseIn(currentTime, initialValue.y, deltaValue.y, durationTime),
                Easing.CubicEaseIn(currentTime, initialValue.z, deltaValue.z, durationTime));
                break;
            case EasingType.CUBICINOUT:
                easingValue = new Vector3(
                Easing.CubicEaseInOut(currentTime, initialValue.x, deltaValue.x, durationTime),
                Easing.CubicEaseInOut(currentTime, initialValue.y, deltaValue.y, durationTime),
                Easing.CubicEaseInOut(currentTime, initialValue.z, deltaValue.z, durationTime));
                break;
            case EasingType.CUBICOUTIN:
                easingValue = new Vector3(
                Easing.CubicEaseOutIn(currentTime, initialValue.x, deltaValue.x, durationTime),
                Easing.CubicEaseOutIn(currentTime, initialValue.y, deltaValue.y, durationTime),
                Easing.CubicEaseOutIn(currentTime, initialValue.z, deltaValue.z, durationTime));
                break;
            case EasingType.QUARTOUT:
                easingValue = new Vector3(
                Easing.QuartEaseOut(currentTime, initialValue.x, deltaValue.x, durationTime),
                Easing.QuartEaseOut(currentTime, initialValue.y, deltaValue.y, durationTime),
                Easing.QuartEaseOut(currentTime, initialValue.z, deltaValue.z, durationTime));
                break;
            case EasingType.QUARTIN:
                easingValue = new Vector3(
                Easing.QuartEaseIn(currentTime, initialValue.x, deltaValue.x, durationTime),
                Easing.QuartEaseIn(currentTime, initialValue.y, deltaValue.y, durationTime),
                Easing.QuartEaseIn(currentTime, initialValue.z, deltaValue.z, durationTime));
                break;
            case EasingType.QUARTINOUT:
                easingValue = new Vector3(
                Easing.QuartEaseInOut(currentTime, initialValue.x, deltaValue.x, durationTime),
                Easing.QuartEaseInOut(currentTime, initialValue.y, deltaValue.y, durationTime),
                Easing.QuartEaseInOut(currentTime, initialValue.z, deltaValue.z, durationTime));
                break;
            case EasingType.QUARTOUTIN:
                easingValue = new Vector3(
                Easing.QuartEaseOutIn(currentTime, initialValue.x, deltaValue.x, durationTime),
                Easing.QuartEaseOutIn(currentTime, initialValue.y, deltaValue.y, durationTime),
                Easing.QuartEaseOutIn(currentTime, initialValue.z, deltaValue.z, durationTime));
                break;
            case EasingType.QUINTOUT:
                easingValue = new Vector3(
                Easing.QuintEaseOut(currentTime, initialValue.x, deltaValue.x, durationTime),
                Easing.QuintEaseOut(currentTime, initialValue.y, deltaValue.y, durationTime),
                Easing.QuintEaseOut(currentTime, initialValue.z, deltaValue.z, durationTime));
                break;
            case EasingType.QUINTIN:
                easingValue = new Vector3(
                Easing.QuintEaseIn(currentTime, initialValue.x, deltaValue.x, durationTime),
                Easing.QuintEaseIn(currentTime, initialValue.y, deltaValue.y, durationTime),
                Easing.QuintEaseIn(currentTime, initialValue.z, deltaValue.z, durationTime));
                break;
            case EasingType.QUINTINOUT:
                easingValue = new Vector3(
                Easing.QuintEaseInOut(currentTime, initialValue.x, deltaValue.x, durationTime),
                Easing.QuintEaseInOut(currentTime, initialValue.y, deltaValue.y, durationTime),
                Easing.QuintEaseInOut(currentTime, initialValue.z, deltaValue.z, durationTime));
                break;
            case EasingType.QUINTOUTIN:
                easingValue = new Vector3(
                Easing.QuintEaseOutIn(currentTime, initialValue.x, deltaValue.x, durationTime),
                Easing.QuintEaseOutIn(currentTime, initialValue.y, deltaValue.y, durationTime),
                Easing.QuintEaseOutIn(currentTime, initialValue.z, deltaValue.z, durationTime));
                break;
            case EasingType.ELASTICOUT:
                easingValue = new Vector3(
                Easing.ElasticEaseOut(currentTime, initialValue.x, deltaValue.x, durationTime),
                Easing.ElasticEaseOut(currentTime, initialValue.y, deltaValue.y, durationTime),
                Easing.ElasticEaseOut(currentTime, initialValue.z, deltaValue.z, durationTime));
                break;
            case EasingType.ELASTICIN:
                easingValue = new Vector3(
                Easing.ElasticEaseIn(currentTime, initialValue.x, deltaValue.x, durationTime),
                Easing.ElasticEaseIn(currentTime, initialValue.y, deltaValue.y, durationTime),
                Easing.ElasticEaseIn(currentTime, initialValue.z, deltaValue.z, durationTime));
                break;
            case EasingType.ELASTICINOUT:
                easingValue = new Vector3(
                Easing.ElasticEaseInOut(currentTime, initialValue.x, deltaValue.x, durationTime),
                Easing.ElasticEaseInOut(currentTime, initialValue.y, deltaValue.y, durationTime),
                Easing.ElasticEaseInOut(currentTime, initialValue.z, deltaValue.z, durationTime));
                break;
            case EasingType.ELASTICOUTIN:
                easingValue = new Vector3(
                Easing.ElasticEaseOutIn(currentTime, initialValue.x, deltaValue.x, durationTime),
                Easing.ElasticEaseOutIn(currentTime, initialValue.y, deltaValue.y, durationTime),
                Easing.ElasticEaseOutIn(currentTime, initialValue.z, deltaValue.z, durationTime));
                break;
            case EasingType.BOUNCEOUT:
                easingValue = new Vector3(
                Easing.BounceEaseOut(currentTime, initialValue.x, deltaValue.x, durationTime),
                Easing.BounceEaseOut(currentTime, initialValue.y, deltaValue.y, durationTime),
                Easing.BounceEaseOut(currentTime, initialValue.z, deltaValue.z, durationTime));
                break;
            case EasingType.BOUNCEIN:
                easingValue = new Vector3(
                Easing.BounceEaseIn(currentTime, initialValue.x, deltaValue.x, durationTime),
                Easing.BounceEaseIn(currentTime, initialValue.y, deltaValue.y, durationTime),
                Easing.BounceEaseIn(currentTime, initialValue.z, deltaValue.z, durationTime));
                break;
            case EasingType.BOUNCEINOUT:
                easingValue = new Vector3(
                Easing.BounceEaseInOut(currentTime, initialValue.x, deltaValue.x, durationTime),
                Easing.BounceEaseInOut(currentTime, initialValue.y, deltaValue.y, durationTime),
                Easing.BounceEaseInOut(currentTime, initialValue.z, deltaValue.z, durationTime));
                break;
            case EasingType.BOUNCEOUTIN:
                easingValue = new Vector3(
                Easing.BounceEaseOutIn(currentTime, initialValue.x, deltaValue.x, durationTime),
                Easing.BounceEaseOutIn(currentTime, initialValue.y, deltaValue.y, durationTime),
                Easing.BounceEaseOutIn(currentTime, initialValue.z, deltaValue.z, durationTime));
                break;
            case EasingType.BACKOUT:
                easingValue = new Vector3(
                Easing.BackEaseOut(currentTime, initialValue.x, deltaValue.x, durationTime),
                Easing.BackEaseOut(currentTime, initialValue.y, deltaValue.y, durationTime),
                Easing.BackEaseOut(currentTime, initialValue.z, deltaValue.z, durationTime));
                break;
            case EasingType.BACKIN:
                easingValue = new Vector3(
                Easing.BackEaseIn(currentTime, initialValue.x, deltaValue.x, durationTime),
                Easing.BackEaseIn(currentTime, initialValue.y, deltaValue.y, durationTime),
                Easing.BackEaseIn(currentTime, initialValue.z, deltaValue.z, durationTime));
                break;
            case EasingType.BACKINOUT:
                easingValue = new Vector3(
                Easing.BackEaseInOut(currentTime, initialValue.x, deltaValue.x, durationTime),
                Easing.BackEaseInOut(currentTime, initialValue.y, deltaValue.y, durationTime),
                Easing.BackEaseInOut(currentTime, initialValue.z, deltaValue.z, durationTime));
                break;
            case EasingType.BACKOUTIN:
                easingValue = new Vector3(
                Easing.BackEaseOutIn(currentTime, initialValue.x, deltaValue.x, durationTime),
                Easing.BackEaseOutIn(currentTime, initialValue.y, deltaValue.y, durationTime),
                Easing.BackEaseOutIn(currentTime, initialValue.z, deltaValue.z, durationTime));
                break;
            default:
                break;
        }

        switch(property)
        {
            case Property.POSITION:
                transform.position = easingValue; 
                break;
            case Property.ROTATION:
                transform.localRotation = Quaternion.Euler(easingValue); 
                break;
            case Property.SCALE:
                transform.localScale = easingValue; 
                break;
            default:
                break;
        }

        currentTime += Time.deltaTime;

        if(currentTime >= durationTime)
        {
            switch(property)
            {
                case Property.POSITION:
                    transform.position = finalValue;
                    break;
                case Property.ROTATION:
                    transform.localRotation = Quaternion.Euler(finalValue);
                    break;
                case Property.SCALE:
                    transform.localScale = finalValue;
                    break;
                default:
                    break;
            }

            currentTime = durationTime;

            Vector3 ini = finalValue;
            finalValue = initialValue;
            initialValue = ini;
            deltaValue = finalValue - initialValue;
 
            if (loopCounter > 0)
            {
                currentTime = 0;
                loopCounter--;
            }
            //else
            //{
            //    cooldown.Activate();
            //    loopCounter = 1; 
            //}
        }
    }
}
