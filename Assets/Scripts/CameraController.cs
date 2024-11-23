using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour 
{
    [SerializeField] private Player player;
    [SerializeField] private float smoothSpeed = 2f;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float forwardOffset = 10f;
    [SerializeField] private float transitionSpeed = 3f; 
    
    private Vector3 currentVelocity; 
    private Vector3 targetPosition;
    private float currentForwardOffset;
    
    private void Start()
    {
        targetPosition = player.transform.position + offset;
        currentForwardOffset = 0f;
    }

    private void LateUpdate()
    {
        float moveInput = player.GetMoveInput();
        float targetForwardOffset = moveInput != 0 ? forwardOffset : 0f;
        
        currentForwardOffset = Mathf.Lerp(currentForwardOffset, targetForwardOffset, Time.deltaTime * transitionSpeed);
        
        Vector3 desiredPosition = player.transform.position + offset + player.transform.up * currentForwardOffset;
        
        targetPosition = Vector3.SmoothDamp(
            transform.position,
            desiredPosition,
            ref currentVelocity,
            smoothSpeed * Time.deltaTime,
            Mathf.Infinity,
            Time.deltaTime
        );
        
        transform.position = targetPosition;
    }
}