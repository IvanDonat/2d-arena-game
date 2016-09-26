using UnityEngine;
using System.Collections;

public class PointToNearestEnemy : MonoBehaviour 
{
    private Transform nearestEnemy;
    public SpriteRenderer rend;
    private float lastRefreshedTime = 0;

    void Awake()
    {
        nearestEnemy = GetNearestEnemy();
    }

    void Update()
    {
        if (!nearestEnemy || Time.time - lastRefreshedTime > 0.5f)
        {
            nearestEnemy = GetNearestEnemy();
            lastRefreshedTime = Time.time;
            return;
        }

        rend.enabled = GetDistance() > 5;

        Vector3 dir =  nearestEnemy.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, angle);
    }

    private Transform GetNearestEnemy()
    {
        GameObject[] en = GameObject.FindGameObjectsWithTag("Enemy");
        Transform enemy = null;
        if (en.Length > 0)
        {
            float dist = 1000f;
            foreach (GameObject e in en)
            {
                if (Vector3.Distance(transform.position, e.transform.position) < dist)
                {
                    enemy = e.transform;
                    dist = Vector3.Distance(transform.position, e.transform.position);
                }
            }
        }

        if (enemy)
        {
            rend.enabled = true;
            return enemy;
        }

        // point to stations if no enemeis found
        en = GameObject.FindGameObjectsWithTag("SpaceStation");
        if (en.Length > 0)
        {
            float dist = 1000f;
            foreach (GameObject e in en)
            {
                if (Vector3.Distance(transform.position, e.transform.position) < dist)
                {
                    enemy = e.transform;
                    dist = Vector3.Distance(transform.position, e.transform.position);
                }
            }
        }

        if (enemy)
            rend.enabled = true;
        else
            rend.enabled = false;
        return enemy;
    }

    private float GetDistance()
    {
        return Vector3.Distance(transform.position, nearestEnemy.transform.position);
    }
}
