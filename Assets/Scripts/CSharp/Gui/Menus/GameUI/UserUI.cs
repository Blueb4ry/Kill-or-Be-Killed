using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace kobk.csharp.gui.game
{
    public class UserUI : MonoBehaviour
    {
        public const int RANGED = 0, MELEE = 1;

        [SerializeField] private TextMeshProUGUI HP_percent = null;
        [SerializeField] private TextMeshProUGUI AmmoCount = null;
        [SerializeField] private Slider HP_slider = null;
        [SerializeField] private Image[] WeaponSelection = null;

        private int curWeapon = 0;

        public void use(bool s)
		{
            gameObject.SetActive(s);
		}

        public void selectWeapon(int id)
		{
            WeaponSelection[curWeapon].gameObject.SetActive(false);
            curWeapon = id;
            WeaponSelection[curWeapon].gameObject.SetActive(true);
        }

        public void setWeaponAmmo(int a)
		{
            AmmoCount.text = a.ToString();
		}

        public void setHpPercent(int percent)
		{
            HP_percent.text = percent.ToString() + "%";
            HP_slider.value = 100 - percent;
		}
    }
}