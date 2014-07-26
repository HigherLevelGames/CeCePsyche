using UnityEngine;
using System; // for EventArgs
using System.Collections;
using Common;

namespace Common
{
	// A delegate type for hooking up change notifications.
	public delegate void ChangedEventHandler(object sender, EventArgs e, int index);
}

public class Menu
{
	// An event MenuManager can use to be notified whenever the menu should change
	public event ChangedEventHandler Changed;
	
	// Invoke the Changed event; called whenever menu changes
	protected virtual void OnChanged(EventArgs e, int index)
	{
		if (Changed != null)
		{
			Changed(this, e, index);
		}
	}

	public virtual void Update() { }

	public virtual void ShowMe() { }
}