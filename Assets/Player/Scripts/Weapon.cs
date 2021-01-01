using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private int damage = 10;

    public Camera camera;

    public ParticleSystem muzzleFlash;

    public GameObject impactEffect;

    void Shoot()
    {
        muzzleFlash.Play();

        RaycastHit hitTarget;
        var isHit = Physics.Raycast(camera.transform.position, camera.transform.forward, out hitTarget);

        if (isHit)
        {
            var collapsable = hitTarget.transform.GetComponent<Collapsable>();

            if (collapsable)
            {
                collapsable.Collapse();
            }

            var target = hitTarget.transform.GetComponent<Target>();

            if (target)
            {
                target.TakeDamage(damage);
            }

            var impactFlare = Instantiate(impactEffect, hitTarget.point, Quaternion.LookRotation(hitTarget.normal));
            Destroy(impactFlare, 0.2f);
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }
}
