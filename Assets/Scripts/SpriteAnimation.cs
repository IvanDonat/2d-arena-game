using UnityEngine;
using System.Collections;

public class SpriteAnimation : MonoBehaviour {

    public Sprite[] sprites;
    private int currIndex = 0;
    private SpriteRenderer spriteRenderer;

    public float swapInterval = 0.2f;
    private float lastTimeSwapped;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update () {
        if (Time.time - lastTimeSwapped >= swapInterval)
        {
            currIndex++;
            if (currIndex >= sprites.Length)
                currIndex = 0;
            
            spriteRenderer.sprite = sprites[currIndex];
            lastTimeSwapped = Time.time;
        }
    }
}
