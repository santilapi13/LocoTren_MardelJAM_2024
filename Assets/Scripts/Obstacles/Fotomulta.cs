using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fotomulta : MonoBehaviour
{
    [SerializeField] private float pointsPenalty = 2000f;
    [SerializeField] private string audioName;
    private bool isInCooldown = false;
    
    public void OnTriggerStay2D(Collider2D other) {
        if (isInCooldown || !other.CompareTag("Player") || !other.TryGetComponent(out Player player)) return;
        if (player.SpeedPercentage < 1f) return;

        Debug.Log("Pollo: Fotomulta por ir a " + player.SpeedPercentage);
        GameManager.Instance.LoosePoints(pointsPenalty);
        AudioManager.Instance.PlaySFXOneShot(audioName);
        isInCooldown = true;
        StartCoroutine(RestoreCooldown());
    }

    private IEnumerator RestoreCooldown() {
        yield return new WaitForSeconds(5f);
        isInCooldown = false;
    }
}
