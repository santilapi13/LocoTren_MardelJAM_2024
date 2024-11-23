using UnityEngine;

public class Player : MonoBehaviour {
    [SerializeField] private float speed = 5f;
    [SerializeField] private float turnSpeed = 200f;
    [SerializeField] private Transform pivotPoint;

    private Vector2 direction;
    private float moveInput;

    private void Update() {
        moveInput = Input.GetAxis("Vertical"); // W/S o flechas adelante/atrás.
        var turnInput = Input.GetAxis("Horizontal"); // A/D o flechas izquierda/derecha.

        transform.Translate(Vector3.up * (moveInput * speed * Time.deltaTime));

        if (moveInput != 0) {
            var turn = turnInput * turnSpeed * Time.deltaTime;
            transform.RotateAround(pivotPoint.position, Vector3.forward, -turn);
        }
    }

    public float GetMoveInput() {
        return moveInput;
    }

    public float GetSpeed() {
        return speed;
    }
}