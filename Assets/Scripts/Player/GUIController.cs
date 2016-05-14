using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUIController : MonoBehaviour {
    public Text guiHealth;
    private Color initialHPColor;
    private int health = 100;


    public Text guiEnemyCount; 
    private int enemyCount = -1;

	// Use this for initialization
	void Start () {
        initialHPColor = guiHealth.color;
	}
	
	void Update () {
        guiHealth.text = "HEALTH: " + health.ToString();
        guiHealth.color = Color.Lerp(guiHealth.color, initialHPColor, Time.deltaTime * 5f);

        guiEnemyCount.text = "ENEMIES: " + enemyCount.ToString();
        guiEnemyCount.color = Color.Lerp(guiEnemyCount.color, initialHPColor, Time.deltaTime * 5f);
	}

    public void SetHP(int hp)
    {
        health = Mathf.Clamp(hp, 0, int.MaxValue);
        guiHealth.color = Color.red;
    }

    public void SetEnemyCount(int c)
    {
        if (c != enemyCount)
            guiEnemyCount.color = Color.gray;
        enemyCount = c;
    }
}
