using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public sealed class Timer : MonoBehaviour
{
    [SerializeField] private float _duration = 60f;
    [SerializeField] private float _timeToEnd;
    private float _timerFillCount;
    private Image _timerImage;

    public event System.Action<bool> OnTimerStateChange;
    private bool _isActive;
    private bool _pause;

    void Start()
    {
        _timeToEnd = _duration;
        _timerImage = GetComponent<Image>();
    }

    public void StartCountdown()
    {
        StartCoroutine(StartCountdownCorutine());
    }
    public void RefreshTimer()
    {
        _timeToEnd = _duration;
        StopAllCoroutines(); // clear previous timer courutine if game was restarted
        StartCountdown();
    }
    public void PauseTimer(bool state)
    {
        _pause = state;
    }

    private IEnumerator StartCountdownCorutine()
    {
        _isActive = true;
        OnTimerStateChange(_isActive);
        while (_timeToEnd > 0)
        {
            yield return new WaitForFixedUpdate();
            if (_pause)
                continue;
            _timeToEnd -= Time.fixedDeltaTime;
            _timerFillCount = _timeToEnd / _duration;
            _timerImage.fillAmount = _timerFillCount;
        }
        _isActive = false;
        OnTimerStateChange(_isActive);
    }
}
