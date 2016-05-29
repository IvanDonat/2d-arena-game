using UnityEngine;
using System.Collections;

public class PickupHealth : MonoBehaviour {
    
    public AudioSource pickupAudio;

    public int restoreHealth = 30;

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
        // Doesn't destroy if weapon is the same as currently wielded

        if (c.isTrigger)
            return;
        if(c.tag == "Player")
        {
            if (c.GetComponent<PlayerScript>().health < c.GetComponent<PlayerScript>().maxHealth)
            {
                c.transform.BroadcastMessage("TakeDamage", -restoreHealth, SendMessageOptions.RequireReceiver);

                pickupAudio.transform.parent = null;
                pickupAudio.Play();
                pickupAudio.gameObject.AddComponent<DestroyAfterTime>();

                Destroy(gameObject);
            }
        }

    }
}
