using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{

    public bool createTileBackground = true;
    public int width = 48, height = 48;
    private GameObject[,] tiles; // this exists solely for area damage

    private int groupW = 8, groupH = 8;
    private GameObject[,] groupParent;

    // true if main menu, immutable tiles
    public bool immutable = false;

    public int numEnemies = 7;
    private ArrayList enemies;

    private bool paused = false;

    void Awake()
    {
        enemies = new ArrayList();
        foreach(GameObject en in GameObject.FindGameObjectsWithTag("Enemy"))
            enemies.Add(en);
    }

    void Start()
    {
        tiles = new GameObject[width, height];
        GenerateArena();
        SpawnPlayerAndEnemies();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            paused = !paused;
            if (paused)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
        }
    }

    void GenerateArena()
    {
        GameObject bgParent = new GameObject("Background");
        bgParent.transform.parent = transform;
        GameObject tileParent = new GameObject("Tiles");
        tileParent.transform.parent = transform;

        groupParent = new GameObject[groupW, groupH];

        // Create background
        int offset_w = -Mathf.FloorToInt(width / 2);
        int offset_h = Mathf.FloorToInt(height / 2);
       
        string floor = "Arena/Floor";
        string borderStraight = "Arena/BorderStraight";
        string borderCurve = "Arena/BorderCurve";

        if (createTileBackground)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (groupParent[x / groupW, y / groupH] == null)
                    {
                        groupParent[x / groupW, y / groupH] = new GameObject("Group " + x / groupW + " " + y / groupH);
                        groupParent[x / groupW, y / groupH].transform.parent = bgParent.transform;
                    }

                    GameObject instance;
                    if (!(x == 0 || y == 0 || x == width - 1 || y == height - 1))
                    {
                        instance = Instantiate(Resources.Load(floor, typeof(GameObject))) as GameObject;
                        instance.transform.position = new Vector2(x + offset_w, -y + offset_h);
                        //   instance.transform.parent = bgParent.transform;
                        instance.transform.parent = groupParent[x / groupW, y / groupH].transform;
                    }
                }
            }
        }

        // Create Border
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

        // Generate Tiles
        for (int y = 4; y < height - 5; y+=3)
        {
            for (int x = 4; x < width - 5; x+=3)
            {
                int rr = Random.Range(0, 100);
                if (rr <= 10)
                {
                    GameObject instance = Instantiate(Resources.Load("Arena/Tiles/Meteor", typeof(GameObject))) as GameObject;
                    instance.transform.position = new Vector2(x + offset_w + Random.Range(-1, 1), -y + offset_h + Random.Range(-1, 1));

                    float scale = Random.Range(0.3f, 3f);
                    instance.transform.localScale = new Vector3(scale, scale, 1);

                    instance.transform.parent = tileParent.transform;
                    if (immutable)
                        instance.GetComponent<Tile>().destroyable = false;
                    tiles[x, y] = instance;
                }
               /* if (rr >= 90)
                {
                    GameObject instance = Instantiate(Resources.Load("Arena/Tiles/Crate", typeof(GameObject))) as GameObject;
                    instance.transform.position = new Vector2(x + offset_w + Random.Range(-1, 1), -y + offset_h + Random.Range(-1, 1));

                    float scale = Random.Range(0.8f, 1.2f);
                    instance.transform.localScale = new Vector3(scale, scale, 1);

                    if (immutable)
                        instance.GetComponent<Tile>().destroyable = false;
                    tiles[x, y] = instance;
                } */
            }
        }
    }

    void SpawnPlayerAndEnemies()
    {
        int offset_w = -Mathf.FloorToInt(width / 2);
        int offset_h = Mathf.FloorToInt(height / 2);

        for (int i = 0; i < numEnemies; i++)
        {
            Vector2 randomPos = new Vector2(Random.Range(2, width - 2) + offset_w, -Random.Range(2, height - 2) + offset_h);
            GameObject enemy = Instantiate(Resources.Load("Enemy"), randomPos, Quaternion.identity) as GameObject;
            enemies.Add(enemy);
        }
    }

    // Returns all GameObject tiles around a point (xf_c, yf_c) with a given radius
    public ArrayList GetAround(float xf_c, float yf_c, int radius)
    {
        int offset_w = -Mathf.FloorToInt(width / 2);
        int offset_h = Mathf.FloorToInt(height / 2);

        int x_c = Mathf.FloorToInt(xf_c - offset_w);
        int y_c = Mathf.FloorToInt(height - yf_c - offset_h);
        
        ArrayList list = new ArrayList();
        for (int x = x_c - radius; x <= x_c + radius; x++)
        {
            for (int y = y_c - radius; y <= y_c + radius; y++)
            {
                if (x < 0 || y < 0)
                    continue;
                if (x >= width || y >= height)
                    continue;

                if (tiles[x, y])
                    list.Add(tiles[x, y]);
            }
        }
        return list;
    }

    public ArrayList GetEnemies()
    {
        ArrayList toRemove = new ArrayList();
        foreach (GameObject go in enemies)
        {
            if (!go)
                toRemove.Add(go);
        }
        foreach (GameObject t in toRemove)
            enemies.Remove(t);
        return enemies;
    }

    public bool GetPaused()
    {
        return paused;
    }
}
