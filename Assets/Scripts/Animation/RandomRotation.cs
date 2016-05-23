using UnityEngine;
using System.Collections;

public class RandomRotation : MonoBehaviour {
    void Start () {
        transform.Rotate(0, 0, Random.Range(0f, 360f));
    }
}
