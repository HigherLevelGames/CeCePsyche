using UnityEngine;
using UnityEditor;
using System.Collections;

namespace TestEditor
{
    [CustomEditor (typeof(Editor2DManager))]
    public class Editor2D : Editor
    {
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

            switch (EData.SetType)
            {
                case ToolsetType.Walkable:
                    WalkablesUpdate(e, mp);
                    break;
                case ToolsetType.Impassable:
                    ImpassablesUpdate(e, mp);
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
                    if (EData.Manager.SelectedImpassable > -1 && EData.Manager.Impassables.Length > 0)
                    {
                        GameObject o = EData.Manager.Impassables [EData.Manager.SelectedImpassable];
                        Vector2 p = o.transform.position;

                        PolygonCollider2D poly = o.GetComponent<PolygonCollider2D>();
                        for (int i = 0; i < poly.pathCount; i++)
                        {
                            Vector2[] points = poly.GetPath(i);
                            for (int j = 0; j < points.Length; j++)
                            {
                                points [j] = Handles.FreeMoveHandle(points [j] + p, o.transform.rotation, 0.1f, Vector3.zero, Handles.DotCap) - p.ToVector3();
                            }
                            poly.SetPath(i, points);
                        }
                        p = Handles.PositionHandle(p, o.transform.rotation);

                        o.transform.position = p;
                    }
                    break;
            }
        }

        void WalkablesUpdate(Event e, Vector2 mp)
        {
            switch (EData.ToolType)
            {
                case ToolType.Place:
                    if (leftClick)
                    {
                        EData.Manager.AddWalkable(mp);
                        EData.ToolType = ToolType.Edit;
                    }
                    break;
                case ToolType.Edit:
                    if (EData.Manager.SelectedWalkable > -1 && EData.Manager.Walkables.Length > 0)
                    {
                        GameObject o = EData.Manager.Walkables [EData.Manager.SelectedWalkable];
                        EdgeCollider2D edge = o.GetComponent<EdgeCollider2D>();
                        if (edge.points.Length > 1)
                            Handles.DrawLine(o.transform.position.ToVector2() + edge.points [edge.points.Length - 1], mp.ToVector3());
                        if (leftClick)
                            EData.Manager.AddPointToWalkable(mp);
                        else if (rightClick)
                            EData.Manager.RemovePointFromWalkable();
                    }
                    break;
            }
        }

        bool leftClick = false;
        bool rightClick = false;

    }
}