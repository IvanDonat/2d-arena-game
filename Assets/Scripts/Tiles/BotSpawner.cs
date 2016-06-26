using UnityEngine;
using System.Collections;

public class BotSpawner : MonoBehaviour {
    public GameObject botPrefab;
    public float health = 300f;
    public HealthBarScript healthBar;

    public float spawnInterval = 5f;
    private float spawnTime = 0f;

    public int maxEnemies = 5;
    private ArrayList spawnedEnemies;

    private GameManager gm;

    public AudioSource deathSound;
    public ParticleSystem particleDeath;
    public Transform debris;

    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        spawnedEnemies = new ArrayList(maxEnemies);
        transform.GetComponent<SpriteRenderer>().color = botPrefab.GetComponent<Enemy>().tint;

        healthBar.SetMaxHealth(health);
        healthBar.SetHealth(health);

        if (debris)
            debris.gameObject.SetActive(false);
    }

    void Update()
    {
        spawnTime += Time.deltaTime;
        if (spawnTime >= spawnInterval)
        {
            RefreshEnemies();
            if (spawnedEnemies.Count < maxEnemies)
                SpawnEnemy();

            spawnTime = 0f;
        }
    }


    public void SpawnEnemy()
    {
        GameObject enemy = (GameObject) Instantiate(botPrefab, transform.position, Quaternion.identity);
        spawnedEnemies.Add(enemy);

        gm.RefreshEnemyList();
    }

    public void RefreshEnemies()
    {
        ArrayList toRemove = new ArrayList();
        foreach (GameObject go in spawnedEnemies)
        {
            if (!go)
                toRemove.Add(go);
        }
        foreach (GameObject t in toRemove)
            spawnedEnemies.Remove(t);
    }

    public void TakeDamage(float dmg)
    {
        health -= dmg;
        if(healthBar)
            healthBar.SetHealth(health);

        if (dmg > 0)
        {
            GameObject damageNum = (GameObject) GameObject.Instantiate(Resources.Load(Paths.DAMAGE_NUMBERS), transform.position + (Vector3.up * transform.localScale.y) * 3, Quaternion.identity);;
            damageNum.GetComponent<DamageNumbersScript>().SetDamage(dmg);
        }

        if (health <= 0)
        {
            if (GameObject.FindGameObjectWithTag("GUI"))
                GameObject.FindGameObjectWithTag("GUI").SendMessage("PushNotification", "Destroyed station");

            if (particleDeath)
            {           
                particleDeath.transform.parent = null;
                particleDeath.GetComponent<Renderer>().sortingLayerName = "Foreground";
                particleDeath.gameObject.AddComponent<DestroyAfterTime>();
                particleDeath.Play();
            }

            if (deathSound)
            {
                deathSound.transform.parent = null;
                deathSound.gameObject.AddComponent<DestroyAfterTime>();
                deathSound.Play();
            }

            if(healthBar)
                Destroy(healthBar.gameObject);

            if (debris)
            {
                debris.gameObject.SetActive(true);
                debris.GetComponent<SpriteRenderer>().color = GetComponent<SpriteRenderer>().color;
                debris.parent = null;
            }

            Destroy(gameObject);
        }
    }
}
