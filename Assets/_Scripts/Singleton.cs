using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Component
{
    protected static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<T>();
            return instance;
        }
    }

    [SerializeField] protected bool dontDestroyOnLoad = false;

    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
        }
        else if (instance != this as T)
        {
            Destroy(gameObject);
            return;
        }

        Init();

        if (dontDestroyOnLoad)
            DontDestroyOnLoad(gameObject);
    }

    protected virtual void Init() { }
    protected virtual void PrepareToDestroy() { }

    protected virtual void OnDestroy()
    {
        if (instance == this as T)
        {
            PrepareToDestroy();
            instance = null;
        }
    }
}
