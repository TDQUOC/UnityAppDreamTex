using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;

public class DebugOnGUI : MonoBehaviour
{
    [SerializeField]
    private GUIStyle guiStyle = new GUIStyle();

    private StringBuilder logBuilder = new StringBuilder();
    private Stack logStacks = new Stack();

    private bool isOpenLog = false;
    private bool isOpenAdvancedSetting = false;

    private float rValue = 1.0f;
    private float gValue = 1.0f;
    private float bValue = 1.0f;
    private float aValue = 1.0f;

    private Vector2 scrollPosition;

    private int selectedAlignment = 0;
    private string[] alignments = { "Upper left", "Upper center", "Upper right" };

    private int selectedColor = 0;
    private string[] colors = { "White", "Red", "Black" };

    private void Awake()
    {
        guiStyle.wordWrap = true;
        guiStyle.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        guiStyle.normal.textColor = new Color(rValue, gValue, bValue, aValue);
    }

    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        if (!isOpenLog)
            return;

        logBuilder.Length = 0;
        logBuilder.Append("\n [");
        logBuilder.Append(DateTime.Now.ToLongTimeString());
        logBuilder.Append("] : ");
        logBuilder.Append(logString);
        logBuilder.Append("\n");
        logBuilder.Append(stackTrace);

        logStacks.Push(logBuilder.ToString());
        logBuilder.Length = 0;
        foreach (string log in logStacks)
        {
            logBuilder.Append(log);
            logBuilder.Append("\n---------------------------------------------------------");
        }
    }

    void OnGUI()
    {
        if (isOpenLog)
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.MaxWidth(500f));

            GUILayout.Label(logBuilder.ToString(), guiStyle);

            GUILayout.EndScrollView();

            #region font size
            GUILayout.BeginHorizontal();
            try
            {
                GUILayout.Label("Font size: ", GUILayout.Width(80f));
                guiStyle.fontSize = int.Parse(GUILayout.TextField(guiStyle.fontSize.ToString(), 9));
            }
            catch (Exception)
            {
                guiStyle.fontSize = 10;
            }
            GUILayout.EndHorizontal();
            #endregion

            if (isOpenAdvancedSetting)
            {
                #region red slider
                GUILayout.BeginHorizontal();
                GUILayout.Label(string.Format("R: {0:0.##}", rValue), GUILayout.Width(50f));
                rValue = GUILayout.HorizontalSlider(rValue, 0f, 1.0f);
                GUILayout.EndHorizontal();
                #endregion

                #region green slider
                GUILayout.BeginHorizontal();
                GUILayout.Label(string.Format("G: {0:0.##}", gValue), GUILayout.Width(50f));
                gValue = GUILayout.HorizontalSlider(gValue, 0f, 1.0f);
                GUILayout.EndHorizontal();
                #endregion

                #region blue slider
                GUILayout.BeginHorizontal();
                GUILayout.Label(string.Format("B: {0:0.##}", bValue), GUILayout.Width(50f));
                bValue = GUILayout.HorizontalSlider(bValue, 0f, 1.0f);
                GUILayout.EndHorizontal();
                #endregion

                #region alpha slider
                GUILayout.BeginHorizontal();
                GUILayout.Label(string.Format("A: {0:0.##}", aValue), GUILayout.Width(50f));
                aValue = GUILayout.HorizontalSlider(aValue, 0f, 1.0f);
                GUILayout.EndHorizontal();
                #endregion

                guiStyle.normal.textColor = new Color(rValue, gValue, bValue, aValue);

                GUILayout.BeginHorizontal();

                selectedAlignment = GUILayout.SelectionGrid(selectedAlignment, alignments, 3);

                switch (selectedAlignment)
                {
                    case 0:
                        guiStyle.alignment = TextAnchor.UpperLeft;
                        break;
                    case 1:
                        guiStyle.alignment = TextAnchor.UpperCenter;
                        break;
                    case 2:
                        guiStyle.alignment = TextAnchor.UpperRight;
                        break;
                    default:
                        break;
                }

                GUILayout.EndHorizontal();
            }
            else
            {
                GUILayout.BeginHorizontal();

                selectedColor = GUILayout.SelectionGrid(selectedColor, colors, 3);

                switch (selectedColor)
                {
                    case 0:
                        guiStyle.normal.textColor = Color.white;
                        break;
                    case 1:
                        guiStyle.normal.textColor = Color.red;
                        break;
                    case 2:
                        guiStyle.normal.textColor = Color.black;
                        break;
                    default:
                        break;
                }

                GUILayout.EndHorizontal();
            }

            guiStyle.wordWrap = GUILayout.Toggle(guiStyle.wordWrap, "Word wrap");
            isOpenAdvancedSetting = GUILayout.Toggle(isOpenAdvancedSetting, "Advanced setting");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F11))
        {
            logStacks.Clear();
            logBuilder.Length = 0;
            isOpenLog = !isOpenLog;
        }
    }
}