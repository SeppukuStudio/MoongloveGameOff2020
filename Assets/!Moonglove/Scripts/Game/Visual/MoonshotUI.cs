using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

namespace SeppukuStudio
{
    /// <summary>
    /// Se encarga de controlar el canvas del juego:
    /// Numero de bateos
    /// Siguiente bandera a recoger
    /// </summary>
    public class MoonshotUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI hitsText;
        [SerializeField] private Image flagImage;

        private SpacialBase[] spacialBases;     // Bases ordenadas por orden en recogerlas

        /// <summary>
        /// Obtiene referencias
        /// </summary>
        private void Awake()
        {
            // Obtiene info de las bases
            spacialBases = FindObjectsOfType<SpacialBase>().ToList().OrderBy(t => t.NumBase).ToArray();
        }

        /// <summary>
        /// Suscripcion a eventos
        /// Inicializa variables
        /// </summary>
        private void Start()
        {
            MoonshotManager.Instance.OnHit += UpdateText;
            MoonshotManager.Instance.OnFlagTaken += UpdateFlag;

            hitsText.text = "0";
            flagImage.color = spacialBases[1].GetComponentInChildren<SpriteRenderer>().color;
        }

        private void UpdateText(int numHits)
        {
            hitsText.text = numHits.ToString();
        }

        private void UpdateFlag(int lastBase)
        {
            // Si el jugador ha alcanzado la ultima base
            if (lastBase == (spacialBases.Length - 1))
                flagImage.enabled = false;
            else
                flagImage.color = spacialBases[lastBase + 1].GetComponentInChildren<SpriteRenderer>().color;
        }
    }
}