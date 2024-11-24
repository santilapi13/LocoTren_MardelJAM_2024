using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour {
    [SerializeField] private float slowTime = 2f;
    [SerializeField] private float slowAmount = 0.5f;
    [SerializeField] private float pointsPenalty = 100f;
    [SerializeField] private string audioName;
    
    public void OnTriggerEnter2D(Collider2D other) {
        if (!other.CompareTag("Player")) return;
        
        GameManager.Instance.Crash(slowTime, slowAmount, pointsPenalty);
        AudioManager.Instance.PlaySFXOneShot(audioName);
        
        Destroy(gameObject);
    }
}
