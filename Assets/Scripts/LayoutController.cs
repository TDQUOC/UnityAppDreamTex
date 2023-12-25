using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DreamTex.Layout;
using UnityEngine;

namespace DreamTex.Layout
{
    public class LayoutController : SingletonBase<LayoutController>
    {
        [SerializeField] private List<Layout> layouts = new List<Layout>();
        [SerializeField] private NotifyLayout notifyLayout;

        public void ActiveLayout(string name, Action callback = null)
        {
            foreach (var layout in layouts)
            {
                if (layout.Name == name)
                    layout.Show();
                else
                    layout.Hide();
            }
            if(callback!= null)
            callback();
        }
        
        public void ShowNotifyLayout(string msg,Action callback = null, bool isShowOKButton = true)
        {
            notifyLayout.ShowNotify(msg,callback,isShowOKButton);
        }

        public void HideNotifyLayout()
        {
            notifyLayout.gameObject.SetActive(false);
        }

        // public void SetSummaryText()
        // {
        //     if (ApplicationController.Instance.DaySummaryData != null)
        //     {
        //         layouts.FirstOrDefault(l => l.Name == "main").GetComponent<MainLayout>().SetsumaryText();
        //     }
        // }
    }
}
