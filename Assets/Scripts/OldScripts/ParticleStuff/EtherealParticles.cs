using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EtherealParticles : MonoBehaviour {
	public Sprite sprite;
	public GameObject particle;
	Vector2[] pixelpositions;
	void Start () 
	{
		Color[] pixels = sprite.texture.GetPixels ();
		List<Vector2> temp = new List<Vector2> ();
		for (int i = 0; i < pixels.Length; i++) 
			if (pixels [i].a > 0.5f)
				temp.Add (new Vector2 (i % sprite.texture.width, i / sprite.texture.width));
		pixelpositions = temp.ToArray();

	}

	void Update () {
		int pixidx = (int)(Random.value * pixelpositions.Length);
		SpawnParticle (pixelpositions[pixidx]);
	}

	void SpawnParticle(Vector2 v) {
		Instantiate(particle as GameObject, this.transform.position + new Vector3(v.x, v.y, 0), this.transform.rotation);

	}
}
