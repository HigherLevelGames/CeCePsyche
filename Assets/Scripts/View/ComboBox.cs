/*
coded by Jordan Nguyen (Jordan.N.Nguyen@asu.edu)
modified by Jesse McIntosh for profile selection and loading (jmmcinto@asu.edu)
remodified to be generic by Jordan Nguyen
*/

using UnityEngine;
using System.Collections;

public class ComboBox
{
    private bool startDropOn = false;
	private Vector2 startScrollPosition = Vector2.zero;

	// constructor
	public ComboBox() { }

	// boundingBox = position of dropdown button
	// selected = index of selected item
	// texts = string of text to display in dropdown menu
	// returns -1 if a dropdown option was not selected,
	// else returns index of selected item
	// Note: DOES NOT STORE THE SELECTED INDEX
	public int DrawComboBox(Rect boundingBox, int selected, string[] texts)
	{
		return DrawComboBox(boundingBox, selected, texts, 100);
	}

	// above is a wrapper, this does all the actual work
	// does not test if selected is a valid index of texts
	public int DrawComboBox(Rect boundingBox, int selected, string[] texts, int minHeight)
	{
		// dropdown box
		if(GUI.Button(boundingBox, texts[selected]))
		{
			startDropOn = !startDropOn;
		}

		int num = -1;
		if(startDropOn)
		{
			// calculate area that can be seen at once
			Rect scrollPosition = boundingBox;
			scrollPosition.y += boundingBox.height;
			scrollPosition.height = Mathf.Min(minHeight, texts.Length*boundingBox.height);

			// calculate entire area for all items
			Rect contentArea = new Rect(0, 0, boundingBox.width - 16, texts.Length*boundingBox.height);

			// draw gui box background
			GUI.Box(scrollPosition, "");

			// draw scrolling view
			startScrollPosition = GUI.BeginScrollView(
				scrollPosition, // same as GUI.Box (i.e. viewable area on screen)
				startScrollPosition, // zero
				contentArea, // size of content area
				false, // do not show horizontal scrollbar unless necessary
				true // always show vertical scrollbar
				);

			// draw vertical button selection grid items
			num = GUI.SelectionGrid(contentArea, num, texts, 1);
			if(num!=-1)
			{
				startDropOn = false;
			}
			
			GUI.EndScrollView();
		}
		return num;
	}
}