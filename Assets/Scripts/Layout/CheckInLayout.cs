using System;
using System.Collections;
using System.Collections.Generic;
using DreamTex.Layout;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class CheckInLayout : Layout
{
    public enum Type
    {
        CheckIn,
        CheckOut
    }
    [SerializeField] private Button submitButton;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private RawImage image;
    [SerializeField] private Text submitBtnText;

    private Type type;

    private void OnEnable()
    {
        cameraController.OnTakePictureSuccess += OnTakePictureSuccess;
    }

    public void SetType(Type type)
    {
        this.type = type;
        if (type == Type.CheckIn)
        {
            submitBtnText.text = "CHECK IN";
        }else if (this.type == Type.CheckOut)
        {
            submitBtnText.text = "CHECK OUT";
        }
    }

    private void OnDisable()
    {
        cameraController.OnTakePictureSuccess -= OnTakePictureSuccess;
    }

    public override void Show(Action callback = null)
    {
        base.Show();
        submitButton.interactable = false;
        image.texture = null;

    }

    private void OnTakePictureSuccess(Texture2D obj)
    {
        submitButton.interactable = true;
    }

    public void OnClickTakePicture()
    {
        cameraController.OpenCamera();
    }

    public void Submit()
    {
        Texture2D output = ModuleTextureScaler.ScaleTexture(cameraController.rawImage.texture as Texture2D,
            ApplicationController.Instance.TargetWidth,
            ApplicationController.Instance.TargetHeight);
        if (type == Type.CheckIn)
        {
            HTTPManager.Instance.SubmitCheckIn(output);
        }else if (type == Type.CheckOut)
        {
            HTTPManager.Instance.SubmitCheckOut(output);
        }
        
    }
}