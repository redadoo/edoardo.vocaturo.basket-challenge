using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using AudioSystem;
using Utility;

public enum MenuPage
{
    None,
    Main,
    Settings,
    SelectGameType,
    SelectCampType,
    CreateMatchType,
    MatchInfo,
    MatchResult
}

public class UIMainMenu : GenericSingleton<UIMainMenu>
{
    [Header("Scripts")]
    [SerializeField] private UIMatchResult uIMatchResult;

    [Header("Buttons")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button gameTypeButton;
    [SerializeField] private Button backButton;

    [Header("Menu Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject gameTypePanel;
    [SerializeField] private GameObject campTypePanel;
    [SerializeField] private GameObject createCampTypePanel;
    [SerializeField] private GameObject matchInfoPanel;
    [SerializeField] private GameObject matchResultPanel;

    [Header("3D Model")]
    [SerializeField] private GameObject playerModel;

    [Header("Currency UI")]
    [SerializeField] private TMP_Text moneyText;
    [SerializeField] private TMP_Text goldText;

    [Header("Sound Data")]
    [SerializeField] private SoundData navigationSoundData;
    [SerializeField] private SoundData backButtonSoundData;

    private Dictionary<MenuPage, GameObject> menuPanels;
    private readonly Stack<MenuPage> navigationStack = new();
    private MenuPage currentPage = MenuPage.None;

    private void OnEnable()
    {
        SetupMenuPanelDictionary();

        if (GameManager.TryGetInstance() != null)
            GameManager.Instance.OnMoneyChange += OnMoneyChange;

        if (LoadingSceneManager.TryGetInstance() != null)
            LoadingSceneManager.Instance.OnSceneChange += OnSceneChange;
    }

    private void Start()
    {
        SetButtons();
        NavigateTo(MenuPage.Main, clearHistory: true);
    }

    private void OnDisable()
    {
        if (GameManager.TryGetInstance() != null)
            GameManager.Instance.OnMoneyChange -= OnMoneyChange;

        if (LoadingSceneManager.TryGetInstance() != null)
            LoadingSceneManager.Instance.OnSceneChange -= OnSceneChange;
    }

    private void SetupMenuPanelDictionary()
    {
        menuPanels = new Dictionary<MenuPage, GameObject>
        {
            { MenuPage.Main, mainMenuPanel },
            { MenuPage.SelectGameType, gameTypePanel },
            { MenuPage.SelectCampType, campTypePanel },
            { MenuPage.CreateMatchType, createCampTypePanel },
            { MenuPage.MatchInfo, matchInfoPanel },
            { MenuPage.MatchResult, matchResultPanel }
        };
    }

    private void SetButtons()
    {
        playButton.onClick.AddListener(() => NavigateTo(MenuPage.SelectGameType));
        gameTypeButton.onClick.AddListener(() => NavigateTo(MenuPage.SelectCampType));
        backButton.onClick.AddListener(OnBackPressed);
    }

    public void NavigateTo(MenuPage newPage, bool clearHistory = false)
    {
        if (currentPage == newPage)
            return;

        if (clearHistory)
            navigationStack.Clear();
        else if (currentPage != MenuPage.None)
            navigationStack.Push(currentPage);

        currentPage = newPage;
        UpdateUI();

        SoundManager.Instance.CreateSound()
            .WithSoundData(navigationSoundData)
            .Play();
    }

    private void UpdateUI()
    {
        foreach (var pair in menuPanels)
            pair.Value.SetActive(pair.Key == currentPage);

        playerModel.SetActive(currentPage == MenuPage.Main || currentPage == MenuPage.MatchInfo);
        backButton.gameObject.SetActive(currentPage != MenuPage.Main);

        if (currentPage == MenuPage.MatchResult)
            OnMoneyChange();
    }

    private void OnBackPressed()
    {
        if (navigationStack.Count > 0)
        {
            MenuPage previousPage = navigationStack.Pop();
            currentPage = previousPage;
            UpdateUI();

            SoundManager.Instance.CreateSound()
                .WithSoundData(backButtonSoundData)
                .Play();
        }
    }

    private void OnSceneChange(object sender, Scene e)
    {
        if (e == Scene.MainMenu)
            StartCoroutine(WaitForMatchResultAndNavigate());
    }

    private IEnumerator WaitForMatchResultAndNavigate()
    {
        yield return new WaitUntil(() => uIMatchResult != null);
        yield return null;

        NavigateTo(MenuPage.MatchResult);
    }

    private void OnMoneyChange()
    {
        moneyText.text = GameManager.Instance?.money.ToString() ?? "0";
        goldText.text = GameManager.Instance?.gold.ToString() ?? "0";
    }

    #region Editor Context Menus

    [ContextMenu("GoToMain")]
    private void GoToMain() => NavigateTo(MenuPage.Main, clearHistory: true);

    [ContextMenu("GoToSelectGameType")]
    private void GoToSelectGameType() => NavigateTo(MenuPage.SelectGameType);

    [ContextMenu("GoToSelectCampType")]
    private void GoToSelectCampType() => NavigateTo(MenuPage.SelectCampType);

    #endregion
}
