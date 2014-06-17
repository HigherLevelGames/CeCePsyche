using UnityEngine;
using System.Collections;

public class ParticleScroll : MonoBehaviour {

	public float speed = 0; 
    public Renderer parentRenderer;
	public Vector3 startPosition;
	public Vector3 currentPosition; 

    void Start(){
     
	startPosition = transform.localPosition;
	currentPosition = startPosition;	
    parentRenderer = transform.parent.renderer;

    }
    

	void Update () {
    	float x = startPosition.x - parentRenderer.material.mainTextureOffset.x %1;
		currentPosition.x = x + (x < -.5? 1:0);
        transform.localPosition = currentPosition; 
		//transform.localPosition = startPosition + new Vector3(-x, parentRenderer.material.mainTextureOffset.y, 0);
		//Vector3.f parentRenderer.material.mainTextureOffset
		renderer.material.mainTextureOffset = new Vector2 ((Time.time * speed) % 1, 0f);
	
	}
}
