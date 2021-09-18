using UnityEngine;

namespace MdAds
{
    public class MdManager : MonoBehaviour
    {
        private void Awake()
        {
            InitMdSdk();
        }

        private static void InitMdSdk()
        {
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
#if UNITY_ANDROID
            AndroidJavaClass activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject activityContext = activityClass.GetStatic<AndroidJavaObject>("currentActivity");

            AndroidJavaClass helperClass = new AndroidJavaClass("unity.idfa.helper.Helper");
            return helperClass.GetStatic<string>("GAID");
#endif
            return "";
        }
    }
}