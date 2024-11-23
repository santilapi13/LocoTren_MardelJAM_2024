using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.Mathematics;
using UnityEngine;

public class CameraControllerCinemachin : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    
    [Header("Drift Settings")]
    [SerializeField] private float rotationTransitionSpeed = 5f;
    
    [Header("Movement Settings")]
    [SerializeField] private float baseFOV = 60f;
    [SerializeField] private float maxFOV = 2f;
    [SerializeField] private float maxYoffset = 5f;
    [SerializeField] private float fovTransitionSpeed = 2f; // Velocidad de interpolación para cambiar el FOV.
    
    private CinemachineFramingTransposer framingTransposer;
    private float yOffset;
    
    private void Start()
    {
        // Obtén el componente FramingTransposer para controlar el offset.
        framingTransposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        yOffset = 0f;
    }
    
    private void Update()
    {
        var speedPercentage = player.CurrentSpeed / player.MaxSpeed;
        var newFov = Mathf.Lerp(baseFOV, maxFOV, speedPercentage);
        virtualCamera.m_Lens.FieldOfView = Mathf.Lerp(virtualCamera.m_Lens.FieldOfView, newFov, fovTransitionSpeed * Time.deltaTime);
        yOffset =  Mathf.Lerp(yOffset,speedPercentage > 0.7f? 0f : maxYoffset*speedPercentage,fovTransitionSpeed * Time.deltaTime);
        framingTransposer.m_TrackedObjectOffset =  new Vector3(0.0f, yOffset, 0.0f);
        
        
        virtualCamera.m_Lens.Dutch += Mathf.DeltaAngle(virtualCamera.m_Lens.Dutch, player.Rotation) * rotationTransitionSpeed * Time.deltaTime;
        
        
        
    }

}
