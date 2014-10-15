using System; // for EventArgs
using UnityEngine;
using System.Collections; // not needed
using Common;

public class Bar : MonoBehaviour
{
	public enum Direction
	{
		Up,
		Down,
		Left,
		Right
	}
	public Direction fillDirection = Direction.Right;
	public float startFill = 0.5f;

	public Color emptyColor = Color.red;
	public Color halfColor = Color.yellow;
	public Color fillColor = Color.green;

	public Rect area = new Rect(0f, 0f, 10f, 5f);
	private Vector2 offset = Vector2.zero;
	public Texture2D bgTexture;
	public Texture2D fillTexture;
	public float buffer = 5f;
	
	public delegate void EmptyEventHandler(object sender, EventArgs e);
	public static event EmptyEventHandler EmptyEvent;
	protected virtual void OnEmptyEvent(EventArgs e)
	{
		if (EmptyEvent != null)
		{
			EmptyEvent(this, e);
		}
	}
	
	private float barFill = 0.5f;
	public float BarFill
	{
		get
		{
			return barFill;
		}
		set
		{
			barFill = Mathf.Clamp(value, 0.0f, 1.0f);
			if (Empty)
			{
				OnEmptyEvent(EventArgs.Empty);
			}
		}
	}
	
	public bool Full
	{
		get
		{
			return barFill >= 1.0f;
		}
	}
	
	public bool Empty
	{
		get
		{
			return barFill <= 0.0f;
		}
	}
	
	// Use this for initialization
	void Start()
	{
		if (bgTexture == null)
		{
			Texture2D temp = new Texture2D(1, 1);
			temp.SetPixel(0, 0, Color.white);
			temp.Apply();
			bgTexture = temp;
		}
		if (fillTexture == null)
		{
			Texture2D temp = new Texture2D(1, 1);
			temp.SetPixel(0, 0, Color.white);
			temp.Apply();
			fillTexture = temp;
		}
		offset = new Vector2(area.x, area.y);
		barFill = startFill;
	}

	void Update()
	{
		// Debugging
		if(Input.GetKey(KeyCode.I))
		{
			increaseBar(0.1f);
		}
		if(Input.GetKey(KeyCode.J))
		{
			decreaseBar(0.1f);
		}

		// keeping the bar relative to the object
		Vector3 position = Camera.main.WorldToScreenPoint(this.transform.position);
		Vector2 pos = Utility.unadjPoint(Utility.toVector2(position));
		area.x = offset.x + pos.x;
		area.y = offset.y + 100.0f - pos.y;
	}
	
	void OnGUI()
	{
		GUI.depth = -1; // draw on top of everything
		
		//GUIStyle style;
		
		Rect area = Utility.adjRect(this.area);
		// draw the background:
		GUI.BeginGroup(area);
		GUI.DrawTexture(new Rect(0f, 0f, area.width, area.height), bgTexture);
		
		// change color of fill based on current amount
		if(barFill < 0.5f)
		{
			GUI.color = Color.Lerp(emptyColor, halfColor, barFill*2);
		}
		else
		{
			GUI.color = Color.Lerp(halfColor, fillColor, (barFill-0.5f)*2);
		}

		// draw the filled-in part
		Rect fillRect = new Rect(buffer, buffer, area.width - 2*buffer, area.height - 2*buffer);
		switch(fillDirection)
		{
		case Direction.Right:
			fillRect.width *= barFill;
			break;
		case Direction.Down:
			fillRect.height *= barFill;
			break;
		case Direction.Left:
			fillRect.x += (1-barFill)*fillRect.width;
			fillRect.width *= barFill;
			break;
		case Direction.Up:
			fillRect.y += (1-barFill)*fillRect.height;
			fillRect.height *= barFill;
			break;
		default:
			break;
		}
		GUI.DrawTexture(fillRect, fillTexture);
		GUI.EndGroup();
	}
	
	public void increaseBar(float amount)
	{
		BarFill += amount;
	}
	
	public void decreaseBar(float amount)
	{
		BarFill -= amount;
	}
	
	public void setAmount(float amount)
	{
		BarFill = amount;
	}
	
}