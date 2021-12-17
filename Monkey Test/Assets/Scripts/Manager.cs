using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Manager : MonoBehaviour
{
    public GameField gameField;
    public InfoPanel infoPanel;
    public GameOver gameOver;
    public PauseMenu pauseMenu;

    private PlayerHighScore _playerScore;
    private int _localGameHighScore;

    private void Start()
    {

        _playerScore = SaveSystem.LoadScore();
        if (_playerScore == null)
        {
            _playerScore = new PlayerHighScore(infoPanel.score);
        }
        _localGameHighScore = _playerScore.HighScore;        
        gameOver.SetHighScore(_localGameHighScore);

        gameField.OnEndGameFieldClear += EndGame;
        pauseMenu.SetOnPause += gameField.PauseGame;
        pauseMenu.Restart += gameField.RestartAndRun;
        pauseMenu.Restart += infoPanel.timer.RefreshTimer;
        pauseMenu.SetOnPause += infoPanel.timer.PauseTimer;
    }

    private void EndGame()
    {
        if(infoPanel.score.score > _localGameHighScore)
        {
            gameOver.EndGame(infoPanel.score.score, infoPanel.score.score , true);
            _localGameHighScore = infoPanel.score.score;
            SaveSystem.SaveScore(infoPanel.score);
        }
        else
            gameOver.EndGame(infoPanel.score.score, _localGameHighScore , false);
    }

    private void OnDisable()
    {
        gameField.OnEndGameFieldClear -= EndGame;
        pauseMenu.SetOnPause -= gameField.PauseGame;
        pauseMenu.Restart -= gameField.RestartAndRun;
        pauseMenu.Restart -= infoPanel.timer.RefreshTimer;
        pauseMenu.SetOnPause -= infoPanel.timer.PauseTimer;
    }
}
