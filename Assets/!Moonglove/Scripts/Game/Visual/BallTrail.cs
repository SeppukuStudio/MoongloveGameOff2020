using System.Collections;
using UnityEngine;

namespace SeppukuStudio
{
    /// <summary>
    /// Se encarga de activar el trail de la pelota cuando corresponde
    /// Cutre pero funciona
    /// </summary>
    public class BallTrail : MonoBehaviour
    {
        private TrailRenderer trail;

        /// <summary>
        /// Obtiene referencias
        /// </summary>
        private void Awake()
        {
            trail = GetComponent<TrailRenderer>();
            trail.emitting = false;
            MoonshotManager.Instance.OnStartRound += StartTrail;
            MoonshotManager.Instance.OnEndRound += StopTrail;
        }

        private void StartTrail()
        {
            StartCoroutine(StartTrailRoutine());
        }

        private IEnumerator StartTrailRoutine()
        {
            yield return new WaitForEndOfFrame();
            trail.emitting = true;
        }

        private void StopTrail(float resetTime)
        {
            trail.emitting = false;
        }
    }
}