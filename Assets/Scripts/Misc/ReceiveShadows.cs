using UnityEngine;
using System.Collections;

public class ReceiveShadows : MonoBehaviour
{
    void Awake()
    {
        GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        GetComponent<Renderer>().receiveShadows = true;
    }
}
