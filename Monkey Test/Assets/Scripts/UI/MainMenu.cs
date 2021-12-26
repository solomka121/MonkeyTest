using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu : PopUpPanel , Iinitialize
{
    [Header("Panels")]
    public PopUpPanel mainButtonsPanel;
    [Header("Buttons")]
    public Button play;
    public Toggle Volume;
    public Button themes;
    [SerializeField] private ShowUpGroup buttonsGroup;

    private CanvasGroup _canvasGroup;
    private PopUpPanel _activePanel;

    public void Init()
    {
        _activePanel = mainButtonsPanel;
        mainButtonsPanel.gameObject.SetActive(false);

        play.onClick.AddListener(PlayGame);

        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Start()
    {
        mainButtonsPanel.ShrinkOut();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            if (_activePanel == mainButtonsPanel)
                Quit();
            else
                ChangeActivePanel(mainButtonsPanel);
        }
    }

    private void ChangeActivePanel(PopUpPanel panel)
    {
        _activePanel.ShrinkIn();
        _activePanel = panel;
        _activePanel.ShrinkOut();
    }

    private void OpenMainButtonsMenu()
    {
        ChangeActivePanel(mainButtonsPanel);
    }

    private void PlayGame()
    {
        //background.LeanAlpha(0, 0.1f).setEase(LeanAlphaType);
        buttonsGroup.GroupShowUp();
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
