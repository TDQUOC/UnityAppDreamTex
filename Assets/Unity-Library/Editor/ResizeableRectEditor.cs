using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(ResizeableRect), true)]
public class ResizeableRectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        ResizeableRect rs = (ResizeableRect)target;
        if (GUILayout.Button("Overide Setting"))
        {
            rs.OverideSetting();
        }
        if (GUILayout.Button("Reset Setting"))
        {
            rs.ResetSize();
        }
    }

}
