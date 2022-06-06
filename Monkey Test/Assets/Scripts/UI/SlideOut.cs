using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideOut : MonoBehaviour, IShowUp, Iinitialize
{
    [SerializeField] private bool _playOnStart = false;
    [SerializeField] private RectTransform _rect;
    [SerializeField] private float _slideOutDelay;
    [SerializeField] private LeanTweenType _slideOutEase = LeanTweenType.easeSpring;
    [SerializeField] private float _timeToSlideOut = 0.4f;
    private Vector3 _startPosition;
    private Vector3 _hidenPosition;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private bool _moveX = true;
    [SerializeField] private bool _moveY = true;
    [SerializeField] private bool _reverse = false;
    [SerializeField] private bool _pingPong = false;
    public bool activate;

    public void Init()
    {
        _hidenPosition = _rect.localPosition;
        if (_moveX)
        {
            _hidenPosition.x = -transform.localPosition.x;
            _startPosition.x = transform.localPosition.x;
        }
        if (_moveY)
        {
            _hidenPosition.y = -transform.localPosition.y;
            _startPosition.y = transform.localPosition.y;
        }

        _hidenPosition += _offset;

        if (_playOnStart)
        {
            Hide();
            ShowUp();
        }
    }

    public void ShowUp()
    {
        StartCoroutine(SlideOutCorutine());
    }

    public IEnumerator SlideOutCorutine()
    {
            Hide();

        yield return new WaitForSeconds(_slideOutDelay);

        if (_moveX)
        {
            float xValue = _reverse ? -_startPosition.x: _startPosition.x;
            transform.LeanMoveLocalX(xValue, _timeToSlideOut).setEase(_slideOutEase);
        }
        if (_moveY)
        {
            float yValue = _reverse ? -_startPosition.y : _startPosition.y; 
            transform.LeanMoveLocalY(yValue , _timeToSlideOut).setEase(_slideOutEase);
        }

        if (_pingPong)
            Reverse();
    }

    public void Hide()
    {
        if (_reverse)
            return;

        _rect.localPosition = _hidenPosition;
    }

    public void Reverse()
    {
        _reverse = !_reverse;
    }

    private void Update()
    {
        if (activate)
        {
            ShowUp();
            activate = false;
        }
    }
}
