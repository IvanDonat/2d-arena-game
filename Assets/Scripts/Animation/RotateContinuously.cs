using UnityEngine;
using System.Collections;

public class RotateContinuously : MonoBehaviour {
    public float speed = 30;
    void Update () {
        transform.Rotate(0, 0, speed * Time.deltaTime);
    }
}
