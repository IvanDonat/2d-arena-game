using UnityEngine;
using System.Collections;

public class GunScript : MonoBehaviour
{

    public Transform bulletPrefab;
    public float shootDelay = 1f;
    public bool repeated = true;
    // can hold if repeated

    public bool useBuildupTime = false;
    public float buildupTime = 1f;
   
    private float lastTimeShot;

    // treba delayat jer ako se stisnu dvije tipke u isto vrijeme, zapravo je jedna ranije
    // ako je shootDelay dovoljno mali, ne kuzi se
    private bool keyHeld = false;
    private float keyClicked;
    private float holdTime;

    void Start()
    {
        
    }


    public void CustomUpdate(InputInfo ii)
    {

        if (ii.shootUp != 0 || ii.shootRight != 0)
        {
            if (!keyHeld)
                holdTime = Time.time;

            keyHeld = true;
            if (Time.time - keyClicked > .4f || shootDelay < 0.4f || keyHeld)
            {
                keyClicked = Time.time;

                Vector3 dir = new Vector3(ii.shootRight, ii.shootUp, 0);
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, angle);

                if (useBuildupTime && Time.time - holdTime >= buildupTime)
                {
                    Shoot();
                }
                else if (!useBuildupTime && Time.time - lastTimeShot >= shootDelay)
                {
                    Shoot();
                }
            }
        }
        else
            keyHeld = false;

    }

    private void Shoot()
    {
        GameObject bullet = GameObject.Instantiate(bulletPrefab, transform.position + transform.right * 0.5f, transform.rotation) as GameObject;
        lastTimeShot = Time.time;
        holdTime = Time.time;
    }
}
