using UnityEngine;
using System.Collections;

public class MainMenuGraphics : MonoBehaviour {    
    public int numEnemies = 12;

    // set value because WorldBounds is this size
    private int width = 50, height = 50;

    void Start()
    {
        GenerateArena();
        SpawnEnemies();

        Camera.main.GetComponent<CameraScript>().SetBounds(width, height);
    }

    void GenerateArena()
    {
        GameObject tileParent = new GameObject("Tiles");
        tileParent.transform.parent = transform;


        int offset_w = -Mathf.FloorToInt(width / 2);
        int offset_h = Mathf.FloorToInt(height / 2);

        GameObject bounds = Instantiate(Resources.Load(Paths.SCENE + "WorldBounds"), Vector3.zero, Quaternion.identity) as GameObject;
        bounds.transform.position = Vector3.zero;
        bounds.transform.parent = transform;

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

    void SpawnEnemies()
    {
        int offset_w = -Mathf.FloorToInt(width / 2);
        int offset_h = Mathf.FloorToInt(height / 2);

        for (int i = 0; i < numEnemies; i++)
        {
            Vector2 randomPos = new Vector2(Random.Range(2, width - 2) + offset_w, -Random.Range(2, height - 2) + offset_h);
            GameObject inst = (GameObject) Instantiate(Resources.Load(Paths.ENEMIES + "Huangse"), randomPos, Quaternion.identity);
            inst.GetComponent<Enemy>().DisableHealthBar();
        }
    }
}
