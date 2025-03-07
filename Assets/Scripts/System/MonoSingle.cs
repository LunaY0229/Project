using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingle<T> : MonoBehaviour where T : Component
{
    protected static T instance;
    public static T Instance 
    {
        get
        {
            if(instance == null)
            {
                GameObject t = new GameObject(typeof(T).Name);
                instance = t.AddComponent<T>();
            }

            return instance;
        }
    }

    public static T GetInstance()
    {
        return Instance;
    }
}
