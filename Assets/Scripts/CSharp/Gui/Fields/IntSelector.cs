using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;
using TMPro;

namespace kobk.csharp.gui.fields
{
    public class IntSelector : MonoBehaviour
    {
        public int _default = 0;
        public int value = 0;

        //public InputField infield;
        public TMP_InputField infield;

        // Start is called before the first frame update
        void Start()
        {
            value = _default;
            infield.text = value.ToString();
        }

        public void changeValue()
        {
            string val = infield.text.Trim();
            int.TryParse(val, out value);
            infield.text = value.ToString();
        }

        public void editValue(int val)
        {
            value += val;
            infield.text = value.ToString();
        }
    }
}