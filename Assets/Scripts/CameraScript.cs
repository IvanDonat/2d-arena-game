using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour
{
    public bool followEnemies = false;

    public Transform player;
    private float dampTime = .1f;
    private Vector3 velocity;

    private float lastLookedForPlayer;

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
            if (GameObject.FindGameObjectWithTag("Enemy") != null)
            {
                Vector3 pos = GameObject.FindGameObjectWithTag("Enemy").transform.position;
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
