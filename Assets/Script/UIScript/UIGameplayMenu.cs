using UnityEngine;
using UnityEngine.UI;

public class UIGameplayMenu : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject menuPageUI;
    [SerializeField] private GameObject leavePageUI;

    [Header("Buttons")]
    [SerializeField] private Button menuButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button leaveMatchButton;
    [SerializeField] private Button quitMatchButton;
    [SerializeField] private Button keepPlayingButton;

    private void Start()
    {
        RegisterButtonCallbacks();
    }

    /// <summary>
    /// Assigns UI button click event handlers.
    /// </summary>
    private void RegisterButtonCallbacks()
    {
        menuButton.onClick.AddListener(ToggleMenuPanel);
        closeButton.onClick.AddListener(HideMenuPanel);
        optionsButton.onClick.AddListener(OpenOptionsMenu);
        leaveMatchButton.onClick.AddListener(ShowLeaveConfirmation);
        quitMatchButton.onClick.AddListener(QuitMatch);
        keepPlayingButton.onClick.AddListener(CloseAllPanels);
    }

    /// <summary>
    /// Toggles the visibility of the menu panel.
    /// </summary>
    private void ToggleMenuPanel()
    {
        if (leavePageUI.activeSelf)
        {
            leavePageUI.SetActive(false);
            return;
        }

        menuPageUI.SetActive(!menuPageUI.activeSelf);
    }

    /// <summary>
    /// Hides the menu panel.
    /// </summary>
    private void HideMenuPanel()
    {
        menuPageUI.SetActive(false);
    }

    /// <summary>
    /// Placeholder for options menu logic.
    /// </summary>
    private void OpenOptionsMenu()
    {
        // TODO: Implement options menu logic
    }

    /// <summary>
    /// Shows the leave confirmation panel and hides the menu.
    /// </summary>
    private void ShowLeaveConfirmation()
    {
        menuPageUI.SetActive(false);
        leavePageUI.SetActive(true);
    }

    /// <summary>
    /// Loads the main menu scene.
    /// </summary>
    private void QuitMatch()
    {
        CloseAllPanels();
        LoadingSceneManager.Instance.LoadScene(Scene.MainMenu);
    }

    /// <summary>
    /// Hides all panels and resumes gameplay.
    /// </summary>
    private void CloseAllPanels()
    {
        leavePageUI.SetActive(false);
        menuPageUI.SetActive(false);
    }
}
