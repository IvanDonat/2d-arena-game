using UnityEngine;
using System.Collections;

public struct InputInfo
{
    public int up, right;
    public int shootUp, shootRight;
}

public class PlayerScript : MonoBehaviour {

    private Rigidbody2D rbody;
    private PlayerHeadScript headScript;
    public GunScript currentGun;

    private float health = 100;

    public AudioSource audioHitWall;



    void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
        headScript = GetComponentInChildren<PlayerHeadScript>();
    }

    void Start()
    {

    }

    void Update()
    {
        InputInfo ii = GetInput();

        currentGun.CustomUpdate(ii);
        headScript.CustomUpdate(ii);
    }

    void FixedUpdate()
    {
        InputInfo ii = GetInput();
        Vector2 moveDirKeys = new Vector2(ii.right, ii.up);
       // Vector2 shootDirKeys = new Vector2(ii.shootRight, ii.shootUp);

        rbody.AddForce(moveDirKeys.normalized, ForceMode2D.Impulse);

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

        if (health <= 0)
        {
            print("DIED");
        }
    }
}