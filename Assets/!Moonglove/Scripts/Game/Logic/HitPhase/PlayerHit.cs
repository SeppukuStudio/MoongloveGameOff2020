using System.Collections;
using UnityEngine;

namespace SeppukuStudio
{
    /// <summary>
    /// Componente encargado de realizar el bateo del jugador
    /// </summary>
    public class PlayerHit : MonoBehaviour
    {
        private enum HitState { PLATFORM, IDLE, CHARGING, BALLHIT };

        [Header("Attributes")]
        // Carga de fuerza
        [SerializeField] private float maxForceTime = 5.0f;                 // Maxima fuerza con la que se puede cargar(tiempo)
        [SerializeField] private float extraForceTime = 0.5f;               // Tiempo extra de bateo

        //Tiempos de no poder golpear
        [SerializeField] private float timeHit = 1.0f;                      // Tiempo de duración del bateo
        [SerializeField] private float timeWaitAfterHit = 0.3f;             // Tiempo de espera antes de poder volver a batear

        [SerializeField] private float timeStun = 3.0f;                     // Tiempo de stuneo tras haber seleccionado demasiada fuerza
        [SerializeField] private float failForce = 200f;           // Fuerza que aplica al personaje si falla el bateo


        //Delegates
        public Delegates.BoolDelegate OnHitPhase;            // True = startHitPhase / False = endHitPhase
        public Delegates.BoolDelegate OnCharging;
        public Delegates.VoidDelegate OnHit;

        public Delegates.FloatDelegate OnSetttingForce;

        // Variables
        private HitState hitState;              // Estado actual del bateo
        private float nextTimeHit;              // Siguiente instante de tiempo en el que puede golpear
        private float actualTimeForce;          // Fuerza actual

        public void StartHit()
        {
            hitState = HitState.IDLE;
            nextTimeHit = Time.time + timeHit + timeWaitAfterHit;
            OnHitPhase?.Invoke(true);
        }

        private void Update()
        {

            // Empieza a cargar el bateo
            if (hitState == HitState.IDLE && Input.GetKeyDown(KeyCode.Space) && Time.time > nextTimeHit)
            {
                Debug.Log("Cargando fuerza ...");
                hitState = HitState.CHARGING;

                actualTimeForce = 0.0f;
                OnCharging?.Invoke(true);

                //Seleccion de fuerza
                StartCoroutine(SetForceCoroutine());
            }

            //Cuando se suelta la tecla, se batea
            else if (hitState == HitState.CHARGING && Input.GetKeyUp(KeyCode.Space))
            {
                //Debug.Log("Fuerza seleccionada. Inicio de bateo.");
                //Debug.Log("Tiempo tardado: " + actualTimeForce);
                //Animación bateo

                hitState = HitState.BALLHIT;//Estado de bateo

                //la barra no se ve
                OnCharging?.Invoke(false);

                // Corrutina tiempo de duración del bateo
                StartCoroutine(SetInitHitCoroutine());

                nextTimeHit = Time.time + timeHit + timeWaitAfterHit;//Tiempo de estar sin poder batear de nuevo la pelota         
                                                                     //Debug.Log("Siguiente bateo en: " + nextTimeHit);
                                                                     //Debug.Log(Time.time);
            }
        }

        /// <summary>
        /// Corrutina para elegir la fuerza
        /// </summary>
        /// <returns></returns>
        private IEnumerator SetForceCoroutine()
        {
            while (hitState == HitState.CHARGING && actualTimeForce < maxForceTime + extraForceTime)
            {
                //No se ha llegado al final de la barra de fuerza. Cargando fuerza
                if (actualTimeForce < maxForceTime)
                    OnSetttingForce?.Invoke(actualTimeForce / maxForceTime);

                //Tiempo extra antes de ser stuneado
                //else
                //Debug.Log("Se acaba el tiempo para elegir fuerza!");

                actualTimeForce += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            if (hitState == HitState.CHARGING && actualTimeForce >= extraForceTime + maxForceTime)
            {
                //Debug.Log("Se acabó el tiempo de elegir fuerza. Jugador Stuneado");
                hitState = HitState.IDLE;
                nextTimeHit = Time.time + timeStun;

                OnCharging?.Invoke(false);
            }
        }


        /// <summary>
        /// Corrutina de tiempo de espera del bateo
        ///  informa al MoonshotManager de que empiece la fase de plataformas
        /// </summary>
        /// <returns></returns>
        IEnumerator SetInitHitCoroutine()
        {
            //Se está bateando
            yield return new WaitForSeconds(timeHit);

            if (hitState != HitState.PLATFORM)
                hitState = HitState.IDLE;

            else
            {
                MoonshotManager.Instance.StartPlatformPhase();
                OnHitPhase?.Invoke(false);
            }
        }

        /// <summary>
        /// Se comprueba si la bola ha entrado en el trigger de bateo
        /// Golpea la bola
        /// </summary>
        /// <param name="collision"></param>
        private void OnTriggerEnter2D(Collider2D collision)
        {
            Ball ball = collision.GetComponent<Ball>(); //Obtenemos la referencia de la pelota

            if (ball != null && hitState == HitState.BALLHIT)
            {
                ball.Hit(GetForceMultiplier());
                hitState = HitState.PLATFORM;
                OnHit?.Invoke();
            }
        }

        /// <summary>
        /// Calcula la fuerza total con la que batea el jugador en base a la fuerza elegida en la barra de fuerza (3 posibilidades)
        /// </summary>
        /// <returns></returns>
        private float GetForceMultiplier()
        {
            float multiplier = 0.0f;

            Debug.Log("Tiempo de bateo: " + actualTimeForce + " -- Tiempo maximo: " + maxForceTime);

            //Barra verde
            if (actualTimeForce < maxForceTime / 2)
            {
                multiplier = 1.0f;
                Debug.Log("Fuerza 1");
            }
            //Barra amarilla
            else if (actualTimeForce < (maxForceTime * 7.0f) / 8.0f)
            {
                multiplier = 1.05f;
                Debug.Log("Fuerza 2");
            }
            //Barra roja
            else if (actualTimeForce < maxForceTime)
            {
                multiplier = 1.1f;
                Debug.Log("Fuerza 3");
            }

            return multiplier;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (hitState != HitState.PLATFORM)
            {
                Ball ball = collision.collider.GetComponent<Ball>();

                if (ball != null)
                {
                    Rigidbody2D playerRB = GetComponent<Rigidbody2D>();

                    Vector2 dir = transform.position - ball.transform.position;

                    playerRB.AddForce(dir.normalized * failForce);
                }
            }
        }
    }

}