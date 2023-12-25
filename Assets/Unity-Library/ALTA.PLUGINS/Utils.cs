using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;
using System.Security.Cryptography;
using System;
namespace Alta.Plugin
{
    public static class Utils{
        public static bool IsEmulator()
        {
#if UNITY_ANDROID
            AndroidJavaClass osBuild;
            osBuild = new AndroidJavaClass("android.os.Build");
            string fingerPrint = osBuild.GetStatic<string>("FINGERPRINT");
            return fingerPrint.Contains("generic");
#endif
            return false;
        }
    }
}