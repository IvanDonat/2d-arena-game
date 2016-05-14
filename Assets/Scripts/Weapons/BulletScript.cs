using UnityEngine;
using System.Collections;

public enum DestroyType
{
    HIT,
    TIME
}

public class BulletScript : MonoBehaviour
{

    public float speed = 5f;
    public float dmg = 5f;

    public DestroyType destroyType = DestroyType.HIT;
    public float timeToDestroy = 3f;
    private float spawnTime;

    public bool areaDamage = false;
    public int areaRadius;

    public AudioSource soundSpawn;
    public AudioSource soundDelete;
    public ParticleSystem destroyParticles;

    private GameManager gm;

    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        GetComponent<Rigidbody2D>().velocity = transform.right * speed;
        soundSpawn.transform.parent = null;
        soundSpawn.gameObject.AddComponent<DestroyAfterTime>();

        spawnTime = Time.time;
    }

    void Update()
    {
        if (destroyType == DestroyType.TIME && Time.time - spawnTime >= timeToDestroy)
        {
            Explode();
        }
    }

    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.isTrigger)
            return;
        if (destroyType == DestroyType.HIT)
        {
            if (!areaDamage)
            {
                soundDelete.transform.parent = null;
                soundDelete.gameObject.AddComponent<DestroyAfterTime>();
                soundDelete.Play();

                destroyParticles.transform.parent = null;
                destroyParticles.gameObject.AddComponent<DestroyAfterTime>();
                destroyParticles.Play();

                c.gameObject.SendMessage("TakeDamage", dmg, SendMessageOptions.DontRequireReceiver);
                Destroy(gameObject);
            }
            else
            {
                Explode();
            }
        }
    }

    void Explode()
    {
        soundDelete.transform.parent = null;
        soundDelete.gameObject.AddComponent<DestroyAfterTime>();
        soundDelete.Play();

        destroyParticles.transform.parent = null;
        destroyParticles.gameObject.AddComponent<DestroyAfterTime>();
        destroyParticles.Play();


        if (!areaDamage)
        {
            Debug.LogError("Bullet: " + name + "  destroys on time, but doesn't do area damage");
        }
        else
        {
            ArrayList tiles = gm.GetAround(transform.position.x, transform.position.y, areaRadius);
            foreach (GameObject tile in tiles)
            {
                Vector2 deltaPos = transform.position - tile.transform.position;
                float calculatedDamage = dmg - (deltaPos.magnitude / areaRadius)*dmg;
                tile.gameObject.SendMessage("TakeDamage", calculatedDamage, SendMessageOptions.DontRequireReceiver);
            }

            if (transform.tag == "PlayerBullet")
            {
                var enemies = gm.GetEnemies();
                foreach(GameObject en in enemies)
                {
                    Vector2 deltaPos = transform.position - en.transform.position;
                    if (deltaPos.sqrMagnitude <= areaRadius * areaRadius)
                    {
                        float calculatedDamage = dmg - (deltaPos.magnitude / areaRadius)*dmg;
                        en.SendMessage("TakeDamage", calculatedDamage, SendMessageOptions.RequireReceiver);
                    }
                }
            }
            else if (transform.tag == "EnemyBullet")
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player)
                {
                    Vector2 deltaPos = transform.position - player.transform.position;
                    if (deltaPos.sqrMagnitude <= areaRadius * areaRadius)
                    {
                        float calculatedDamage = dmg - (deltaPos.magnitude / areaRadius)*dmg;
                        player.SendMessage("TakeDamage", calculatedDamage, SendMessageOptions.RequireReceiver);
                    }
                }
            }
            else
            {
                Debug.LogError("Untagged bullet/projectile, BulletScript.cs");
            }
        }

        Destroy(gameObject);
    }
}
