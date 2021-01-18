using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicLegAnimation : MonoBehaviour
{
    //https://www.youtube.com/watch?v=e6Gjhr1IP6w

    [SerializeField] GameObject fixedPoint;

    [SerializeField] GameObject trueTargetPoint;
    [SerializeField] GameObject targetPoint;

    [SerializeField] float maxDistanceFromTargetPoint;

    [SerializeField] LayerMask targetLayer;

    [Header("Animation")]
    [SerializeField] bool animateLeg = false;
    [SerializeField] float animationTime = 0.5f;
    float currentAnimationTime = 0;
    bool LegMoving = false;
    Vector3 oldLegPosition;
    [SerializeField] GameObject animationCurvePoint;
    [SerializeField] float curveStrength = 0.3f;
    Vector3 relativeCurvePoint;

    private void Update()
    {
        if(!LegMoving)
            RaycastFromTargetPoint();

        if(checkDistanceFromTargetPoint() && !LegMoving)
        {
            if(!animateLeg)
            { 
                fixedPoint.transform.position = targetPoint.transform.position;
            }
            else
            {
                oldLegPosition = fixedPoint.transform.position;
                relativeCurvePoint = animationCurvePoint.transform.position - oldLegPosition;
                LegMoving = true;
            }
        }

        if(LegMoving)
        {
            AnimateLeg();
        }


    }

    void RaycastFromTargetPoint()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, trueTargetPoint.transform.position - transform.position, out hit, 100.0f, targetLayer))
        {
            print("Found an object - distance: " + hit.distance);
            targetPoint.transform.position = hit.point;
        }
    }

    bool checkDistanceFromTargetPoint()
    {
        return (Vector3.Distance(targetPoint.transform.position, fixedPoint.transform.position) > maxDistanceFromTargetPoint);
    }

    void AnimateLeg()
    {
        currentAnimationTime += Time.deltaTime;
        float sinValue = currentAnimationTime / animationTime;

        if(sinValue >=1)
        {
            fixedPoint.transform.position = targetPoint.transform.position;
            currentAnimationTime = 0;
            LegMoving = false;
        }

        fixedPoint.transform.position = Vector3.Lerp(oldLegPosition, targetPoint.transform.position, sinValue);

        sinValue = Mathf.Sin(sinValue * Mathf.PI);
        fixedPoint.transform.position += relativeCurvePoint * sinValue * curveStrength;

    }


}
