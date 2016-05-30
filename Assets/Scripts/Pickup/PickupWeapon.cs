using UnityEngine;
using System.Collections;

public class PickupWeapon : MonoBehaviour {
    
    public bool dropWeapon = true;
    private string wep = "NOT_SET";

    public AudioSource pickupAudio;

    private Vector2 initialPos;
    private float timeOffset;

    void Start()
    {
        initialPos = transform.position;
        timeOffset = Random.Range(0f, 1f);
    }

    void Update()
    {
        transform.position = initialPos + (Vector2.up * Mathf.Sin((Time.time + timeOffset) * 5)) / 5f;
    }

    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.isTrigger)
            return;
        
        if(c.tag == "Player")
        {
            /*
            if (dropWeapon && c.GetComponent<PlayerScript>().SetWeapon(wep))
            {
                pickupAudio.transform.parent = null;
                pickupAudio.Play();
                pickupAudio.gameObject.AddComponent<DestroyAfterTime>();

                Destroy(gameObject);
            } */
            PlayerScript ps = c.GetComponent<PlayerScript>();
            ps.TouchedPickup(this);
        }
    }

    void OnTriggerExit2D(Collider2D c)
    {
        if (c.isTrigger)
            return;
        
        if(c.tag == "Player")
        {
            PlayerScript ps = c.GetComponent<PlayerScript>();
            ps.ExitedPickup(this);
        }
    }

    public void Used()
    {
        pickupAudio.transform.parent = null;
        pickupAudio.Play();
        pickupAudio.gameObject.AddComponent<DestroyAfterTime>();

        Destroy(gameObject);
    }

    public void SetWeapon(string s)
    {
        wep = s;
    }

    public string GetName()
    {
        return wep;
    }
}
