using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using kobk.csharp.game.sound;
using UnityEngine.PlayerLoop;

namespace kobk.csharp.editor
{
    [CustomEditor(typeof(NetworkSoundSystem))]
    public class NetworkSoundSystemEditor : Editor
    {
		private static GUILayoutOption minButtonWidth = GUILayout.Width(20f);

		private bool showSources = false;
		private NetworkSoundSource tempHold = null;

		public override void OnInspectorGUI()
		{

			//checks if on object
			if (target is null)
				return;

			//grab object
			NetworkSoundSystem e = (NetworkSoundSystem)target;

			if(EditorUtility.IsPersistent(target))
			{
				EditorGUILayout.HelpBox("You can only edit values of the system in the scene", MessageType.Info);
				return;
			}

			if(NetworkSoundSystem.singleton != e && !EditorUtility.IsPersistent(target))
			{
				if (NetworkSoundSystem.singleton is null)
				{
					NetworkSoundSystem.singleton = e;
				}
				else
				{
					EditorGUILayout.HelpBox("There are multiple NetworkSoundSystem objects in the scene and this object is not the active system", MessageType.Warning);
					if (GUILayout.Button("Make This system active"))
					{
						NetworkSoundSystem.singleton = e;
					}
				}
			}

			showSources = EditorGUILayout.Foldout(showSources, "Sources");

			if(showSources)
			{
				if (e.sources is null)
					e.sources = new List<NetworkSoundSource>();

				if (e.sources.Count == 0)
				{
					EditorGUILayout.HelpBox("There is no sources connected", MessageType.Info);
				}
				else
				{
					EditorGUI.indentLevel++;

					//runs through every item
					for (int x = 0; x < e.sources.Count; x++)
					{
						EditorGUILayout.BeginHorizontal();

						EditorGUI.BeginDisabledGroup(true);
						EditorGUILayout.ObjectField("Id: " + x.ToString(), e.sources[x], typeof(NetworkSoundSource), !EditorUtility.IsPersistent(target));
						EditorGUI.EndDisabledGroup();

						if(GUILayout.Button("-", EditorStyles.miniButton, minButtonWidth))
						{
							removeFromNetwork(x, e);
						}

						EditorGUILayout.EndHorizontal();
					}

					EditorGUI.indentLevel--;
				}

				EditorGUILayout.BeginHorizontal();
				tempHold = (NetworkSoundSource)EditorGUILayout.ObjectField("Add Object", tempHold, typeof(NetworkSoundSource), !EditorUtility.IsPersistent(target));
				if (GUILayout.Button("+", EditorStyles.miniButton, minButtonWidth))
				{
					addToNetwork(tempHold, e);
					tempHold = null;
				}
				EditorGUILayout.EndHorizontal();
			}

			if (GUILayout.Button("update all objects"))
			{
				//for(int x = 0; x < e.sources.Count; x++)
				//{
				//	updateOnNetwork(e.sources[x], e);
				//}
				updateAllOnNetwork(e);
			}

			if(GUILayout.Button("Reset and Add all to system"))
			{
				NetworkSoundSystem.singleton = e;

				clearNetwork(e);

				NetworkSoundSource[] toAdd = GameObject.FindObjectsOfType<NetworkSoundSource>();

				for(int x = 0; x < toAdd.Length; x++)
				{
					addToNetwork(toAdd[x], e);
				}
			}

			//Check for changes and record changes
			if (GUI.changed)
			{
				Undo.RecordObject(target, "NetworkSoundSource");
			}
		}

		private void addToNetwork(NetworkSoundSource e, NetworkSoundSystem sys)
		{

			if (e.connectedSystem == sys)
			{
				Debug.LogWarning("Cannot add to system because object is already connected!");
			}

			sys.sources.Add(e);
			e.connectedSystem = sys;
			e.SourceId = sys.sources.IndexOf(e);
		}

		private void updateOnNetwork(NetworkSoundSource e, NetworkSoundSystem sys)
		{

			if (e.connectedSystem != sys)
			{
				Debug.LogError("Cannot update to system because object isnt connected!");
				return;
			}

			e.SourceId = sys.sources.IndexOf(e);
		}

		private void updateAllOnNetwork(NetworkSoundSystem sys)
		{
			for (int x = 0; x < sys.sources.Count; x++)
			{
				updateOnNetwork(sys.sources[x], sys);
			}
		}

		private void removeFromNetwork(int id, NetworkSoundSystem sys)
		{

			if(id < 0 || id >= sys.sources.Count)
			{
				Debug.LogError("Cannot remove object from network because id is not valid");
				return;
			}

			NetworkSoundSource e = sys.sources[id];

			e.connectedSystem = null;
			e.SourceId = -1;

			sys.sources.RemoveAt(id);

			updateAllOnNetwork(sys);
		}

		private void clearNetwork(NetworkSoundSystem sys)
		{
			for(int x = 0; x < sys.sources.Count; x++)
			{
				NetworkSoundSource e = sys.sources[x];
				e.connectedSystem = null;
				e.SourceId = -1;
			}

			sys.sources.Clear();
		}

	}
}