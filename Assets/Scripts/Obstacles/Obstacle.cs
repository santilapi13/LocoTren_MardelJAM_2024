using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour {
    [SerializeField] private float slowTime = 2f;
    [SerializeField] private float slowAmount = 0.5f;
    [SerializeField] private float pointsPenalty = 100f;
    [SerializeField] private string audioName;
    
    SpriteRenderer spriteRenderer;
    BoxCollider2D collider2D;

    [SerializeField] private float velocityToBrack = 0.7f;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider2D = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        collider2D.isTrigger = GameManager.Instance.PlayerPercentage > velocityToBrack;
    }


    public void OnTriggerEnter2D(Collider2D other) {
        if (!other.CompareTag("Player")) return;
        
        GameManager.Instance.Crash(slowTime, slowAmount, pointsPenalty);
        AudioManager.Instance.PlaySFXOneShot(audioName);

        StartCoroutine(regenerateObstacle());
    }

    public IEnumerator regenerateObstacle()
    {
        spriteRenderer.enabled = false;
        collider2D.enabled = false;
        yield return new WaitForSeconds(5f);
        spriteRenderer.enabled = true;
        collider2D.enabled = true;
    }
}
