using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SeppukuStudio
{
    /// <summary>
    /// Panel de nivel completado
    /// </summary>
    public class LevelCompletedPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private TextMeshProUGUI hitsText;
        [SerializeField] private Image[] glovesImages;

        public void Init(int numHits)
        {
            // Pausa el juego
            PauseManager.Instance.Pause();
            
            // Muestra el panel
            gameObject.SetActive(true);

            // Actualiza texto
            hitsText.text = numHits.ToString();
            levelText.text = GameManager.instance.LevelIndex.ToString();

            // Actualiza imagenes
            for (int i = 0; i < 3; i++)
                glovesImages[i].enabled = numHits <= GameManager.instance.Levels[GameManager.instance.LevelIndex].Gloves[i];
        }

        public void NextLevel()
        {
            GameManager.instance.NextLevel();
        }
    }
}