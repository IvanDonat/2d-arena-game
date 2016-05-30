using UnityEngine;
using System.Collections;

public struct InputInfo
{
    public int up, right;
    public int shootUp, shootRight;
}

public class PlayerScript : MonoBehaviour
{

    private Rigidbody2D rbody;
    private PlayerHeadScript headScript;
    public GunScript[] guns;
    private GunScript currentGun;
    private GUIController gui;
    private GameManager gm;

    public ParticleSystem particleDeath;

    [System.NonSerialized]
    public float maxHealth;
    public float health = 100;

    public AudioSource audioHitWall;
    public AudioSource audioHurt;

    private float enemyCountLastUpdated = -5f;
    private float enemyCountUpdateInterval = .2f;

    private PickupWeapon lastPickupContacted = null;


    void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
        headScript = GetComponentInChildren<PlayerHeadScript>();
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        gui = GameObject.FindGameObjectWithTag("GUI").GetComponent<GUIController>();

        maxHealth = health;

        currentGun = guns[0];
    }

    void Start()
    {

    }

    void Update()
    {
        if (gm.GetPaused())
        {
            gui.SetPaused(true);
            return;
        }
        else
        {
            gui.SetPaused(false);
        }

        if (lastPickupContacted && lastPickupContacted.GetName() != currentGun.name)
        {
            gui.OnPickup(lastPickupContacted, true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                SetWeapon(lastPickupContacted.GetName());
                lastPickupContacted.Used();
            }
        }
        else
        {
            gui.OnPickup(null, false);
        }

        if (Time.time - enemyCountLastUpdated >= enemyCountUpdateInterval)
        {
            gui.SetEnemyCount(gm.GetEnemies().Count);

            enemyCountLastUpdated = Time.time;
        }

        InputInfo ii = GetInput();
        currentGun.CustomUpdate(ii);
        headScript.CustomUpdate(ii);
    }

    void FixedUpdate()
    {
        InputInfo ii = GetInput();
        Vector2 moveDirKeys = new Vector2(ii.right, ii.up);

        rbody.AddForce(moveDirKeys.normalized * 0.6f, ForceMode2D.Impulse);
        headScript.CustomFixedUpdate(ii);
    }

    private InputInfo GetInput()
    {
        int upKey = 0;
        int rightKey = 0;
        int shootUpKey = 0;
        int shootRightKey = 0;

        if (Input.GetKey(KeyCode.W))
            upKey = 1;
        if (Input.GetKey(KeyCode.S))
            upKey = -1;
        if (Input.GetKey(KeyCode.A))
            rightKey = -1;
        if (Input.GetKey(KeyCode.D))
            rightKey = 1;

        if (Input.GetKey(KeyCode.UpArrow))
            shootUpKey = 1;
        if (Input.GetKey(KeyCode.DownArrow))
            shootUpKey = -1;
        if (Input.GetKey(KeyCode.LeftArrow))
            shootRightKey = -1;
        if (Input.GetKey(KeyCode.RightArrow))
            shootRightKey = 1;

        InputInfo ii;
        ii.up = upKey;
        ii.right = rightKey;
        ii.shootUp = shootUpKey;
        ii.shootRight = shootRightKey;

        return ii;
    }

    private Vector2 lastCollisionPos = Vector2.zero;

    void OnCollisionEnter2D(Collision2D c)
    {
        if (c.relativeVelocity.magnitude > 5 && (lastCollisionPos - (Vector2)transform.position).magnitude > 1f
            && (c.collider.CompareTag("Background") || c.collider.CompareTag("Tile")))
        {
            audioHitWall.Play();
        }
        lastCollisionPos = transform.position;
    }

    public void TakeDamage(float dmg)
    {
        health -= dmg;
        health = Mathf.Clamp(health, 0, maxHealth);
        gui.SetHP((int)health);

        if (dmg < 0)
        {
            gui.PushNotification("Healed for " + -(int)dmg);
        }
        else
        {
            // workaround so it works if you die
            GameObject g = Instantiate(audioHurt.gameObject, transform.position, Quaternion.identity) as GameObject;
            g.GetComponent<AudioSource>().Play();
            g.AddComponent<DestroyAfterTime>();
        }

        if (health <= 0)
        {
            gui.PushNotification("You died");
            gm.PlayerDied();

            if (particleDeath)
            {           
                particleDeath.transform.parent = null;
                particleDeath.gameObject.AddComponent<DestroyAfterTime>();
                particleDeath.Play();
            }

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
                gui.PushNotification("Picked up: " + gs.transform.name);
                return true;
            }
        }
        Debug.LogWarning("GunScript SetWeapon(string), no such gun found: " + wep);
        return false;
    }

    public void TouchedPickup(PickupWeapon pw)
    {
        this.lastPickupContacted = pw;
    }

    public void ExitedPickup(PickupWeapon pw)
    {
        if (this.lastPickupContacted == pw)
            this.lastPickupContacted = null;
    }
}