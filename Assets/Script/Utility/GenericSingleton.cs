using UnityEngine;

public class GenericSingleton<T> : MonoBehaviour where T : Component
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<T>();

            return instance;
        }
    }

    [SerializeField]
    private bool dontDestroyOnLoad = false;

    public virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;

            if (dontDestroyOnLoad)
                DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
}