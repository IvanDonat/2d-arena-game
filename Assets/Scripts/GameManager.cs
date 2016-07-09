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

    private GUIController gui;

    public bool pausable = false;

    public float cosmicRaySpawnInterval = -1f; // negative number = disabled
    private float cosmicRayTimer = 0;

    private GameState state = GameState.PLAYING;
    public Transform fadeIn;
    private float gameoverTimestamp;

    private ArrayList tiles;
    private ArrayList enemies;
    private ArrayList stations; // spawners

    private float playTime = 0;

    // set value because WorldBounds is this size
    private int width = 50, height = 50;

    private bool paused = false;

    void Awake()
    {
        tiles = new ArrayList();
        enemies = new ArrayList();
        stations = new ArrayList();
        RefreshTileList();
        RefreshEnemyList();
        RefreshStationList();

        gui = GameObject.FindGameObjectWithTag("GUI").GetComponent<GUIController>();

        cosmicRayTimer = cosmicRaySpawnInterval;
    }

    void Start()
    {
        // set because of WorldBounds size
        Camera.main.GetComponent<CameraScript>().SetBounds(width, height);
    }

    void Update()
    {
        CheckWinConditions();

        if (state != GameState.PLAYING)
        {
            if (Time.time - gameoverTimestamp >= 2f)
            {
                GameOver();
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

        if (!paused)
        {
            playTime += Time.deltaTime;

            cosmicRayTimer -= Time.deltaTime;
            if (cosmicRaySpawnInterval > 0 && cosmicRayTimer <= 0)
            { // negative interval = disabled
                SpawnCosmicRay();
                cosmicRayTimer = cosmicRaySpawnInterval;
            }
        }
    }

    private void SpawnCosmicRay()
    {
        // position around the edge of the world
        int x;
        int y;
        if (Random.Range(0, 100) >= 50)
        {
            x = width + 3;
            y = Random.Range(0, height) + 3;
        }
        else
        {
            x = Random.Range(0, width) + 3;
            y = height + 3;
        }

        if (Random.Range(0, 100) >= 50)
            x *= -1;
        if (Random.Range(0, 100) >= 50)
            y *= -1;

        Vector3 position = new Vector3(x, y, 0);

        Vector2 dir = - position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        angle += Random.Range(-30, 30);
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        GameObject.Instantiate(Resources.Load(Paths.EVENTS + "CosmicRays"), position, rotation);
    }

    private void CheckWinConditions()
    {
        if (state == GameState.PLAYING && GetEnemies().Count == 0 && GetStations().Count == 0)
        {
            state = GameState.WON;
            gameoverTimestamp = Time.time;
            Instantiate(fadeIn);
            gui.gameObject.SetActive(false);
        }
    }

    private void GameOver()
    {
        if (state == GameState.WON)
            GameOverManager.infoWon = true;
        else
            GameOverManager.infoWon = false;

        int score = 0;
        score += (int) Mathf.Clamp(1000 - playTime*2, 0, Mathf.Infinity);

        GameOverManager.infoScore = score;
        GameOverManager.infoTime = (int) playTime;
        GameOverManager.scene = SceneManager.GetActiveScene().name;

        if (state == GameState.LOST)
        {
            GameOverManager.infoScore = 0;
            GameOverManager.infoNewBest = false;
        }

        if (state == GameState.WON)
        {
            if (SaveManagement.SaveScore(GameOverManager.scene, GameOverManager.infoScore, GameOverManager.infoTime))
                GameOverManager.infoNewBest = true;
            else
                GameOverManager.infoNewBest = false;
        }

        SceneManager.LoadScene("GameOver");
    }

    public void PlayerDied()
    {
        if (state == GameState.PLAYING)
        {
            state = GameState.LOST;
            gameoverTimestamp = Time.time;
            Instantiate(fadeIn);
            gui.gameObject.SetActive(false);
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

    public void RefreshTileList()
    {
        tiles.Clear();
        foreach (GameObject t in GameObject.FindGameObjectsWithTag("Tile"))
            tiles.Add(t);
    }

    public void RefreshEnemyList()
    {
        enemies.Clear();
        foreach(GameObject en in GameObject.FindGameObjectsWithTag("Enemy"))
            enemies.Add(en);
    }

    public void RefreshStationList()
    {
        stations.Clear();
        foreach(GameObject s in GameObject.FindGameObjectsWithTag("SpaceStation"))
            stations.Add(s);
    }

    public ArrayList GetTiles()
    {
        while (tiles.Contains(null))
            tiles.RemoveAt(tiles.IndexOf(null));
        return tiles;
    }

    public ArrayList GetEnemies()
    {
        while (enemies.Contains(null))
            enemies.RemoveAt(enemies.IndexOf(null));
        return enemies;
    }

    public ArrayList GetStations()
    {
        while (stations.Contains(null))
            stations.RemoveAt(stations.IndexOf(null));
        return stations;
    }

    public Vector2 GetWorldDimensions()
    {
        return new Vector2(width, height);
    }

    public bool GetPaused()
    {
        return paused;
    }

    public bool IsGameOver()
    {
        return state == GameState.LOST || state == GameState.WON;
    }
}
