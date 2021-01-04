using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Camera camera;

    public ParticleSystem muzzleFlash;

    public GameObject impactEffect;

    private int damage = 10;

    public float hitForce = 60f;

    public float fireRate = 15f;

    private float nextTimeToFire = 0f;

    public int totalAmmo = 90;

    public int cabinSize = 30;

    private int currentAmmo;

    public int reloadTime;

    private void Start()
    {
        currentAmmo = cabinSize;
    }

    void Shoot()
    {
        currentAmmo--;

        muzzleFlash.Play();

        RaycastHit hitTarget;
        var isHit = Physics.Raycast(camera.transform.position, camera.transform.forward, out hitTarget);

        if (isHit)
        {
            var collapsable = hitTarget.transform.GetComponent<Collapsable>();

            if (collapsable != null)
            {
                collapsable.Collapse();
            }

            var target = hitTarget.transform.GetComponent<Target>();

            if (target != null)
            {
                target.TakeDamage(damage);
            }

            if (hitTarget.rigidbody != null)
            {
                hitTarget.rigidbody.AddForce(-hitTarget.normal * hitForce);
            }

            if (hitTarget.transform.tag != "Player")
            {
                var impactFlare = Instantiate(impactEffect, hitTarget.point, Quaternion.LookRotation(hitTarget.normal));
                Destroy(impactFlare, 0.2f);
            }
        }
    }

    void Reload()
    {
        if (currentAmmo == cabinSize || totalAmmo <= 0)
            return;


        if (currentAmmo <= 0)
        {
            if (totalAmmo <= cabinSize)
            {
                currentAmmo = totalAmmo;
                totalAmmo = 0;
            } else
            {
                currentAmmo = cabinSize;
                totalAmmo -= cabinSize;
            }
        } else
        {
            var missingAmmo = cabinSize - currentAmmo;

            if (totalAmmo <= missingAmmo)
            {
                currentAmmo += totalAmmo;
                totalAmmo = 0;
            } else
            {
                currentAmmo += missingAmmo;
                totalAmmo -= missingAmmo;
            }
        }
    }

    void Update()
    {
        if (currentAmmo <= 0)
        {
            if (totalAmmo > 0)
            {
                Reload();
            }
            
            return;
        }

        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            Shoot();
            nextTimeToFire = Time.time + 1f / fireRate;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
    }
}
