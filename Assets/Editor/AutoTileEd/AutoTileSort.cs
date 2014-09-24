using UnityEngine;
//using System.Collections;
using UnityEditor;

// JNN: custom hierarchy sort for AutoTile game objects, made possible with Unity 4.5
public class AutoTileRowSort : BaseHierarchySort
{
	public override int Compare(GameObject lhs, GameObject rhs)
	{
		if (lhs == rhs) return 0;
		if (lhs == null) return -1;
		if (rhs == null) return 1;

		if(lhs.transform.position.y < rhs.transform.position.y)
		{
			return -1;
		}
		else if(lhs.transform.position.y > rhs.transform.position.y)
		{
			return 1;
		}
		else if(lhs.transform.position.x < rhs.transform.position.x)
		{
			return -1;
		}
		else if(lhs.transform.position.x > rhs.transform.position.x)
		{
			return 1;
		}
		else
		{
			return 0;
		}
	}
}

public class AutoTileColumnSort : BaseHierarchySort
{
	public override int Compare(GameObject lhs, GameObject rhs)
	{
		if (lhs == rhs) return 0;
		if (lhs == null) return -1;
		if (rhs == null) return 1;
		
		if(lhs.transform.position.x < rhs.transform.position.x)
		{
			return -1;
		}
		else if(lhs.transform.position.x > rhs.transform.position.x)
		{
			return 1;
		}
		else if(lhs.transform.position.y < rhs.transform.position.y)
		{
			return -1;
		}
		else if(lhs.transform.position.y > rhs.transform.position.y)
		{
			return 1;
		}
		else
		{
			return 0;
		}
	}
}

public class AutoTileDiagonalSort : BaseHierarchySort
{
	public override int Compare(GameObject lhs, GameObject rhs)
	{
		if (lhs == rhs) return 0;
		if (lhs == null) return -1;
		if (rhs == null) return 1;

		float left = lhs.transform.position.x + lhs.transform.position.y;
		float right = rhs.transform.position.x + rhs.transform.position.y;
		if(left < right)
		{
			return -1;
		}
		else if(left > right)
		{
			return 1;
		}
		else if(lhs.transform.position.y < rhs.transform.position.y)
		{
			return -1;
		}
		else if(lhs.transform.position.y > rhs.transform.position.y)
		{
			return 1;
		}
		else
		{
			return 0;
		}
	}
}