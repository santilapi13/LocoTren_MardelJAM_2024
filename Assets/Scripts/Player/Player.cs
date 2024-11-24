using System;
using System.Collections;
using UnityEngine;


public enum PlayerState {
    accelerating,
    constantSpeed,
    braking,
    stopped
}

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour {
    [Header("Movement Settings")] [SerializeField]
    private float maxSpeed = 10f; // Velocidad máxima.

    [SerializeField] private float acceleration = 5f; // Aceleración.
    [SerializeField] private float brakingForce = 6f; // Fuerza de frenado.
    public float SpeedPercentage => rb.velocity.magnitude / maxSpeed;
    public bool Drifting => SpeedPercentage > 0.7f && Mathf.Abs(turnInput) > 0.5f;
    
    [Header("Bus Settings")] [SerializeField]
    private Vector2 rearOffset = new(0, -1); // Distancia del pivote de giro desde el centro del colectivo.
    [SerializeField] Animator animator;

    [Header("Drift Settings")] [SerializeField]
    private float driftFactor = 0.9f; // Derrape.
    
    private Vector2 previousVelocityDirection;
    [SerializeField] private float rotationCooldown = 0.2f;
    private float rotationTimer;

    public Rigidbody2D rb;
    [SerializeField] private PlayerAnimation playerAnim;
    private float turnInput;
    public float MoveInput { get; private set; }

    public PlayerState PlayerState { get; private set; }

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        rb.centerOfMass = rearOffset;
        rb.rotation = 0;
    }

    private void Update() {
        MoveInput = Input.GetAxis("Vertical");
        turnInput = Input.GetAxis("Horizontal");
    }

    private void FixedUpdate() {
        if (rb.velocity.magnitude < 0.1f && MoveInput != 0) {
            previousVelocityDirection = transform.up;
        }
        
        HandleState();
        HandleDrift();
        HandleMovement();
        HandleSteering();
    }

    private void HandleDrift() {
        // Calculamos la velocidad en la dirección previa al giro
        Vector2 previousDirectionVelocity = previousVelocityDirection * 
            Vector2.Dot(rb.velocity, previousVelocityDirection);
        
        // Calculamos la velocidad en la dirección actual
        Vector2 forwardVelocity = transform.up * Vector2.Dot(rb.velocity, transform.up);
        Vector2 rightVelocity = transform.right * Vector2.Dot(rb.velocity, transform.right);
        
        // Interpolamos entre la dirección previa y la actual
        float inertiaFactor = Mathf.Clamp01(rotationTimer / rotationCooldown);
        Vector2 finalVelocity = Vector2.Lerp(
            forwardVelocity + rightVelocity * driftFactor,
            previousDirectionVelocity,
            inertiaFactor
        );
        
        rb.velocity = finalVelocity;
    }

    private void HandleMovement() {
        if (MoveInput > 0) {
            rb.AddForce(transform.up * (acceleration * MoveInput), ForceMode2D.Force);
        } else if (MoveInput < 0) {
            rb.AddForce(transform.up * (brakingForce * MoveInput), ForceMode2D.Force);
        } 
        if (rb.velocity.magnitude > maxSpeed) {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }

    private void HandleSteering() {
        rotationTimer -= Time.deltaTime;
        
        if (rb.velocity.magnitude <= 0.2f || turnInput == 0 || rotationTimer > 0) {
            return;
        }
        
        // Guardamos la dirección de velocidad actual antes de rotar
        if (rb.velocity.magnitude > 0.2f) {
            previousVelocityDirection = rb.velocity.normalized;
        }
        
        float newAngle = rb.rotation + (turnInput < 0 ? 30f : -30f);
        newAngle = Mathf.Repeat(newAngle, 360f);
        newAngle = Mathf.Round(newAngle / 30f) * 30f;
        
        rb.MoveRotation(newAngle);
        
        rotationTimer = rotationCooldown;
        playerAnim.ChangeDirection(newAngle);
    }


    public void Slow(float slowTime, float slowAmount) {
        rb.velocity *= 1 - slowAmount;
        StartCoroutine(FinishSlow(slowTime, slowAmount));
    }

    private IEnumerator FinishSlow(float slowTime, float slowAmount) {
        yield return new WaitForSeconds(slowTime);
        rb.velocity /= 1 - slowAmount;
    }
    
    
    // Estados 
    private void HandleState() {
        if (rb.velocity.magnitude < 0.1f) {
            SetPlayerState(PlayerState.stopped);
            return;
        }

        if (MoveInput * rb.velocity.normalized.y > 0.1f && SpeedPercentage < 0.7) {
            SetPlayerState(PlayerState.accelerating);
            return;
        }

        if (SpeedPercentage > 0.7) {
            SetPlayerState(PlayerState.constantSpeed);
            return;
        }


        SetPlayerState(PlayerState.braking);
    }

    private void SetPlayerState(PlayerState newState) {
        if (PlayerState == newState) return;

        PlayerState = newState;
        OnStateChanged();
    }

    private void OnStateChanged() {
        switch (PlayerState) {
            case PlayerState.stopped:
                HandleStoppedState();
                break;
            case PlayerState.accelerating:
                HandleAcceleratingState();
                break;
            case PlayerState.constantSpeed:
                HandleConstantSpeedState();
                break;
            case PlayerState.braking:
                HandleBrakingState();
                break;
        }
    }


    private void HandleStoppedState() {
        AudioManager.Instance.StopAceleration();
    }

    private void HandleAcceleratingState() {
        //AudioManager.Instance.PlayAceleration("arranque");
    }

    private void HandleConstantSpeedState() {
        //AudioManager.Instance.PlayAceleration("constante");
    }

    private void HandleBrakingState() {
        //AudioManager.Instance.PlayAceleration("desacelerar");
    }

    public void AnimatorIndex(int num, bool flip)
    {
        animator.SetInteger("index",num);
        animator.SetBool("flip", flip);
    }
}