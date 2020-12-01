using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace SeppukuStudio
{
    /// <summary>
    /// Controlador del movimiento y salto del personaje
    /// </summary>
    public class Player : GravityBody
    {
        [Header("Attributes")]
        [SerializeField] private float moveSpeed = 8f;
        [SerializeField] private float jumpForce = 220f;

        [Header("References")]
        [SerializeField] private LayerMask groundedMask;

        // Delegates
        public Delegates.VoidDelegate OnJump;

        // Variables
        public bool CanMove { get; set; }
        public float MoveDir { get; private set; }
        public bool Grounded { get; private set; }

        private List<CelestialBody> celestialBodies;
        private float groundedRayHeight;                     // Altura del rayo que comprueba si el personaje esta apoyado en el suelo

        /// <summary>
        /// Inicializa variables
        /// </summary>
        protected override void Start()
        {
            base.Start();

            // La rotación es controlada por gravityAttractor
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;

            // Crea una lista con todos los celestial bodies
            celestialBodies = new List<CelestialBody>();
            celestialBodies = FindObjectsOfType<CelestialBody>().ToList();

            // Calcula groundedRayHeight
            CapsuleCollider2D capsule = GetComponent<CapsuleCollider2D>();
            groundedRayHeight = capsule.size.y/2 + capsule.offset.y + 0.1f;
        }

        /// <summary>
        /// Obtiene input de movimiento y salto
        /// Realiza el salto en caso de input
        /// Detecta si está apoyado en el suelo actualmente
        /// </summary>
        private void Update()
        {
            if (!PauseManager.GameIsPaused)
            {
                if (CanMove)
                {
                    // Movimiento
                    MoveDir = Input.GetAxisRaw("Horizontal");

                    // Salto
                    if (Input.GetButtonDown("Jump") && Grounded) // && No estar en fase de bateo
                    {
                        rb.AddForce(transform.up * jumpForce);
                        OnJump?.Invoke();
                    }
                }
                else
                    MoveDir = 0f;

                Grounded = (Physics2D.Raycast(transform.position, -transform.up, groundedRayHeight, groundedMask));
            }
        }

        private Vector2 oldDir;

        /// <summary>
        /// Aplica el movimiento mediante fuerzas distinguiendo entre si está en el suelo o en el aire
        /// Actualiza la rotación
        /// </summary>
        private void FixedUpdate()
        {
            if (Grounded)
            {
                if (MoveDir == 0.0f)
                {
                    ApplyForceToReachVelocity(rb, transform.TransformDirection(-oldDir), 10f);
                    oldDir = oldDir * 0.1f;
                }
                else
                {
                    oldDir = new Vector2(MoveDir * moveSpeed, 0f);
                    ApplyForceToReachVelocity(rb, transform.TransformDirection(oldDir), 10f);
                }
            }

            UpdateRotation();
        }

        public static void ApplyForceToReachVelocity(Rigidbody2D rigidbody, Vector2 velocity, float force = 1, ForceMode2D mode = ForceMode2D.Force)
        {
            if (force == 0 || velocity.magnitude == 0)
                return;

            velocity = velocity + velocity.normalized * 0.2f * rigidbody.drag;

            //force = 1 => need 1 s to reach velocity (if mass is 1) => force can be max 1 / Time.fixedDeltaTime
            force = Mathf.Clamp(force, -rigidbody.mass / Time.fixedDeltaTime, rigidbody.mass / Time.fixedDeltaTime);

            //dot product is a projection from rhs to lhs with a length of result / lhs.magnitude https://www.youtube.com/watch?v=h0NJK4mEIJU
            if (rigidbody.velocity.magnitude == 0)
            {
                rigidbody.AddForce(velocity * force, mode);
            }
            else
            {
                var velocityProjectedToTarget = (velocity.normalized * Vector2.Dot(velocity, rigidbody.velocity) / velocity.magnitude);
                rigidbody.AddForce((velocity - velocityProjectedToTarget) * force, mode);
            }
        }

        /// <summary>
        /// Actualiza la rotación del personaje para que siempre apunte hacia el planeta mas cercano
        /// </summary>
        private void UpdateRotation()
        {
            // Obtiene el planeta mas cercano
            CelestialBody nearestBody = celestialBodies.OrderBy(t => Vector3.Distance(transform.position, t.transform.position)).FirstOrDefault();

            Vector2 dir = (transform.position - nearestBody.transform.position).normalized;

            // Rotación hacia el centro del asteroide
            transform.rotation = Quaternion.FromToRotation(transform.up, dir) * transform.rotation;
        }
    }
}