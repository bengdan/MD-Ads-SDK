using System;
using UnityEngine;
using UnityEngine.UI;

namespace MdAds
{
    public class UIAds : MonoBehaviour
    {
        public InputField inputField;
        private UniWebView _webView;

        private int _width;
        private int _height;

        private void Start()
        {
            Application.RequestAdvertisingIdentifierAsync((string advertisingId, bool trackingEnabled, string error) => TrafficInfo.Idfa = advertisingId);
            InitWebView();
            Destroy(GetComponent<Image>());
        }

        public void LoadAd()
        {
            var url = $"http://ads.game.melozen.com/get_ads?placementwidth={_width}placementheight={_height}&os={TrafficInfo.OS}&devicemodel={TrafficInfo.DeviceModel}&idfa={TrafficInfo.Idfa}&deviceid={TrafficInfo.DeviceId}&appname={TrafficInfo.AppName}&bundle={TrafficInfo.Bundle}&appversion={TrafficInfo.AppVersion}";
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