using UnityEngine;

namespace _Main.Scripts.Utils
{
    public abstract class BaseSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance != null) return _instance;
           
                _instance = FindObjectOfType<T>();

                if (_instance != null) return _instance;
              
                var singletonObject = new GameObject(typeof(T).Name);
                _instance = singletonObject.AddComponent<T>();
                DontDestroyOnLoad(singletonObject);

                return _instance;
            }
        }

        protected virtual void Awake()
        {
            if (_instance == null) _instance = this as T;
            else if (_instance != this) Destroy(gameObject);
        }
    }
}