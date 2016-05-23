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
    public float force = 1f;

    public DestroyType destroyType = DestroyType.HIT;
    public float timeToDestroy = 3f;
    private float spawnTime;

    public bool areaDamage = false;
    public int areaRadius;

    public bool isHoming = false;
    public float homingDist = 10f;
    public float rotateSpeed = 10f;
    private Transform target;
    private float lastLookedForTarget;

    public AudioSource soundSpawn;
    public AudioSource soundDelete;
    public ParticleSystem destroyParticles;

    private GameManager gm;
    private Rigidbody2D rbody;

    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        rbody = GetComponent<Rigidbody2D>();
        rbody.velocity = transform.right * speed;
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

        if (isHoming)
        {
            if (target)
            {
                Vector2 dir = target.position - transform.position;
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                Quaternion rot = Quaternion.AngleAxis(angle, Vector3.forward);

                transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * rotateSpeed);
                rbody.velocity = transform.right * speed;
            }
            else
            {
                if (Time.time - lastLookedForTarget > 1f)
                {
                    if (transform.tag == "PlayerBullet")
                    {
                        ArrayList enemies = gm.GetEnemies();
                        float minDist = homingDist;
                        foreach(GameObject en in enemies)
                        {
                            Vector2 dir = new Vector2(en.transform.position.x - transform.position.x, en.transform.position.y - transform.position.y);
                            float angle = Vector2.Angle(transform.forward, dir);

                            if (dir.magnitude < minDist)
                            {
                                target = en.transform;
                                minDist = dir.magnitude;
                            }
                        }
                    }
                    else if (transform.tag == "EnemyBullet")
                    {
                        if(GameObject.FindGameObjectWithTag("Player"))
                            target = GameObject.FindGameObjectWithTag("Player").transform;
                    }
                    else
                    {
                        Debug.LogError("Unknown tag homing bullet, BulletScript");
                    }

                    lastLookedForTarget = Time.time;
                }
            }
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
                if (c.GetComponent<Rigidbody2D>())
                {
                    c.GetComponent<Rigidbody2D>().AddForce(transform.GetComponent<Rigidbody2D>().velocity.normalized * force, ForceMode2D.Impulse);
                }

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
            ArrayList tiles = gm.GetTiles();
            foreach (GameObject tile in tiles)
            {
                if ((tile.transform.position - transform.position).sqrMagnitude > areaRadius * areaRadius)
                    continue;
                
                Vector2 deltaPos = transform.position - tile.transform.position;
                float tileRadius = (tile.transform.localScale.x + tile.transform.localScale.y) / 2;
                float calculatedDamage = dmg - ((deltaPos.magnitude - tileRadius) / areaRadius)*dmg;
                tile.gameObject.SendMessage("TakeDamage", calculatedDamage, SendMessageOptions.DontRequireReceiver);
            }

            if (transform.tag == "PlayerBullet")
            {
                var enemies = gm.GetEnemies();
                foreach(GameObject en in enemies)
                {
                    Vector2 deltaPos = -transform.position + en.transform.position;
                    if (deltaPos.sqrMagnitude <= areaRadius * areaRadius)
                    {
                        float calculatedDamage = dmg - (deltaPos.magnitude / areaRadius)*dmg;
                        en.SendMessage("TakeDamage", calculatedDamage, SendMessageOptions.RequireReceiver);

                        Rigidbody2D rb = en.GetComponent<Rigidbody2D>();
                        rb.AddForce(deltaPos.normalized * force * (1 - deltaPos.magnitude / areaRadius), ForceMode2D.Impulse);
                    }
                }
            }
            else if (transform.tag == "EnemyBullet")
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player)
                {
                    Vector2 deltaPos = -transform.position + player.transform.position;
                    if (deltaPos.sqrMagnitude <= areaRadius * areaRadius)
                    {
                        float calculatedDamage = dmg - (deltaPos.magnitude / areaRadius)*dmg;
                        player.SendMessage("TakeDamage", calculatedDamage, SendMessageOptions.RequireReceiver);

                        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
                        rb.AddForce(deltaPos.normalized * force * (1 - deltaPos.magnitude / areaRadius), ForceMode2D.Impulse);
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
