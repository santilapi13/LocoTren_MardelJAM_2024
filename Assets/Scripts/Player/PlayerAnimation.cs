using UnityEngine;

public class PlayerAnimation : MonoBehaviour {
    private SpriteRenderer sr;
    [SerializeField] private Player player;
    [SerializeField] private Sprite[] sprites;
    private int directionalIndex;  // 0-6
    private int orderIndex;   // 0-3
    private Sprite[,] spriteMatrix;
    private float orderChangeTime = 0.3f;
    private float timer;

    private void Start() {
        timer = orderChangeTime;
        sr = GetComponent<SpriteRenderer>();
        spriteMatrix = new Sprite[7, 4];
        var index = 0;
        for (var i = 0; i < 7; i++) {
            for (var j = 0; j < 4; j++) {
                if (index >= sprites.Length) continue;
                
                spriteMatrix[i, j] = sprites[index];
                index++;
            }
        }
    }

    public void ChangeDirection(float newAngle) {

        directionalIndex = newAngle switch {
            0 => 0,
            30 or 330 => 1,
            60 or 300 => 2,
            90 or 270 => 3,
            120 or 240 => 4,
            150 or 210 => 5,
            180 => 6,
            _ => directionalIndex 
        };

        sr.flipX = newAngle is > 0 and < 180;
        ChangeSprite();
    }

    private void FixedUpdate() {
        transform.position = player.transform.position;
        
        timer -= Time.deltaTime;
        if (!(timer <= 0)) return;
        
        timer = orderChangeTime;

        orderIndex = (orderIndex < 3) ? orderIndex + 1 : 0;
        ChangeSprite();
    }

    private void ChangeSprite() {
        sr.sprite = spriteMatrix[directionalIndex, orderIndex];
    }
}