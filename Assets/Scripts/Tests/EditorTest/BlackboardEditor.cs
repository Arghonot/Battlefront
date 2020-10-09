using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BlackBoardTest))]
public class BlackboardEditor : Editor
{
    SerializedProperty Elems;

    void OnEnable()
    {
        Elems = serializedObject.FindProperty("elems");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        var blackboard = target as BlackBoardTest;
        var targetelems = serializedObject.FindProperty("elems"); 
        EditorGUILayout.PropertyField(Elems);

        foreach (var item in blackboard.elems)
        {
            EditorGUI.FloatField(EditorGUILayout.BeginVertical(), 0f);
        }        
    }
}
