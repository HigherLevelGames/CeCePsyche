using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class AlphaGlow : MonoBehaviour {

	public float SecondsToFade = 1.0f;

    CanvasGroup group;
	float frame;
    float oneoverseconds;
	void Start () {
        group = this.GetComponent<CanvasGroup>();
		frame = SecondsToFade;
        oneoverseconds = (float)1 / SecondsToFade;
	}

	void Update () {
		frame += Time.deltaTime;
		if (frame > SecondsToFade)
						frame -= SecondsToFade;
		float a =  frame * oneoverseconds;
        float newAlpha = Mathf.Sin(a * Mathf.PI);
        group.alpha = newAlpha;
	}
}
