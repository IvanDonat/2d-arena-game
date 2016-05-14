using UnityEngine;
using System.Collections;

public enum EnemyState
{
    ROAMING,
    AGRESSIVE,
    RETREATING
};

public class EnemyScript : MonoBehaviour
{

    private Rigidbody2D rbody;
    private EnemyHeadScript headScript;
    public ParticleSystem particleDeath;
    private GameManager gm;

    private GunScript currentGun;
    public GunScript[] guns;

    public float moveForce = 0.7f;
    private float health = 75;

    private float holdDistance = 1;

    public AudioSource audioHitWall;

    private Transform player;
    private ArrayList enemies;

    private float evadeDirReset = -5;
    private float evadeDirResetInterval = 3;
    private Vector2 evadeDir = Vector2.zero;

    private EnemyState state;

    private float lastLookedForPlayer;

    void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
        headScript = GetComponentInChildren<EnemyHeadScript>();
        if(GameObject.FindGameObjectWithTag("Player"))
            player = GameObject.FindGameObjectWithTag("Player").transform;
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        holdDistance += Random.Range(0f, 4f);

        currentGun = guns[Random.Range(0, guns.Length)];
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

        GameObject closestEnemy = GetClosestEnemy();
        if (closestEnemy != null)
        {
            Vector2 aimDir = closestEnemy.transform.position - transform.position;
            if (state == EnemyState.AGRESSIVE)
            {
                emulatedII.shootUp = (int) aimDir.y;
                emulatedII.shootRight = (int) aimDir.x;
            }
        }

        currentGun.CustomUpdate(emulatedII);
        headScript.CustomUpdate(emulatedII);
    }

    void FixedUpdate()
    {
        //Transform closestEnemy = GetClosestEnemy().transform;
        Transform closestEnemy = (player != null) ? player : null;
        if (closestEnemy == null)
            state = EnemyState.ROAMING;
        else
        {
            if ((closestEnemy.position - transform.position).sqrMagnitude <= 10 * 10)
            {
                state = EnemyState.AGRESSIVE;
            }
            else
            {
                state = EnemyState.ROAMING;
            }
        }

        Vector2 moveDir = Vector2.zero;


        if (state == EnemyState.AGRESSIVE || state == EnemyState.RETREATING)
        {

            moveDir = closestEnemy.position - transform.position;
            if (moveDir.magnitude < holdDistance)
            {
                moveDir *= -1;
                state = EnemyState.RETREATING;
            }
        }
        else if(state == EnemyState.ROAMING)
        {
            // the random evadeDir addition will do just fine
        }

        moveDir.Normalize();
        moveDir += evadeDir;
        rbody.AddForce(moveDir.normalized * moveForce, ForceMode2D.Impulse);
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

            GameObject debris = Instantiate(Resources.Load("Drops/Debris", typeof(GameObject)), transform.position, Quaternion.identity) as GameObject;
            debris.transform.localScale = new Vector3(0.8f, 0.8f, 1f);
            debris.GetComponent<WeaponDrop>().SetWeapon(currentGun.transform.name);

            Destroy(gameObject);
        }
    }

    public bool SetWeapon(string wep)
    {
        if (wep == currentGun.transform.name)
            return false;
        foreach (GunScript gs in guns)
        {
            if (gs.transform.name == wep)
            {
                currentGun = gs;
                return true;
            }
        }
        Debug.LogWarning("GunScript SetWeapon(string), no such gun found: " + wep);
        return false;
    }
}