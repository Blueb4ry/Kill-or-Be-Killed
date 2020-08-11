using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kobk.csharp.gui.gizmos
{
    public class GizmosRenderer : MonoBehaviour
    {

        [SerializeField] private Texture img = null;
        [SerializeField] private Rect rect = default(Rect);

        private void OnDrawGizmos()
        {
            Gizmos.DrawGUITexture(rect, img);
        }
    }
}