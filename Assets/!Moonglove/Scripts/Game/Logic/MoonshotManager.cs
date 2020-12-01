using System.Collections;
using UnityEngine;
using WanzyeeStudio;
using System.Linq;

namespace SeppukuStudio
{
    /// <summary>
    /// Manager de la escena principal de juego
    /// </summary>
    public class MoonshotManager : MonoBehaviour
    {
        /// <summary>
        /// Singleton
        /// </summary>
        public static MoonshotManager Instance { get { return Singleton<MoonshotManager>.instance; } }

        [Header("Attributes")]
        [SerializeField] private float resetRoundTime;

        [Header("References")]
        [SerializeField] private LevelCompletedPanel levelCompletedPanel;

        // Delegates
        public Delegates.VoidDelegate OnStartRound;
        public Delegates.FloatDelegate OnEndRound;
        public Delegates.IntDelegate OnHit;
        public Delegates.IntDelegate OnFlagTaken;
        public Delegates.VoidDelegate OnLevelCompleted;

        // References
        private Player player;
        private PlayerHit playerHit;
        private Ball ball;

        private SpacialBase[] spacialBases;     // Bases ordenadas por orden en recogerlas

        // Variables
        private int lastBase = 0;       // Indice de la ultima base por la que ha pasado el jugador
        private int numHits = 0;        // Numero de bateos desde el inicio del nivel
        private int numSpacialBases;    // Numero total de bases en la escena
        private bool restartingRound = false; // Indica si se esta reiniciando la ronda

        /// <summary>
        /// Obtiene referencias
        /// Crea el nivel
        /// </summary>
        private void Awake()
        {
            player = FindObjectOfType<Player>();
            playerHit = player.GetComponent<PlayerHit>();
            ball = FindObjectOfType<Ball>();

            Instantiate(GameManager.instance.Levels[GameManager.instance.LevelIndex], transform.parent);

            // Obtiene info de las bases
            spacialBases = FindObjectsOfType<SpacialBase>().ToList().OrderBy(t => t.NumBase).ToArray();
            numSpacialBases = spacialBases.Length;
        }

        /// <summary>
        /// Inicializa variables
        /// Empieza la fase de Hit
        /// </summary>
        private void Start()
        {
            player.CanMove = false;
            spacialBases[lastBase].TakeFlag();

            StartRound();
        }

        /// <summary>
        /// Inicia la fase de bateo
        /// Coloca al player en la base correspondiente para iniciar el bateo
        /// Dispara la pelota hacia el jugador
        /// Informa a los suscritos
        /// </summary>
        private void StartRound()
        {
            player.transform.position = spacialBases[lastBase].transform.position;
            player.transform.rotation = spacialBases[lastBase].transform.rotation;

            ball.transform.position = -spacialBases[lastBase].transform.position;
            ball.ShootBall();

            playerHit.StartHit();
            OnStartRound?.Invoke();
        }

        /// <summary>
        /// Es llamado cuando el jugador batea
        /// Aumenta el numero de bateos y permite al jugador moverse
        /// </summary>
        public void StartPlatformPhase()
        {
            AddHit();
            player.CanMove = true;
        }

        /// <summary>
        /// Añade un golpe al contador, informando a los suscritos
        /// </summary>
        private void AddHit()
        {
            numHits++;
            OnHit?.Invoke(numHits);
        }

        /// <summary>
        /// Es llamado cuando una base espacial es alcanzada por el jugador
        /// Detecta si es la base correcta y avanza el indice a la siguiente
        /// Informa a los suscritos
        /// </summary>
        /// <param name="spacialBase"></param>
        public void SpacialBaseReached(SpacialBase spacialBase)
        {
            // Si el jugador ha entrado en la base que le toca
            if (spacialBase.NumBase == lastBase + 1)
            {
                lastBase++;
                spacialBase.TakeFlag();
                OnFlagTaken?.Invoke(lastBase);

                // Si el jugador ha alcanzado la ultima base
                if (lastBase == (numSpacialBases - 1))
                    LevelCompleted();
            }
        }

        /// <summary>
        /// Es llamado cuando todas las bases espaciales son alcanzadas
        /// Muestra el panel de nivel completado
        /// </summary>
        private void LevelCompleted()
        {
            levelCompletedPanel.Init(numHits);
            OnLevelCompleted?.Invoke();
        }

        /// <summary>
        /// Es llamado cuando la pelota alcanza la luna
        /// Desactiva el movimiento del jugador y empieza la rutina de reinicio de bateo
        /// </summary>
        public void BallFallen()
        {
            if (!restartingRound)
            {
                if (!player.CanMove)
                    PlayerDead();
                else
                {
                    restartingRound = true;
                    player.CanMove = false;
                    StartCoroutine(EndRoundRoutine());
                }
            }
        }

        /// <summary>
        /// Es llamado cuando el jugador sale al espacio exterior
        /// Empieza la rutina de reinicio de bateo
        /// </summary>
        public void PlayerDead()
        {
            if (!restartingRound)
            {
                restartingRound = true;

                // El jugador no ha llegado a golpear, se le cuenta como un golpe
                if (!player.CanMove)
                    AddHit();

                StartCoroutine(EndRoundRoutine());
            }
        }

        /// <summary>
        /// Rutina de espera hasta que se vuelve a iniciar el bateo
        /// Cuando acaba, empieza la fase de bateo
        /// </summary>
        /// <returns></returns>
        private IEnumerator EndRoundRoutine()
        {
            OnEndRound?.Invoke(resetRoundTime);
            yield return new WaitForSeconds(resetRoundTime);
            StartRound();
            restartingRound = false;
        }
    }
}