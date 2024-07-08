using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance != null)
                return instance;

            if (instance == null)
            {
                instance = FindObjectOfType<T>();
            }

            if (instance != null)
            {
                if(FindObjectsOfType<T>().Length > 1)
                {
                    Debug.LogError($"Multiple {typeof(T).Name} is Running!");
                }

                return instance;
            }

            return null;
        }
    }

}
