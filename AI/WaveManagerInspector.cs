using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WaveManager))]
public class WaveManagerInspector : Editor
{
    public override void OnInspectorGUI()
    {
        WaveManager myScript = (WaveManager)target;
        EditorGUILayout.BeginHorizontal();
        if(GUILayout.Button("+Goblin", GUILayout.Width(75), GUILayout.Height(25)))
        {
            myScript.AddGoblin();
        }
        if(GUILayout.Button("+Troll", GUILayout.Width(75), GUILayout.Height(25)))
        {
            myScript.AddTroll();
        }
        if(GUILayout.Button("+Ogre", GUILayout.Width(75), GUILayout.Height(25)))
        {
            myScript.AddOgre();
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        if(GUILayout.Button("+Add", GUILayout.Width(75), GUILayout.Height(25)))
        {
            myScript.AddWave();
        }
        if(GUILayout.Button("+Get", GUILayout.Width(75), GUILayout.Height(25)))
        {
            myScript.GetWave();
        }
        if(GUILayout.Button("+Remove", GUILayout.Width(75), GUILayout.Height(25)))
        {
            myScript.RemoveWave();
        }
        EditorGUILayout.EndHorizontal();


        DrawDefaultInspector();

    }
}