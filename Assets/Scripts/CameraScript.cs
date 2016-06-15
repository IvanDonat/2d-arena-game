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

    private float minx, maxx;
    private float miny, maxy;

    void Start()
    {
        if(GameObject.FindGameObjectWithTag("Player"))
            player = GameObject.FindGameObjectWithTag("Player").transform;

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
            // pick closest
            // for example when player dies

            pickEnemyResetTime -= Time.deltaTime;
            if(!enemy || pickEnemyResetTime <= 0)
            {
                GameObject[] en = GameObject.FindGameObjectsWithTag("Enemy");
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

                    pickEnemyResetTime = 10f;
                }
                else
                {
                    // Couldn't find enemy, look again in 1 second
                    pickEnemyResetTime = 1f;
                }
            }

            Vector3 pos = (enemy != null) ? enemy.position : transform.position;
            Vector3 point = Camera.main.WorldToViewportPoint(pos);
            Vector3 delta = pos - Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
            destination = transform.position + delta;

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

        ClampPosition();
    }

    void ClampPosition()
    {
        Vector3 newPos = transform.position;
        newPos.x = Mathf.Clamp(newPos.x, minx, maxx);
        newPos.y = Mathf.Clamp(newPos.y, miny, maxy);
        transform.position = newPos;
    }

    public void SetBounds(float w, float h)
    {
        w--; h--;
        w /= 2.0f;
        h /= 2.0f;

        float vertExtent = Camera.main.orthographicSize;
        float horzExtent = vertExtent * (Screen.width / (float) Screen.height);

        this.miny = vertExtent - h;
        this.maxy = h - vertExtent;        
        this.minx = horzExtent - w;
        this.maxx = w - horzExtent;
    }
}
