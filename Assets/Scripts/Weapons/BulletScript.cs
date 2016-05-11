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
        if (destroyType == DestroyType.HIT)
        {
            soundDelete.transform.parent = null;
            soundDelete.gameObject.AddComponent<DestroyAfterTime>();
            soundDelete.Play();

            destroyParticles.transform.parent = null;
            destroyParticles.gameObject.AddComponent<DestroyAfterTime>();
            destroyParticles.Play();


            if (!areaDamage)
            {
                c.gameObject.SendMessage("TakeDamage", dmg, SendMessageOptions.DontRequireReceiver);
            }
            else
            {
                ArrayList tiles = gm.GetAround(c.transform.position.x, c.transform.position.y, areaRadius);
                // @TODO + add entities around it to take damage

                foreach (GameObject tile in tiles)
                {
                    tile.gameObject.SendMessage("TakeDamage", dmg, SendMessageOptions.DontRequireReceiver);
                }
            }

            Destroy(gameObject);
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
            // @TODO + add entities around it to take damage

            foreach (GameObject tile in tiles)
            {
                tile.gameObject.SendMessage("TakeDamage", dmg, SendMessageOptions.DontRequireReceiver);
            }
        }

        Destroy(gameObject);
    }
}
