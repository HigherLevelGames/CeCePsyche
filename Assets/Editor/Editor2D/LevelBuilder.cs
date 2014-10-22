﻿using UnityEngine;
using UnityEditor;
using System.Collections;

namespace TestEditor
{
    public enum ToolsetType
    {
        None = -1,
        Impassable = 0,
        Walkable = 1,
        SpriteObject
    }

    public enum ToolType
    {
        None = -1,
        Place = 0,
        Edit = 1,
    }
        
    public static class EData
    {
        public static Editor2DManager Manager;
        public static ToolsetType SetType = ToolsetType.None;
        public static ToolType ToolType = ToolType.None;
        public static Rect SRect = new Rect(0, 0, 20, 20);
        public static bool PolyCenter = true;
    }
        
    public class LevelBuilder : EditorWindow
    {
        bool modeFoldout = true;
        bool subFoldout1 = true;
        bool subFoldout2 = true;
        Vector2 scrollPosition1 = new Vector2();
        Vector2 scrollPosition2 = new Vector2();

        [MenuItem("Window/2D Editor")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(LevelBuilder));
        }

        void OnFocus()
        {
            this.minSize = new Vector2(200, 600);
            CheckForManagers();
        }

        void CreateManagers()
        {
            if (!EData.Manager)
            {
                GameObject man = new GameObject();
                GameObject imas = new GameObject();
                GameObject wmas = new GameObject();
                man.name = "Editor2D";
                man.AddComponent<Editor2DManager>();
                EData.Manager = man.GetComponent<Editor2DManager>();
                imas.name = "ImpassableMaster";
                wmas.name = "WalkableMaster";
                imas.transform.parent = wmas.transform.parent = EData.Manager.transform;
                EData.Manager.ImpassableMaster = imas.transform;
                EData.Manager.WalkableMaster = wmas.transform;
                EData.Manager.Walkables = new GameObject[0];
                EData.Manager.Impassables = new GameObject[0];
            }
        }

        void CheckForManagers()
        {
            GameObject[] objs = (GameObject[])SceneView.FindObjectsOfType(typeof(GameObject));
            if (!EData.Manager)
            {
                for (int i = 0; i < objs.Length; i++)
                    if (objs [i].name == "Editor2D")
                    {
                        Selection.activeGameObject = objs [i];
                        EData.Manager = (Editor2DManager)objs [i].GetComponent(typeof(MonoBehaviour));
                        break;
                    }
                CreateManagers();
            }
            int iter = 0;
            while (!(EData.Manager.ImpassableMaster && EData.Manager.WalkableMaster) && iter < objs.Length)
            {
                if (objs [iter].name == "Editor2D")
                {
                    Selection.activeGameObject = objs [iter];
                }
                if (objs [iter].name == "ImpassableMaster")
                {
                    EData.Manager.ImpassableMaster = objs [iter].transform;
                }
                if (objs [iter].name == "WalkableMaster")
                {
                    EData.Manager.WalkableMaster = objs [iter].transform;
                }
                iter++;
            }

            EData.Manager.CheckImpassables();
            EData.Manager.CheckWalkables();
            if (EData.Manager.Walkables.Length < 1 || EData.Manager.Impassables.Length < 1)
                for (int i = 0; i < objs.Length; i++)
                {
                    if (objs [i].name == "Impassable")
                    {
                        EData.Manager.AddImpassable(objs [i]);
                    }
                    if (objs [i].name == "Walkable")
                    {
                        EData.Manager.AddWalkable(objs [i]);
                    }
                }

        }

        void OnGUI()
        {
            if (EData.Manager)
            {
                this.Repaint();
                modeFoldout = EditorGUILayout.Foldout(modeFoldout, "Tools");
                if (modeFoldout)
                {
                    EData.SetType = (ToolsetType)GUILayout.Toolbar((int)EData.SetType, new string[]
                    {
                        "Impassable",
                        "Walkable"
                    });
                }

                switch (EData.SetType)
                {
                    case ToolsetType.Impassable:
                        ImpassableEditing();
                        break;
                    case ToolsetType.Walkable:
                        WalkableEditing();
                        break;
                    case ToolsetType.SpriteObject:
                        ObjectEditing();
                        break;
                    case ToolsetType.None:
                        break;
                }
            }
        }

