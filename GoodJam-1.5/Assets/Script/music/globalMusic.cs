using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalMusic : MonoBehaviour
{
    static GlobalMusic _instance;
    public static GlobalMusic instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GlobalMusic>();
                DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else if (this != _instance)
        {
            Destroy(gameObject);
        }
    }
}
