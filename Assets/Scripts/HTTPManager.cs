
using System;
using BestHTTP;
using DataStruct;
using DreamTex.Layout;
using Newtonsoft.Json;
using UnityEngine;

public class HTTPManager : SingletonBase<HTTPManager>
{
    [SerializeField] private string hostUrl = "http://localhost:5503";
    [SerializeField] private string loginUrl = "/api/employee/login";
    [SerializeField] private string updateDaySummaryUrl = "/api/day-summary/getbyid";
    [SerializeField] private string checkInUrl = "/api/day-summary/addCheckIn";
    [SerializeField] private string checkOutUrl = "/api/day-summary/addCheckOut";

    // ReSharper disable Unity.PerformanceAnalysis
    public void Login(string username, string password)
    {
        try
        {
            ResponeData responeData = new ResponeData();
            HTTPRequest request =
                new HTTPRequest(new Uri(hostUrl + loginUrl), HTTPMethods.Post, (req, res) =>
                {
                    responeData = JsonConvert.DeserializeObject<ResponeData>(res.DataAsText);
                    LoginLayout.OnLoginRes?.Invoke(responeData);
                });
            request.AddField("username", username);
            request.AddField("password", password);
            request.Send();
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }
    
    public async void UpdateDaySummary(bool isBackToHome = false)
    {
        try
        {
            HTTPRequest request = new HTTPRequest(new Uri(hostUrl + updateDaySummaryUrl), HTTPMethods.Post, (req, res) =>
            {
                ResponeData responeData = JsonConvert.DeserializeObject<ResponeData>(res.DataAsText);
                DaySummaryData daySummaryData = JsonConvert.DeserializeObject<DaySummaryData>(responeData.Data.ToString());
                ApplicationController.Instance.SetDaySummaryData(daySummaryData);
                //LayoutController.Instance.ActiveLayout("main");
                LayoutController.Instance.HideNotifyLayout();
                if (isBackToHome)
                {
                    LayoutController.Instance.ActiveLayout("main");
                }
            });
            request.AddField("id", ApplicationController.Instance.DaySummaryData._id);
            request.Send();
        }catch(Exception e)
        {
            Debug.LogError(e);
        }
    }

    public void SubmitCheckIn(Texture2D image)
    {
        try
        {
            LayoutController.Instance.ShowNotifyLayout("Đang check in...",null,false);
            byte[] bytes = image.EncodeToPNG();
            ResponeData responeData = new ResponeData();
            HTTPRequest request =
                new HTTPRequest(new Uri(hostUrl + checkInUrl), HTTPMethods.Post, (req, res) =>
                {
                    responeData = JsonConvert.DeserializeObject<ResponeData>(res.DataAsText);
                    Debug.Log(responeData.Msg);
                    LayoutController.Instance.ShowNotifyLayout(responeData.Msg.ToString(), () =>
                    {
                        ApplicationController.Instance.UpdateDaySummary(true);
                    });
                });
            request.AddBinaryData("image", bytes, "image.png", "image/png");
            request.AddField("id", ApplicationController.Instance.DaySummaryData._id);
            request.AddField("employeeId", ApplicationController.Instance.EmployeeData._id);
            request.AddField("time", GetTime("HH:mm:ss"));
            request.Send();
            
        }catch(Exception e)
        {
            Debug.LogError(e);
        }
    }
    
    public void SubmitCheckOut(Texture2D image)
    {
        try
        {
            LayoutController.Instance.ShowNotifyLayout("Đang check out...",null,false);
            byte[] bytes = image.EncodeToPNG();
            ResponeData responeData = new ResponeData();
            HTTPRequest request =
                new HTTPRequest(new Uri(hostUrl + checkOutUrl), HTTPMethods.Post, (req, res) =>
                {
                    responeData = JsonConvert.DeserializeObject<ResponeData>(res.DataAsText);
                    Debug.Log(res.DataAsText);
                    Debug.Log(responeData.Msg);
                    LayoutController.Instance.ShowNotifyLayout(responeData.Msg.ToString(), () =>
                    {
                        ApplicationController.Instance.UpdateDaySummary(true);
                    });
                });
            request.AddBinaryData("image", bytes, "image.png", "image/png");
            request.AddField("id", ApplicationController.Instance.DaySummaryData._id);
            request.AddField("employeeId", ApplicationController.Instance.EmployeeData._id);
            request.AddField("time", GetTime("HH:mm:ss"));
            request.Send();
            
        }catch(Exception e)
        {
            Debug.LogError(e);
        }
    }

    private string GetTime(string formatString)
    {
        return DateTime.Now.ToString(formatString);
    }

    
}