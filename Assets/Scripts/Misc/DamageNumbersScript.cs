using UnityEngine;
using System.Collections;

public class DamageNumbersScript : MonoBehaviour {
    private TextMesh textMesh;
    private int dmg = 999999;
    private float timeSpawned;
    private static Color colorTransparent = new Color(0, 0, 0, 0);

    void Awake()
    {
        textMesh = GetComponent<TextMesh>();
        timeSpawned = Time.time;
    }

    void Update()
    {
        float timeAlive = Time.time - timeSpawned;

        if (timeAlive > 0.9f)
        {
            Destroy(gameObject);
        }

        transform.Translate(Vector3.up * Time.deltaTime);
        textMesh.color = Color.Lerp(textMesh.color, colorTransparent, Time.deltaTime);
    }

    public void SetDamage(float dmg)
    {
        this.dmg = (int) dmg;
        
        if (dmg > 0)
        {
            textMesh.text = "-" + this.dmg.ToString();
        }
        else
        {
            textMesh.text = "+" + this.dmg.ToString();
            textMesh.color = Color.green;
        }
    }
}
