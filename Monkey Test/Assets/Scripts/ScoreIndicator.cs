using UnityEngine;
using TMPro;

public sealed class ScoreIndicator : MonoBehaviour
{
    [SerializeField] private Vector2 indicatorOffset;
    public LeanTweenType easeScaleOutType;
    [SerializeField] private float _scaleOutTime;
    public LeanTweenType easeScaleInType;
    [SerializeField] private float _scaleInTime;
    private TMP_Text _indicatorText;
    private CanvasGroup _canvasGroup;
    private RectTransform _rectTransform;
    void Awake()
    {
        _indicatorText = GetComponent<TMP_Text>();
        _canvasGroup = GetComponent<CanvasGroup>();
        _rectTransform = GetComponent<RectTransform>();
        transform.localScale = Vector2.zero;
    }

    public void StartAction(int scoreIndicator)
    {
        _indicatorText.text ="+" + scoreIndicator.ToString();
        _canvasGroup.alpha = 0;
        LeanTween.alphaCanvas(_canvasGroup, 1, _scaleOutTime / 2);
        transform.position += new Vector3(indicatorOffset.x, indicatorOffset.y, 0f);
        LeanTween.scale(gameObject , Vector2.one , _scaleOutTime).setEase(easeScaleOutType).setOnComplete(ShrinkIn);
    }

    private void ShrinkIn()
    {
        LeanTween.scale(gameObject, Vector2.zero, _scaleInTime).setEase(easeScaleInType).setOnComplete(Disable);
    }

    private void Disable()
    {
        gameObject.SetActive(false);
    }
}
