using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace DreamTex.Layout
{
    public class Layout : MonoBehaviour
    {
        public String Name => name;

        [SerializeField] private String name;
        [SerializeField] private RectTransform rectTransform;
        private Action callBackAction;

        public virtual void Show(Action callback = null)
        {
            gameObject.SetActive(true);
            //rectTransform.DOScale(1, ApplicationController.Instance.AnimDuration);
            rectTransform.localPosition = new Vector3(-ApplicationController.Instance.Canvas.sizeDelta.x, 0);
            rectTransform.DOAnchorPos(new Vector2(0, 0), ApplicationController.Instance.AnimDuration).OnComplete((() =>
            {
                
            }));
            if(callback!= null)
            callback();
        }

        public virtual void Hide()
        {
            rectTransform.DOAnchorPos(new Vector2(ApplicationController.Instance.Canvas.sizeDelta.x, 0),
                ApplicationController.Instance.AnimDuration).OnComplete((() => gameObject.SetActive(false)));
        }
    }
}