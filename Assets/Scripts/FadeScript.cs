using UnityEngine;
using System.Collections;

public class FadeScript : MonoBehaviour {
    public bool appear = true;
    public float seconds;
    private float time = 0;
    private SpriteRenderer rnd;


    void Awake()
    {
        rnd = GetComponent<SpriteRenderer>();
        time = Time.realtimeSinceStartup;

        transform.position = Camera.main.transform.position + Vector3.forward;
    }

    void Update()
    {
        if(appear)
            rnd.color = new Color(rnd.color.r, rnd.color.g, rnd.color.b, (Time.realtimeSinceStartup - time) / seconds);
        else
            rnd.color = new Color(rnd.color.r, rnd.color.g, rnd.color.b, 1 - ((Time.realtimeSinceStartup - time) / seconds));

        if (!appear && Time.realtimeSinceStartup - time >= seconds)
            Destroy(gameObject);
    }
}
