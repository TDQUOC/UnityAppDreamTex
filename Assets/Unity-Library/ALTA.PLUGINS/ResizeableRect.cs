using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alta.Plugin;
using System.IO;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class ResizeableRect : MonoBehaviour
{
    [Tooltip("Refer to folder where save the setting file(root is StreamingAssets folder)")]
    public string SaveFolder;

    [HideInInspector]
    public RectSize rectSize;
    private FileInfo settingFile;
    private RectTransform thisPanel;
    private bool Effect = false;
    private bool flagClick = false;
    private Rect windowRect0;
    private EventSystem system;
    public bool isAutoRead = true;
    private int id;
    Vector2 currentPos;
    private string AnchorMinX, AnchorMinY, AnchorMaxX, AnchorMaxY, PivotX, PivotY,
        SizeX, SizeY, ScaleX, ScaleY, ScaleZ, RotationX, RotationY, RotationZ, PositionX, PositionY;
    private void Awake()
    {
        thisPanel = gameObject.GetComponent<RectTransform>();

        if (isAutoRead)
            GetSetting();
    }

    private void Start()
    {
        if (isAutoRead)
            ResetSize();
    }

    // create setting folder if it does not exist 
    public void GetSetting()
    {
        string tmpFolder = Path.Combine(Application.streamingAssetsPath, SaveFolder);
        if (!Directory.Exists(tmpFolder))
            Directory.CreateDirectory(tmpFolder);

        settingFile = new FileInfo(Path.Combine(tmpFolder, gameObject.name + ".xml"));

        if (settingFile.Exists)
            rectSize = XmlExtention.Read<RectSize>(settingFile.FullName);
        else
        {
            rectSize = XmlExtention.Read<RectSize>(settingFile.FullName);
            OverideSetting();
        }

        this.ResetSize();
    }



    public void HidePanel()
    {
        flagClick = false;
        Effect = false;
    }

    public void ShowPanel(Vector2 _poisition, int _id)
    {
        Effect = true ;
        id = _id;
        currentPos = _poisition;
        if (this.GetComponent<Button>())
            this.GetComponent<Button>().enabled = !Effect;

        if (Effect)
        {
            SetFirstValue(rectSize);
        }
       

        Debug.Log("Realtime setting mode ==> " + Effect);
    }


    private void Update()
    {

        if (!Effect || Time.frameCount % 10 != 0) return;
        try // try for read file exeption
        {
            RectSize tmp = null;
            tmp = XmlExtention.Read<RectSize>(settingFile.FullName);
            if (CompareRectSize(tmp, rectSize))
                return;
            rectSize = tmp;
            ResetSize();
            SetFirstValue(rectSize);
        }
        catch { }
    }

    public void ResetSize()
    {
        if (thisPanel == null) return;
        // set where panel dock to in canvas -- default is (0, 1)
        thisPanel.anchorMin = rectSize.AnchorMin;
        thisPanel.anchorMax = rectSize.AnchorMax;

        // set pivot(the point in panel to check distance with anchor) for panel
        thisPanel.pivot = rectSize.Pivot;

        // set distance between anchor and pivot
        thisPanel.anchoredPosition = rectSize.Position;

        // set size of panel
        thisPanel.sizeDelta = rectSize.Size;

        thisPanel.transform.localScale = rectSize.Scale;
        thisPanel.transform.localRotation = Quaternion.Euler(rectSize.Rotation);
    }



    public void OverideSetting()
    {
        if (thisPanel == null) return;
        rectSize.AnchorMin = thisPanel.anchorMin;
        rectSize.AnchorMax = thisPanel.anchorMax;
        rectSize.Pivot = thisPanel.pivot;
        rectSize.Position = thisPanel.anchoredPosition;
        rectSize.Size = thisPanel.sizeDelta;
        rectSize.Scale = thisPanel.transform.localScale;
        rectSize.Rotation = thisPanel.transform.localRotation.eulerAngles;
        rectSize.Write();
    }
    private bool CompareRectSize(RectSize a, RectSize b)
    {
        if (a.Pivot == b.Pivot && a.AnchorMin == b.AnchorMin && a.AnchorMax == b.AnchorMax && a.Position == b.Position
            && a.Size == b.Size && a.Scale == b.Scale && a.Rotation == b.Rotation)
            return true;
        return false;
    }
    void OnGUI()
    {
        
        if (Effect)
        {
            windowRect0 = GUI.Window(id, new Rect(currentPos.x, currentPos.y, 280, 280), DoMyWindow, this.name);
        }
     
    }
    private void SetFirstValue(RectSize rectSize)
    {
        AnchorMinX = rectSize.AnchorMin.x.ToString();
        AnchorMinY = rectSize.AnchorMin.y.ToString();
        AnchorMaxX = rectSize.AnchorMax.x.ToString();
        AnchorMaxY = rectSize.AnchorMax.y.ToString();
        PivotX = rectSize.Pivot.x.ToString();
        PivotY = rectSize.Pivot.y.ToString();
        SizeX = rectSize.Size.x.ToString();
        SizeY = rectSize.Size.y.ToString();
        ScaleX = rectSize.Scale.x.ToString();
        ScaleY = rectSize.Scale.y.ToString();
        ScaleZ = rectSize.Scale.z.ToString();
        RotationX = rectSize.Rotation.x.ToString();
        RotationY = rectSize.Rotation.y.ToString();
        RotationZ = rectSize.Rotation.z.ToString();
        PositionX = rectSize.Position.x.ToString();
        PositionY = rectSize.Position.y.ToString();
    }
    public void DoMyWindow(int windowID)
    {
        
        GUI.backgroundColor = Color.blue;
        GUI.Label(new Rect(10, 20, 100, 20), "AnchorMin:");
        AnchorMinX = GUI.TextField(new Rect(90, 20, 50, 20), AnchorMinX);
        AnchorMinY = GUI.TextField(new Rect(150, 20, 50, 20), AnchorMinY);
        GUI.Label(new Rect(10, 50, 100, 20), "AnchorMax:");
        AnchorMaxX = GUI.TextField(new Rect(90, 50, 50, 20), AnchorMaxX);
        AnchorMaxY = GUI.TextField(new Rect(150, 50, 50, 20), AnchorMaxY);
        GUI.Label(new Rect(10, 80, 100, 20), "Pivot:");
        PivotX = GUI.TextField(new Rect(90, 80, 50, 20), PivotX);
        PivotY = GUI.TextField(new Rect(150, 80, 50, 20), PivotY);
        GUI.Label(new Rect(10, 110, 100, 20), "Size:");
        SizeX = GUI.TextField(new Rect(90, 110, 50, 20), SizeX);
        SizeY = GUI.TextField(new Rect(150, 110, 50, 20), SizeY);
        GUI.Label(new Rect(10, 140, 100, 20), "Scale:");
        ScaleX = GUI.TextField(new Rect(90, 140, 50, 20), ScaleX);
        ScaleY = GUI.TextField(new Rect(150, 140, 50, 20), ScaleY);
        ScaleZ = GUI.TextField(new Rect(210, 140, 50, 20), ScaleZ);
        GUI.Label(new Rect(10, 170, 100, 20), "Rotation:");
        RotationX = GUI.TextField(new Rect(90, 170, 50, 20), RotationX);
        RotationY = GUI.TextField(new Rect(150, 170, 50, 20), RotationY);
        RotationZ = GUI.TextField(new Rect(210, 170, 50, 20), RotationZ);
        GUI.Label(new Rect(10, 200, 100, 20), "Position:");
        PositionX = GUI.TextField(new Rect(90, 200, 50, 20), PositionX);
        PositionY = GUI.TextField(new Rect(150, 200, 50, 20), PositionY);
        if (GUI.Button(new Rect(80, 230, 60, 30), "OK"))
        {
            RectSize realtmp = new RectSize()
            {
                AnchorMin = new Vector2(ParseStringToFloat(AnchorMinX), ParseStringToFloat(AnchorMinY)),
                AnchorMax = new Vector2(ParseStringToFloat(AnchorMaxX), ParseStringToFloat(AnchorMaxY)),
                Pivot = new Vector2(ParseStringToFloat(PivotX), ParseStringToFloat(PivotY)),
                Size = new Vector2(ParseStringToFloat(SizeX), ParseStringToFloat(SizeY)),
                Scale = new Vector3(ParseStringToFloat(ScaleX), ParseStringToFloat(ScaleY), ParseStringToFloat(ScaleZ)),
                Rotation = new Vector3(ParseStringToFloat(RotationX), ParseStringToFloat(RotationY), ParseStringToFloat(RotationZ)),
                Position = new Vector2(ParseStringToFloat(PositionX), ParseStringToFloat(PositionY))
            };
            XmlExtention.Write<RectSize>(realtmp, settingFile.FullName);
        }
        if (GUI.Button(new Rect(150, 230, 60, 30), "Cancel"))
        {
          
           
            HidePanel();
        }
    }
    private float ParseStringToFloat(string ss)
    {
        try
        {
            return float.Parse(ss);
        }
        catch
        {
            return 0;
        }
    }


}
[Serializable]
public class RectSize
{
    public Vector2 AnchorMin = new Vector2(0, 1);
    public Vector2 AnchorMax = new Vector2(0, 1);
    public Vector2 Pivot = new Vector2(0, 1);
    public Vector2 Position = new Vector2(0, 0);
    public Vector2 Size = new Vector2(1920, 1080);
    public Vector3 Scale = Vector3.one;
    public Vector3 Rotation = Vector3.zero;
}