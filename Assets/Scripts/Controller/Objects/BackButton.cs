﻿using UnityEngine;
using System.Collections;

// Code placed on "Exit" button in Concept and Credits scene
public class BackButton : MonoBehaviour
{
	public string levelName;
	
	void OnMouseDown() {
        Application.LoadLevel(levelName);
    }
	
	 /*void OnMouseEnter() {
	 transform.localScale += new Vector2(2.782f, 1.09f);
     renderer.material.color = Color.red;
    }
    */
}
