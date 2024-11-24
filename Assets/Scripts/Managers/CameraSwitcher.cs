using Cinemachine;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    [Header("Camera")] [SerializeField] private CinemachineVirtualCamera slowSpeedCamera; // Cámara para baja velocidad.

    [SerializeField] private CinemachineVirtualCamera highSpeedCamera;

    [Header("Player")] [SerializeField] private Player player; // Referencia al auto o jugador.

    [SerializeField] private float speedThreshold = 0.7f; // Velocidad para cambiar de cámara.

    private void Start()
    {
        highSpeedCamera.Priority = 5;
        slowSpeedCamera.Priority = 10;
    }

    private void Update()
    {
        // Cambia la prioridad de las cámaras según la velocidad.
        if (player.SpeedPercentage >= speedThreshold && player.MoveInput > 0)
        {
            // Alta velocidad: prioriza la cámara de alta velocidad.
            highSpeedCamera.Priority = 10;
            slowSpeedCamera.Priority = 5;
        }
        else
        {
            // Baja velocidad: prioriza la cámara de baja velocidad.
            highSpeedCamera.Priority = 5;
            slowSpeedCamera.Priority = 10;
        }
    }
}