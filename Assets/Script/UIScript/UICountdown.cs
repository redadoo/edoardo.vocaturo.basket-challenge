using System.Collections;
using UnityEngine;
using System;
using TMPro;
using Utility;

public class UICountdown : GenericSingleton<UICountdown>
{
    [SerializeField] private TMP_Text countdownText;

    public event Action OnCooldownEnd;


    private void Start()
    {
        StartCoroutine(CountdownSequence());
    }

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
