using System.Collections;
using System.Collections.Generic;
using UAI.Demo.Terrain;
using UAI.GeneralAI;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Navigator))]
public class NavigatorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        Navigator navigator = (Navigator)target;

        if (DrawDefaultInspector())
        {

        }

        if (GUILayout.Button("Navigate")) {
            navigator.FindPath(navigator.start, navigator.target);
        }
    }
}
