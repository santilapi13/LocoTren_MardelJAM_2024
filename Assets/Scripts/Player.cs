using System;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float maxSpeed = 10f; // Velocidad máxima.
    [SerializeField] private float acceleration = 5f; // Aceleración.
    [SerializeField] private float deceleration = 4f; // Desaceleración natural.
    [SerializeField] private float brakingForce = 6f; // Fuerza de frenado.
    [SerializeField] private float turnSpeed = 200f; // Velocidad de giro.
    
    [Header("Bus Settings")]
    [SerializeField] private float pivotOffset = 1.5f; // Distancia del pivote de giro desde el centro del colectivo.

    [Header("Drift Settings")]
    [SerializeField] private float driftFactor = 0.9f; // Derrape.

    private Rigidbody2D rb;
    public float MoveInput { get; private set; } 
    private float turnInput;

    public float SpeedPercentage => rb.velocity.magnitude / maxSpeed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Capturar entradas del jugador.
        MoveInput = Input.GetAxis("Vertical");
        turnInput = Input.GetAxis("Horizontal");
    }

    private void FixedUpdate()
    {
        HandleDrift();
        HandleMovement();
        HandleSteering();
        Debug.Log($"Speed: {rb.velocity}");
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
        {
            // Aceleración hacia adelante.
            rb.AddForce(transform.up * (acceleration * MoveInput), ForceMode2D.Force);
        }
        else if (MoveInput < 0)
        {
            // Frenado hacia atrás.
            rb.AddForce(transform.up * (brakingForce * MoveInput), ForceMode2D.Force);
        }
        else
        {
            // Desaceleración natural.
            rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, deceleration * Time.fixedDeltaTime);
        }

        // Limitar la velocidad máxima.
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }
    
    
    private void HandleSteering()
    {
        if (rb.velocity.magnitude > 0.1f)
        {
            // Determinar el punto de pivote (frente del autobús).
            Vector2 pivotPoint = rb.position + (Vector2)transform.up * pivotOffset;

            // Calcular giro según la entrada y la velocidad.
            float velocityFactor = rb.velocity.magnitude / maxSpeed;
            float rotationAmount = -turnInput * turnSpeed * velocityFactor * Time.fixedDeltaTime;

            // Aplicar giro alrededor del pivote.
            rb.MoveRotation(rb.rotation + rotationAmount);

            // Ajustar posición para que el cuerpo siga al pivote.
            Vector2 newBusPosition = pivotPoint - (Vector2)transform.up * pivotOffset;
            rb.MovePosition(newBusPosition);
        }
    }
}

