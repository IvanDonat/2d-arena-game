using UnityEngine;
using System.Collections;

public class WeaponDrop : MonoBehaviour {
    public string[] weaponNames;
    private string wep;

    public TextMesh text;

    private Vector2 initialPos;
    private float timeOffset;

    void Awake()
    {
        initialPos = transform.position;
        timeOffset = Random.Range(0f, 1f);

        wep = weaponNames[Random.Range(0, weaponNames.Length)];
        text.text = wep.Substring(0, 1);
    }

    void Update()
    {
        transform.position = initialPos + (Vector2.up * Mathf.Sin((Time.time + timeOffset) * 5)) / 5f;
    }

    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.isTrigger)
            return;
        if (c.tag == "Enemy" || c.tag == "Player")
        {
            c.transform.BroadcastMessage("SetWeapon", wep, SendMessageOptions.RequireReceiver);
            Destroy(gameObject);
        }
    }

}
