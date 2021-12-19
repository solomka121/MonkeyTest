using UnityEngine;

public class PopUpPanel : MonoBehaviour
{
    protected bool DisableAfterShring;
    public LeanTweenType LeanAlphaType = LeanTweenType.easeOutCubic;
    public AnimationCurve ShrinkOutType;
    public float timeToShrinkOut = 1f;
    public LeanTweenType ShrinkInType = LeanTweenType.easeInCubic;
    public float timeToShrinkIn = 0.5f;
    public Transform panel;
    public CanvasGroup background;

    [SerializeField] protected Vector2 startPosition;

    protected virtual void Awake()
    {
        //gameObject.SetActive(false);
    }

    public virtual void ShrinkOut()
    {
        background.alpha = 0;
        panel.transform.localPosition = startPosition;

        if(DisableAfterShring)
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
        if(DisableAfterShring)
            gameObject.SetActive(false);
    }
}
