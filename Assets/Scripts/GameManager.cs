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
    private ArrayList tiles;

    private GUIController gui;

    public bool pausable = false;

    private GameState state = GameState.PLAYING;
    public Transform fadeIn;
    private float gameoverTimestamp;

    private ArrayList enemies;
    private ArrayList stations; // spawners

    private float playTime = 0;

    // set value because WorldBounds is this size
    private int width = 50, height = 50;

    private bool paused = false;

    void Awake()
    {
        enemies = new ArrayList();
        stations = new ArrayList();
        RefreshEnemyList();
        RefreshStationList();
        gui = GameObject.FindGameObjectWithTag("GUI").GetComponent<GUIController>();
    }

    void Start()
    {
        tiles = new ArrayList();

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
        }
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

    public void RefreshEnemyList()
    {
        enemies.Clear();
        foreach(GameObject en in GameObject.FindGameObjectsWithTag("Enemy"))
            enemies.Add(en);
    }

    public void RefreshStationList()
    {
        stations.Clear();
        foreach(GameObject en in GameObject.FindGameObjectsWithTag("SpaceStation"))
            stations.Add(en);
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

    public ArrayList GetStations()
    {
        ArrayList toRemove = new ArrayList();
        foreach (GameObject go in stations)
        {
            if (!go)
                toRemove.Add(go);
        }
        foreach (GameObject t in toRemove)
            stations.Remove(t);
        return stations;
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
