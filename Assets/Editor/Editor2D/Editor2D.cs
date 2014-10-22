using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

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
                    {/*
                        GameObject o = EData.Manager.Impassables [EData.Manager.SelectedImpassable];
                        Vector2 p = o.transform.position;

                        PolygonCollider2D poly = o.GetComponent<PolygonCollider2D>();
                        Vector2[] path = poly.GetPath(0);
                        Vector3[] handlePath = new Vector3[path.Length + 1];
                        for (int i = 0; i < path.Length; i++)
                            handlePath [i] = path [i] + p;
                        handlePath [path.Length] = path [0] + p;
                        Vector2 v = HandleUtility.ClosestPointToPolyLine(handlePath);

                        if (closestPoint > -1)
                        {
                            if (e.isMouse && e.type == EventType.MouseDrag && e.button == 0)
                                path [closestPoint] = Handles.FreeMoveHandle(path [closestPoint], o.transform.rotation, 0.1f, Vector3.zero, Handles.DotCap) - p.ToVector3();
                            
                            if (e.type == EventType.MouseUp)
                                closestPoint = -1;
                        } else
                        {
                            if (leftClick)
                            {
                                for (int i = 0; i < path.Length; i++)
                                    if (Vector2.Distance(path [i] + p, mp) < 0.1f)
                                        closestPoint = i;
                                if (closestPoint == -1)
                                {
                                    if (closestPath > -1)
                                    {
                                        addPointToPoly(closestPath, v - p, poly);
                                        closestPoint = closestPath + 1;
                                        path = poly.GetPath(0);
                                    }

                                }
                            }

                            Handles.FreeMoveHandle(v, o.transform.rotation, 0.1f, Vector3.zero, Handles.DotCap);
                        }

                        float closestDist = 1f;
                        for (int j = 0; j < path.Length; j++)
                        {
                            int j2 = j == path.Length - 1 ? 0 : j + 1;
                            float dist = Vector3.Distance(closestPointOnLine(path [j], path [j2], mp - p) + p, mp);
                            if (dist < closestDist)
                            {
                                closestPath = j;
                                closestDist = dist;
                            }

                            path [j] = Handles.FreeMoveHandle(path [j] + p, o.transform.rotation, 0.1f, Vector3.zero, Handles.DotCap) - p.ToVector3();
                        }
                        if (closestPoint == -1)
                        {
                            p = Handles.PositionHandle(p, o.transform.rotation);
                            o.transform.position = p;
                        }
                        poly.SetPath(0, path);
                    */}
                    break;
            }
        }

        int closestPath = -1;
        int closestPoint = -1;

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