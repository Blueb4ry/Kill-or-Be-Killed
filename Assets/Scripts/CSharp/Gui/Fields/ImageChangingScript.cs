using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace kobk.csharp.gui
{
    public class ImageChangingScript : MonoBehaviour
    {
        [Header("Img Changing")]
        public GameObject[] ImgOptions;
        public int selectedImg = 0;
        private int prev = -1;

        void Start()
        {
            if(ImgOptions != null && ImgOptions.Length > 0) {
                clearAll();
                prev = selectedImg > ImgOptions.Length? selectedImg % ImgOptions.Length : selectedImg;
                ImgOptions[selectedImg].SetActive(true);
            }
        }

        void LateUpdate()
        {
            if(ImgOptions != null && ImgOptions.Length > 0 && prev != selectedImg) {
                clearAll();
                prev = selectedImg > ImgOptions.Length? selectedImg % ImgOptions.Length : selectedImg;
                ImgOptions[selectedImg].SetActive(true);
            }
        }

        private void clearAll() {
            foreach(var gm in ImgOptions) {
                gm.SetActive(false);
            }
        }
    }
}