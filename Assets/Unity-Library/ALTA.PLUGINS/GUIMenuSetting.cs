using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class GUIMenuSetting : MonoBehaviour
{
    private Button btnSetting;
    private bool isOpenSetting;

    // GUI component
    List<Vector2> screenRess;
    List<FullScreenMode> screenModes;
    List<string> screenQualities;

    int currentQuality = 0;
    int currentScreen = 0;
    int currentRes = 0;


    private void Awake()
    {
        // Thêm nút mở GUI
        if (gameObject.GetComponent<Button>() == null) { btnSetting = gameObject.AddComponent<Button>(); btnSetting.onClick.AddListener(() => OpenSetting()); }
        else { btnSetting.onClick.AddListener(() => OpenSetting()); }

        // Loại Resolution
        screenRess = new List<Vector2>();
        foreach (Resolution res in Screen.resolutions)
            screenRess.Add(new Vector2(res.width, res.height));
        screenRess = screenRess.Distinct().ToList();

        // Loại màn hình
        screenModes = new List<FullScreenMode>();
        foreach (FullScreenMode mode in Enum.GetValues(typeof(FullScreenMode)))
            screenModes.Add(mode);


        // Chất lượng mành hình
        screenQualities = new List<string>();
        foreach (string name in QualitySettings.names)
            screenQualities.Add(name);
    }


    void OnGUI()
    {
        if (isOpenSetting)
        {
            float widthSetting = Screen.height / 2f;
            float heightSetting = Screen.width;
            float widthBox = Screen.width / 2 - widthSetting / 2;
            float heightBox = Screen.height / 2 - heightSetting / 2;

            GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
            GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "Setting");
            GUILayout.EndArea();

            GUILayout.BeginArea(new Rect(widthBox, 25, widthSetting / 2, heightSetting));
            GUILayout.BeginVertical();

            // Resolution 
            GUILayout.Label("Resolution:");
            for (int i = 0; i < screenRess.Count; i++)
            {
                bool isRes = GUILayout.Toggle(i == currentRes, screenRess[i].x + " x " + screenRess[i].y);
                if (isRes)
                {
                    //Debug.Log(modes[i] + " : " + i);
                    currentRes = i;
                }
            }

            GUILayout.EndVertical();
            GUILayout.EndArea();

            GUILayout.BeginArea(new Rect(widthBox + widthSetting / 2, 25, widthSetting / 2, heightSetting));
            GUILayout.BeginVertical();

            // Fullscreen 
            GUILayout.Label("");
            GUILayout.Label("Mode:");
            for (int i = 0; i < screenModes.Count; i++)
            {
                bool isMode = GUILayout.Toggle(i == currentScreen, screenModes[i].ToString());
                if (isMode)
                {
                    //Debug.Log(modes[i] + " : " + i);
                    currentScreen = i;
                }
            }

            // Quality 
            GUILayout.Label("");
            GUILayout.Label("Quality:");
            for (int i = 0; i < screenQualities.Count; i++)
            {
                bool isQuality = GUILayout.Toggle(i == currentQuality, screenQualities[i]);
                if (isQuality)
                {
                    // Debug.Log(names[i] + " : " + i);
                    currentQuality = i;
                }
            }

            // 
            GUILayout.Label("");
            if (GUILayout.Button("Apply"))
            {
                Debug.Log("Cáo từ");
                isOpenSetting = !isOpenSetting;
                StartCoroutine(ApplySetting(currentRes, currentScreen, currentQuality));
            }

            GUILayout.EndVertical();
            GUILayout.EndArea();
        }
    }


    private void OpenSetting()
    {
        currentQuality = QualitySettings.GetQualityLevel();
        currentScreen = screenModes.IndexOf(Screen.fullScreenMode);
        currentRes = screenRess.IndexOf(new Vector2(Screen.width, Screen.height));

        isOpenSetting = !isOpenSetting;
    }


    IEnumerator ApplySetting(int screenRes, int screenMode, int screenQuality)
    {
        Screen.fullScreenMode = (FullScreenMode)Enum.GetValues(typeof(FullScreenMode)).GetValue(screenMode);
        yield return new WaitForSeconds(0.5f);
        QualitySettings.SetQualityLevel(screenQuality);
        yield return new WaitForSeconds(0.5f);
        Screen.SetResolution((int)screenRess[screenRes].x, (int)screenRess[screenRes].y, Screen.fullScreen);
    }
}
