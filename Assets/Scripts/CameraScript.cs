using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour
{
    public bool followEnemies = false;

    private Transform player;
    private Transform enemy;

    private float dampTime = .1f;
    private Vector3 velocity;

    private float lastLookedForPlayer;
    private float pickEnemyResetTime = 10f;

    void Start()
    {
        if(GameObject.FindGameObjectWithTag("Player"))
            player = GameObject.FindGameObjectWithTag("Player").transform;
        
        if (GameObject.FindGameObjectWithTag("Enemy"))
            enemy = GameObject.FindGameObjectWithTag("Enemy").transform;

        lastLookedForPlayer = Time.time;
    }

    void LateUpdate()
    {
        Vector3 destination = Vector3.zero;
        if (player)
        {
            Vector3 point = Camera.main.WorldToViewportPoint(player.position);
            Vector3 delta = player.position - Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
            destination = transform.position + delta;
        }
        else
        {
            if (GameObject.FindGameObjectWithTag("Enemy") != null)
            {
                pickEnemyResetTime -= Time.deltaTime;
                if(pickEnemyResetTime <= 0)
                {
                    GameObject[] en = GameObject.FindGameObjectsWithTag("Enemy");
                    enemy = en[Random.Range(0, en.Length)].transform;
                    pickEnemyResetTime = 10f;
                }

                Vector3 pos = enemy.position;
                Vector3 point = Camera.main.WorldToViewportPoint(pos);
                Vector3 delta = pos - Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
                destination = transform.position + delta;
            }

            // Look for player every second
            if (Time.time - lastLookedForPlayer > 1f)
            {
                GameObject p = GameObject.FindGameObjectWithTag("Player");
                if (p != null)
                {
                    player = GameObject.FindGameObjectWithTag("Player").transform;
                }
                lastLookedForPlayer = Time.time;
            }
        }
        transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
    }
}
