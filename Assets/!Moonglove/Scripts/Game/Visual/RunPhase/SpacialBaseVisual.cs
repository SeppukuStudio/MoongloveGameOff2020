using UnityEngine;
using DG.Tweening;

namespace SeppukuStudio
{
    /// <summary>
    /// Realiza la animacion de bandera bajando cuando el jugador activa el checkpoint
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpacialBaseVisual : MonoBehaviour
    {
        [SerializeField] private float flagDelta;
        [SerializeField] private float takeSeconds;

        private SpriteRenderer flag;

        /// <summary>
        /// Obtiene referencias
        /// Suscripcion a eventoS
        /// </summary>
        private void Awake()
        {
            flag = GetComponent<SpriteRenderer>();
            GetComponentInParent<SpacialBase>().OnFlagTaken += TakeFlag;
        }

        /// <summary>
        /// Realiza una animación de la bandera bajando
        /// </summary>
        private void TakeFlag()
        {
            flag.transform.DOLocalMoveY(flagDelta, takeSeconds).OnComplete(() => flag.enabled = false).SetEase(Ease.InQuad);
        }

    }
}