using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraControl : MonoBehaviour
{

		public Camera MyCamera;
		public Transform PlayerTransform;
		public BoxCollider[] LevelBoxes;
		public float SpringSpeed = 2.0f;
		Vector2 Target;
		CameraBounds[] camBounds;
		int curBounds;

		void Start ()
		{
				curBounds = -1;
				Target = new Vector2 ();
				camBounds = new CameraBounds[LevelBoxes.Length];
				for (int i = 0; i < LevelBoxes.Length; i++) {
						camBounds [i] = new CameraBounds (LevelBoxes, i);
						if (LevelBoxes [i].bounds.Contains (PlayerTransform.position))
								curBounds = i;
				}
		}

		void Update ()
		{ 
			Vector2 cp = new Vector2 (MyCamera.transform.position.x, MyCamera.transform.position.y);
			Vector2 p = new Vector2 (PlayerTransform.position.x, PlayerTransform.position.y);
			FindClosestBounds (p);
			Target = camBounds[curBounds].InBoundsPoint(p);
			cp += (Target - cp) * Time.deltaTime * SpringSpeed;
			MyCamera.transform.position = new Vector3 (cp.x, cp.y, MyCamera.transform.position.z);
			
		}
		void FindClosestBounds(Vector2 p)
		{
			if (curBounds != -1) 
			{
				if(camBounds[curBounds].Contains(p))
					return;
				for (int i = 0; i < camBounds[curBounds].neighbours.Length; i++) 
				{
					if (camBounds [camBounds[curBounds].neighbours[i]].Contains (p))
					{
						curBounds = camBounds[curBounds].neighbours[i];
						return;
					}	
				}
			} 
			else 
			{
				for (int i = 0; i < camBounds.Length; i++) 
				{
					if (camBounds [i].Contains (p))
						curBounds = i;
				}
			}
		}
}

public class Rectangle
{
		public float Left, Right, Top, Bottom;

		public Rectangle ()
		{

		}

		public void SetBounds (BoxCollider box)
		{
				Left = box.bounds.min.x;
				Right = box.bounds.max.x;
				Top = box.bounds.max.y;
				Bottom = box.bounds.min.y;
		}
}

public class CameraBounds : Rectangle
{
		public int[] neighbours;

		public CameraBounds (BoxCollider[] boxes, int i)
		{
				SetBounds (boxes [i]);
				FindNeighbours (boxes, i);
		}
		/// <summary>
		/// A point inside the rectangle
		/// </summary>
		/// <returns>A Vector2 converted into a valid Vector2 at the nearest point inside the rectangle.</returns>
		/// <param name="v">The desired Vector2 for conversion.</param>
		public Vector2 InBoundsPoint (Vector2 v)
		{
				Vector2 returnVector = v;
				if (v.x > Right)
						returnVector.x = Right;
				else if (v.x < Left)
						returnVector.x = Left;
				if (v.y > Top)
						returnVector.y = Top;
				else if (v.y < Bottom)
						returnVector.y = Bottom;

				return returnVector;
		}

		public bool Contains (Vector2 v)
		{
				return (v.x < Right && v.x > Left && v.y < Top && v.y > Bottom);
		}

		public void FindNeighbours (BoxCollider[] boxes, int index)
		{
				List<int> idxs = new List<int> ();
				for (int i = 0; i < boxes.Length; i++) {
						if (i != index) {
								if (boxes [i].bounds.Intersects (boxes [index].bounds))
										idxs.Add (i);
						}
				}
				neighbours = new int[idxs.Count];
				for (int i = 0; i < idxs.Count; i++) {
						neighbours [i] = idxs [i];
				}
		}
}
