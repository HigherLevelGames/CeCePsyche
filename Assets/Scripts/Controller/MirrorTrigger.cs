using UnityEngine;
using System.Collections;

public class MirrorTrigger : MonoBehaviour
{
	GameObject player;
	public string levelName;
	public GameObject blurb; // "What's that Sound?" thought bubble
	bool showDescription = false;

	// variables for mirrors "floating" effect
	private Vector3 pivot; // mirrors slowly move around a pivot point
	private Vector3 curDir; // current movement direction
	public float MaxDistanceFromPivot = 0.5f;
	public float speed = 0.1f;
	Vector3 RandDir
	{
		get
		{
			return (new Vector3(Random.Range(-1.0f,1.0f), Random.Range(-1.0f,1.0f),0.0f)).normalized;
		}
	}

	// camera zoom variables
	float xVelocity = 0.0f;
	float yVelocity = 0.0f;
	float smoothTime = 0.3f;
	
	void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player");
		pivot = this.transform.localPosition;
		curDir = RandDir;
	}

	bool isZoomingIn = false;
	void Update()
	{
		// Move around pivot point
		Vector3 newPos = this.transform.localPosition + curDir*speed*Time.deltaTime;
		if(Vector3.Distance(pivot, newPos) > MaxDistanceFromPivot)
		{
			curDir = RandDir;
		}
		else
		{
			this.transform.localPosition = newPos;
		}

		// have the audio clip fade out
		if(this.audio != null && this.audio.isPlaying)
		{
			fadeOut();
		}

		if(isZoomingIn)
		{
			// camera zoom
			Camera.main.orthographicSize -= 5.0f * (Time.deltaTime/4.0f);

			// smooth dampen camera movement towards mirror position
			Vector3 target = this.transform.position + new Vector3(0,0,-10.0f);
			float newPositionX = Mathf.SmoothDamp(Camera.main.transform.position.x, target.x, ref xVelocity, smoothTime);
			float newPositionY = Mathf.SmoothDamp(Camera.main.transform.position.y, target.y, ref yVelocity, smoothTime);
			Camera.main.transform.position = new Vector3(newPositionX, newPositionY, Camera.main.transform.position.z);
		}
	}

	void OnGUI()
	{
		if(showDescription)
		{
			string description = "Level: " + levelName;
			Vector2 dimensions = GUI.skin.box.CalcSize(new GUIContent(description));
			Vector2 location = Camera.main.WorldToScreenPoint(this.transform.position);
			Rect r = new Rect(location.x,Screen.height - location.y,dimensions.x,dimensions.y);
			GUI.Box (r, description);
		}
	}

	void OnMouseEnter()
	{
		showDescription = true;
	}

	/*
	void OnTriggerStay2D(Collider2D other)
	{
		showDescription = false;
	}//*/
	//* // I think this will be better in CeCiMenuController.cs
	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.GetComponent<CeciMenuController>().enterButton != null)
		{
			other.GetComponent<CeciMenuController>().enterButton.enabled = true;
		}
	}
	
	void OnTriggerStay2D(Collider2D other)
	{
		showDescription = false;
		if (Input.GetKeyDown(KeyCode.Return))
		{
			loadTheLevel();
		}
	}
	
	void OnTriggerExit2D(Collider2D other)
	{
		if(other.GetComponent<CeciMenuController>().enterButton != null)
		{
			other.GetComponent<CeciMenuController>().enterButton.enabled = false;
		}
	}
	//*/

	void OnMouseExit()
	{
		showDescription = false;
	}

	void OnMouseDown()
	{
		player.SendMessage("GoTo", this.gameObject);
	}
	
	void loadTheLevel()
	{
		if(this.audio != null)
		{
			audioStartTime = Time.time;
			this.audio.Play();
		}
		if(blurb != null)
		{
			blurb.renderer.enabled = true;
		}

		isZoomingIn = true;
		if(Application.CanStreamedLevelBeLoaded(levelName))
		{
			Invoke("Load", (this.audio == null) ? 0.0f : 3.0f);//this.audio.clip.length); // invokes Load() in 3 seconds
		}
		else
		{
			Debug.Log("Error: level " + levelName + "cannot be loaded");
		}
	}

	void Load()
	{
		Application.LoadLevel(levelName);
	}

	float audioStartTime;
	void fadeOut()
	{
		this.audio.volume = Mathf.Lerp(1.0f,0.0f,(Time.time-audioStartTime)/3.0f);//this.audio.clip.length);
	}
}
