using UnityEngine;
using UnityEditor;
using System.Collections;

// DO DUST AT FEET
namespace TestEditor
{
    [CustomEditor (typeof(TestEditorManager))]
    public class TestEditor : Editor
    {
        public void OnEnable()
        {
            SceneView.onSceneGUIDelegate = EditorUpdate;
            Tools.current = Tool.None;
            EData.Manager = (TestEditorManager)target;
        }

        void EditorUpdate(SceneView sceneView)
        {
            Event e = Event.current;
            Ray r = Camera.current.ScreenPointToRay(new Vector3(e.mousePosition.x, -e.mousePosition.y + Camera.current.pixelHeight));
            Vector2 mp = new Vector2(r.origin.x, r.origin.y);
            leftClick = (e.isMouse && e.type == EventType.MouseDown && e.button == 0);
            rightClick = (e.isMouse && e.type == EventType.MouseDown && e.button == 1);
            /*
            shift = (e.isKey && e.type == EventType.KeyDown && (e.keyCode == KeyCode.LeftShift || e.keyCode == KeyCode.RightShift));
            ctrl = (e.isKey && e.type == EventType.KeyDown && 
                (e.keyCode == KeyCode.LeftControl || e.keyCode == KeyCode.RightControl || 
                e.keyCode == KeyCode.LeftCommand || e.keyCode == KeyCode.RightCommand)); */
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
                case ToolType.Box:
                    if (leftClick)
                        EData.Manager.AddImpassable(mp);
                    else if (rightClick)
                        EData.Manager.RemoveImpassable(mp);
                    break;
                case ToolType.Ledge:
                    break;
                case ToolType.Edit:
                    GameObject o = EData.Manager.Impassables [EData.Manager.SelectedImpassable];
                    Vector2 p = o.transform.position;
                    BoxCollider2D b = o.GetComponent<BoxCollider2D>();
                    float x = p.x + b.center.x;
                    float y = p.y + b.center.y;
                    Vector2 left = new Vector2(x - b.size.x / 2, y);
                    Vector2 right = new Vector2(x + b.size.x / 2, y);
                    Vector2 top = new Vector2(x, y + b.size.y / 2);
                    Vector2 bot = new Vector2(x, y - b.size.y / 2);
                    float sx = 0;
                    float sy = 0;

                    left = Handles.FreeMoveHandle(left, o.transform.rotation, 0.1f, Vector3.zero, Handles.DotCap);
                    sx += x - left.x;
                    //b.center += x / 2;
                    right = Handles.FreeMoveHandle(right, o.transform.rotation, 0.1f, Vector3.zero, Handles.DotCap);
                    sx += right.x - x;
                    top = Handles.FreeMoveHandle(top, o.transform.rotation, 0.1f, Vector3.zero, Handles.DotCap);
                    sy += top.y - y;
                    bot = Handles.FreeMoveHandle(bot, o.transform.rotation, 0.1f, Vector3.zero, Handles.DotCap);
                    sy += y - bot.y;
                    b.size = new Vector2(sx, sy);
                    break;
            }
        }

        void WalkablesUpdate(Event e, Vector2 mp)
        {
            switch (EData.ToolType)
            {
                case ToolType.Box:

                    break;
                case ToolType.Ledge:
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
                        if (edge.points.Length > 0)
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