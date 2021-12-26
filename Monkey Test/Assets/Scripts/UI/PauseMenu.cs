using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public sealed class PauseMenu : PopUpPanel , Iinitialize
{
    //public Button pause; // do manually
    public Button restart;
    public Toggle music;
    public Button exit;
    public Button resume;

    public event Action<bool> SetOnPause;
    public event Action Restart;

    public void Init()
    {
        startPosition = new Vector2(0, Screen.width * 2);

        //pause.onClick.AddListener(PauseGame);
        restart.onClick.AddListener(RestartGame);
        resume.onClick.AddListener(ResumeGame);
        exit.onClick.AddListener(ExitGame);

        gameObject.SetActive(false);
    }

    public void PauseGame()
    {
        SetOnPause?.Invoke(true);
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
        SetOnPause?.Invoke(false);
        ShrinkIn();
    }
}
