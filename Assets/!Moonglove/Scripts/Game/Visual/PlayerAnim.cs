using UnityEngine;

namespace SeppukuStudio
{
    /// <summary>
    /// Controla las animaciones del personaje
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class PlayerAnim : MonoBehaviour
    {
        private Player player;
        private SpriteRenderer sprite;
        private Animator anim;

        /// <summary>
        /// Obtiene referencias
        /// </summary>
        private void Awake()
        {
            player = GetComponentInParent<Player>();
            sprite = GetComponent<SpriteRenderer>();
            anim = GetComponent<Animator>();

            MoonshotManager.Instance.OnStartRound += ResetFlip;
        }

        private void Update()
        {
            anim.SetFloat("Movement", Mathf.Abs(player.MoveDir));

            // Controla la orientación del personaje
            if (player.MoveDir > 0f)
                sprite.flipX = false;

            else if (player.MoveDir < 0f)
                sprite.flipX = true;

            anim.SetBool("Grounded", player.Grounded);
        }

        /// <summary>
        /// Resetea la direccion en la que debe mirar el personaje
        /// </summary>
        private void ResetFlip()
        {
            sprite.flipX = false;
        }
    }
}