using UnityEngine.UI;
using UnityEngine;
using System;

public class CampTypeUISystem : MonoBehaviour
{
    [SerializeField] private Image leftArrow;
    [SerializeField] private Image rightArrow;

    [SerializeField] private Button leftButton;
    [SerializeField] private Button rightButton;

    [SerializeField] private ScrollRect scrollRect;


    private void Start()
    {
        scrollRect.onValueChanged.AddListener(OnScrollChanged);
    }

    private void OnScrollChanged(Vector2 arg0)
    {
        print(arg0);
    }

    public void ChangeArrow(int indexContent)
    {
        if (indexContent == 0)
            leftArrow.gameObject.SetActive(false);
    }
}
