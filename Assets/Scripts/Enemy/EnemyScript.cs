using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour
{

    private Rigidbody2D rbody;
    private EnemyHeadScript headScript;
    public GunScript currentGun;
    public ParticleSystem particleDeath;
    private GameManager gm;

    private float health = 100;

    private float holdDistance = 3;

    public AudioSource audioHitWall;

    private Transform player;
    private ArrayList enemies;

    private float evadeDirReset;
    private float evadeDirResetInterval = 3;
    private Vector2 evadeDir = Vector2.zero;

    private bool agressive = false;

    private Light myLight;
    private float lastLookedForPlayer;

    void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
        headScript = GetComponentInChildren<EnemyHeadScript>();
        myLight = GetComponentInChildren<Light>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        holdDistance += Random.Range(0f, 4f);
    }

    void Update()
    {
        if (Time.time - evadeDirReset >= evadeDirResetInterval)
        {
            evadeDir = Random.insideUnitCircle;

            evadeDirReset = Time.time;
            evadeDirResetInterval = Random.Range(0.2f, 1f);
        }

        if (player)
        {
        }
        else
        {
            // Look every second
            if (Time.time - lastLookedForPlayer > 1f)
            {
                GameObject p = GameObject.FindGameObjectWithTag("Player");
                if (p != null)
                {
                    player = p.transform;
                }
                lastLookedForPlayer = Time.time;
            }
        }
            

        InputInfo emulatedII; 

        int yMovement = (int) rbody.velocity.y;
        int xMovement = (int) rbody.velocity.x;
        emulatedII.up = yMovement;
        emulatedII.right = xMovement;

        emulatedII.shootUp = 0;
        emulatedII.shootRight = 0;
        if (agressive)
        {
            emulatedII.shootUp = yMovement;
            emulatedII.shootRight = xMovement;
        }

        currentGun.CustomUpdate(emulatedII);
        headScript.CustomUpdate(emulatedII);
    }

    void FixedUpdate()
    {
        Transform closestEnemy = GetClosestEnemy().transform;
        Vector2 moveDir = Vector2.zero;

        moveDir = closestEnemy.position - transform.position;
        if (moveDir.magnitude < holdDistance)
        {
            moveDir *= -1;
            agressive = true;       
        }

        moveDir.Normalize();
        moveDir += evadeDir;
        rbody.AddForce(moveDir, ForceMode2D.Impulse);
    }

    GameObject GetClosestEnemy()
    {
        enemies = gm.GetEnemies();
        GameObject closestEnemy = null;
        float minDist = 1000;
        foreach (GameObject g in enemies)
        {
            if (g == gameObject)
                continue;
            if ((g.transform.position - transform.position).sqrMagnitude < minDist)
            {
                closestEnemy = g;
                minDist = (g.transform.position - transform.position).sqrMagnitude;
            }
        }

        if (player != null && (player.position - transform.position).sqrMagnitude < minDist)
        {
            closestEnemy = player.gameObject;
            minDist = (player.position - transform.position).sqrMagnitude;
        }

        return closestEnemy;
    }

    public void TakeDamage(float dmg)
    {
        health -= dmg;

        if (health <= 0)
        {   
            if (particleDeath)
            {           
                particleDeath.transform.parent = null;
                particleDeath.gameObject.AddComponent<DestroyAfterTime>();
                particleDeath.Play();
            }

            Destroy(gameObject);
        }
    }
}