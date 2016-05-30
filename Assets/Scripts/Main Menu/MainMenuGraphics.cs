using UnityEngine;
using System.Collections;

public class MainMenuGraphics : MonoBehaviour {
    public int width = 48, height = 48;
    public int numEnemies = 12;

    void Start()
    {
        GenerateArena();
        SpawnEnemies();
    }

    void GenerateArena()
    {
        GameObject bgParent = new GameObject("Background");
        bgParent.transform.parent = transform;
        GameObject tileParent = new GameObject("Tiles");
        tileParent.transform.parent = transform;


        int offset_w = -Mathf.FloorToInt(width / 2);
        int offset_h = Mathf.FloorToInt(height / 2);

        string borderStraight = Paths.ARENA + "BorderStraight";
        string borderCurve = Paths.ARENA + "BorderCurve";

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (x == 0 || y == 0 || x == width - 1 || y == height - 1)
                {
                    bool curve = false;
                    int rot = 90;
                    if (x == 0 && y == 0)
                    {
                        curve = true;
                        rot = 0;
                    }
                    else if (x == width - 1 && y == 0)
                    {
                        curve = true;
                        rot = 270;
                    }
                    else if (x == width - 1 && y == height - 1)
                    {
                        curve = true;
                        rot = 180;
                    }
                    else if (x == 0 && y == width - 1)
                    {
                        curve = true;
                        rot = 90;
                    }

                    if (!curve)
                    {
                        if (y == 0)
                            rot = 0;
                        else if (x == 0)
                            rot = 90;
                        else if (x == width - 1)
                            rot = -90;
                        else
                            rot = 180;
                    }

                    GameObject instance;
                    if (curve)
                    {
                        instance = Instantiate(Resources.Load(borderCurve, typeof(GameObject))) as GameObject;
                    }
                    else
                    {
                        instance = Instantiate(Resources.Load(borderStraight, typeof(GameObject))) as GameObject;
                    }
                    instance.transform.position = new Vector2(x + offset_w, -y + offset_h);
                    instance.transform.parent = bgParent.transform;
                    instance.transform.Rotate(0, 0, rot);
                }
            }
        }

        for (int y = 4; y < height - 5; y+=3)
        {
            for (int x = 4; x < width - 5; x+=3)
            {
                int rr = Random.Range(0, 100);
                if (rr <= 20)
                {
                    GameObject instance = Instantiate(Resources.Load(Paths.TILES + "Meteor", typeof(GameObject))) as GameObject;
                    instance.transform.position = new Vector2(x + offset_w + Random.Range(-1, 1), -y + offset_h + Random.Range(-1, 1));

                    float scale = Random.Range(0.3f, 3f);
                    instance.transform.localScale = new Vector3(scale, scale, 1);

                    instance.transform.parent = tileParent.transform;
                }
            }
        }
    }

    void SpawnEnemies()
    {
        int offset_w = -Mathf.FloorToInt(width / 2);
        int offset_h = Mathf.FloorToInt(height / 2);

        for (int i = 0; i < numEnemies; i++)
        {
            Vector2 randomPos = new Vector2(Random.Range(2, width - 2) + offset_w, -Random.Range(2, height - 2) + offset_h);
            Instantiate(Resources.Load(Paths.ENEMIES + "Huangse"), randomPos, Quaternion.identity);
        }
    }

}
