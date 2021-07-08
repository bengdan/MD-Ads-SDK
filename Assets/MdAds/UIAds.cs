using System.Collections;
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
        }

        public void LoadAd(bool showAfter = false)
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
        
    }
}