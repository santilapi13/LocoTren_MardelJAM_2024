using Cinemachine;
using UnityEngine;

public class CameraControllerCinemachin : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    [Header("Movement Settings")] [SerializeField]
    private float baseFOV = 60f;

    [SerializeField] private float maxFOV = 2f;
    [SerializeField] private float maxYoffset = 5f;
    [SerializeField] private float fovTransitionSpeed = 2f; // Velocidad de interpolación para cambiar el FOV.

    private void Update()
    {
        var newFov = Mathf.Lerp(baseFOV, maxFOV, player.SpeedPercentage);
        virtualCamera.m_Lens.FieldOfView =
            Mathf.Lerp(virtualCamera.m_Lens.FieldOfView, newFov, fovTransitionSpeed * Time.deltaTime);
    }
}