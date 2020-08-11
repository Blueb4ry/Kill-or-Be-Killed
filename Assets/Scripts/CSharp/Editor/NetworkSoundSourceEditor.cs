using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using kobk.csharp.game.sound;

namespace kobk.csharp.editor
{
    [CustomEditor(typeof(NetworkSoundSource))]
    public class NetworkSoundSourceEditor : Editor
    {

        private bool showClipsArray = false;
        private bool showSoundSystem = false;

		public override void OnInspectorGUI()
        {

            //checks if on object
            if (target is null)
                return;

            //grab object
            NetworkSoundSource e = (NetworkSoundSource)target;

            //Begin Fields
            //Sound source
            e.source = (AudioSource) EditorGUILayout.ObjectField("Audio Source", e.source, typeof(AudioSource), !EditorUtility.IsPersistent(target));

            //Audio Clip array
            showClipsArray = EditorGUILayout.Foldout(showClipsArray, "Sound Clips");

            //if extended
            if(showClipsArray)
			{
                //indent
                EditorGUI.indentLevel++;

                if (e.clips is null)
                    e.clips = new AudioClip[0];

                //Gets size of the array
                int size = EditorGUILayout.IntField("Size", e.clips.Length);

                //Checks if size has changed
                if(size != e.clips.Length)
				{
                    //Creates Empty array of new size
                    AudioClip[] tempClips = new AudioClip[size];

                    //creates reference for old array
                    AudioClip[] oldClips = e.clips;

                    //Fills the array
                    for(int x = 0; x < size; x++)
					{
                        //Checks if current index is invalid for the old array
                        if(x >= oldClips.Length)
						{
                            //If it is not valid, set index to last index of old array
                            tempClips[x] = null;
						} else
						{
                            //If it is valid, set index
                            tempClips[x] = oldClips[x];
						}
					}

                    //sets it to object
                    e.clips = tempClips;
				}// End if size changed

                //Add fields for each object
                for (int x = 0; x < size; x++) {
                    e.clips[x] = (AudioClip) EditorGUILayout.ObjectField(x.ToString(), e.clips[x], typeof(AudioClip), !EditorUtility.IsPersistent(target));
                }

                //remove indent
                EditorGUI.indentLevel--;
            }

            //fold out for the networrk sound system
            showSoundSystem = EditorGUILayout.Foldout(showSoundSystem, "Network Sound System Info");

            //if folded out
            if(showSoundSystem)
			{
                //indent and disabled
                EditorGUI.indentLevel++;
                EditorGUI.BeginDisabledGroup(true);

                //checks if system is active
                if(e.connectedSystem is null || e.SourceId == -1)
				{
                    //If system is not active, indicate
                    EditorGUILayout.HelpBox("The system is not connected to the NetworkSoundSystem object!", MessageType.Warning);
				} else
				{
                    //If system is active, show info
                    EditorGUILayout.ObjectField("Connected SoundSystem", e.connectedSystem, typeof(NetworkSoundSystem), !EditorUtility.IsPersistent(target));
                    EditorGUILayout.IntField("Sound Source Id", e.SourceId);
				}

                //remove indent and disabled
                EditorGUI.EndDisabledGroup();
                EditorGUI.indentLevel--;

                //Button for setting fields
                if(GUILayout.Button("Update Sound System"))
				{
                    AttemptConnectSystem(e);
				}

            }

            //End Fields

            //Check for changes and record changes
            if (GUI.changed)
            {
                Undo.RecordObject(target, "NetworkSoundSource");
            }

        }

        private void AttemptConnectSystem(NetworkSoundSource e)
		{
            if(NetworkSoundSystem.singleton is null || NetworkSoundSystem.singleton.gameObject is null)
			{
                Debug.LogWarning("Can't find a Sound System object, please add Sound System object to scene");
                EditorUtility.DisplayDialog("Cant find Network Sound System", "Cannot find Network Sound System Object automatically. Is object missing?", "Ok");
                e.connectedSystem = null;
                e.SourceId = -1;
			} else
			{
                NetworkSoundSystem sys = NetworkSoundSystem.singleton;

                bool isAlreadyAdded = sys.sources.Contains(e);

                if (!isAlreadyAdded)
                    sys.sources.Add(e);

                e.connectedSystem = sys;
                e.SourceId = sys.sources.IndexOf(e);
                EditorUtility.DisplayDialog("Sound system", (isAlreadyAdded? "Network Sound object synced" : "Network Sound object added"), "Ok");
                Debug.Log((isAlreadyAdded ? "Network Sound object synced" : "Network Sound object added"));
			}
		}
		

    }
}