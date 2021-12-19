using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu : PopUpPanel
{
    [Header("Panels")]
    public PopUpPanel mainButtonsPanel;
    //private CanvasGroup _mainButtonsCanvasGroup;
    public PopUpPanel optionsPanel;
    public Button optionsPanelBackButton;
    //private CanvasGroup _optionsCanvasGroup;
    [Header("Buttons")]
    public Button play;
    public Button options;
    public Button shop;
    public Button themes;
    public Button quit;

    private CanvasGroup _canvasGroup;
    private PopUpPanel ActivePanel;

    void Start()
    {
        ActivePanel = mainButtonsPanel;

        play.onClick.AddListener(PlayGame);
        options.onClick.AddListener(OpenOptionsMenu);
        optionsPanelBackButton.onClick.AddListener(OpenMainButtonsMenu);
        quit.onClick.AddListener(Quit);
        //_mainButtonsCanvasGroup = mainButtonsPanel.GetComponent<CanvasGroup>();
        //_optionsCanvasGroup = optionsPanel.GetComponent<CanvasGroup>();

        _canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            ActivePanel = mainButtonsPanel;
        }
    }

    private void ChangeActivePanel(PopUpPanel panel)
    {
        ActivePanel.ShrinkIn();
        ActivePanel = panel;
        ActivePanel.ShrinkOut();
    }

    private void OpenMainButtonsMenu()
    {
        ChangeActivePanel(mainButtonsPanel);
    }

    private void OpenOptionsMenu()
    {
        ChangeActivePanel(optionsPanel);
    }

    private void PlayGame()
    {
        //background.LeanAlpha(0, 0.1f).setEase(LeanAlphaType);
        ShrinkIn();
        StartCoroutine(LoadGameScene());
    }

    private IEnumerator LoadGameScene()
    {
        yield return new WaitForSeconds(timeToShrinkIn);

        SceneManager.LoadScene(1);
    }

    private void Quit()
    {
        Application.Quit();
    }

}
