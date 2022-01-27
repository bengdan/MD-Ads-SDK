using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace MdAds
{
    public class MdManager : MonoBehaviour
    {
        public static float MdBannerRefreshDuration = 5f;
        public static float EnableMdAdsRate = .3f;
        
        public void Start()
        {
            InitMdSdk();
            StartCoroutine(UpdateConfig());
        }

        public static void InitMdSdk()
        {
            MdBannerRefreshDuration = PlayerPrefs.GetFloat(nameof(MdBannerRefreshDuration), MdBannerRefreshDuration);
            EnableMdAdsRate = PlayerPrefs.GetFloat(nameof(EnableMdAdsRate), EnableMdAdsRate);
#if !UNITY_EDITOR && UNITY_ANDROID
            RequestGaid();
#endif
        }

        private static void RequestGaid()
        {
            AndroidJavaClass activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject activityContext = activityClass.GetStatic<AndroidJavaObject>("currentActivity");

            AndroidJavaClass helperClass = new AndroidJavaClass("unity.idfa.helper.Helper");
            helperClass.CallStatic("getGaid", activityContext);
        }

        public static string GetGaid()
        {
#if UNITY_EDITOR
            return "";
#endif
            
            AndroidJavaClass activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject activityContext = activityClass.GetStatic<AndroidJavaObject>("currentActivity");

            AndroidJavaClass helperClass = new AndroidJavaClass("unity.idfa.helper.Helper");
            return helperClass.GetStatic<string>("GAID");
        }
        
        IEnumerator UpdateConfig() {
            UnityWebRequest www = UnityWebRequest.Get($"http://m1.67yo.net/md/get_config?bundle={TrafficInfo.Bundle}");
            
            yield return www.SendWebRequest();
 
            if (www.result != UnityWebRequest.Result.Success) {
                Debug.Log(www.error);
            }
            else {
                // Show results as text
                var data = www.downloadHandler.text;
                Debug.Log(data);
                var config = JsonUtility.FromJson<BannerConfig>(data);
                MdBannerRefreshDuration = config.RequestTime;
                EnableMdAdsRate = config.ToMax / 100f;
                PlayerPrefs.SetFloat(nameof(MdBannerRefreshDuration),MdBannerRefreshDuration);
                PlayerPrefs.SetFloat(nameof(EnableMdAdsRate),EnableMdAdsRate);
            }
        }

        private class BannerConfig
        {
            public int ToMax;
            public int RequestTime;
        }
    }
}