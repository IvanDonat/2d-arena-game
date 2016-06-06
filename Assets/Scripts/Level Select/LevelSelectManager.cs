using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelSelectManager : MonoBehaviour {
    public Transform contentPanel;
    public GameObject listItemPrefab;

    private Level selectedLevel;
    public Text txtLevelName;
    public Text txtLevelDesc;
    public Text txtLevelPBTime;
    public Text txtLevelPBscore;


    ArrayList levels;

    void Start()
    {
        levels = new ArrayList()
            {
                new Level(Random.Range(-10000, 10000).ToString(), "Generates a random level for you to play at."),
                new Level(Random.Range(-10000, 10000).ToString(), "Generates a random level for you to play at."),
                new Level(Random.Range(-10000, 10000).ToString(), "Generates a random level for you to play at."),
                new Level(Random.Range(-10000, 10000).ToString(), "Generates a random level for you to play at."),
                new Level(Random.Range(-10000, 10000).ToString(), "Generates a random level for you to play at."),
                new Level(Random.Range(-10000, 10000).ToString(), "Generates a random level for you to play at."),
                new Level(Random.Range(-10000, 10000).ToString(), "Generates a random level for you to play at."),
                new Level(Random.Range(-10000, 10000).ToString(), "Generates a random level for you to play at."),
                new Level(Random.Range(-10000, 10000).ToString(), "Generates a random level for you to play at."),
                new Level(Random.Range(-10000, 10000).ToString(), "Generates a random level for you to play at."),
                new Level(Random.Range(-10000, 10000).ToString(), "Generates a random level for you to play at."),
                new Level(Random.Range(-10000, 10000).ToString(), "Generates a random level for you to play at."),
                new Level(Random.Range(-10000, 10000).ToString(), "Generates a random level for you to play at."),
                new Level(Random.Range(-10000, 10000).ToString(), "Generates a random level for you to play at."),
                new Level(Random.Range(-10000, 10000).ToString(), "Generates a random level for you to play at."),
                new Level(Random.Range(-10000, 10000).ToString(), "Generates a random level for you to play at."),
                new Level(Random.Range(-10000, 10000).ToString(), "Generates a random level for you to play at."),
                new Level(Random.Range(-10000, 10000).ToString(), "Generates a random level for you to play at."),
                new Level(Random.Range(-10000, 10000).ToString(), "Generates a random level for you to play at."),
                new Level(Random.Range(-10000, 10000).ToString(), "Generates a random level for you to play at."),
                new Level(Random.Range(-10000, 10000).ToString(), "Generates a random level for you to play at."),
                new Level(Random.Range(-10000, 10000).ToString(), "Generates a random level for you to play at."),
                new Level(Random.Range(-10000, 10000).ToString(), "Generates a random level for you to play at."),
                new Level(Random.Range(-10000, 10000).ToString(), "Generates a random level for you to play at."),
                new Level(Random.Range(-10000, 10000).ToString(), "Generates a random level for you to play at."),
                new Level(Random.Range(-10000, 10000).ToString(), "Generates a random level for you to play at."),
                new Level(Random.Range(-10000, 10000).ToString(), "Generates a random level for you to play at.")
            };
                
        foreach(Level l in levels)
        {
            GameObject newLevel = (GameObject) Instantiate(listItemPrefab);

            LevelSelectListItem info = newLevel.GetComponent<LevelSelectListItem>();
            info.txtName.text = l.name;
            info.SetLevel(l);

            newLevel.transform.SetParent(contentPanel, false);
        }

        contentPanel.parent.GetComponent<ScrollRect>().verticalNormalizedPosition = 1f;
        SelectLevel((Level)levels[0]);
    }

    public void PlayClicked()
    {
        // level info available via selectedLevel
        Debug.Log("Play click not implemented");
        SceneManager.LoadScene("Game");
    }

    public void SelectLevel(Level l)
    {
        this.selectedLevel = l;

        txtLevelName.text = l.name;
        txtLevelDesc.text = l.description;
        txtLevelPBTime.text = "Time: NA";
        txtLevelPBscore.text = "Score: NA";
    }
}

public class Level
{
    public string name;
    public string description;

    public Level(string name, string description)
    {
        this.name = name;
        this.description = description;
    }
}