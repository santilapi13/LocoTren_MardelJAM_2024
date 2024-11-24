using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Fotomulta : MonoBehaviour
{
    private static readonly Color red = new Color(0.91f, 0.12f, 0f, 60f/255f);
    private static readonly Color green = new Color(42f / 255f, 224 / 255f, 47 / 255f,60f/255f);
    
    [SerializeField] private float pointsPenalty = 2000f;
    [SerializeField] private string audioName;
    [SerializeField] private SpriteRenderer child;
    private bool isInCooldown = false;

    private void Update()
    {
        child.color = GameManager.Instance.PlayerPercentage > 0.6f?  red : green;
    }
    
    public void OnTriggerExit2D(Collider2D other) {
        if (isInCooldown || !other.CompareTag("Player") || !other.TryGetComponent(out Player player)) return;
        if (player.SpeedPercentage < 0.6) return;

        Debug.Log("Pollo: Fotomulta por ir a " + player.SpeedPercentage);
        GameManager.Instance.LoosePoints(pointsPenalty);
        AudioManager.Instance.PlaySFXOneShot(audioName);
        isInCooldown = true;
        child.gameObject.SetActive(false);
        StartCoroutine(RestoreCooldown());
    }

    private IEnumerator RestoreCooldown() {
        yield return new WaitForSeconds(5f);
        isInCooldown = false;
        child.gameObject.SetActive(true);
    }
}
