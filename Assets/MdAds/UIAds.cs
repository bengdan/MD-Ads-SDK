using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace MdAds
{
    public class UIAds : MonoBehaviour
    {
        public Animator animator;

        public void LoadAd(bool showAfter = false)
        {
            
        }

        public void ShowAd()
        {

        }

        public void HideAd()
        {
            
        }

        private static IEnumerator UpdateSprite(Image img,string mediaUrl)
        {   
            var request = UnityWebRequestTexture.GetTexture(mediaUrl);
            yield return request.SendWebRequest();
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
        
    }
}