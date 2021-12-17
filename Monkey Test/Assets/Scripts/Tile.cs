using UnityEngine;
using UnityEngine.UI;
using TMPro;

public sealed class Tile : MonoBehaviour
{
    [SerializeField] private TMP_Text _tileText;
    [SerializeField] private Button _coverButton;

    [Header("Animation data")]
    public LeanTweenType textShrinkOutType;
    public float textShrinkOutDuration;
    public LeanTweenType textShrinkInType;
    public float textShrinkInDuration;
    public AnimationCurve coverButtonShrinkOutType;
    public float coverButtonShrinkOutDuration;
    public AnimationCurve coverButtonShrinkInType;
    public float coverButtonShrinkInDuration;

    public bool textAppear;
    public bool start;
    public bool isOpen { get; private set; } = true;
    public int order { get => _order; }
    private int _order;

    private CanvasGroup _canvasGroup;
    private Animator _animator;

    private GameField _gameField;
    private ObjectPool _scoreIndicatorsPool;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _canvasGroup = GetComponent<CanvasGroup>();
    }
    private void Start()
    {
        CoverButtonAddStandartListeners();
    }

    private void FixedUpdate()
    {
        if (start)
        {
            if(textAppear)
                TextShrinkOut(_tileText.gameObject, textShrinkOutType, textShrinkOutDuration);
            else
                TextShrinkIn(_tileText.gameObject, textShrinkInType, textShrinkInDuration);
            start = false;
        }
    }

    public void Inject(GameField manager , ObjectPool indicatorsPool)
    {
        _gameField = manager;
        _scoreIndicatorsPool = indicatorsPool;
    }

    public void SetOrder(int num)
    {
        _coverButton.onClick.AddListener(CallCheckOrder);

        _order = num;
        num++;
        _tileText.text = num.ToString();
        TextShrinkOut(_tileText.gameObject , textShrinkOutType , textShrinkOutDuration);
    }

    public void ShowScoreIndicator(int score)
    {
        ScoreIndicator scoreIndicator = _scoreIndicatorsPool.GetPooledObject();
        scoreIndicator.transform.position = transform.position;
        scoreIndicator.gameObject.SetActive(true);
        scoreIndicator.StartAction(score);
    }

    private void CallCheckOrder()
    {
        _gameField.CheckOrder(_order);
    }

    public void AnimateTextDisappear()
    {
        TextShrinkIn(_tileText.gameObject, textShrinkInType, textShrinkInDuration);
    }

    public void AnimateSpawn()
    {
        Render(true);
        _animator.SetTrigger("TileSpawn");
    }

    public void AnimateHide()
    {
        _animator.SetTrigger("TileHide");
    }

    public void AnimateShake(int flag)
    {
        _animator.SetBool("StopShake" , false);
        _animator.SetTrigger("TileShake");
        _animator.SetInteger("ShakeFlag", flag);
    }

    public void Open()
    {
        if (isOpen)
            return;

        CoverButtonShrinkIn();
        isOpen = true;
    }
    
    public void Close()
    {
        if (!isOpen)
            return;

        CoverButtonShrinkOut();
        isOpen = false;
    }

    private void TextShrinkOut(GameObject objectToShrink, LeanTweenType animeType, float duration)
    { 
        Vector3 _vectorToShrink;
        objectToShrink.transform.localScale = Vector3.zero;
        _vectorToShrink = Vector3.one;

        LeanTween.scale(objectToShrink.gameObject, _vectorToShrink, duration).setEase(animeType);
    }
    private void TextShrinkIn(GameObject objectToShrink , LeanTweenType animeType , float duration)
    {
        Vector3 _vectorToShrink;
        objectToShrink.transform.localScale = Vector3.one;
        _vectorToShrink = Vector3.zero;

        LeanTween.scale(objectToShrink.gameObject , _vectorToShrink, duration).setEase(animeType).setOnComplete(ResetText);
    }
    private void CoverButtonShrinkOut()
    {
        _canvasGroup.blocksRaycasts = true;
        _coverButton.gameObject.SetActive(true);
        _coverButton.gameObject.transform.localScale = Vector3.zero;
        LeanTween.scale(_coverButton.gameObject , Vector3.one , coverButtonShrinkOutDuration).setEase(coverButtonShrinkOutType);
    }
    private void CoverButtonShrinkIn()
    {
        _canvasGroup.blocksRaycasts = false;
        _coverButton.gameObject.SetActive(true);
        _coverButton.gameObject.transform.localScale = Vector3.one;
        LeanTween.scale(_coverButton.gameObject , Vector3.zero , coverButtonShrinkInDuration).setEase(coverButtonShrinkInType)
            .setOnComplete(CoverButtonDisable);
    }

    public void CoverButtonDisable()
    {
        _coverButton.gameObject.SetActive(false);
    }

    public void SetRenderMode(int mode) // animation event
    {
        bool state = mode == 1 ? true : false;
        Render(state);
    }

    public void ResetText() // animation event
    {
        _tileText.text = "";
    }

    public void StopShake() // animation event
    {
        _animator.SetBool("StopShake", true);
    }

    private void CoverButtonAddStandartListeners()
    {
        _coverButton.onClick.AddListener(Open);
    }

    private void CoverButtonRemoveAllListeners()
    {
        _coverButton.onClick.RemoveAllListeners();
    }

    public void Reset()
    {
        ClearListeners();
        AnimateTextDisappear();
        Open();
    }

    public void ClearListeners()
    {
        CoverButtonRemoveAllListeners();
        CoverButtonAddStandartListeners();
    }

    public void Render(bool show)
    {
        if (show)
        {
            _canvasGroup.alpha = 1;
            _canvasGroup.blocksRaycasts = true;
        }
        else
        {
            _canvasGroup.alpha = 0f;
            _canvasGroup.blocksRaycasts = false;
        }
    }
}
