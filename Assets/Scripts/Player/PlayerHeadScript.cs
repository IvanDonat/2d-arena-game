using UnityEngine;
using System.Collections;

public class PlayerHeadScript : MonoBehaviour
{

    private Rigidbody2D rbody;
    private Quaternion targetRot = Quaternion.identity;

    void Awake()
    {
        
    }

    void Start()
    {

    }

    // Called by PlayerScript
    public void CustomUpdate(InputInfo ii)
    {
        if (ii.shootUp != 0 || ii.shootRight != 0)
        {
            // face shooting dir
            Vector3 dir = new Vector3(ii.shootRight, ii.shootUp, 0);
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
            targetRot = Quaternion.Euler(transform.rotation.x, transform.rotation.y, angle);
        }
        else if (ii.up != 0 || ii.right != 0)
        {
            // face movement dir            
            Vector3 dir = new Vector3(ii.right, ii.up, 0);
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
            targetRot = Quaternion.Euler(transform.rotation.x, transform.rotation.y, angle);
        }

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 15f);
    }

    public void CustomFixedUpdate(InputInfo ii)
    {

    }
}
