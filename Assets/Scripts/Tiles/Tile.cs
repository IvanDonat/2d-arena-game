using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour
{

    public bool destroyable = true;
    public float health = 15f;
    public ParticleSystem destroyParticles;
    public Transform drop;
    private string tileDamagePath = "Arena/Tiles/TileDamage";

    void Start()
    {
	
    }


    void Update()
    {
	
    }

    public void TakeDamage(float dmg)
    {
        health -= dmg;

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
                GameObject.Instantiate(drop, transform.position, transform.rotation);
            }

            Destroy(gameObject);
        }
    }
}
