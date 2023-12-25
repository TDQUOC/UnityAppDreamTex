using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Alta.Plugin
{
    public static  class JsonExtension
    {
        public static string GetJsonField(string jsonString, string fieldname)
        {
            JObject job = JObject.Parse(jsonString);
            try
            {
                Debug.Log("get field successful");
                return job[fieldname].ToString();
            }
            catch (Exception e)
            {
                Debug.LogWarning(e.GetBaseException().ToString());
                return null;
            }
        }
    }
}
