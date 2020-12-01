using UnityEngine;
using WanzyeeStudio;

namespace SeppukuStudio
{

    /// <summary>
    /// Singleton persistente entre escenas
    /// Se encarga de controlar el avance de los niveles en el juego
    /// </summary>
    public class GameManager : BaseSingleton<GameManager>
    {
        public Level[] Levels;

        public int LevelIndex { get; set; } = 0;

        public void NextLevel()
        {
            //Si se ha pasado el ultimo nivel
            if (LevelIndex == Levels.Length - 1)
            {
                LevelIndex = 0;
                SceneTransition.Instance.LoadLevel(-2);
            }
            else
            {
                LevelIndex++;
                SceneTransition.Instance.LoadLevel(0);
            }
        }
    }
}