using UnityEngine;
using System.Collections;

public class SpriteVariance : MonoBehaviour
{

    public string spritePath;
    public int gridWidth;
    public int gridHeight;


    void Awake()
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>(spritePath);
        GetComponent<SpriteRenderer>().sprite = sprites[Random.Range(0, gridWidth * gridHeight)];
    }
}
