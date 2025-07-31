using System.Collections;
using UnityEngine;
using Utility;
using System;
using TMPro;

namespace UIScript
{
    public class UICountdown : GenericSingleton<UICountdown>
    {
        [Header("UI Components")]
        [Tooltip("Text component to display the countdown numbers.")]
        [SerializeField] private TMP_Text countdownText;

        public event Action OnCooldownEnd;

        private void Start()
        {
            StartCoroutine(CountdownSequence());
        }

        /// <summary>
        /// Runs a 3-second countdown, updating the UI each second.
        /// </summary>
        /// <returns>Coroutine enumerator.</returns>
        private IEnumerator CountdownSequence()
        {
            countdownText.text = "3";
            yield return new WaitForSeconds(1f);

            countdownText.text = "2";
            yield return new WaitForSeconds(1f);

            countdownText.text = "1";
            yield return new WaitForSeconds(1f);

            OnCooldownEnd?.Invoke();

            gameObject.SetActive(false);
        }
    }
}
