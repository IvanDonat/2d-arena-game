using UnityEngine;
using System.Collections;

public class EnemyHeadScript : MonoBehaviour
{
    private Quaternion targetRot = Quaternion.identity;

    // Use this for initialization
    void Start()
    {
	
    }

    // Called by EnemyScript
    // Uses emulated (faked) InputInfo
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

}
