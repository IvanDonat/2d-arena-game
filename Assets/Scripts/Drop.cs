using UnityEngine;
using System.Collections;

public class Drop : MonoBehaviour {
    
    public bool dropWeapon = true;
    private string wep = "NOT_SET";

    public AudioSource pickupAudio;

    public int restoreHealth = 0;

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
        if (c.tag == "Enemy" || c.tag == "Player")
        {
            if (c.tag == "Enemy")
            {
                bool used = false;
                if (dropWeapon && c.GetComponent<EnemyScript>().SetWeapon(wep))
                {
                    used = true;
                }
                if (restoreHealth >= 1 && c.GetComponent<EnemyScript>().health < c.GetComponent<EnemyScript>().maxHealth)
                {
                    used = true;
                    c.transform.BroadcastMessage("TakeDamage", -restoreHealth, SendMessageOptions.RequireReceiver);
                }
                if (used)
                {
                    Destroy(gameObject);
                }
            }
            if(c.tag == "Player")
            {
                bool used = false;
                if (dropWeapon && c.GetComponent<PlayerScript>().SetWeapon(wep))
                {
                    used = true;
                }
                if (restoreHealth >= 1 && c.GetComponent<PlayerScript>().health < c.GetComponent<PlayerScript>().maxHealth)
                {
                    used = true;
                    c.transform.BroadcastMessage("TakeDamage", -restoreHealth, SendMessageOptions.RequireReceiver);
                }
                if (used)
                {
                    pickupAudio.transform.parent = null;
                    pickupAudio.Play();
                    pickupAudio.gameObject.AddComponent<DestroyAfterTime>();

                    Destroy(gameObject);
                }
            }
        }
    }

    public void SetWeapon(string s)
    {
        wep = s;
    }

}
