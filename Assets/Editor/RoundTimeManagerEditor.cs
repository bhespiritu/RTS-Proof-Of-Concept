using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RoundTimeManager))]
public class RoundTimeManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Start Round"))
        {
            ((RoundTimeManager)target).StartRound();
        }
    }
}
