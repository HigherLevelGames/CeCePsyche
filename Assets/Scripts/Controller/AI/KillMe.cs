using UnityEngine;
using System.Collections;

public class KillMe : MonoBehaviour {
    bool alive = true;
	public void SelfDestruct()
    {
        if (alive)
        {
            Destroy(this.gameObject);
            alive = false;
        }
    }
}
