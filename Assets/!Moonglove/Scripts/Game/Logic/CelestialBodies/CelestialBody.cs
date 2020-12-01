using UnityEngine;

namespace SeppukuStudio
{
    /// <summary>
    /// Objeto que atrae hacia su centro al jugador
    /// </summary>
    public class CelestialBody : MonoBehaviour
    {
        /// <summary>
        /// Constante gravitacional
        /// </summary>
        const float G = 6.674f;

        /// <summary>
        /// A mayor masa, más gravedad afecta a los GravityBody
        /// </summary>
        [SerializeField] private float mass = 1f;

        private GravityBody player;

        /// <summary>
        /// Obtiene referencias
        /// </summary>
        protected virtual void Awake()
        {
            player = FindObjectOfType<Player>().GetComponent<GravityBody>();
        }

        /// <summary>
        /// Atrae hacia su centro al jugador
        /// </summary>
        protected virtual void FixedUpdate()
        {
            Attract(player);
        }

        /// <summary>
        /// Devuelve la gravedad que afecta con "distance"
        /// </summary>
        /// <param name="distance"></param>
        /// <returns></returns>
        public float Gravity(float distance)
        {
            return G * mass / Mathf.Pow(distance, 2);
        }

        /// <summary>
        /// Atrae hacia su centro el objeto especificado
        /// </summary>
        /// <param name="body"></param>
        protected void Attract(GravityBody body)
        {
            Rigidbody2D rb = body.GetComponent<Rigidbody2D>();

            // Dirección de la gravedad
            Vector2 dir = ((Vector2)transform.position - rb.position);
            float distance = dir.magnitude;

            Vector2 force = dir.normalized * Gravity(distance);

            //Simula la gravedad
            rb.AddForce(force);
        }
    }
}