using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


#if UNITY_EDITOR

[CustomEditor(typeof(ResizeRectControl))]
public class ResizeRectControlEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        ResizeRectControl resizeControl = (ResizeRectControl) target;
        if (GUILayout.Button("Add ResizeRect Component"))
        {
            Debug.Log("Clicked");
            foreach (var item in resizeControl.resizeRects)
            {
                ResizeableRect tmpRect = item.GetComponent<ResizeableRect>();
                //if(tmpRect == null)
                //{
                //    item.AddComponent<ResizeableRect>();
                //}
            }
        }
    }
}
#endif