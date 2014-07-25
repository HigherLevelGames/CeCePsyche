using UnityEngine;
using System.Collections;

public class BreakableWall : MonoBehaviour
{
	// Use this for initialization
	void Start () { }
	
	// Update is called once per frame
	void Update () { }

	void OnCollisionStay2D(Collision2D col)
	{
		if(col.gameObject.tag == "Player")
		{
			if(col.gameObject.GetComponent<AbilityManager>().canBreak)
			{
				Destroy(this.gameObject);
				// play breaking SFX
				// play breaking anim
			}
		}
	}
}
