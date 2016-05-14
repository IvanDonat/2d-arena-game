using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUIController : MonoBehaviour {
    public Text guiHealth;
    public Text guiEnemyCount;

    [System.NonSerialized] public int health = 100;
    [System.NonSerialized] public int enemyCount = -1;

	// Use this for initialization
	void Start () {
	
	}
	
	void Update () {
        guiHealth.text = "HEALTH: " + health.ToString();
        guiEnemyCount.text = "ENEMIES: " + enemyCount.ToString();
	}
}
