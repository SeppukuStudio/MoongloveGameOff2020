using UnityEngine;

/// <summary>
/// Objeto que es afectado por la gravedad de los cuerpos celestes
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class GravityBody : MonoBehaviour
{
    protected Rigidbody2D rb;

    /// <summary>
    /// Obtiene referencias
    /// </summary>
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Inicializa variables
    /// </summary>
    protected virtual void Start()
    {
        // La gravedad es controlada por CelestialBody
        rb.gravityScale = 0f;

        // La rotación es controlada por la simulacion fisica
        rb.constraints = RigidbodyConstraints2D.None;
    }
}
