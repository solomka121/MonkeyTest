using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public sealed class GameOver : PopUpPanel
{
    [SerializeField] private Timer _timer;

    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _highScoreText;
    [SerializeField] private AnimationCurve HighScoreBump;
    [SerializeField] private Button _restartButton;
    private CanvasGroup _restartButtonCanvasGroup;
    private bool NeedToUpdateHighScore;

    protected override void Awake()
    {
        base.Awake();

        startPosition = new Vector2(0, -Screen.height * 2);

        _restartButton.onClick.AddListener(RestartGame);
        _restartButtonCanvasGroup = _restartButton.GetComponent<CanvasGroup>();
        _restartButtonCanvasGroup.alpha = 0;
        _restartButtonCanvasGroup.blocksRaycasts = false;

        gameObject.SetActive(false);
    }

    public void EndGame(int score , int highScore , bool UpdateHighScore)
    {
        ShrinkOut();
        StartCoroutine(AnimateScore(score , highScore));
        NeedToUpdateHighScore = UpdateHighScore;
    }

    public override void ShrinkOut()
    {
        base.ShrinkOut();

        // Reset 
        _restartButtonCanvasGroup.alpha = 0f;
        _restartButtonCanvasGroup.blocksRaycasts = false;
        _scoreText.text = "0";
        //
    }

    private IEnumerator AnimateScore(int score , int highScore)
    {
        yield return new WaitForSeconds(timeToShrinkOut);

        float duration = 0.8f;
        _scoreText.gameObject.LeanScale(new Vector3(1.3f , 1.3f , 1) , duration).setEaseOutSine().setOnComplete(SetScoreTextToNormal);

        int currentScore = 0;
        int start = currentScore;
        for (float timer = 0; timer < duration; timer += Time.deltaTime)
        {
            float progress = timer / duration;
            currentScore = (int)Mathf.Lerp(start, score, progress);
            _scoreText.text = currentScore.ToString();
            yield return null;
        }
        _scoreText.text = score.ToString();

        if (NeedToUpdateHighScore)
            AnimateHighScoreBump(highScore);

        ActivateRestartButton();
    }

    private void AnimateHighScoreBump(int highScore)
    {
        _highScoreText.gameObject.LeanScale(new Vector3(1.5f, 1.5f, 1), 0.4f).setEase(HighScoreBump);
        _highScoreText.text = highScore.ToString();
    }

    public void SetHighScore(int highScore)
    {
        _highScoreText.text = highScore.ToString();
    }

    private void SetScoreTextToNormal()
    {
        _scoreText.gameObject.LeanScale( Vector3.one, 0.3f).setEaseOutBack();
    }

    private void RestartGame()
    {
        _timer.RefreshTimer();
        ShrinkIn();
    }

    private void ActivateRestartButton()
    {
        _restartButtonCanvasGroup.alpha = 0;
        _restartButtonCanvasGroup.blocksRaycasts = true;
        LeanTween.alphaCanvas(_restartButtonCanvasGroup, 1 , 0.6f).setEaseOutSine();
    }
}
