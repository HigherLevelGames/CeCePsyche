using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TestEditor
{
    public class Editor2DManager : MonoBehaviour
    {
        // objects
        public Transform WalkableMaster;
        public GameObject[] Walkables;
        public Transform ImpassableMaster;
        public GameObject[] Impassables;
        // options
        public bool IVisible = true;
        public Color IColor = Color.red;
        public bool WVisible = true;
        public Color WColor = Color.white;
        public int SelectedWalkable = 0;
        public int SelectedImpassable = 0;


        void Awake()
        {
            FindChildren();
        }

        void OnDrawGizmos()
        {

            if (IVisible)
            {
                Gizmos.color = IColor;
                for (int i = 0; i < Impassables.Length; i++)
                {
                    Vector2 p = Impassables [i].transform.position;
                    PolygonCollider2D poly = Impassables [i].GetComponent<PolygonCollider2D>();
                    for (int j = 0; j < poly.pathCount; j++)
                    {
                        Vector2[] points = poly.GetPath(j);
                        if (points.Length > 1)
                        {
                            Gizmos.DrawLine(p + points [0], p + points [points.Length - 1]);
                            for (int k = 0; k < points.Length - 1; k++)
                                Gizmos.DrawLine(p + points [k], p + points [k + 1]);
                        }

                    }
                }
            }
            if (WVisible)
            {
                Gizmos.color = WColor;

                for (int i = 0; i < Walkables.Length; i++)
                {
                    EdgeCollider2D edge = Walkables [i].GetComponent<EdgeCollider2D>();
                    Vector2 p = Walkables [i].transform.position.ToVector2();
                    for (int j = 0; j < edge.points.Length - 1; j++)
                        Gizmos.DrawLine(p + edge.points [j], p + edge.points [j + 1]);
                }
            }
        }

        void FindChildren()
        {
            bool impassableFound = false;
            bool walkableFound = false;
            for (int i = 0; i < this.transform.childCount; i++)
            {
                if (this.transform.GetChild(i).name == "ImpassableMaster")
                    impassableFound = true;
                if (this.transform.GetChild(i).name == "WalkableMaster")
                    walkableFound = true;
            }
            if (!impassableFound)
            {
                GameObject impassable = new GameObject();
                impassable.name = "ImpassableMaster";
                impassable.transform.parent = this.transform;
                this.ImpassableMaster = impassable.transform;
            } 
            if (!walkableFound)
            {
                GameObject walkable = new GameObject();
                walkable.name = "WalkableMaster";
                walkable.transform.parent = this.transform;
                this.WalkableMaster = walkable.transform;
            }
        }

        public void AddWalkable(GameObject o)
        {
            for (int i = 0; i < Walkables.Length; i++)
                if (Walkables [i].Equals(o))
                    return;
            List<GameObject> a = new List<GameObject>();
            a.AddRange(Walkables);
            a.Add(o);
            Walkables = a.ToArray();
        }

        public void AddWalkable(Vector2 v)
        {
            GameObject obj = new GameObject();
            obj.name = "Walkable";
            obj.layer = LayerMask.NameToLayer("Ground");
            obj.transform.parent = WalkableMaster;
            obj.transform.position = v.ToVector3();
            obj.AddComponent<EdgeCollider2D>();
            List<GameObject> a = new List<GameObject>();
            a.AddRange(Walkables);
            a.Add(obj);
            Walkables = a.ToArray();
            SelectedWalkable = Walkables.Length - 1;
        }

        public void AddPointToWalkable(Vector2 p)
        {
            if (Walkables.Length > 0)
            {
                GameObject obj = Walkables [SelectedWalkable];
                EdgeCollider2D col = obj.GetComponent<EdgeCollider2D>();
                List<Vector2> pts = new List<Vector2>(); 
                pts.AddRange(col.points);
                pts.Add(-Walkables [SelectedWalkable].transform.position.ToVector2() + p);
                col.points = pts.ToArray();
            }
        }

        public void RemoveWalkable(int idx)
        {
            if (SelectedWalkable > Walkables.Length - 1)
                SelectedWalkable = Walkables.Length - 1;
            List<GameObject> a = new List<GameObject>();
            DestroyImmediate(Walkables [idx]);
            a.AddRange(Walkables);
            a.RemoveAt(idx);
            Walkables = a.ToArray();
        }

        public void AddImpassable(GameObject o)
        {
            for (int i = 0; i < Impassables.Length; i++)
                if (Impassables [i].Equals(o))
                    return;
            List<GameObject> a = new List<GameObject>();
            a.AddRange(Impassables);
            a.Add(o);
            Impassables = a.ToArray();
        }

        public void AddImpassable(Vector3 v)
        {
            GameObject obj = new GameObject();
            obj.name = "Impassable";
            obj.transform.parent = ImpassableMaster;
            obj.transform.position = v;
            PolygonCollider2D poly = obj.AddComponent<PolygonCollider2D>();
            poly.CreatePrimitive(3);
            List<GameObject> a = new List<GameObject>();
            a.AddRange(Impassables);
            a.Add(obj);
            Impassables = a.ToArray();
        }

        public void RemoveImpassable(Vector2 v)
        {
            for (int i = Impassables.Length  - 1; i > -1; i--)
            {
                if (Impassables [i].collider2D.bounds.Contains(v))
                {
                    List<GameObject> a = new List<GameObject>();
                    DestroyImmediate(Impassables [i]);
                    a.AddRange(Impassables);
                    a.RemoveAt(i);
                    Impassables = a.ToArray();
                    return;
                }
            }
        }

        public void RemoveImpassable(int idx)
        {
            if (SelectedImpassable > Impassables.Length - 1)
                SelectedImpassable = Impassables.Length - 1;
            List<GameObject> a = new List<GameObject>();
            DestroyImmediate(Impassables [idx]);
            a.AddRange(Impassables);
            a.RemoveAt(idx);
            Impassables = a.ToArray();
        }

        public void CheckImpassables()
        {
            List<GameObject> a = new List<GameObject>();
            if (Impassables.Length > 0)
                for (int i = 0; i < Impassables.Length; i++)
                    if (Impassables [i])
                        a.Add(Impassables [i]);
            Impassables = a.ToArray();
            if (SelectedImpassable > Impassables.Length - 1)
                SelectedImpassable = Impassables.Length - 1;
        }

        public void CheckWalkables()
        {
            List<GameObject> a = new List<GameObject>();
            if (Walkables.Length > 0)
                for (int i = 0; i < Walkables.Length; i++)
                    if (Walkables [i])
                        a.Add(Walkables [i]);
            Walkables = a.ToArray();
            if (SelectedWalkable > Walkables.Length - 1)
                SelectedWalkable = Walkables.Length - 1;

        }

        public void RemovePointFromWalkable()
        {
            if (Walkables.Length > 0)
            {
                EdgeCollider2D edge = Walkables [SelectedWalkable].GetComponent<EdgeCollider2D>();
                if (edge.points.Length > 2)
                {
                    List<Vector2> a = new List<Vector2>();
                    a.AddRange(edge.points);
                    a.RemoveAt(a.Count - 1);
                    edge.points = a.ToArray();
                } else
                    RemoveWalkable(SelectedWalkable);
            }
        }
    }
}