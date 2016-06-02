using UnityEngine;
using System.Collections;

public class GameOverGraphics : MonoBehaviour {
    private int width = 30, height = 30;

    void Start()
    {
        GenerateArena();
    }

    void GenerateArena()
    {
        GameObject tileParent = new GameObject("Tiles");
        tileParent.transform.parent = transform;


        int offset_w = -Mathf.FloorToInt(width / 2);
        int offset_h = Mathf.FloorToInt(height / 2);

        // Generate tiles
        for (int y = 0; y < height; y+=3)
        {
            for (int x = 0; x < width; x+=3)
            {
                int rr = Random.Range(0, 100);
                if (rr <= 10)
                {
                    GameObject instance = Instantiate(Resources.Load(Paths.ARENA + "Meteor", typeof(GameObject))) as GameObject;
                    instance.transform.position = new Vector2(x + offset_w + Random.Range(-1, 1), -y + offset_h + Random.Range(-1, 1));

                    float scale = Random.Range(0.3f, 3f);
                    instance.transform.localScale = new Vector3(scale, scale, 1);
                    instance.transform.parent = tileParent.transform;
                }
            }
        }
    }
}
