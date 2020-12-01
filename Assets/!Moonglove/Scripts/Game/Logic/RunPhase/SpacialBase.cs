using UnityEngine;

namespace SeppukuStudio
{
    /// <summary>
    /// Componente encargado de detectar cuando el jugador entra en su trigger para informar a MoonshotManager de que ha alcanzado una de las bases
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class SpacialBase : MonoBehaviour
    {
        [Header("Attributes")]
        public int NumBase = 0;

        // Variables
        private bool flagTaken = false;

        // Delegates
        public Delegates.VoidDelegate OnFlagTaken;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!flagTaken)
            {
                Player player = collision.GetComponent<Player>();
                if (player != null)
                    MoonshotManager.Instance.SpacialBaseReached(this);
            }
        }

        /// <summary>
        /// Coge la bandera, informando a los suscritos
        /// </summary>
        public void TakeFlag()
        {
            OnFlagTaken?.Invoke();
            flagTaken = true;
        }
    }
}