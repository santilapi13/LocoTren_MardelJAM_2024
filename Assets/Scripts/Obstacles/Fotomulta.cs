using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fotomulta : MonoBehaviour
{
    [SerializeField] private float pointsPenalty = 2000f;
    [SerializeField] private string audioName;
    
    public void OnTriggerExit2D(Collider2D other) {
        if (!other.CompareTag("Player")) return;
        
        GameManager.Instance.LoosePoints(pointsPenalty);
        AudioManager.Instance.PlaySFXOneShot(audioName);
    }
}
