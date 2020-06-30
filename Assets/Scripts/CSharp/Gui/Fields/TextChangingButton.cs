using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace kobk.csharp.gui
{
    [ExecuteAlways]
    public class TextChangingButton : MonoBehaviour
    {
        [Header("Text Changing")]
        public TextMeshProUGUI TargetText;
        public string[] textOptions;
        public int selectedText = 0;

        void Start() {
            if(textOptions != null && textOptions.Length > 0) {
                TargetText.text = textOptions[((selectedText > textOptions.Length)? selectedText : selectedText % textOptions.Length)];
            }
        }

        void LateUpdate() {
            if(textOptions != null && textOptions.Length > 0) {
                TargetText.text = textOptions[((selectedText > textOptions.Length)? selectedText : selectedText % textOptions.Length)];
            }
        }
        
    }
}