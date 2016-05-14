using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUIController : MonoBehaviour {
    public Transform pausedGUI;
    private bool paused = false;

    public Text guiHealth;
    private Color initialHPColor;
    private int health = 100;

    public Text guiEnemyCount; 
    private int enemyCount = -1;

	// Use this for initialization
	void Start () {
        initialHPColor = guiHealth.color;
        pausedGUI.gameObject.SetActive(false);
	}
	
	void Update () {
        guiHealth.text = "HEALTH: " + health.ToString();
        guiHealth.color = Color.Lerp(guiHealth.color, initialHPColor, Time.deltaTime * 5f);

        guiEnemyCount.text = "ENEMIES: " + enemyCount.ToString();
        guiEnemyCount.color = Color.Lerp(guiEnemyCount.color, initialHPColor, Time.deltaTime * 5f);
	}

    public void SetPaused(bool p)
    {
        paused = p;
        pausedGUI.gameObject.SetActive(paused);
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
