using System;
using UnityEngine;
using UnityEngine.Android;
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
        private bool _isShowing = false;

        private void Awake()
        {
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
            var url = $"http://m1.67yo.net/get_ads?placementwidth={width}&placementheight={height}&os={TrafficInfo.OS}&devicemodel={TrafficInfo.DeviceModel}&idfa={TrafficInfo.Idfa}&deviceid={TrafficInfo.DeviceId}&appname={TrafficInfo.AppName}&bundle={TrafficInfo.Bundle}&appversion={TrafficInfo.AppVersion}&publisher_id=1000163&channel={TrafficInfo.Bundle}%3A{width}x{height}";
            _webView.ReferenceRectTransform = GetComponent<RectTransform>();
            _webView.Load(url);
            
            if (isDebug)
            {
                GUIUtility.systemCopyBuffer = url;
            }
            
            // Add Callback
            _webView.OnPageFinished += (view, code, s) =>
            {
                _webView.SetOpenLinksInExternalBrowser(true);
                
                _noAds = code != 200;
                if (showAfter)
                {
                    ShowAd();
                }

                if (isDebug) ShowTip(code == 200 ? "Ad loaded!" : $"No Ads! code :{code}");
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

        private void RefreshBanner()
        {
            if (_isShowing) LoadAd(false);
        }

        public void ShowAd()
        {
            if (_noAds)
            {
                return;
            }

            _isShowing = true;
            _webView.Show();
            if (!IsInvoking(nameof(RefreshBanner)))
            {
                InvokeRepeating(nameof(RefreshBanner),.1f,MdManager.MdBannerRefreshDuration);
            }
        }

        public void HideAd()
        {
            _isShowing = false;
            _webView.Hide();
        }

        private void InitWebView()
        {
            var webViewGameObject = new GameObject("UniWebView");
            webViewGameObject.transform.SetParent(transform);
            _webView = webViewGameObject.AddComponent<UniWebView>();
        }

        private void ShowTip(string msg)
        {
#if UNITY_ANDROID
            ShowToast(msg);
#elif UNITY_IOS
            Debug.Log(msg);
#endif
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