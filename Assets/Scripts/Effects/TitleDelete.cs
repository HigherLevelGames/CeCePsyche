using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GUIText))]
public class TitleDelete : MonoBehaviour
{
	public Font font;
	private float TimeToDisappear = 5.0f;

	void Start()
	{
		this.transform.position = new Vector3(0.5f, 1.0f, 0.0f);
		this.guiText.text = "Level " + (Application.loadedLevel - 1) + ": " + Application.loadedLevelName;
		this.guiText.color = Color.blue + Color.red;// + Color.magenta;
		this.guiText.anchor = TextAnchor.MiddleCenter;
		this.guiText.alignment = TextAlignment.Center;
		this.guiText.fontSize = 100;
		if(font != null)
		{
			this.guiText.font = font;
		}
	}

	// Update is called once per frame
	void Update ()
	{
		FadeOut();
		this.transform.position += Vector3.down * Time.deltaTime * 0.5f/TimeToDisappear;
		if(Time.time >= TimeToDisappear)
		{
			Destroy(this.gameObject);
		}
	}

	void FadeOut()
	{
		Color c = this.guiText.color;
		c.a = Mathf.SmoothStep(1.0f,0.0f,Time.time/TimeToDisappear);
		this.guiText.color = c;
	}
}
