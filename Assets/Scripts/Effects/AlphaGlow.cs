using UnityEngine;
using System.Collections;

public class AlphaGlow : MonoBehaviour {
	public SpriteRenderer SpriteRendererWithAlpha;
	public float SecondsToFade = 1.0f;
	public float MinimumAlpha = 0.0f;
	float frame;
	// Use this for initialization
	void Start () {
		frame = SecondsToFade;
	}
	
	// Update is called once per frame
	void Update () {
		SecondsToFade = Mathf.Max (SecondsToFade, 0.1f);
		frame -= Time.deltaTime;
		if (frame < -SecondsToFade)
						frame += SecondsToFade * 2;
		float newAlpha = MinimumAlpha + (1 - MinimumAlpha) * (Mathf.Abs (frame) / SecondsToFade);
		Color c = SpriteRendererWithAlpha.color;
		c = new Color (c.r, c.g, c.b, newAlpha);
		SpriteRendererWithAlpha.color = c;
	}
}
