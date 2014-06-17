#pragma strict

	var sensitivity: float = 1;

function Update(){
		transform.position.x = Mathf.Clamp(transform.position.x - Input.GetAxis("Mouse X") * sensitivity, -0.5, 0.5);
	}