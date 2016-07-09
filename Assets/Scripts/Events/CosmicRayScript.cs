using UnityEngine;
using System.Collections;

public class CosmicRayScript : MonoBehaviour {
    public float speed = 5f;

    public float initialDamage = 15f;
    public float continuousDamagePerSecond = 15f;
    private bool collidedWithPlayer = false;

    private float disappearTime = 60f;
    private float spawnTime;

    private PlayerScript ps;

    void Start()
    {
        spawnTime = Time.time;
        GetComponent<Rigidbody2D>().velocity = transform.right * speed;
    }

    void Update()
    {
        if (Time.time + spawnTime >= disappearTime)
            Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.CompareTag("Player"))
        {
            ps = c.GetComponent<PlayerScript>();
            ps.TakeDamage(initialDamage);
            collidedWithPlayer = true;
        }
    }

    void OnTriggerStay2D(Collider2D c)
    {
        if (collidedWithPlayer && c.CompareTag("Player"))
        {
            ps.TakeDamageSilent(continuousDamagePerSecond * Time.fixedDeltaTime);
        }
    }
}
