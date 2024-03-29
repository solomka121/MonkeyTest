﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkOut : MonoBehaviour , IShowUp , Iinitialize
{
    public bool playOnStart = false;
    public float showUpDelay = 0f;
    public LeanTweenType shrinkOutEase = LeanTweenType.easeSpring;
    public float timeToShrinkOut = 0.4f;
    public bool shrinkX = true;
    public bool shrinkY = true;
    private Vector3 startScale;
    private Vector3 shrinkedScale;

    public void Init()
    {
        startScale = transform.localScale;
        shrinkedScale = Vector3.one;
        if (shrinkX)
            shrinkedScale.x = 0;
        if (shrinkY)
            shrinkedScale.y = 0;

        if(playOnStart)
        {
            Hide();
            ShowUp();
        }
    }

    public void Hide()
    {
        transform.localScale = shrinkedScale;
    }

    public void ShowUp()
    {
        StartCoroutine(ShrinkOutWithDelay(showUpDelay));
    }

    public IEnumerator ShrinkOutWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (shrinkX)
            LeanTween.scaleX(gameObject, startScale.x, timeToShrinkOut).setEase(shrinkOutEase);
        if (shrinkY)
            LeanTween.scaleY(gameObject, startScale.y, timeToShrinkOut).setEase(shrinkOutEase);
    }
}
