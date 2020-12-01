using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace SeppukuStudio
{
    /// <summary>
    /// Se encarga de realizar efectos visuales en la escena de Moonshot
    /// </summary>
    public class MoonshotVisual : MonoBehaviour
    {
        [SerializeField] private float fadeInSeconds = 3f;
        [SerializeField] private float cameraSize = 2f;

        [SerializeField] private Image fadeImage;

        // References
        private Player player;

        /// <summary>
        /// Obtiene referencias y suscribe a eventos
        /// </summary>
        private void Awake()
        {
            player = FindObjectOfType<Player>();

            // Necesarios en Awake para forzar orden correcto
            MoonshotManager.Instance.OnStartRound += CameraFadeIn;
            MoonshotManager.Instance.OnEndRound += CameraFadeOut;
        }

        /// <summary>
        /// Es llamado OnStartHitPhase
        /// </summary>
        private void CameraFadeIn()
        {
            Vector3 initialCameraPos = Camera.main.transform.position;
            float initialCameraSize = Camera.main.orthographicSize;

            Camera.main.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, Camera.main.transform.position.z);
            Camera.main.orthographicSize = cameraSize;

            Camera.main.transform.DOMove(initialCameraPos, (2f / 3f) * fadeInSeconds).SetDelay(fadeInSeconds / 3f);
            Camera.main.DOOrthoSize(initialCameraSize, (2f / 3f) * fadeInSeconds).SetDelay(fadeInSeconds / 3f);

            fadeImage.DOFade(0f, fadeInSeconds);
            //Camera.main.FadeIn(fadeInSeconds);
        }

        /// <summary>
        /// Es llamado OnEndRunPhase
        /// </summary>
        private void CameraFadeOut(float time)
        {
            fadeImage.DOFade(1f, time);

            //Camera.main.FadeOut(time);
        }
    }
}