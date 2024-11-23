using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrencitoControler : MonoBehaviour
{
    public float speed = 5f; // Velocidad de movimiento.
    public float turnSpeed = 200f; // Velocidad de giro.
    public Transform pivotPoint; // Punto de pivote para giros (generalmente la parte trasera del autobús).

    private Vector2 direction; // Dirección de movimiento.

    void Update()
    {
        // Entrada del jugador.
        float moveInput = Input.GetAxis("Vertical"); // W/S o flechas adelante/atrás.
        float turnInput = Input.GetAxis("Horizontal"); // A/D o flechas izquierda/derecha.

        // Movimiento hacia adelante o atrás.
        transform.Translate(Vector3.up * moveInput * speed * Time.deltaTime);

        // Giro del autobús alrededor del pivote trasero.
        if (moveInput != 0)
        {
            float turn = turnInput * turnSpeed * Time.deltaTime;
            transform.RotateAround(pivotPoint.position, Vector3.forward, -turn);
        }
    }

    
}
