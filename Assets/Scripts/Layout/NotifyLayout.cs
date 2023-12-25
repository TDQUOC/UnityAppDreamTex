using System;
using System.Collections;
using System.Collections.Generic;
using DreamTex.Layout;
using UnityEngine;
using UnityEngine.UI;

public class NotifyLayout : Layout
{
    [SerializeField] private GameObject notifyPanel;
    [SerializeField] private Text notifyText;
    [SerializeField] private Button okButton;
    
    private Action callbackAction;

    public void ShowNotify(string msg,Action callback = null, bool isShowOKButton = true)
    {
        gameObject.SetActive(true);
        notifyText.text = msg;
        callbackAction = callback;
        okButton.gameObject.SetActive(isShowOKButton);
    }
    
    public void OnOKButtonClick()
    {
        if (callbackAction != null)
            callbackAction();
        gameObject.SetActive(false);
    }
}
