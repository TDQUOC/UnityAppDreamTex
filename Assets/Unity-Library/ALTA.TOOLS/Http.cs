using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Networking;
using System.IO;

namespace Alta.Tools
{
    /// <summary>
    /// Multil Http request suppot restful api
    /// </summary>
    public static class HttpRequest
    {
        public static void GET(this GameObject go, string url, Dictionary<string, string> headers = null)
        {
            Func<UnityWebRequest, bool> callback = (www) =>
            {
                go.SendMessage("httpCallback", www);
                return string.IsNullOrEmpty(www.error);
            };
            REQUEST(url, null, null, headers, callback, false, null);
        }
        public static void GET(string url, Func<UnityWebRequest, bool> callback, Dictionary<string, string> headers = null)
        {
            REQUEST(url, null, null, headers, callback, false, null);
        }

        public static void POST(string url, WWWForm postForm, string rawJson, Func<UnityWebRequest, bool> callback, Dictionary<string, string> headers = null)
        {
            REQUEST(url, postForm, rawJson, headers, callback, false, null);
        }

        

        public static void POST(this GameObject go, string url, WWWForm postForm, Dictionary<string, string> headers = null)
        {
            Func<UnityWebRequest, bool> callback = (www) =>
            {
                go.SendMessage("httpCallback", www);
                return string.IsNullOrEmpty(www.error);
            };
            REQUEST(url, postForm, null, headers, callback, false, null);
        }

        public static void POST(this GameObject go, string url, string rawJson, Dictionary<string, string> headers = null)
        {
            Func<UnityWebRequest, bool> callback = (www) =>
            {
                go.SendMessage("httpCallback", www);
                return string.IsNullOrEmpty(www.error);
            };

            REQUEST(url, null, rawJson, headers, callback, false, null);
        }

        public static void DOWNLOAD(string url, string savePath, Func<UnityWebRequest, bool> callback, WWWForm postForm = null, string rawJson = null, Dictionary<string, string> headers = null)
        {
            REQUEST(url, postForm, rawJson, headers, callback, true, savePath);
        }

        public static void DOWNLOAD(this GameObject go, string url, string savePath, WWWForm postForm = null, string rawJson = null, Dictionary<string, string> headers = null)
        {
            Func<UnityWebRequest, bool> callback = (www) =>
            {
                go.SendMessage("httpCallback", www);
                return string.IsNullOrEmpty(www.error);
            };
            REQUEST(url, postForm, rawJson, headers, callback, true, savePath);
        }

        private static void REQUEST(string url, WWWForm form, string rawJson, Dictionary<string, string> h, Func<UnityWebRequest, bool> c, bool isDownload, string savePath)
        {
            if (RequestManager.Global == null)
            {
                GameObject managerRequest = new GameObject();
                managerRequest.AddComponent<RequestManager>();
#if UNITY_EDITOR
                managerRequest.name = "[HTTTP] MANAGER";
#endif
            }
            RequestManager.Global.REQUEST(url, form, rawJson, h, c, isDownload, savePath);
        }
    }
}