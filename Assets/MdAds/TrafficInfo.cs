using UnityEngine;

namespace MdAds
{
    public static class TrafficInfo
    {
        public static string Idfa { get; set; }
        public static string OS => SystemInfo.operatingSystem;
        public static string DeviceModel => SystemInfo.deviceModel;
        public static string DeviceId => SystemInfo.deviceUniqueIdentifier;
        public static string AppName => Application.productName;
        public static string AppVersion => Application.version;
        public static string Bundle => Application.identifier;
    }
}