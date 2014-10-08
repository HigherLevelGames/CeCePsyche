using UnityEngine;
using System.Collections;

// JNN: made drawing grid area based on screen width/height values
public class AutoTileSetManager : MonoBehaviour
{
	[Header("Grid Options")]
	public Vector2 gridSize=Vector2.one;
	public Vector3 offset;
	public bool displayGrid=true;
	public Color gridColor;
	[Header("Draw Options")]
	public GameObject currentTile;

	void OnDrawGizmos()
	{
		if (displayGrid)
		{
			Gizmos.color=gridColor*0.5f;
			DrawGrid();
		}
	}
	
	void OnDrawGizmosSelected()
	{
		if (displayGrid)
		{
			Gizmos.color=gridColor;
			DrawGrid();
		}
	}
	
	void DrawGrid()
	{
		Vector3 pos = Camera.current.transform.position-Vector3.one;
		float halfWidth = 50.0f;//Screen.width * 0.5f; // 1000000.0f or 1200.0f
		float halfHeight = 25.0f;//Screen.height * 0.5f; // 1000000.0f or 800.0f

		for (float y = pos.y - halfHeight; y < pos.y + halfHeight; y+= gridSize.y)
		{
			Gizmos.DrawLine(new Vector3(pos.x-halfWidth, RoundToValue(y,gridSize.y)+offset.y, offset.z),
			                new Vector3(pos.x+halfWidth, RoundToValue(y,gridSize.y)+offset.y, offset.z));
		}
		
		for (float x = pos.x - halfWidth; x < pos.x + halfWidth; x+= gridSize.x)
		{
			Gizmos.DrawLine(new Vector3(RoundToValue(x,gridSize.x)+offset.x, pos.y-halfHeight, offset.z),
			                new Vector3(RoundToValue(x,gridSize.x)+offset.x, pos.y+halfHeight, offset.z));
		}
	}

	float RoundToValue(float f, float value)
	{
		return Mathf.Floor(f/value) * value;
	}
}
