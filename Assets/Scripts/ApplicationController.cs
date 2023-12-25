using System.Collections;
using System.Collections.Generic;
using DataStruct;
using DreamTex.Layout;
using UnityEngine;

public class ApplicationController : SingletonBase<ApplicationController>
{
    public EmployeeData EmployeeData => employeeData;
    public DaySummaryData DaySummaryData => daySummaryData;
    public int TargetWidth => targetWidth;
    public int TargetHeight => targetHeight;
    public float AnimDuration => animDuration;
    public bool AlreadyCheckIn { get; set; }
    public bool AlreadyCheckOut { get; set; }

    
    public RectTransform Canvas => canvas;

    
    [SerializeField] private RectTransform canvas;
    [SerializeField] private float animDuration = 0.5f;
    [SerializeField] private int targetWidth = 1920;
    [SerializeField] private int targetHeight = 1080;
    

    [SerializeField] private CheckInLayout checkInLayout;
    
    private EmployeeData employeeData;
    private DaySummaryData daySummaryData;
    
    
    public void SetEmployeeData(EmployeeData employeeData)
    {
        this.employeeData = employeeData;
    }
    public void SetDaySummaryData(DaySummaryData daySummaryData)
    {
        this.daySummaryData = daySummaryData;
    }

    public void ShowMainLayout()
    {
        LayoutController.Instance.ActiveLayout("main");
        // LayoutController.Instance.SetSummaryText();
    }

    public void ShowCheckInLayout()
    {
        LayoutController.Instance.ActiveLayout("checkin",(() => Debug.Log("hello")));
    }
    
    public void ShowCheckOutLayout()
    {
        LayoutController.Instance.ActiveLayout("checkin",(() => checkInLayout.SetType(CheckInLayout.Type.CheckOut)));
    }
    public void UpdateDaySummary(bool isBackToHome = false)
    {
        
        LayoutController.Instance.ShowNotifyLayout("Đang trở về màn hình chính...",null, false);
        HTTPManager.Instance.UpdateDaySummary(isBackToHome);
        
    }
}
