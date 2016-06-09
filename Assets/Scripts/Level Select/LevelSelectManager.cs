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
    public Text txtLevelPBTime;
    public Text txtLevelPBscore;

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
        for (int i = 1; i <= 50; i++)
        {
            levels.Add(new Level(i.ToString(), "Game", "level " + i.ToString()));
        }

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
        txtLevelPBTime.text = "Time: NA";
        txtLevelPBscore.text = "Score: NA";
    }
}

public class Level
{
    public string name;
    public string sceneName;
    public string description;

    public Level(string name, string sceneName, string description)
    {
        this.name = name;
        this.sceneName = sceneName;
        this.description = description;
    }
}