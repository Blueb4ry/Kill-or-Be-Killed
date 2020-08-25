using kobk.csharp.game.sound;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kobk.csharp.game.roomObjects
{
    [RequireComponent(typeof(Renderer))]
    public class HidingObject : MonoBehaviour
    {
        [SerializeField] private NetworkSoundSource aSource = null;
        [SerializeField] private string OutlineParam = string.Empty;
        [SerializeField] public Transform TeleportLocation = null;
        //[SerializeField] private AudioClip[] aClips = null;

        private System.Random rnd = new System.Random();
        private Renderer render = null;
        public int networkhideId = -1;

        private void Start()
        {
            //popupObject.SetActive(false);
            render = GetComponent<Renderer>();
            setPop(false);
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawIcon(transform.position, "HidyHoldThingy.png");
        }

        public void setPop(bool s) {
            render.material.SetFloat(OutlineParam, s ? 1f : 0f);
        }

        public void playSound() {
            //if(aClips.Length == 0)
            //    return;

            //aSource.clip = aClips[rnd.Next(aClips.Length)];
            //aSource.Play();

            aSource.RequestNetworkPlay(rnd.Next(aSource.clips.Length), false);
        }
    }
}