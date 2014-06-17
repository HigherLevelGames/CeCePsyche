#pragma strict


var levelName: String; 
var enterButton: Renderer; 
var blurb: GameObject; 
var camera: GameObject;


function Update(){
	if (enterButton.enabled) {
		if (Input.GetKeyDown(KeyCode.E)){
		//(Input.GetButtonDown("Enter")) {
			audio.Play();
			fadeOut();
			PlayerPrefs.SetInt("camera", 0);
			blurb.renderer.enabled = true;
			loadTheLevel();
			}
		}
	}

function loadTheLevel()
{
	yield WaitForSeconds (3);
	Application.LoadLevel(levelName);
}

function OnTriggerEnter2D(other: Collider2D) {
	enterButton.enabled = true; 
	
	//Debug.Log("It Works");
}

function OnTriggerExit2D(other: Collider2D) {
	enterButton.renderer.enabled = false; 
	
	//Debug.Log("It Works");
}

function fadeOut()
{
	audio.volume -= 1 * Time.deltaTime;
}
