#pragma strict
var cameraMain : GameObject;
var increaseY : float;
function Start () {
this.transform.position.y = cameraMain.transform.position.y + increaseY;

}

function Update () {
	this.transform.position.x = cameraMain.transform.position.x;
	
}