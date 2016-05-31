using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public enum GameState
{
    PLAYING,
    WON,
    LOST
}

public class GameManager : MonoBehaviour
{

    public bool createTileBackground = true;
    private ArrayList tiles;

    private GUIController gui;

    public bool pausable = false;

    private GameState state = GameState.PLAYING;
    public Transform fadeOut;
    private float gameoverTimestamp;

    public int numEnemies = 7;
    private ArrayList enemies;

    // set value because WorldBounds is this size
    private int width = 50, height = 50;

    private bool paused = false;

    void Awake()
    {
        enemies = new ArrayList();
        foreach(GameObject en in GameObject.FindGameObjectsWithTag("Enemy"))
            enemies.Add(en);
        gui = GameObject.FindGameObjectWithTag("GUI").GetComponent<GUIController>();
    }

    void Start()
    {
        tiles = new ArrayList();
        GenerateArena();
        SpawnPlayerAndEnemies();

        Camera.main.GetComponent<CameraScript>().SetBounds(width, height);
    }

    void Update()
    {
        CheckWinConditions();

        if (state != GameState.PLAYING)
        {
            if (Time.time - gameoverTimestamp >= 2f)
            {
                if (state == GameState.WON)
                    SceneManager.LoadScene("GameWon");
                else
                    SceneManager.LoadScene("GameLost");
            }
        }

        if (pausable && Input.GetKeyDown(KeyCode.Escape) && state == GameState.PLAYING)
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

    private void CheckWinConditions()
    {
        if (state == GameState.PLAYING && GetEnemies().Count == 0)
        {
            state = GameState.WON;
            gameoverTimestamp = Time.time;
            Instantiate(fadeOut);
            gui.gameObject.SetActive(false);
        }
    }

    public void PlayerDied()
    {
        if (state == GameState.PLAYING)
        {
            state = GameState.LOST;
            gameoverTimestamp = Time.time;
            Instantiate(fadeOut);
            gui.gameObject.SetActive(false);
        }
    }

    void GenerateArena()
    {
        GameObject tileParent = new GameObject("Tiles");
        tileParent.transform.parent = transform;

        // Create background
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
                    tiles.Add(instance);
                }

                if (rr > 10 && rr <= 12)
                {
                    GameObject instance = Instantiate(Resources.Load(Paths.ARENA + "Wormhole", typeof(GameObject))) as GameObject;
                    instance.transform.position = new Vector2(x + offset_w + Random.Range(-1, 1), -y + offset_h + Random.Range(-1, 1));

                    instance.transform.parent = tileParent.transform;
                    tiles.Add(instance);
                }
                if (rr > 12 && rr <= 16)
                {
                    GameObject instance = Instantiate(Resources.Load(Paths.ARENA + "WormholeExit", typeof(GameObject))) as GameObject;
                    instance.transform.position = new Vector2(x + offset_w + Random.Range(-1, 1), -y + offset_h + Random.Range(-1, 1));

                    instance.transform.parent = tileParent.transform;
                    tiles.Add(instance);
                }
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
            GameObject enemy = Instantiate(Resources.Load(Paths.ENEMIES + "Huangse"), randomPos, Quaternion.identity) as GameObject;
            enemies.Add(enemy);
        }
    }

    void SetPaused(bool p)
    {
        paused = p;
        if (paused)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    // Returns all GameObject tiles around a point (xf_c, yf_c) with a given radius
    public ArrayList GetTiles()
    {
        ArrayList toRemove = new ArrayList();
        foreach (GameObject go in tiles)
        {
            if (!go)
                toRemove.Add(go);
        }
        foreach (GameObject t in toRemove)
            tiles.Remove(t);
        return tiles;
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
