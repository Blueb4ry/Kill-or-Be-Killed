using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using kobk.csharp.game.roomObjects;

namespace kobk.csharp.editor
{
    [CustomEditor(typeof(HidingObjectManager))]
    public class HidingObjectManagerEditor : Editor
    {

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			if(GUILayout.Button("Check and Add hiding objects"))
			{
				if (target is null)
					return;

				HidingObjectManager man = (HidingObjectManager) target;

				HidingObject[] newList = GameObject.FindObjectsOfType<HidingObject>();

				//man.hidingObjects.Clear();
				//man.hidingObjects.AddRange(GameObject.FindObjectsOfType<HidingObject>());

				for(int x = 0; x < newList.Length; x++)
				{
					newList[x].networkhideId = x;
				}

				man.hidingObjects = newList;

				Debug.Log("Found all hiding objects");
			}
		}

	}
}