using UnityEngine;
using System.Collections;

public class MirrorTrigger : MonoBehaviour
{
	#region Variables
	public string levelName;
	public Renderer enterButton; // "Press Enter to Begin" png
	public GameObject blurb; // "What's that Sound?" thought bubble
	private bool showDescription = false;
	private GameObject player; // reference to player

	// variables for mirrors "floating" effect
	private Vector3 pivot; // mirrors slowly move around a pivot point
	private Vector3 curDir; // current movement direction
	public float MaxDistanceFromPivot = 0.5f;
	public float speed = 0.1f;
	Vector3 RandDir
	{
		get
		{
			return (new Vector3(Random.Range(-1.0f,1.0f), Random.Range(-1.0f,1.0f), 0.0f)).normalized;
		}
	}

	// camera zoom variables
	bool isZoomingIn = false;
	float xVelocity = 0.0f;
	float yVelocity = 0.0f;
	float smoothTime = 0.3f;
	#endregion
	
	void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player");
		pivot = this.transform.localPosition;
		curDir = RandDir;
	}

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

	#region Mouse Events
	void OnMouseEnter()
	{
		showDescription = true;
	}

	void OnMouseExit()
	{
		showDescription = false;
	}
	
	void OnMouseDown()
	{
		player.SendMessage("GoTo", this.gameObject, SendMessageOptions.DontRequireReceiver);
	}
	#endregion

	#region Trigger Events
	void OnTriggerEnter2D(Collider2D other)
	{
		if(enterButton != null)
		{
			enterButton.enabled = true;
		}
	}
	
	void OnTriggerStay2D(Collider2D other)
	{
		showDescription = false;
		if (Input.GetButtonDown("Interact") && other.gameObject.tag == "Player")
		{
			loadingSequence();
		}
	}
	
	void OnTriggerExit2D(Collider2D other)
	{
		if(enterButton != null)
		{
			enterButton.enabled = false;
		}
	}
	#endregion

	void loadingSequence()
	{
		if(Application.CanStreamedLevelBeLoaded(levelName))
		{
			isZoomingIn = true; // camera zoom
			if(this.audio != null)
			{
				audioStartTime = Time.time;
				this.audio.Play();
			}
			if(blurb != null)
			{
				blurb.renderer.enabled = true;
			}
			Invoke("Load", (this.audio == null) ? 0.0f : timeToFade);//this.audio.clip.length);
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

	float audioStartTime = 0.0f;
	float timeToFade = 3.0f; // number of seconds it takes for the audio clip to fade out
	void fadeOut()
	{
		this.audio.volume = Mathf.Lerp(1.0f,0.0f,(Time.time-audioStartTime)/timeToFade);//this.audio.clip.length);
	}
}
