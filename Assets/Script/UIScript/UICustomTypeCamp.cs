using UnityEngine;
using UnityEngine.UI;

namespace UIScript
{
    /// <summary>
    /// UI component for customizing a new camp type by selecting difficulty and duration.
    /// </summary>
    public class UICustomTypeCamp : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Slider difficultySlider;
        [SerializeField] private Slider durationSlider;
        [SerializeField] private Button startButton;

        [Header("Match Configuration")]
        [Tooltip("Array of available duration options (in seconds).")]
        [SerializeField] private int[] durationOptions = { 40, 60, 80, 120 };

        private int selectedDurationIndex;
        private int difficultyIndex;

        private void Start()
        {
            difficultySlider.onValueChanged.AddListener(OnDifficultySliderChanged);
            durationSlider.onValueChanged.AddListener(OnDurationSliderChanged);
            startButton.onClick.AddListener(OnStartButtonClicked);

            OnDifficultySliderChanged(difficultySlider.value);
            OnDurationSliderChanged(durationSlider.value);
        }

        /// <summary>
        /// Called when the difficulty slider value changes.
        /// </summary>
        /// <param name="value">The new difficulty slider value.</param>
        private void OnDifficultySliderChanged(float value)
        {
            difficultyIndex = Mathf.RoundToInt(value);
        }

        /// <summary>
        /// Called when the duration slider value changes.
        /// </summary>
        /// <param name="value">The new duration slider value.</param>
        private void OnDurationSliderChanged(float value)
        {
            selectedDurationIndex = Mathf.RoundToInt(value);
        }

        /// <summary>
        /// Called when the Start button is clicked. Sends the selected configuration to the GameManager.
        /// </summary>
        private void OnStartButtonClicked()
        {
            int selectedDuration = durationOptions[selectedDurationIndex];
            GameManager.Instance.CreateAndAssignCampType(selectedDuration, difficultyIndex);
            UIMainMenu.Instance.NavigateTo(MenuPage.MatchInfo);
        }
    }

}