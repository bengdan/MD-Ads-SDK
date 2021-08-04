# MD-Ads-SDK

> Unity Ad SDK for Melodong.

## 集成说明

### 导入SDK

* 到[Release](https://github.com/bengdan/MD-Ads-SDK/releases)界面下载最新的SDK：[MD-Ads-SDK.1.5.1.unitypackage](https://github.com/bengdan/MD-Ads-SDK/releases/download/1.5.1/MD-Ads-SDK.1.5.1.unitypackage)

* 导入SDK

将下载的SDK拖拽到Unity的Asset窗口，导入SDK资源。

### 添加广告位

> Banner的广告位的位置是根据UGUI进行定位的，用户可以很方便的将广告位放到UI Canvas中合适的位置。

* 在Project/Assets/MdAds/Prefabs中找到合适尺寸的Prefab。
  * 比如你需要的尺寸是320x50，对应的是Ad Placement 320x50.prefab。
* 将Ad Placement 320x50.prefab放在UI Canvas里面合适的位置。

![image-20210525160627835](https://github.com/bengdan/MD-Ads-SDK/blob/2020/readme%20images/image-20210525160627835.png)

### 显示/隐藏广告

* 自动显示隐藏
  * 选中广告的Prefab，在inspector里面勾选Auto Show和Auto Hide。

![image-20210525161057273](https://github.com/bengdan/MD-Ads-SDK/blob/2020/readme%20images/image-20210525161057273.png)

* 手动显示隐藏（Auto Show/Hide 需要取消勾选）

```c#

    public class AdManager : MonoBehaviour
    {
        // 获取到prefab里面的UIAds组件
        public UIAds uiAds;

        private void Start()
        {
            uiAds.ShowAd();//显示Banner
            
            uiAds.HideAd();// 隐藏Banner
        }
    }

```

## FAQ


### 依赖冲突的解决方案：

1. `play-services-ads-identifier-17.0.0.aar`和`play-services-basement-17.0.0.aar`跟项目其他SDK的出现重复情况：在Project/Assets/MdAds/Plugins/Android/目录中找到冲突的aar文件，删掉即可。
2. `jetified-kotlin-stdlib-1.3.72.jar`和`jetified-androidx.browser.browser-1.0.0-runtime.jar`跟项目其他SDK出现重复情况：在Project/Assets/Editor/UniWebView/settings.asset中有添加Kotlin和Android Browser依赖的开关，取消对应的依赖即可。

![image-20210525163339229](https://github.com/bengdan/MD-Ads-SDK/blob/2020/readme%20images/image-20210525163339229.png)

