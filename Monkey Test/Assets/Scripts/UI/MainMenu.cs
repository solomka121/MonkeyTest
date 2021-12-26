using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu : PopUpPanel
{
    [Header("Panels")]
    public PopUpPanel mainButtonsPanel;
    [Header("Buttons")]
    public Button play;
    public Toggle Volume;
    public Button themes;
    [SerializeField] private ShowUpGroup buttonsGroup;

    private CanvasGroup _canvasGroup;
    private PopUpPanel ActivePanel;

    void Start()
    {
        ActivePanel = mainButtonsPanel;

        play.onClick.AddListener(PlayGame);

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
