using UnityEngine;
using System.Collections;

public class CosmicRayScript : MonoBehaviour {
    public float speed = 5f;

    public float initialDamage = 15f;
    public float continuousDamagePerSecond = 15f;
    private bool collidedWithPlayer = false;

    private GameManager gm;
    private float worldWidth, worldHeight;

    private PlayerScript ps;

    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        worldWidth = gm.GetWorldDimensions().x;
        worldHeight = gm.GetWorldDimensions().y;

        GetComponent<Rigidbody2D>().velocity = transform.right * speed;
    }

    void Update()
    {
        if (transform.position.x >= worldWidth + 5)
            Destroy(gameObject);
        else if (transform.position.x <= -worldWidth - 5)
            Destroy(gameObject);
        else if (transform.position.y >= worldHeight + 5)
            Destroy(gameObject);
        else if (transform.position.y <= -worldHeight - 5)
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
