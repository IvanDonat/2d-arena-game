using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour
{

    public Transform player;
    private float dampTime = .1f;
    private Vector3 velocity;

    private float lastLookedForPlayer;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        lastLookedForPlayer = Time.time;
    }

    void LateUpdate()
    {
        if (player)
        {
            Vector3 point = Camera.main.WorldToViewportPoint(player.position);
            Vector3 delta = player.position - Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
            Vector3 destination = transform.position + delta;
            transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
        }
        else
        {
            // Look every second
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
    }
}
