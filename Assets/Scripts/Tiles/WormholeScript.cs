using UnityEngine;
using System.Collections;

public class WormholeScript : MonoBehaviour {

    public float radius = 15f;
    public float force = 10f;

    private GameManager gm;

    private Transform player;
    private float lookedForPlayerTime;

    void Start () {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        if (GameObject.FindGameObjectWithTag("Player"))
            player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        lookedForPlayerTime -= Time.deltaTime;
        if (!player && lookedForPlayerTime <= 0)
        {
            if (GameObject.FindGameObjectWithTag("Player"))
                player = GameObject.FindGameObjectWithTag("Player").transform;
            lookedForPlayerTime = Time.deltaTime;
        }
    }

    void FixedUpdate () {
        foreach (GameObject g in gm.GetEnemies())
        {
            ApplyForce(g.GetComponent<Rigidbody2D>());
        }

        if (player)
            ApplyForce(player.GetComponent<Rigidbody2D>());
    }

    void ApplyForce(Rigidbody2D rb)
    {
        Vector2 delta = transform.position - rb.transform.position;
        if (delta.magnitude > radius)
            return;

        delta = delta.normalized * (1 - (delta.magnitude / radius));
        rb.AddForce(delta * force, ForceMode2D.Force);
    }

    void OnTriggerEnter2D(Collider2D c)
    {
        GameObject[] exits = GameObject.FindGameObjectsWithTag("WormholeExit");
        c.transform.position = exits[Random.Range(0, exits.Length)].transform.position;
    }
}
