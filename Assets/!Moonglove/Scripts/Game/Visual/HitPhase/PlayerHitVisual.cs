using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SeppukuStudio
{
    public class PlayerHitVisual : MonoBehaviour
    {
        public Image barImage;
        public Image barImageBG;
        private PlayerHit playerHit;

        private void Awake()
        {
            playerHit = GetComponentInParent<PlayerHit>();

        }
        // Start is called before the first frame update
        void Start()
        {
            barImage.enabled = false;
            barImageBG.enabled = false;

            barImage.fillAmount = 0.0f;

            playerHit.OnSetttingForce += UpdateForceBar;
            playerHit.OnCharging += SetActive;
        }
        /// <summary>
        /// Método que actualiza el estado de la barra (visual)
        /// </summary>
        /// <param name="force"></param>
        private void UpdateForceBar(float force)
        {
            barImage.fillAmount = force;
        }

        /// <summary>
        /// Método que activa o desactiva las barras de UI
        /// </summary>
        /// <param name="enabled"></param>
        private void SetActive(bool enabled)
        {
            barImage.enabled = enabled;
            barImageBG.enabled = enabled;

            barImage.fillAmount = 0.0f;
        }
    }
}