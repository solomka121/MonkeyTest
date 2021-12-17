using UnityEngine;
using TMPro;

public sealed class DifficultyLevel : MonoBehaviour
{
    private TMP_Text _difficultyText;
    private Animator _animator;
    private int _difficulty;
    void Start()
    {
        _animator = GetComponent<Animator>();
        _difficultyText = GetComponent<TMP_Text>();
    }
    public void ChangeDifficulty(int value)
    {
        int temp = _difficulty;
        _difficulty = value;
        UpdateDifficultyValue(temp);
    }

    public void SetDifficulty(int difficulty)
    {
        _difficulty = difficulty;
        UpdateText();
    }

    public void UpdateDifficultyValue(int previousDifficulty)
    {
        if (_difficulty > previousDifficulty)
        {
            _animator.SetTrigger("DifficultyIncrease");
        }
        else
        {
            _animator.SetTrigger("DifficultyDecrease");
        }
        UpdateText();
    }

    private void UpdateText()
    {
        _difficultyText.text = "lvl : " + _difficulty.ToString();
    }
}
