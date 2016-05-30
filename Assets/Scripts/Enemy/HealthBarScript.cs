using UnityEngine;
using System.Collections;

public class HealthBarScript : MonoBehaviour {
    
    private Transform owner;
    private SpriteRenderer render;

    private float offset;
    private float health;
    private float maxHealth;
    private float transparency = 0.5f;

    private Color colorFull;
    private Color colorEmpty;

    void Start()
    {
        // health bar unparents itself from the ship, and follows its position
        // both have a reference to each other

        owner = transform.parent;
        offset = transform.localPosition.y;
        render = GetComponent<SpriteRenderer>();

        colorFull = Color.green;
        colorEmpty = Color.red;

        colorFull.a = transparency;
        colorEmpty.a = transparency;
    }

    void Update()
    {
        if (!owner)
            Destroy(gameObject);

        transform.position = owner.position + (Vector3.up * offset);
        transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(health / 20f, transform.localScale.y, transform.localScale.z), Time.deltaTime * 10f);
        render.color = Color.Lerp(colorEmpty, colorFull, health / maxHealth);
    }

    public void SetHealth(float hp)
    {
        this.health = hp;
    }

    public void SetMaxHealth(float mhp)
    {
        this.maxHealth = mhp;
    }
}
