using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kobk.csharp.game.roomObjects
{
    public class HidingObjectManager : MonoBehaviour
    {
        public static HidingObjectManager instance = null;

		public HidingObject[] hidingObjects = null;

		private void Start()
		{
			instance = this;
		}
	}
}