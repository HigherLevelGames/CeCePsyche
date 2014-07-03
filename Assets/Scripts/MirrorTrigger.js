#pragma strict


var levelName: String; 
var enterButton: Renderer; // "Press Enter to Begin" png
var blurb: GameObject; // "What's that Sound?" thought bubble
var camera: GameObject; // not sure
private var isInTrigger:boolean = false;

function Update()
{
	if (isInTrigger)
	{
		if (Input.GetKeyDown(KeyCode.E)) //(Input.GetButtonDown("Enter"))
		{
			if(audio != null)
			{
				audio.Play();
				fadeOut();
			}
			PlayerPrefs.SetInt("camera", 0);
			if(blurb != null)
			{
				blurb.renderer.enabled = true;
			}
			loadTheLevel();
		}
	}
}

function loadTheLevel()
{
	yield WaitForSeconds (3);
	Application.LoadLevel(levelName);
}

function OnTriggerEnter2D(other: Collider2D)
{
	isInTrigger = true;
	if(enterButton != null)
	{
		enterButton.enabled = true;
	}
}

function OnTriggerExit2D(other: Collider2D)
{
	isInTrigger = false;
	if(enterButton != null)
	{
		enterButton.renderer.enabled = false;
	}
}

function fadeOut()
{
	audio.volume -= 1 * Time.deltaTime;
}
