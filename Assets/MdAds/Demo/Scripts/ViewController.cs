using System.Collections.Generic;
using UnityEngine;

namespace MdAds.Demo.Scripts
{
    public class ViewController : MonoBehaviour
    {
        public List<GameObject> views;
        public List<UIAds> ads;

        public void ChangeView(int index)
        {
            ads.ForEach(ad =>
            {
                if (ad.gameObject.activeInHierarchy)
                {
                    ad.HideAd();
                }
            });
            views.ForEach(v => v.SetActive(views.IndexOf(v) == index));
        }
    }
}