using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager_MainMenu : MBSingleton<UIManager_MainMenu>
{
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject credits;
    [SerializeField] private GameObject tutorial;

    private void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;

    private void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadMode)
    {
        // ...
    }

    public void GoToGameplay()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(1);
    }

    public void CloseGame() => Application.Quit();

    #region Element Visibility
    public void SetMenuPanelVisibility(bool newVisibility) => menuPanel.SetActive(newVisibility);

    public void SetCreditsVisibility(bool newVisibility) => credits.SetActive(newVisibility);

    public void SetTutorialVisibility(bool newVisibility) => tutorial.SetActive(newVisibility);
    #endregion
}