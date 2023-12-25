using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    public event Action<Texture2D> OnTakePictureSuccess;
    public RawImage rawImage;

    void Start()
    {
        //OpenCamera();
    }

    public void OpenCamera()
    {
        NativeCamera.Permission permission = NativeCamera.CheckPermission(true);

        if (permission == NativeCamera.Permission.Granted)
        {
            NativeCamera.TakePicture((path) =>
            {
                if (!string.IsNullOrEmpty(path))
                {
                    // Load hình ảnh từ đường dẫn và hiển thị lên RawImage
                    StartCoroutine(LoadImage(path));
                }
            });
        }
        else
        {
            Debug.LogError("Quyền truy cập camera bị từ chối.");
        }
    }

    IEnumerator LoadImage(string path)
    {
        // Đọc dữ liệu hình ảnh từ đường dẫn
        WWW www = new WWW("file://" + path);
        yield return www;

        // Tạo Texture2D từ dữ liệu hình ảnh
        Texture2D texture = www.texture;

        // Hiển thị hình ảnh trên RawImage
        rawImage.texture = texture;
        if(texture!=null)
            OnTakePictureSuccess?.Invoke(texture);
    }
}
