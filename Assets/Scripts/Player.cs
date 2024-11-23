using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float maxSpeed = 10f; // Velocidad máxima.
    [SerializeField] private float acceleration = 5f; // Aceleración.
    [SerializeField] private float deceleration = 4f; // Desaceleración cuando no hay input.
    [SerializeField] private float brakingForce = 6f; // Fuerza de frenado cuando retrocede.
    [SerializeField] private float turnSpeed = 200f; // Velocidad de giro.

    [Header("Drift Settings")]
    [SerializeField] private float driftFactor = 0.9f; // Cuánto derrapa el vehículo.

    private Rigidbody2D rb;
    public float MoveInput { get; private set;} // Entrada del eje vertical (W/S o flechas).
    private float turnInput; // Entrada del eje horizontal (A/D o flechas).
    
    public float SpeedPercentage => rb.velocity.magnitude / maxSpeed;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        // Obtener entradas del jugador.
        MoveInput = Input.GetAxis("Vertical");
        turnInput = Input.GetAxis("Horizontal");

        // Velocidad actual.
        Vector2 forwardVelocity = transform.up * Vector2.Dot(rb.velocity, transform.up);
        Vector2 rightVelocity = transform.right * Vector2.Dot(rb.velocity, transform.right);

        // Derrape: Reduce la velocidad lateral.
        rb.velocity = forwardVelocity + rightVelocity * driftFactor;

        // Aplicar aceleración o frenado según la entrada del jugador.
        if (MoveInput > 0)
        {
            rb.AddForce(transform.up * (acceleration * MoveInput), ForceMode2D.Force);
        }
        else if (MoveInput < 0)
        {
            rb.AddForce(transform.up * (brakingForce * MoveInput), ForceMode2D.Force);
        }
        else
        {
            // Desacelerar naturalmente cuando no hay entrada.
            rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, deceleration * Time.fixedDeltaTime);
        }

        // Limitar la velocidad máxima.
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }

        // Girar el vehículo.
        if (rb.velocity.magnitude > 0.1f)
        {
            float rotationAmount = -turnInput * turnSpeed * Time.fixedDeltaTime;
            rb.MoveRotation(rb.rotation + rotationAmount);
        }
    }
}


