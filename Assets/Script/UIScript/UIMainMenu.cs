using UnityEngine.UI;
using UnityEngine;
using TMPro;

public enum MenuPage
{
    Main,
    SelectGameType,
    SelectCampType,
    LoadGame
}

public class UIMainMenu : MonoBehaviour, IDataPersistence
{
    [SerializeField] private MenuPage currentPage;

    [Header("Buttons")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button gameTypeButton;
    [SerializeField] private Button campTypeButton;
    [SerializeField] private Button backButton;

    [Header("Menu Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject gameTypePanel;
    [SerializeField] private GameObject campTypePanel;

    [Header("3d Model")]
    [SerializeField] private GameObject playerModel;

    [Header("Currency")]
    [SerializeField] private TMP_Text moneyText;
    [SerializeField] private TMP_Text goldText;

    private void Start()
    {
        playButton.onClick.AddListener(() => ChangeMenuPage(MenuPage.SelectGameType));
        gameTypeButton.onClick.AddListener(() => ChangeMenuPage(MenuPage.SelectCampType));
        campTypeButton.onClick.AddListener(() => ChangeMenuPage(MenuPage.LoadGame));
        backButton.onClick.AddListener(() => GetBackPage());

        ChangeMenuPage(currentPage);
    }

    private void ChangeMenuPage(MenuPage newPage)
    {
        currentPage = newPage;

        if (currentPage == MenuPage.LoadGame)
            LoadingScene.Instance.LoadScene(1);
        
        playerModel.SetActive(currentPage == MenuPage.Main);
        backButton.gameObject.SetActive(currentPage != MenuPage.Main);
        mainMenuPanel.SetActive(currentPage == MenuPage.Main);
        gameTypePanel.SetActive(currentPage == MenuPage.SelectGameType);
        campTypePanel.SetActive(currentPage == MenuPage.SelectCampType);
    }

    private void GetBackPage()
    {
        switch (currentPage)
        {
            case MenuPage.SelectGameType:
                ChangeMenuPage(MenuPage.Main);
                break;
            case MenuPage.SelectCampType:
                ChangeMenuPage(MenuPage.SelectGameType);
                break;
            case MenuPage.LoadGame:
                ChangeMenuPage(MenuPage.SelectCampType);
                break;
            default:
                break;
        }
    }

    public void LoadData(GameData data)
    {
        moneyText.text = data.money.ToString();
        goldText.text = data.gold.ToString();
    }

    public void SaveData(ref GameData data)
    {
        if (!int.TryParse(moneyText.text, out int money))
        {
            Debug.LogWarning($"Failed to parse money: '{moneyText.text}'. set to 0.");
            money = 0;
        }

        if (!int.TryParse(goldText.text, out int gold))
        {
            Debug.LogWarning($"Failed to parse gold: '{goldText.text}'. set to 0.");
            gold = 0;
        }

        data.money = money;
        data.gold = gold;
    }
}
