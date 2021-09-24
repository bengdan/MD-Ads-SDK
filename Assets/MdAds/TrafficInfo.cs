using System;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace MdAds
{
    public static class TrafficInfo
    {
        public static string Idfa => MdManager.GetGaid();
        public static string OS => UrlEncode(SystemInfo.operatingSystem);
        public static string DeviceModel => UrlEncode(SystemInfo.deviceModel);
        public static string DeviceId => UrlEncode(SystemInfo.deviceUniqueIdentifier);
        public static string AppName => UrlEncode(Application.productName);
        public static string AppVersion => UrlEncode(Application.version);
        public static string Bundle => UrlEncode(MdManager.AppId == ""?Application.identifier : MdManager.AppId);
        private static string UrlEncode(string param) => Uri.EscapeUriString(param);
    }
}