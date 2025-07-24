using System;
using UnityEngine;

public class BasketTrigger : MonoBehaviour
{
    public event Action OnTriggered;

    private void OnTriggerEnter(Collider other)
    {
        OnTriggered?.Invoke();
    }
}
