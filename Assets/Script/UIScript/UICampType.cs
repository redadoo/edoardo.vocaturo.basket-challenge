using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles the UI for selecting a camp type in the main menu.
/// </summary>
public class UICampType : MonoBehaviour
{
    [Header("Camp Selection Buttons")]
    [SerializeField] private List<Button> campButtons;

    [Header("Create Match Type Button")]
    [SerializeField] private Button createMatchTypeButton;

    private void Start()
    {
        for (int i = 0; i < campButtons.Count; i++)
        {
            if (campButtons[i] != null)
            {
                AssignCampButton(i, campButtons[i]);
            }
            else
            {
                Debug.LogWarning($"Camp button at index {i} is null.");
            }
        }

        if (createMatchTypeButton != null)
        {
            createMatchTypeButton.onClick.AddListener(() =>
                UIMainMenu.Instance.NavigateTo(MenuPage.CreateMatchType));
        }
        else
        {
            Debug.LogWarning("CreateMatchTypeButton is not assigned.");
        }
    }

    /// <summary>
    /// Assigns the logic to a camp button based on its index.
    /// </summary>
    private void AssignCampButton(int index, Button button)
    {
        button.onClick.AddListener(() =>
        {
            if (!GameManager.Instance.HasEnoughMoneyForFee())
                return;

            GameManager.Instance.SetMatchInfo(index);
            UIMainMenu.Instance.NavigateTo(MenuPage.MatchInfo);
        });
    }
}
