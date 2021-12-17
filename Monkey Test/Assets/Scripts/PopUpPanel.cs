using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PopUpPanel : MonoBehaviour
{
    public LeanTweenType LeanAlphaType;
    public AnimationCurve ShrinkOutType;
    public float timeToShrinkOut = 2f;
    public LeanTweenType ShrinkInType;
    public float timeToShrinkIn = 1f;
    public Transform panel;
    public CanvasGroup background;

    protected Vector2 startPosition;

    protected virtual void Awake()
    {
        gameObject.SetActive(false);
        SetStartPosition();
    }

    protected abstract void SetStartPosition();

    protected virtual void ShrinkOut()
    {
        background.alpha = 0;
        panel.transform.localPosition = startPosition;

        gameObject.SetActive(true);
        background.LeanAlpha(1, timeToShrinkOut).setEase(LeanAlphaType);
        panel.LeanMoveLocal(Vector2.zero, timeToShrinkOut).setEase(ShrinkOutType);
    }

    public void ShrinkIn()
    {
        background.LeanAlpha(0, timeToShrinkIn).setEaseLinear();
        panel.LeanMoveLocal(startPosition, timeToShrinkIn).setEase(ShrinkInType).setOnComplete(DisablePanel);
    }

    public void DisablePanel()
    {
        gameObject.SetActive(false);
    }
}
