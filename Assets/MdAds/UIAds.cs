using System;
using UnityEngine;
using UnityEngine.UI;

namespace MdAds
{
    public class UIAds : MonoBehaviour
    {
        public InputField inputField;
        private UniWebView _webView;

        private void Start()
        {
            InitWebView();
            Destroy(GetComponent<Image>());
        }

        public void LoadAd()
        {
            _webView.ReferenceRectTransform = GetComponent<RectTransform>();
            _webView.Load(inputField.text);
        }

        public void ShowAd()
        {
            _webView.Show();
        }

        public void HideAd()
        {
            _webView.Hide();
        }

        private void InitWebView()
        {
            var webViewGameObject = new GameObject("UniWebView");
            _webView = webViewGameObject.AddComponent<UniWebView>();
        }
    }
}