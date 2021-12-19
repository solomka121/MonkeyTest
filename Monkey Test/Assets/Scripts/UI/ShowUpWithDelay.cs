using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowUpWithDelay : MonoBehaviour
{
    public float showUpDelay = 1f;
    public LeanTweenType shrinkOutEase = LeanTweenType.easeSpring;
    public float timeToShrinkOut = 0.4f;
    public bool shrinkX = true;
    public bool shrinkY = true;
    private Vector3 startScale;

    private void Awake()
    {
        startScale = transform.localScale;
        Vector3 scale = Vector3.one;
        if (shrinkX)
            scale.x = 0;
        if (shrinkY)
            scale.y = 0;
        transform.localScale = scale;
        StartCoroutine(ShrinkOut());
    }

    public IEnumerator ShrinkOut()
    {
        yield return new WaitForSeconds(showUpDelay);

        if (shrinkX)
            LeanTween.scaleX(gameObject, startScale.x, timeToShrinkOut).setEase(shrinkOutEase);
        if (shrinkY)
            LeanTween.scaleY(gameObject, startScale.y, timeToShrinkOut).setEase(shrinkOutEase);
    }
}
