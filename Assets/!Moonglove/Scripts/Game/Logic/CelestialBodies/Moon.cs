using UnityEngine;

namespace SeppukuStudio
{
    /// <summary>
    /// La luna a parte de atraer al jugador, atrae hacia su centro a la pelota
    /// Detecta la colisiion con la pelota e informa a MoonshotManager
    /// </summary>
    public class Moon : CelestialBody
    {
        private GravityBody ball;

        protected override void Awake()
        {
            base.Awake();
            ball = FindObjectOfType<Ball>().GetComponent<GravityBody>();
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            Attract(ball);
        }

        /// <summary>
        /// Detecta la colision con la pelota e informa al MoonshotManager
        /// </summary>
        /// <param name="collision"></param>
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject == ball.gameObject)
                MoonshotManager.Instance.BallFallen();
        }
    }
}