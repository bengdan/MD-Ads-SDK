using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace MdAds
{
    public class UIAds : MonoBehaviour
    {
        public Animator animator;
        public Image iconImage;
        public Image viceIconImage;
        public Sprite defaultSprite;
        public Text ctaText;
        public Button button;

        private DateTime _lastRefreshTime;

        private void OnEnable()
        {
            LoadAd();
        }

        private void LoadAd()
        {
            var url =
                $"http://ads.playforhigh.com/get_ads?placementwidth=512&placementheight=512&os={TrafficInfo.OS}&devicemodel={TrafficInfo.DeviceModel}&ifa={TrafficInfo.Idfa}&deviceid={TrafficInfo.DeviceId}&appname={TrafficInfo.AppName}&bundle={TrafficInfo.Bundle}&appversion={TrafficInfo.AppVersion}&publisher_id=1000163&channel={TrafficInfo.Bundle}%3A512x512&adtype=native";
            StartCoroutine(GetCampaign(url));
            _lastRefreshTime = DateTime.Now;
            Invoke(nameof(RefreshCampaign),15.5f);
        }

        private void RefreshCampaign()
        {
            if (gameObject.activeInHierarchy && DateTime.Now - _lastRefreshTime >= TimeSpan.FromSeconds(15) ) LoadAd();
        }

        IEnumerator GetCampaign(string url)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
            {
                // Request and wait for the desired page.
                yield return webRequest.SendWebRequest();

                switch (webRequest.result)
                {
                    case UnityWebRequest.Result.ConnectionError:
                    case UnityWebRequest.Result.DataProcessingError:
                        break;
                    case UnityWebRequest.Result.ProtocolError:
                        break;
                    case UnityWebRequest.Result.Success:
                        if (!webRequest.downloadHandler.text.Contains("not match"))
                        {
                            var result = JsonUtility.FromJson<Campaign>(webRequest.downloadHandler.text);
                            UpdateCampaign(result);
                        }
                        break;
                }
            }
        }

        private void UpdateCampaign(Campaign result)
        {
            //set content from internet
            StartCoroutine(UpdateSprite(iconImage, result.ImgMain));
            ctaText.text = result.CtaText == "" ? "Play" : result.CtaText;
            if (result.ImgVice != "") StartCoroutine(UpdateSprite(viceIconImage, result.ImgVice));
            
            // update animation
            if (Random.Range(0, 2) == 0)
                animator.Play(result.ImgVice != "" ? "3 Parts" : "2 Parts");
            else
                animator.Play("Shake");
            
            // bind landing page
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(call: () =>
            {
                Application.OpenURL(result.LandingPage);
                foreach (var click in result.ClickTracking) StartCoroutine(FireLink(click));
            });
            
            // fire imps
            foreach (var imp in result.ImpTracking)
            {
                StartCoroutine(FireLink(imp));
            }
        }
        
        IEnumerator FireLink(string url)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
            {
                // Request and wait for the desired page.
                yield return webRequest.SendWebRequest();
                if (webRequest.result == UnityWebRequest.Result.Success) Debug.Log($"link reported : {url}");
            }
        }

        private IEnumerator UpdateSprite(Image img,string mediaUrl)
        {   
            var request = UnityWebRequestTexture.GetTexture(mediaUrl);
            yield return request.SendWebRequest();
            img.sprite = defaultSprite;
            if (request.result == UnityWebRequest.Result.Success)
            {
                var tx = ((DownloadHandlerTexture)request.downloadHandler).texture;
                img.sprite = Sprite.Create(tx,new Rect(0,0,tx.width,tx.height),Vector2.zero);
            }
            else
            {
                Debug.Log(request.error);
            }
        }

        private class Campaign
        {
            public string ImgMain;
            public string ImgVice;
            public string CtaText;
            public string LandingPage;
            public List<string> ImpTracking;
            public List<string> ClickTracking;
        }
    }
}