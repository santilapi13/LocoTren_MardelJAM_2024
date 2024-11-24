using UnityEngine;

public class PlayerAnimation : MonoBehaviour {
    private SpriteRenderer sr;
    [SerializeField] private Player player;
    [SerializeField] private Sprite[] sprites;
    private int directionalIndex;  // 0-6
    private int orderIndex;   // 0-3
    private Sprite[,] spriteMatrix;
    private float orderChangeTime = 1f;
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

    private void FixedUpdate() {
        transform.position = player.transform.position;
        var angle = player.transform.eulerAngles.z;
        directionalIndex = angle switch {
            < 10 or > 350 => 0,
            < 30 or > 330 => 1,
            < 80 or > 280 => 2,
            < 100 or > 260 => 3,
            < 150 or > 210 => 4,
            < 170 or > 190 => 5,
            _ => 6
        };

        sr.flipX = angle is > 0 and < 180;

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