using UnityEngine;
using System.Collections;

public class LevelSelectManager : MonoBehaviour {
    public Transform contentPanel;
    public GameObject listItemPrefab;

    ArrayList levels;

    void Start()
    {
        levels = new ArrayList()
        {
                new Level("Random Game", "Generates a random level for you to play at."),
        };

        foreach(Level l in levels)
        {
            GameObject newLevel = (GameObject) Instantiate(listItemPrefab);

            LevelSelectListItem info = newLevel.GetComponent<LevelSelectListItem>();
            info.txtName.text = l.name;

            newLevel.transform.SetParent(contentPanel);
            newLevel.transform.localScale = Vector3.one;
        }
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