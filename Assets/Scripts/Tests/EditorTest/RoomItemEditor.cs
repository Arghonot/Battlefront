using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RoomItem))]
public class RoomItemEditor : Editor
{
    public override void OnInspectorGUI()
    {
        RoomItem item = target as RoomItem;

        base.DrawDefaultInspector();

        EditorGUILayout.Separator();

        for (int i = 0; i < item.targetPoints.Length; i++)
        {
            EditorGUILayout.Vector4Field("value : ", Vector4.one * item.targetPoints[i]);
            item.targetPoints[i] = EditorGUILayout.IntField(item.targetPoints[i]);
        }

        if (GUILayout.Button("Display !"))
        {
            item.Display();
        }

        //serializedObject.Update();
        //var controller = target as RoomItem;
        //EditorGUIUtility.LookLikeInspector();
        //SerializedProperty tps = serializedObject.FindProperty("targetPoints");
        //EditorGUI.BeginChangeCheck();
        //EditorGUILayout.PropertyField(tps, true);
        //if (EditorGUI.EndChangeCheck())
        //    serializedObject.ApplyModifiedProperties();
        //EditorGUIUtility.LookLikeControls();
        //// ...
    }
}