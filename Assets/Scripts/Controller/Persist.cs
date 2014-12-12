using UnityEngine;
using System.Collections;

public class Persist : MonoBehaviour
{
    static Persist singleton;
    void Awake()
    {
        if (singleton == null)
        {
            DontDestroyOnLoad(this);
            singleton = this;
        } else if (singleton != this)
            Destroy(this.gameObject);
    }

}
