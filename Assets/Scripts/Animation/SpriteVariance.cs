using UnityEngine;
using System.Collections;

public class SpriteVariance : MonoBehaviour
{
    public string spritePath;
    public int gridWidth;
    public int gridHeight;

    void Start()
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>(spritePath);
        GetComponent<SpriteRenderer>().sprite = sprites[Random.Range(0, gridWidth * gridHeight)];

        transform.localScale = new Vector3(0.8f, 0.8f, 1);
    }
}
