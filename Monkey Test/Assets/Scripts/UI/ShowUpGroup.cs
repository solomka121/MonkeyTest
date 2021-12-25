using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowUpGroup : MonoBehaviour
{
    [SerializeField] private bool _playOnEnable = true;
    [SerializeField] private float _startDelay;
    [SerializeField] private float _delayBetweenShrinks;
    [SerializeField] private List<IShrinkOut> _shrinkOutList = new List<IShrinkOut>();

    private void Awake()
    {
        _shrinkOutList.AddRange(GetComponentsInChildren<IShrinkOut>());
    }

    public void GroupShrinkOut()
    {
        StartCoroutine(GroupShrinkOutCorutine());
    }

    public IEnumerator GroupShrinkOutCorutine()
    {   
        foreach (IShrinkOut shrink in _shrinkOutList)
            shrink.Hide();

        yield return new WaitForSeconds(_startDelay);

        foreach (IShrinkOut shrink in _shrinkOutList)
        {
            shrink.ShrinkOut();
            yield return new WaitForSeconds(_delayBetweenShrinks);
        }
    }

    private void OnEnable()
    {
        if(_playOnEnable)
            GroupShrinkOut();
    }
}
