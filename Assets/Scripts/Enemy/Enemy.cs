using UnityEngine;
using System.Collections;

public enum EnemyState
{
    ROAMING,
    AGRESSIVE,
    RETREATING
};

public enum MoveStyle
{
    STANDARD,
    BURSTS
}

public class Enemy : MonoBehaviour
{
    private Rigidbody2D rbody;
    private EnemyHeadScript headScript;
    public ParticleSystem particleDeath;
    public HealthBarScript healthBar;

    public Color tint;
    public Transform debris;

    private GunScript currentGun;
    public GunScript[] guns;

    public MoveStyle style = MoveStyle.STANDARD;
    public float moveForce = 0.7f;
    public float burstInterval = 3f;
    private float stepsSinceLastBurst = 100f;

    [System.NonSerialized]
    public float maxHealth;
    public float health = 75;

    private float holdDistance = 1;

    public bool silentUntilProvoked = false;
    public float provokedRange = 3f;
    private bool provoked = false;

    public AudioSource deathSound;   

    private Transform player;
    private ArrayList enemies;

    private float evadeDirReset = -5;
    private float evadeDirResetInterval = 3;
    private Vector2 evadeDir = Vector2.zero;

    private EnemyState state;

    private float lastLookedForPlayer;

    void Awake()
    {
        if (tint.a <= 0.9f)
            Debug.Log("Tint alpha <= 0.9f on: " + transform.name);

        rbody = GetComponent<Rigidbody2D>();
        headScript = GetComponentInChildren<EnemyHeadScript>();
        headScript.GetComponent<SpriteRenderer>().color = tint;
        if(GameObject.FindGameObjectWithTag("Player"))
            player = GameObject.FindGameObjectWithTag("Player").transform;

        debris = transform.FindChild("Debris");
        if(debris)
            debris.gameObject.SetActive(false);

        maxHealth = health;
        if (healthBar)
        {
            healthBar.SetMaxHealth(maxHealth);
            healthBar.SetHealth(health);
        }

        holdDistance += Random.Range(0f, 4f);

        SetWeapon(guns[Random.Range(0, guns.Length)].name);
    }

    public virtual void Update()
    {
        if (Time.time - evadeDirReset >= evadeDirResetInterval)
        {
            evadeDir = Random.insideUnitCircle;

            evadeDirReset = Time.time;
            evadeDirResetInterval = Random.Range(0.2f, 1f);
        }

        if (!player)
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
            
        if (silentUntilProvoked && !provoked)
            return;

        InputInfo emulatedII; 

        int yMovement = (int) rbody.velocity.y;
        int xMovement = (int) rbody.velocity.x;
        emulatedII.up = yMovement;
        emulatedII.right = xMovement;

        emulatedII.shootUp = 0;
        emulatedII.shootRight = 0;

        Transform closestEnemy = (player != null) ? player : null;
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

    public virtual void FixedUpdate()
    {
        Transform closestEnemy = (player != null) ? player : null;
        if (closestEnemy == null)
            state = EnemyState.ROAMING;
        else
        {
            if ((closestEnemy.position - transform.position).sqrMagnitude <= 50)
            {
                state = EnemyState.AGRESSIVE;
            }
            else
            {
                state = EnemyState.ROAMING;
            }

            if ((closestEnemy.position - transform.position).sqrMagnitude <= provokedRange * provokedRange)
                provoked = true;
        }

        if (silentUntilProvoked && !provoked)
            return;
        
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
        if (style == MoveStyle.STANDARD)
            rbody.AddForce(moveDir.normalized * moveForce, ForceMode2D.Impulse);
        else if (style == MoveStyle.BURSTS)
        {
            stepsSinceLastBurst++;
            if (stepsSinceLastBurst >= burstInterval)
            {
                rbody.AddForce(moveDir.normalized * moveForce, ForceMode2D.Impulse);
                stepsSinceLastBurst = 0;
            }
        }
    }

    public void TakeDamage(float dmg)
    {
        health -= dmg;
        health = Mathf.Clamp(health, 0, maxHealth);
        if(healthBar)
            healthBar.SetHealth(health);

        if (dmg > 0)
        {
            provoked = true;
            GameObject damageNum = (GameObject) GameObject.Instantiate(Resources.Load(Paths.DAMAGE_NUMBERS), transform.position + (Vector3.up * transform.localScale.y), Quaternion.identity);;
            damageNum.GetComponent<DamageNumbersScript>().SetDamage(dmg);
        }

        if (health <= 0)
        {   
            string name = transform.name;
            if (name.Contains("("))
                name = name.Substring(0, name.IndexOf("(") - 1);
            if(GameObject.FindGameObjectWithTag("GUI"))
                GameObject.FindGameObjectWithTag("GUI").SendMessage("PushNotification", "Destroyed " + name);

            if (particleDeath)
            {           
                particleDeath.transform.parent = null;
                particleDeath.gameObject.AddComponent<DestroyAfterTime>();
                particleDeath.Play();
            }

            if (deathSound)
            {
                deathSound.transform.parent = null;
                deathSound.gameObject.AddComponent<DestroyAfterTime>();
                deathSound.Play();
            }

            if(healthBar)
                Destroy(healthBar.gameObject);

            if (debris)
            {
                debris.gameObject.SetActive(true);
                debris.parent = null;
                debris.GetComponent<PickupWeapon>().SetWeapon(currentGun.transform.name);
                debris.GetComponent<SpriteRenderer>().color = tint;
            }

            Destroy(gameObject);
        }
    }

    public bool SetWeapon(string wep)
    {
        if (currentGun && wep == currentGun.transform.name)
            return false;
        foreach (GunScript gs in guns)
        {
            if (gs.transform.name == wep)
            {
                currentGun = gs;
                currentGun.color = tint;
                return true;
            }
        }
        Debug.LogWarning("GunScript SetWeapon(string), no such gun found: " + wep);
        return false;
    }

    void OnCollisionEnter2D(Collision2D c)
    {
        evadeDirReset = -5f;
    }

    public void DisableHealthBar()
    {
        Destroy(healthBar.gameObject);
    }
}