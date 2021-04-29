using System;
using UnityEngine;

namespace MdAds
{
    public class MdManager : MonoBehaviour
    {
        public void Start()
        {
            InitMdSdk();
        }

        public static void InitMdSdk()
        {
            RequestGaid();
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
            AndroidJavaClass activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject activityContext = activityClass.GetStatic<AndroidJavaObject>("currentActivity");

            AndroidJavaClass helperClass = new AndroidJavaClass("unity.idfa.helper.Helper");
            return helperClass.GetStatic<string>("GAID");
        }
    }
}