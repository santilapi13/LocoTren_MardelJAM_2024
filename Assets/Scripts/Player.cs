using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float maxSpeed = 10f; // Velocidad máxima.
    [SerializeField] private float acceleration = 5f; // Aceleración.
    [SerializeField] private float deceleration = 4f; // Desaceleración cuando no hay input.
    [SerializeField] private float brakingForce = 6f; // Fuerza de frenado cuando retrocede.
    [SerializeField] private float turnSpeed = 200f; // Velocidad de giro.

    [Header("Drift Settings")]
    [SerializeField] private float driftFactor = 0.9f; // Factor de derrape (0.9 es moderado).
    [SerializeField] private float highSpeedTurnReduction = 0.5f; // Reducción del giro a alta velocidad.
    [SerializeField] private float driftSlowdown = 0.95f; // Reducción de velocidad al derrapar.
    
    public float CurrentSpeed { get; private set;}// Velocidad actual del vehículo.
    public float MoveInput { get; private set;}// Entrada del eje vertical (W/S o flechas).
    public float TurnInput { get; private set;} // Entrada del eje horizontal (A/D o flechas).
    public float Rotation => transform.eulerAngles.z;
    public float MaxSpeed => maxSpeed;

    private void Update()
    {
        // Obtener entradas del jugador.
        MoveInput = Input.GetAxis("Vertical"); 
        TurnInput = Input.GetAxis("Horizontal");

        // Aceleración o frenado según la entrada del jugador.
        if (MoveInput > 0)
        {
            CurrentSpeed = Mathf.MoveTowards(CurrentSpeed, maxSpeed, acceleration * Time.deltaTime);
        }
        else if (MoveInput < 0)
        {
            CurrentSpeed = Mathf.MoveTowards (CurrentSpeed, -maxSpeed / 2, brakingForce * Time.deltaTime);
        }
        else
        {
            CurrentSpeed = Mathf.MoveTowards(CurrentSpeed, 0, deceleration * Time.deltaTime);
        }

        // Aplicar movimiento.
        transform.Translate(Vector3.up * (CurrentSpeed * Time.deltaTime));

        // Reducir capacidad de giro a altas velocidades.
        float adjustedTurnSpeed = turnSpeed * (1 - (Mathf.Abs(CurrentSpeed) / maxSpeed) * (1 - highSpeedTurnReduction));

        // Rotar el vehículo.
        if (CurrentSpeed != 0)
        {
            transform.Rotate(Vector3.forward, - TurnInput * adjustedTurnSpeed * Time.deltaTime);
        }

        // Simular derrape.
        if (Mathf.Abs(TurnInput) > 0.5f && Mathf.Abs(CurrentSpeed) > maxSpeed * 0.7f)
        {
            // Aplicar desaceleración por derrape.
            CurrentSpeed *= driftSlowdown;
        }
        
        
    }
    
}
