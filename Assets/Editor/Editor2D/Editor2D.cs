using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace TestEditor
{
    [CustomEditor (typeof(Editor2DManager))]
    public class Editor2D : Editor
    {
        bool leftClick = false;
        bool rightClick = false;
        bool ctrl = false;

        public void OnEnable()
        {
            SceneView.onSceneGUIDelegate = EditorUpdate;
            Tools.current = Tool.None;
            EData.Manager = (Editor2DManager)target;
        }

        void EditorUpdate(SceneView sceneView)
        {
            Event e = Event.current;
            Ray r = Camera.current.ScreenPointToRay(new Vector3(e.mousePosition.x, -e.mousePosition.y + Camera.current.pixelHeight));
            Vector2 mp = new Vector2(r.origin.x, r.origin.y);
            leftClick = (e.isMouse && e.type == EventType.MouseDown && e.button == 0);
            rightClick = (e.isMouse && e.type == EventType.MouseDown && e.button == 1);
            if (e.isKey)
            {
                if (e.keyCode == KeyCode.LeftControl || e.keyCode == KeyCode.RightControl)
                {
                    if (e.type == EventType.KeyDown)
                        ctrl = true;
                    else if (e.type == EventType.KeyUp)
                        ctrl = false;
                }
				else{
					ctrl = e.command;
				}
            }

            switch (EData.SetType)
            {
                case ToolsetType.Walkable:
                    WalkablesUpdate(e, mp);
                    break;
                case ToolsetType.Impassable:
                    ImpassablesUpdate(e, mp);
                    break;
                case ToolsetType.SpriteObject:
                    break;
                case ToolsetType.CameraBounds:

                    break;
                     
                case ToolsetType.None:
                    break;
            }

        }

        void ImpassablesUpdate(Event e, Vector2 mp)
        {
            switch (EData.ToolType)
            {
                case ToolType.Place:
                    if (leftClick)
                        EData.Manager.AddImpassable(mp);
                    else if (rightClick)
                        EData.Manager.RemoveImpassable(mp);
                    break;
                case ToolType.Edit:
                    break;
            }
        }

        void addPointToPoly(int idx, Vector2 v, PolygonCollider2D poly)
        {
            List<Vector2> pts = new List<Vector2>();
            pts.AddRange(poly.GetPath(0));
            pts.Insert(idx + 1, v);
            poly.CreatePrimitive(pts.Count + 1);
            poly.SetPath(0, pts.ToArray());
        }

        Vector2 closestPointOnLine(Vector2 a, Vector2 b, Vector2 p)
        {
            Vector2 v1 = (b - a);
            Vector2 v2 = (p - a);
            float dot = Mathf.Clamp(Vector2.Dot(v2.normalized, v1.normalized) * v2.magnitude, 0, v1.magnitude);

            return a + v1.normalized * dot;
        }

        bool creatingWalkable = false;

        void WalkablesUpdate(Event e, Vector2 mp)
        {
            switch (EData.ToolType)
            {
                case ToolType.Place:
                    if (leftClick)
                    {
                        creatingWalkable = true;
                        EData.ToolType = ToolType.Edit;
                        EData.Manager.AddWalkable(mp, mp);
                    }

                    break;
                case ToolType.Edit:
                    if (EData.Manager.SelectedWalkable > -1 && EData.Manager.SelectedWalkable < EData.Manager.Walkables.Length)
                    {
                        GameObject o = EData.Manager.Walkables [EData.Manager.SelectedWalkable];
                        EdgeCollider2D edge = o.GetComponent<EdgeCollider2D>();

                        if (edge.points.Length > 1 && ctrl)
                            Handles.DrawLine(o.transform.position.ToVector2() + edge.points [edge.points.Length - 1], mp.ToVector3());
                        if (creatingWalkable)
                        {
                            edge.points = new Vector2[]
                            {
                                edge.points [0],
                                mp - o.transform.position.ToVector2()
                            };
                            if (leftClick)
                                creatingWalkable = false;
                            else if (rightClick)
                                EData.Manager.RemoveWalkable(EData.Manager.SelectedWalkable);
                        
                        } else
                        {
                            if (leftClick && ctrl)
                                EData.Manager.AddPointToWalkable(mp);
                            else if (rightClick)
                                EData.Manager.RemovePointFromWalkable();
                        }
                    }
                    break;
            }
        }
    }
}