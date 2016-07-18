using UnityEngine;
using System.Collections;

public class CosmicRayScript : MonoBehaviour {
    public float speed = 5f;

    public float initialDamage = 15f;
    public float continuousDamagePerSecond = 15f;
    private bool collidedWithPlayer = false;

    private Color color;
    private float colorTimeOffset;

    private GameManager gm;
    private float worldWidth, worldHeight;

    private PlayerScript ps;
    private new SpriteRenderer renderer;

    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        worldWidth = gm.GetWorldDimensions().x;
        worldHeight = gm.GetWorldDimensions().y;

        renderer = GetComponent<SpriteRenderer>();
        color = renderer.color;

        colorTimeOffset = Random.Range(0.0f, 3.0f);

        GetComponent<Rigidbody2D>().velocity = transform.right * speed;
    }

    void Update()
    {
        if (IsOutOfBounds())
            Destroy(gameObject);

        color.a = Mathf.Sin(Time.time * 2 + colorTimeOffset) / 4 + 0.4f;
        renderer.color = color;
    }

    bool IsOutOfBounds()
    {
        if (transform.position.x >= worldWidth + 5)
             return true;
        else if (transform.position.x <= -worldWidth - 5)
            return true;
        else if (transform.position.y >= worldHeight + 5)
            return true;
        else if (transform.position.y <= -worldHeight - 5)
            return true;
        return false;
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

    void OnTriggerExit2D(Collider2D c)
    {
        if (c.CompareTag("Player"))
            collidedWithPlayer = false;
    }
}
