using UnityEngine;

namespace kobk.csharp.game.map
{
    public class SpawnPoint : MonoBehaviour
    {
        void Awake() => SpawnPointManager.addSpawnPoint(this);
        void OnDestroy() => SpawnPointManager.removeSpawnPoint(this);

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(transform.position, 0.5f);
        }
    }
}