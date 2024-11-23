using System;
using System.Collections;
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
    [SerializeField] private Vector2 rearOffset = new Vector2(0,-1); // Distancia del pivote de giro desde el centro del colectivo.

    [Header("Drift Settings")]
    [SerializeField] private float driftFactor = 0.9f; // Derrape.

    private Rigidbody2D rb;
    public float MoveInput { get; private set; } 
    private float turnInput;

    public float SpeedPercentage => rb.velocity.magnitude / maxSpeed;
    
    private bool moving;

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
        if(rb.velocity.magnitude < 0.1 && MoveInput  != 0) moving = true;
        HandleDrift();
        HandleMovement();
        HandleSteering();
        //if (moving) StartCoroutine(AcelerationSoundManager());
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
            // Calculamos el torque de acuerdo con la entrada del jugador.
            float velocityFactor = rb.velocity.magnitude / maxSpeed;
            float torqueAmount = -turnInput * turnSpeed * velocityFactor; // Calculamos el torque en función de la entrada.

            // Aplicamos el torque para girar el autobús.
            rb.AddTorque(torqueAmount, ForceMode2D.Force);

            float desiredInertia = Mathf.Lerp(1f, 10f, SpeedPercentage);
            rb.inertia = Mathf.Lerp(rb.inertia, desiredInertia, Time.fixedDeltaTime);

            //if (torqueAmount > 0 && SpeedPercentage > 0.5) AudioManager.Instance.PlaySFXOneShot("frenada");
        }
    }

    /*
    private IEnumerator AcelerationSoundManager()
    {
        moving = false;
        var condition = new Func<bool> (()=> rb.velocity.magnitude > 0.1f);
        AudioManager.Instance.PlaySFXUntil("acelerada_loop", condition);
        yield return new WaitUntil(condition);
        //AudioManager.Instance.PlaySFXOneShot("frenada");
    }
    
    */
}

