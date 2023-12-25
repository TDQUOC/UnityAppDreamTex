using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class RequestManager : MonoBehaviour
{
    public static RequestManager Global;
    private Dictionary<int, Func<UnityWebRequest, bool>> queue;
    private Coroutine httpCoroutine;
    private void Awake()
    {
        if (Global != null)
        {
            Destroy(this);
            return;
        }
        Global = this;
        DontDestroyOnLoad(gameObject);
        queue = new Dictionary<int, Func<UnityWebRequest, bool>>();
    }

    public void REQUEST(string url, WWWForm form, string rawJson, Dictionary<string, string> header, Func<UnityWebRequest, bool> c, bool isDownload, string savePath)
    {
        UnityWebRequest www;
        if (form == null && string.IsNullOrEmpty(rawJson))
        {
            www = UnityWebRequest.Get(url);
        }
        else
        {
            if (string.IsNullOrEmpty(rawJson))
                www = UnityWebRequest.Post(url, form);
            else
            {
                www = UnityWebRequest.Put(url, System.Text.Encoding.UTF8.GetBytes(rawJson));
                www.method = "POST";
                www.SetRequestHeader("Content-Type", "application/json");
            }
        }
        if (header != null)
        {
            foreach (string k in header.Keys)
            {
                if (string.IsNullOrEmpty(www.GetRequestHeader(k)))
                    www.SetRequestHeader(k, header[k]);
            }
        }
        if (c != null)
            queue.Add(www.GetHashCode(), c);
        if (isDownload)
        {
            FileInfo file = new FileInfo(savePath);
            if (file.Exists)
                file.Delete();
            if (!file.Directory.Exists)
                file.Directory.Create();
            www.downloadHandler = new DownloadHandlerFile(savePath);
        }
        httpCoroutine = StartCoroutine(WaitForRequest(www));
    }

    private IEnumerator WaitForRequest(UnityWebRequest www)
    {
        yield return www.SendWebRequest();
        int code = www.GetHashCode();
        if (this.queue.ContainsKey(code))
        {
            this.queue[code](www);
            this.queue.Remove(code);
        }
        else
        {
            Debug.LogError("HTTP: " + www.url);
        }
    }
}

