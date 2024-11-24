using System.Collections;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [Header("Movement Settings")] [SerializeField]
    private float maxSpeed = 10f; // Velocidad máxima.

    [SerializeField] private float acceleration = 5f; // Aceleración.
    [SerializeField] private float deceleration = 4f; // Desaceleración natural.
    [SerializeField] private float brakingForce = 6f; // Fuerza de frenado.
    [SerializeField] private float turnSpeed = 200f; // Velocidad de giro.

    [Header("Bus Settings")] [SerializeField]
    private Vector2 rearOffset = new(0, -1); // Distancia del pivote de giro desde el centro del colectivo.

    [Header("Drift Settings")] [SerializeField]
    private float driftFactor = 0.9f; // Derrape.

    private bool moving;

    private Rigidbody2D rb;
    private float turnInput;
    public float MoveInput { get; private set; }

    public float SpeedPercentage => rb.velocity.magnitude / maxSpeed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.centerOfMass = rearOffset;
    }

    private void Update()
    {
        // Capturar entradas del jugador.
        MoveInput = Input.GetAxis("Vertical");
        turnInput = Input.GetAxis("Horizontal");
    }

    private void FixedUpdate()
    {
        if (rb.velocity.magnitude < 0.1 && MoveInput != 0) moving = true;
        HandleDrift();
        HandleMovement();
        HandleSteering();
    }

    private void HandleDrift()
    {
        // Reducir la velocidad lateral (efecto de derrape).
        Vector2 forwardVelocity = transform.up * Vector2.Dot(rb.velocity, transform.up);
        Vector2 rightVelocity = transform.right * Vector2.Dot(rb.velocity, transform.right);
        rb.velocity = forwardVelocity + rightVelocity * driftFactor;
    }

    private void HandleMovement()
    {
        if (MoveInput > 0)
            // Aceleración hacia adelante.
            rb.AddForce(transform.up * (acceleration * MoveInput), ForceMode2D.Force);
        else if (MoveInput < 0)
            // Frenado hacia atrás.
            rb.AddForce(transform.up * (brakingForce * MoveInput), ForceMode2D.Force);
        else
            // Desaceleración natural.
            rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, deceleration * Time.fixedDeltaTime);


        // Limitar la velocidad máxima.
        if (rb.velocity.magnitude > maxSpeed) rb.velocity = rb.velocity.normalized * maxSpeed;
    }
    private void HandleSteering()
    {
        if (rb.velocity.magnitude > 0.1f)
        {
            var velocityFactor = rb.velocity.magnitude / maxSpeed;
            var torqueAmount = -turnInput * turnSpeed * velocityFactor;

            rb.AddTorque(torqueAmount, ForceMode2D.Force);

            var desiredInertia = Mathf.Lerp(1f, 10f, SpeedPercentage);
            rb.inertia = Mathf.Lerp(rb.inertia, desiredInertia, Time.fixedDeltaTime);

            //RestrictRotation();
        }
    }

    private void RestrictRotation()
    {
        float currentAngle = transform.eulerAngles.z;
        float restrictedAngle = Mathf.Round(currentAngle / 30f) * 30f;
        transform.rotation = Quaternion.Euler(0, 0, restrictedAngle);
    }

    public void Slow(float slowTime, float slowAmount) {
        rb.velocity *= 1 - slowAmount;
        StartCoroutine(FinishSlow(slowTime, slowAmount));
    }

    private IEnumerator FinishSlow(float slowTime, float slowAmount) {
        yield return new WaitForSeconds(slowTime);
        rb.velocity /= 1 - slowAmount;
    }
    
}

/*
[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float maxSpeed = 10f; // Velocidad máxima.
    [SerializeField] private float acceleration = 5f; // Aceleración.
    [SerializeField] private float deceleration = 4f; // Desaceleración natural.
    [SerializeField] private float brakingForce = 6f; // Fuerza de frenado.

    [Header("Turn Settings")]
    [SerializeField] private float turnAngleStep = 30f; // Ángulo por cada giro.
    [SerializeField] private float driftFactor = 0.9f; // Derrape.

    private Rigidbody2D rb;
    private float currentAngle; // Ángulo actual del taxi.
    private bool isTurning; // Indica si el taxi está girando en este momento.
    public float moveInput { get; private set; } 
    private float turnInput; // Entrada del jugador para girar (eje horizontal).
    
    public float SpeedPercentage => rb.velocity.magnitude / maxSpeed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Capturar entradas del jugador
        moveInput = Input.GetAxis("Vertical");
        turnInput = Input.GetAxis("Horizontal");

        // Manejar el giro solo si hay entrada horizontal
        if (!isTurning && Mathf.Abs(turnInput) > 0.1f)
        {
            Turn();
        }
    }

    private void FixedUpdate()
    {
        // Manejar derrape y movimiento
        HandleDrift();
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (moveInput > 0)
        {
            // Aceleración hacia adelante
            rb.AddForce(transform.up * (acceleration * moveInput), ForceMode2D.Force);
        }
        else if (moveInput < 0)
        {
            // Frenado hacia atrás
            rb.AddForce(transform.up * (brakingForce * moveInput), ForceMode2D.Force);
        }
        else
        {
            // Desaceleración natural
            rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, deceleration * Time.fixedDeltaTime);
        }

        // Limitar la velocidad máxima
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }

    private void HandleDrift()
    {
        // Reducir la velocidad lateral (efecto de derrape)
        Vector2 forwardVelocity = transform.up * Vector2.Dot(rb.velocity, transform.up);
        Vector2 rightVelocity = transform.right * Vector2.Dot(rb.velocity, transform.right);
        rb.velocity = forwardVelocity + rightVelocity * driftFactor;
    }

    private void Turn()
    {
        // Iniciar el giro
        isTurning = true;

        // Determinar el nuevo ángulo basado en la entrada
        float turnAmount = turnInput > 0 ? turnAngleStep : -turnAngleStep;
        currentAngle = Mathf.Repeat(currentAngle + turnAmount, 360f); // Mantener el ángulo entre 0-359

        // Aplicar la rotación al taxi
        transform.rotation = Quaternion.Euler(0, 0, currentAngle);

        // Finalizar el giro después de un frame
        StartCoroutine(ResetTurn());
    }

    private IEnumerator ResetTurn()
    {
        yield return new WaitForFixedUpdate();
        isTurning = false;
    }
    
    public void Slow(float slowTime, float slowAmount) {
        rb.velocity *= 1 - slowAmount;
        StartCoroutine(FinishSlow(slowTime, slowAmount));
    }

    private IEnumerator FinishSlow(float slowTime, float slowAmount) {
        yield return new WaitForSeconds(slowTime);
        rb.velocity /= 1 - slowAmount;
    }
}
*/
