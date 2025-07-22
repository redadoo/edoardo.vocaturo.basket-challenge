using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum MenuPage
{
    Main,
    SelectGameType,
    SelectCampType,
    LoadGame
}

public class UIMainMenu : MonoBehaviour
{
    [SerializeField] private MenuPage currentPage;

    [Header("Buttons")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button gameTypeButton;
    [SerializeField] private Button campTypeButton;

    [Header("Menu Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject gameTypePanel;
    [SerializeField] private GameObject campTypePanel;

    [Header("3d Model")]
    [SerializeField] private GameObject playerModel;

    private void Start()
    {
        playButton.onClick.AddListener(() => ChangeMenuPage(MenuPage.SelectGameType));
        gameTypeButton.onClick.AddListener(() => ChangeMenuPage(MenuPage.SelectCampType));
        campTypeButton.onClick.AddListener(() => ChangeMenuPage(MenuPage.LoadGame));

        ChangeMenuPage(currentPage);
    }

    private void ChangeMenuPage(MenuPage newPage)
    {
        currentPage = newPage;

        if (currentPage == MenuPage.LoadGame)
        {
            LoadingScene.Instance.LoadScene(1);
        }
        playerModel.SetActive(currentPage == MenuPage.Main);
        mainMenuPanel.SetActive(currentPage == MenuPage.Main);
        gameTypePanel.SetActive(currentPage == MenuPage.SelectGameType);
        campTypePanel.SetActive(currentPage == MenuPage.SelectCampType);
    }
}
