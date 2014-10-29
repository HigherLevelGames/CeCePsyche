using UnityEngine;
using System.Collections;

public class Destructible : MonoBehaviour {
	public GameObject destroyedBy;
	public float decayRate = 1f;
	private bool isDestroyed;
	private float alphaLvl = 1f;
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		if(isDestroyed)
		{
			alphaLvl -= Time.deltaTime * decayRate;
			SpriteRenderer thisSprite = this.gameObject.GetComponent<SpriteRenderer>();
			thisSprite.color = new Color(1f,1f,1f,alphaLvl);
			if (alphaLvl < 0)
			{
				Destroy(this.gameObject);
			}

		}
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		GameObject stuff = col.gameObject;
		if(stuff.name.Length >= 8)
		{
			string last7 = stuff.name.Substring(stuff.name.Length-7);
			if(last7 == "(Clone)")
			{
				stuff.name = stuff.name.Substring(0,stuff.name.Length-7); // remove "(Clone)" at end of name
			}
		}
		if(stuff.ToString() == destroyedBy.ToString())
		{
			isDestroyed = true;
			this.gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
			this.gameObject.GetComponent<Collider2D>().isTrigger = true;

		}
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		GameObject stuff = col.gameObject;
		if(stuff.name.Length > 7)
			stuff.name = stuff.name.Substring(0,stuff.name.Length-7); // remove "(Clone)" at end of name

		if(stuff.ToString() == destroyedBy.ToString())
		{
			isDestroyed = true;
		}
	}
}
