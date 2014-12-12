using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveStateManager : MonoBehaviour
{
    public static SaveStateManager data;
    void Awake()
    {
        if (data == null)
            data = this;
    }
    void OnLevelWasLoaded(int level)
    {
    }
}

