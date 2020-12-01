using UnityEngine;

namespace SeppukuStudio
{
    /// <summary>
    /// Se encarga de realizar las animaciones del bate de beisbol
    /// </summary>
    public class BatAnim : MonoBehaviour
    {
        private SpriteRenderer sprite;
        private Animator anim;
        private PlayerHit playerHit;

        /// <summary>
        /// Obtiene referencias
        /// Suscripcion a eventos
        /// </summary>
        private void Awake()
        {
            sprite = GetComponent<SpriteRenderer>();
            anim = GetComponent<Animator>();
            playerHit = GetComponentInParent<PlayerHit>();

            playerHit.OnHitPhase += ShowBat;
            playerHit.OnCharging += SetCharge;
        }

        private void ShowBat(bool show)
        {
            sprite.enabled = show;
        }

        private void SetCharge(bool charging)
        {
            anim.SetBool("Charge", charging);
        }

    }
}