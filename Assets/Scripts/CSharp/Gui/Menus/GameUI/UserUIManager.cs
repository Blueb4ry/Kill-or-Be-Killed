using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;

namespace kobk.csharp.gui.game
{
    public class UserUIManager : MonoBehaviour
    {
        public static UserUIManager instance = null;
		public const int NINJA = 0, 
						 SOLDIER = 1;

		[SerializeField] private UserUI[] AvaliableUi = null;
		public UserUI current
		{
			get;
			private set;
		} = null;

		private void OnEnable()
		{
			instance = this;
		}

		public void setUI(int id)
		{
			for(int x = 0; x < AvaliableUi.Length; x++)
			{
				AvaliableUi[x].use(id == x);
			}
			current = AvaliableUi[id];
		}
		
    }
}