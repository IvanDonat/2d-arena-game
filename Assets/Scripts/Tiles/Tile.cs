using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour
{

    public bool destroyable = true;
    private float maxHealth;
    public float health = 15f;
    public ParticleSystem destroyParticles;
    public Transform drop;
    private string tileDamagePath = "Arena/Tiles/TileDamage";

    void Start()
    {
        maxHealth = health;
    }


    void Update()
    {
	
    }

    public void TakeDamage(float dmg)
    {
        health -= dmg;
        health = Mathf.Clamp(health, 0, maxHealth);

        GameObject tileDmg = GameObject.Instantiate(Resources.Load(tileDamagePath), transform.position, Quaternion.Euler(0, 0, Random.Range(0, 4) * 90)) as GameObject;
        tileDmg.transform.parent = transform;

        if (destroyable && health <= 0)
        {
            if (destroyParticles)
            {           
                destroyParticles.transform.parent = null;
                destroyParticles.gameObject.AddComponent<DestroyAfterTime>();
                destroyParticles.Play();
            }

            if (drop)
            {
                GameObject dr = Instantiate(Resources.Load("Drops/MeteorDebris", typeof(GameObject)), transform.position, Quaternion.identity) as GameObject;
                dr.transform.localScale = transform.localScale;
            }

            Destroy(gameObject);
        }
    }
}
