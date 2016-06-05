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
                new Level("Level 1", "asdad"),
                new Level("Level 2", "desc"),
                new Level("Level 3", "info"),                
                new Level("Level 4", "asdad"),
                new Level("Level 5", "desc"),
                new Level("Level 6", "info"),
                new Level("Level 7", "asdad"),
                new Level("Level 8", "desc"),
                new Level("Level 9", "info"),
                new Level("Level 10", "asdad"),
                new Level("Level 11", "desc"),
                new Level("Level 12", "info"),
                new Level("Level 10", "asdad"),
                new Level("Level 11", "desc"),
                new Level("Level 12", "info"),
                new Level("Level 10", "asdad"),
                new Level("Level 11", "desc"),
                new Level("Level 12", "info"),
                new Level("Level 10", "asdad"),
                new Level("Level 11", "desc"),
                new Level("Level 12", "info")
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