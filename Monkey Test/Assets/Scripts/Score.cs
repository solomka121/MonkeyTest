using UnityEngine;
using TMPro;

public sealed class Score : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreNumber;
    [SerializeField] private AnimationCurve UpdateScoreAnimationCurve;
    public int score { get; private set; }

    public void AddScore(int value)
    {
        score += value;
        UpdateScoreValue();
    }
    public void ResetScore()
    {
        score = 0;
        _scoreNumber.text = score.ToString();
    }
    public void UpdateScoreValue()
    {
        _scoreNumber.text = score.ToString();
        LeanTween.scale(_scoreNumber.gameObject , new Vector3(1.4f , 1.4f , 1) , 0.1f).setEase(UpdateScoreAnimationCurve);
    }
}
