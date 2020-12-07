using System;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace MdAds
{
    public static class TrafficInfo
    {
        public static string Idfa { get; set; }
        public static string OS => UrlEncode(SystemInfo.operatingSystem);
        public static string DeviceModel => UrlEncode(SystemInfo.deviceModel);
        public static string DeviceId => UrlEncode(SystemInfo.deviceUniqueIdentifier);
        public static string AppName => UrlEncode(Application.productName);
        public static string AppVersion => UrlEncode(Application.version);
        public static string Bundle => UrlEncode(Application.identifier);

        private static string UrlEncode(string param)
        {
            return Uri.EscapeUriString(param);
        }
    }
}