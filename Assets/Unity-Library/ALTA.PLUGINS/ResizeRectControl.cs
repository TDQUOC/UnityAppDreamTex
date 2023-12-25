using System;
using UnityEngine;

public class ResizeRectControl : MonoBehaviour
{
    public ResizeableRect[] resizeRects;
    [SerializeField] bool isShowControl;

    Rect windowRect;

    private void Awake()
    {
        resizeRects = FindObjectsOfType<ResizeableRect>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F12))
        {
            isShowControl = !isShowControl;

        }
    }

    public void OnGUI()
    {
        if (isShowControl)
        {
            windowRect = GUI.Window(0, new Rect(0, 0, 280, 580), DrawControlPanel, this.name);
        }
    }
    Vector2 scrollViewVector;
    private void DrawControlPanel(int id)
    {
        GUI.backgroundColor = Color.green;
        GUI.Label(new Rect(10, 20, 100, 20), "Resize List:");
        scrollViewVector = GUI.BeginScrollView(new Rect(100, 20, 300, 500), scrollViewVector, new Rect(0, 0, 300, 25* resizeRects.Length), true, true);
        GUI.Box(new Rect(0, 0, 300, 500), "");
        for (int i = 0; i < resizeRects.Length; i++)
        {
            if (GUI.Button(new Rect(0, i * 25, 150, 25), ""))
            {
                ShowSelectedPanel(i);
            }
            GUI.Label(new Rect(5, i * 25, 150, 25), resizeRects[i].name);
        }
        GUI.EndScrollView();
    }

    private void ShowSelectedPanel(int _index)
    {
        for (int i = 0; i < resizeRects.Length; i++)
        {
            ResizeableRect rect = resizeRects[i].GetComponent<ResizeableRect>();
            if (i == _index)
            {
                rect.ShowPanel(new Vector2(310, 0), 1);
            }
            else
            {
                rect.HidePanel();
            }
        }
    }
}