        void ImpassableEditing()
        {
            GUILayout.Label("Impassable Tools");
            EData.ToolType = (ToolType)GUILayout.Toolbar((int)EData.ToolType, new string[]{
                "New Polygon",
                "Edit"
            });

            subFoldout1 = EditorGUILayout.Foldout(subFoldout1, "Impassables (" + EData.Manager.Impassables.Length.ToString() + " total)");
            if (subFoldout1)
            {
                scrollPosition1 = GUILayout.BeginScrollView(scrollPosition1, false, true);
                for (int i = 0; i < EData.Manager.Impassables.Length; i++)
                {
                    GameObject o = EData.Manager.Impassables [i];
                    if (o)
                    {
                        Vector3 p = o.transform.position;
                        EditorObjectButton(o, p, i, i == EData.Manager.SelectedImpassable);
                    } else
                        EData.Manager.RemoveImpassable(i);
                }
                GUILayout.EndScrollView();
            }
            switch (EData.ToolType)
            {
                case ToolType.Place:
                    GUILayout.Label("Click a point in the scene to place a new Box Collider");
                    EData.SRect = EditorGUILayout.RectField("init size", EData.SRect);
                    EData.PolyCenter = EditorGUILayout.Toggle("center", EData.PolyCenter);
                    break;
                case ToolType.Edit:
                    if (EData.Manager.SelectedImpassable > -1 && EData.Manager.SelectedImpassable < EData.Manager.Impassables.Length)
                    {
                        GameObject o = EData.Manager.Impassables [EData.Manager.SelectedImpassable];
                        Vector2 v = EditorGUILayout.Vector2Field("Parent", new Vector2(o.transform.position.x, o.transform.position.y));
                        o.transform.position = new Vector3(v.x, v.y, o.transform.position.z);
                        subFoldout2 = EditorGUILayout.Foldout(subFoldout2, "Data");
                        if (subFoldout2)
                        {
                            scrollPosition2 = GUILayout.BeginScrollView(scrollPosition2, false, true);

                            GUILayout.EndScrollView();
                        }
                        GUILayout.Label("Do something");

                    } else
                        EData.Manager.SelectedImpassable = 0;
                    break;
            }
        }

        void WalkableEditing()
        {
            GUILayout.Label("Walkable Tools");
            EData.ToolType = (ToolType)GUILayout.Toolbar((int)EData.ToolType, new string[]
            {
                "New Ledge",
                "Edit"
            });
            subFoldout1 = EditorGUILayout.Foldout(subFoldout1, "Walkables (" + EData.Manager.Walkables.Length + " total)");
            if (subFoldout1)
            {
                scrollPosition1 = GUILayout.BeginScrollView(scrollPosition1, false, true);
                for (int i = 0; i < EData.Manager.Walkables.Length; i++)
                {
                    GameObject o = EData.Manager.Walkables [i];
                    Vector3 p = o.transform.position;
                    EditorObjectButton(o, p, i, i == EData.Manager.SelectedWalkable);
                }
                GUILayout.EndScrollView();
            }
            switch (EData.ToolType)
            {
                case ToolType.Place:
                    GUILayout.Label("Click a point in the scene to start a new ledge");
                    break;
                case ToolType.Edit:
                    if (EData.Manager.SelectedWalkable > -1 && EData.Manager.SelectedWalkable < EData.Manager.Walkables.Length)
                    {
                        GameObject o = EData.Manager.Walkables [EData.Manager.SelectedWalkable];
                        Vector2 v = EditorGUILayout.Vector2Field("Parent", o.transform.position.ToVector2());
                        o.transform.position = new Vector3(v.x, v.y, o.transform.position.z);
                        EdgeCollider2D edge = o.GetComponent<EdgeCollider2D>();
                        subFoldout2 = EditorGUILayout.Foldout(subFoldout2, "Points on Ledge" + EData.Manager.SelectedWalkable.ToString() + " (" + edge.pointCount.ToString() + " total)");
                        if (subFoldout2)
                        {
                            scrollPosition2 = GUILayout.BeginScrollView(scrollPosition2, false, true);
                            for (int i = 0; i < edge.pointCount; i++)
                                edge.points [i] = EditorGUILayout.Vector2Field("Point" + i.ToString(), edge.points [i]);
                            GUILayout.EndScrollView();
                        }
                        GUILayout.Label("Click a point in the scene to add points to the ledge.");
                    } else
                        EData.Manager.SelectedWalkable = 0;
                    break;
            }
        }

        bool hasCollider = false;
        Vector2 newPosition = new Vector2();
        Vector2 newScale = new Vector2();
        float newRotation = 0.0f;

        void ObjectEditing()
        {
            GUILayout.Label("2D Sprite Objects");
            hasCollider = GUILayout.Toggle(hasCollider, "Has Collider");
            newPosition = EditorGUILayout.Vector2Field("Position", newPosition);
            newScale = EditorGUILayout.Vector2Field("Scale", newScale);
            newRotation = EditorGUILayout.FloatField("Rotation", newRotation);
            if (GUILayout.Button("Create"))
            {

            }


        }

        void EditorObjectButton(GameObject o, Vector2 p, int idx, bool selected)
        {
            GUILayout.BeginHorizontal();
            Color c = GUI.backgroundColor;
            if (selected)
                GUI.backgroundColor = Color.gray;
            if (GUILayout.Button(o.name + idx.ToString()))
            {
                Vector3 sp = SceneView.lastActiveSceneView.pivot;
                SceneView.lastActiveSceneView.pivot = new Vector3(p.x, p.y, sp.z);
                Selection.activeGameObject = o;
                if (EData.SetType == ToolsetType.Walkable)
                    EData.Manager.SelectedWalkable = idx;
                if (EData.SetType == ToolsetType.Impassable)
                    EData.Manager.SelectedImpassable = idx;
                EData.ToolType = ToolType.Edit;
            }
            if (GUILayout.Button("X", GUILayout.Width(30)))
            {
                if (EData.SetType == ToolsetType.Walkable)
                    EData.Manager.RemoveWalkable(idx);
                if (EData.SetType == ToolsetType.Impassable)
                    EData.Manager.RemoveImpassable(idx);
            }
            if (selected)
                GUI.backgroundColor = c;
            GUILayout.EndHorizontal();
        }
    }
}