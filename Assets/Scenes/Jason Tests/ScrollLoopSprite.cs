using UnityEngine;
using System.Collections;

public class ScrollLoopSprite : MonoBehaviour {

	public SpriteRenderer Render1, Render2;
	public float speed = 0.3f;
	float offsetx1, offsetx2;

	void Start () {
		offsetx2 = -0.5f;
	}

	void Update () {
		offsetx1 += Time.deltaTime * speed;
		offsetx2 += Time.deltaTime * speed;
		if (offsetx2 > 0.5f)
			offsetx2 -= 1.0f;
		if (offsetx1 > 0.5f)
			offsetx1 -= 1.0f;
		Render1.material.mainTextureOffset = new Vector2 (offsetx1, 0);
		Render2.material.mainTextureOffset = new Vector2 (offsetx2, 0);
	}
}