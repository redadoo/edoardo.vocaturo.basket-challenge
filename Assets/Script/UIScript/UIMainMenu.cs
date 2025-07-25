using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.Collections.Generic;

public enum MenuPage
{
    None,
    Main,
    Settings,
    SelectGameType,
    SelectCampType,
    MatchInfo,
    MatchResult
}

public class UIMainMenu : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button gameTypeButton;
    [SerializeField] private Button campTypeButton;
    [SerializeField] private Button backButton;

    [Header("Menu Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject gameTypePanel;
    [SerializeField] private GameObject campTypePanel;
    [SerializeField] private GameObject matchInfoPanel;
    [SerializeField] private GameObject matchResultPanel;

    [Header("3d Model")]
    [SerializeField] private GameObject playerModel;

    [Header("Currency")]
    [SerializeField] private TMP_Text moneyText;
    [SerializeField] private TMP_Text goldText;

    private Dictionary<MenuPage, GameObject> menuPanels;
    private Stack<MenuPage> navigationStack = new();
    private MenuPage currentPage = MenuPage.None;

    private void Awake()
    {
        menuPanels = new Dictionary<MenuPage, GameObject>()
        {
            { MenuPage.Main, mainMenuPanel },
            { MenuPage.SelectGameType, gameTypePanel },
            { MenuPage.SelectCampType, campTypePanel },
            { MenuPage.MatchInfo, matchInfoPanel },
            { MenuPage.MatchResult, matchResultPanel }
        };
    }

    private void OnEnable()
    {
        GameManager.Instance.OnMoneyChange += OnMoneyChange;
    }

    private void OnDisable()
    {
        GameManager gm = GameManager.TryGetInstance();
        if (gm != null)
            gm.OnMoneyChange -= OnMoneyChange;
    }

    private void Start()
    {
        SetButtons();
        NavigateTo(MenuPage.Main, false);
    }

    private void SetButtons()
    {
        playButton.onClick.AddListener(() => NavigateTo(MenuPage.SelectGameType));
        gameTypeButton.onClick.AddListener(() => NavigateTo(MenuPage.SelectCampType));
        campTypeButton.onClick.AddListener(() => NavigateTo(MenuPage.MatchInfo));
        backButton.onClick.AddListener(OnBackPressed);
    }

    public void NavigateTo(MenuPage newPage, bool clearHistory = false)
    {
        if (currentPage == newPage)
            return;

        if (clearHistory) navigationStack.Clear();
        else navigationStack.Push(currentPage);

        currentPage = newPage;
        UpdateUI();
    }

    private void UpdateUI()
    {
        foreach (var val in menuPanels)
            val.Value.SetActive(val.Key == currentPage);
        
        playerModel.SetActive(currentPage == MenuPage.Main || currentPage == MenuPage.MatchInfo);
        backButton.gameObject.SetActive(currentPage != MenuPage.Main);
    }

    private void OnBackPressed()
    {
        if (navigationStack.Count > 0)
        {
            MenuPage previousPage = navigationStack.Pop();
            currentPage = previousPage;
            UpdateUI();
        }
    }

    private void OnMoneyChange()
    {
        moneyText.text = GameManager.Instance.money.ToString();
        goldText.text = GameManager.Instance.gold.ToString();
    }

    #region Editor Helpers

    [ContextMenu("GoToMain")]
    private void GoToMain() => 
        NavigateTo(MenuPage.Main, clearHistory: true);

    [ContextMenu("GoToSelectGameType")]
    private void GoToSelectGameType() =>
        NavigateTo(MenuPage.SelectGameType);

    [ContextMenu("GoToSelectCampType")]
    private void GoToSelectCampType() => 
        NavigateTo(MenuPage.SelectCampType);

    #endregion
}
