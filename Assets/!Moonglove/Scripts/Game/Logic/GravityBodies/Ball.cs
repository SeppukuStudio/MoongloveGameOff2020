using UnityEngine;

namespace SeppukuStudio
{
    /// <summary>
    /// Controlador de la pelota
    /// </summary>
    public class Ball : GravityBody
    {
        [SerializeField] private float dragAfterHit = 0.0f; //rozamiento después de batear la pelota

        // References
        private Moon moon;

        /// <summary>
        /// Obtiene referencias
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            moon = FindObjectOfType<Moon>();
        }

        /// <summary>
        /// Añade una fuerza inicial para que gire alrededor de la luna
        /// Modifica el linear drag para que no caiga
        /// </summary>
        public void ShootBall()
        {
            rb.velocity = GetOrbitalVelocity() * GetOrbitalDirection() * -1;
            rb.drag = 0f;
        }

        /// <summary>
        /// Aplica una fuerza a la pelota
        /// Modifica el linear drag para que la pelota vaya cayendo poco a poco
        /// </summary>
        /// <param name="force"></param>
        public void Hit(float forceMultiplier)
        {
            rb.velocity = GetOrbitalVelocity() * GetOrbitalDirection() * forceMultiplier;
            rb.drag = dragAfterHit;
            //Invoke?.OnHit();
        }

        /// <summary>
        /// Devuelve la fuerza que tiene que aplicar para entrar en orbita
        /// </summary>
        /// <returns></returns>
        private float GetOrbitalVelocity()
        {
            float distance = (transform.position - Vector3.zero).magnitude;
            return Mathf.Sqrt(moon.Gravity(distance) * distance);
        }

        /// <summary>
        /// Obtiene la dirección que tiene que tener la pelota para entrar en órbita
        /// </summary>
        /// <returns></returns>
        private Vector2 GetOrbitalDirection()
        {
            //Distancia entre la pelota y el centro de la luna
            Vector2 distance = (transform.position - Vector3.zero).normalized;

            //Se halla el vector perpendicular
            Vector2 vForce = Vector2.Perpendicular(distance).normalized;

            //Como el sentido es antihorario y queremos horario, cambiamos las coordenadas
            vForce.y = -vForce.y;
            vForce.x = -vForce.x;
            Debug.Log("Vector distancia: " + distance.normalized);
            Debug.Log("Vector director: " + vForce);
            return vForce;
        }
    }
}
