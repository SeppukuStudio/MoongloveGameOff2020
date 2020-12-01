using UnityEngine;

namespace SeppukuStudio
{
    /// <summary>
    /// Zona del espacio de la cual si el personaje sale, morira
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class SafeZone : MonoBehaviour
    {
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.GetComponent<Player>())
                MoonshotManager.Instance.PlayerDead();
        }
    }
}