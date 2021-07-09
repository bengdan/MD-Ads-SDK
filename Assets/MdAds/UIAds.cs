using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace MdAds
{
    public class UIAds : MonoBehaviour
    {
        public Animator animator;
        public Image iconImage;
        public Image viceIconImage;
        public Sprite defaultSprite;

        private void Start()
        {
            StartCoroutine(UpdateSprite(iconImage, "https://play-lh.googleusercontent.com/Ar4Z09h1iNhpA7c9cGam6-QvUrTu45RYkR4ojiA0Oy5X7J1BR_1Z_a63FFCgfSoSojE"));
            StartCoroutine(UpdateSprite(viceIconImage, "https://play-lh.googleusercontent.com/bAqioAlSPETHgkEoAk3ZumIpuBBrE7-MWi7XPqPz7CkLVwcsK4SGsa47HFYhHu2QOg"));
            animator.Play("3 Parts");
            StartCoroutine(GetRequest("http://ads.playforhigh.com/get_ads?placementwidth=512&placementheight=512&os=Android%20OS%2010%20/%20API-29%20(QKQ1.190716.003/2011091812)&devicemodel=OnePlus%20GM1910&idfa=&deviceid=0c0371bfa92a830ed319db989917be9d&appname=MD-Ads-SDK&bundle=com.MLD.MDAdsSDK&appversion=1.1.0&publisher_id=1000163&channel=MD-SDK%3A512x512&adtype=native"));
        }
        
        
        IEnumerator GetRequest(string uri)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
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
                        var result = JsonUtility.FromJson<Campaign>(webRequest.downloadHandler.text);
                        print(result);
                        break;
                }
            }
        }

        public void LoadAd()
        {
            
        }

        public void ShowAd()
        {

        }

        public void HideAd()
        {
            
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
        
        public class Campaign
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