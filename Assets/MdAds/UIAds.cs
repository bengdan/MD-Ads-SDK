using System;
using UnityEngine;
using UnityEngine.UI;

namespace MdAds
{
    public class UIAds : MonoBehaviour
    {
        [SerializeField]
        private int width;

        [SerializeField]
        private int height;

        [SerializeField] 
        private bool isDebug;
        
        [SerializeField] 
        private bool autoShow;

        [SerializeField] 
        private bool autoHide;

        private UniWebView _webView;
        private bool _noAds = true;

        private void Awake()
        {
            if (TrafficInfo.Idfa == default)
            {
                Application.RequestAdvertisingIdentifierAsync((string advertisingId, bool trackingEnabled, string error) => TrafficInfo.Idfa = advertisingId);
            }
            InitWebView();
            Destroy(GetComponent<Image>());
        }

        private void OnEnable()
        {
            if (!autoShow) return;
            LoadAd(true);
        }

        private void OnDisable()
        {
            if (autoHide)
            {
                HideAd();
            }
        }

        public void LoadAd(bool showAfter = false)
        {
            var url = $"http://ads.game.melozen.com/get_ads?placementwidth={width}&placementheight={height}&os={TrafficInfo.OS}&devicemodel={TrafficInfo.DeviceModel}&idfa={TrafficInfo.Idfa}&deviceid={TrafficInfo.DeviceId}&appname={TrafficInfo.AppName}&bundle={TrafficInfo.Bundle}&appversion={TrafficInfo.AppVersion}&publisher_id=1000163&channel=MD-SDK%3A{width}x{height}";
            _webView.ReferenceRectTransform = GetComponent<RectTransform>();
            _webView.Load(url);
            
            if (isDebug)
            {
                GUIUtility.systemCopyBuffer = url;
            }
            
            // Add Callback
            _webView.OnPageFinished += (view, code, s) =>
            {
                _noAds = code != 200;
                if (showAfter)
                {
                    ShowAd();
                }
                if (!isDebug) return;
                ShowToast(code== 200 ? "Ad loaded!":$"No Ads! code :{code}");
            };

            // Capture Landing Pages
            _webView.AddUrlScheme("md");
            _webView.OnMessageReceived += (view, message) =>
            {
                if (message.Scheme == "md")
                {
                    if (message.Path != "landing-page") return;
                    
                    var hasUrl = message.Args.ContainsKey("url");
                    if (!hasUrl) return;
                    
                    var landingPage = message.Args["url"];
                    Application.OpenURL(Uri.UnescapeDataString(landingPage));
                }
            };
        }

        public void ShowAd()
        {
            if (_noAds)
            {
                return;
            }
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

        private void ShowToast(string msg)
        {
            //create a Toast class object
            AndroidJavaClass toastClass =
                new AndroidJavaClass("android.widget.Toast");

            //create an array and add params to be passed
            object[] toastParams = new object[3];
            AndroidJavaClass unityActivity =
                new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            toastParams[0] =
                unityActivity.GetStatic<AndroidJavaObject>
                    ("currentActivity");
            toastParams[1] = msg;
            toastParams[2] = toastClass.GetStatic<int>
                ("LENGTH_LONG");

            //call static function of Toast class, makeText
            AndroidJavaObject toastObject =
                toastClass.CallStatic<AndroidJavaObject>
                    ("makeText", toastParams);

            //show toast
            toastObject.Call("show");
        }

    }
}