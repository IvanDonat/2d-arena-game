using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

public class LevelSelectManager : MonoBehaviour {
    public Color selectedColor = Color.white;
    public Color unselectedColor = Color.gray;

    public Text[] levelListObjects;

    public Text txtLevelName;
    public Text txtLevelDesc;
    public Text txtLevelDifficulty;
    public Text txtLevelPBScore;
    public Text txtLevelPBTime;

    private Level selectedLevel;
    private int selectedLevelIndex;
    private int topIndex;
    private ArrayList levels;

    void Start()
    {
        SetupLevelList();
        SelectLevel(0);
    }

    private void SetupLevelList()
    {
        levels = new ArrayList();
        levels.Add(new Level("To Battle I", "Win by elimination\n\nThis level will introduce you to several different kinds of enemies", Difficulty.EASY));
        levels.Add(new Level("Hordes I", "Win by elimination\n\nThis level is full of small packs.", Difficulty.NORMAL));
        levels.Add(new Level("Space Base I", "Win by elimination\n\nDestroy all the Stations and Enemies of the base..", Difficulty.HARD));

        if (levels.Count < levelListObjects.Length)
        {
            for (int i = levels.Count; i < levelListObjects.Length; i++)
            {
                levelListObjects[i].gameObject.SetActive(false);
            }
        }

        Array.Resize(ref levelListObjects, Mathf.Min(levels.Count, levelListObjects.Length));
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
            SceneManager.LoadScene("MainMenu");

        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Return))
            Play();


        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            SelectLevel(selectedLevelIndex - 1);

        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            SelectLevel(selectedLevelIndex + 1);
        
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            SelectLevel(selectedLevelIndex - 5);
        
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            SelectLevel(selectedLevelIndex + 5);
    }

    public void Play()
    {
        print("Loading level: " + selectedLevel.sceneName);
        SceneManager.LoadScene(selectedLevel.sceneName);
    }

    public void SelectLevel(int index)
    {
        if (index >= levels.Count)
            index = 0;
        if (index < 0)
            index = levels.Count - 1;

        bool wentDown = index > this.selectedLevelIndex;

        this.selectedLevel = (Level) levels[index];
        selectedLevelIndex = index;

        if (wentDown)
        {
            if (topIndex < selectedLevelIndex - levelListObjects.Length + 1)
            {
                topIndex = selectedLevelIndex - levelListObjects.Length + 1;
                if (topIndex < 0)
                    index = 0;
            }
        }
        else
        {
            if (selectedLevelIndex < topIndex)
                topIndex = selectedLevelIndex;
        }

        for(int i = 0; i < levelListObjects.Length; i++)
        {
            levelListObjects[i].color = unselectedColor;
            if(selectedLevelIndex == i + topIndex)
                levelListObjects[i].color = selectedColor;
            
            levelListObjects[i].text = ((Level)levels[topIndex + i]).name;
        }

        txtLevelName.text = selectedLevel.name;
        txtLevelDesc.text = selectedLevel.description;
        txtLevelDifficulty.text = "Difficulty: " + selectedLevel.difficulty.ToString();

        txtLevelPBScore.text = "Score: " + SaveManagement.GetPersonalBestScore(selectedLevel.sceneName);
        txtLevelPBTime.text = "Time: " +  SaveManagement.GetPersonalBestTime(selectedLevel.sceneName);
    }
}

public class Level
{
    public string name;
    public string description;
    public Difficulty difficulty;
    public string sceneName;

    public Level(string name, string description, Difficulty difficulty, string sceneName)
    {
        this.name = name;
        this.description = description;
        this.difficulty = difficulty;
        this.sceneName = sceneName;
    }

    public Level(string name, string description, Difficulty difficulty)
    {
        this.name = name;
        this.description = description;
        this.difficulty = difficulty;
        this.sceneName = name;
    }
}

public enum Difficulty
{
    EASY,
    NORMAL,
    HARD,
    INSANE
}