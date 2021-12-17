using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public sealed class PauseMenu : PopUpPanel
{
    public Button pause;
    public Button restart;
    public Button options;
    public Button exit;
    public Button resume;

    public event Action<bool> IsPaused;
    public event Action Restart;

    protected override void Awake()
    {
        base.Awake();

        pause.onClick.AddListener(PauseGame);
        restart.onClick.AddListener(RestartGame);
        resume.onClick.AddListener(ResumeGame);
        exit.onClick.AddListener(ExitGame);
    }
    protected override void SetStartPosition()
    {
        startPosition = new Vector2(0, Screen.width);
    }

    public void PauseGame()
    {
        IsPaused?.Invoke(true);
        ShrinkOut();
    }
    public void RestartGame()
    {
        ResumeGame();
        Restart?.Invoke();
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void ResumeGame()
    {
        IsPaused?.Invoke(false);
        ShrinkIn();
    }
}
