using UnityEngine;
using System.Collections;

public class DestroyAfterTime : MonoBehaviour {
    public float lifetime = 2f;
    private float spawnTime;

	void Start () {
            spawnTime = Time.time;
	}
	
	void Update () {
        if (Time.time - spawnTime >= lifetime)
            Destroy(gameObject);
	}
}
