using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowUpGroup : MonoBehaviour , Iinitialize
{
    [SerializeField] private bool _playOnEnable = true;
    [SerializeField] private float _startDelay;
    [SerializeField] private float _delayBetweenShrinks;
    [SerializeField] private bool _findObjectsOnStart = true;
    [SerializeField] private SlideOut[] _slides;
    [SerializeField] private ShrinkOut[] _shrinks;
    [SerializeField] private List<IShowUp> _shrinkOutList = new List<IShowUp>();

    public void Init()
    {
        if (_findObjectsOnStart)
        {
            _shrinkOutList.AddRange(GetComponentsInChildren<IShowUp>());
        }
        else
        {
            _shrinkOutList.AddRange(_slides);
            _shrinkOutList.AddRange(_shrinks);
        }

    }

    public void GroupShowUp()
    {
        StartCoroutine(GroupShrinkOutCorutine());
    }

    public IEnumerator GroupShrinkOutCorutine()
    {   
        foreach (IShowUp shrink in _shrinkOutList)
            shrink.Hide();

        yield return new WaitForSeconds(_startDelay);

        foreach (IShowUp shrink in _shrinkOutList)
        {
            shrink.ShowUp();
            yield return new WaitForSeconds(_delayBetweenShrinks);
        }
    }

    private void OnEnable()
    {
        if(_playOnEnable)
            GroupShowUp();
    }
}
