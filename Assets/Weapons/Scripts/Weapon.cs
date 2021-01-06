using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Camera camera;

    public ParticleSystem muzzleFlash;

    public GameObject impactEffect;

    public UIManager uIManager;

    public Animator animator;

    public bool spray;

    public int damage = 10;

    public float hitForce = 60f;

    public float fireRate = 15f;

    private float nextTimeToFire = 0f;

    public int totalAmmo = 90;

    public int cabinSize = 30;

    private int currentAmmo = -1;

    public float reloadTime = 1f;

    private bool isReloading;

    private void Start()
    {
        currentAmmo = cabinSize;
        uIManager.UpdateAmmo(currentAmmo, totalAmmo);
    }

    private void OnEnable()
    {
        isReloading = false;
        animator.SetBool("Reloading", false);

        if (currentAmmo != -1)
        {
            uIManager.UpdateAmmo(currentAmmo, totalAmmo);
        }
    }

    void Shoot()
    {
        if (isReloading || currentAmmo <= 0)
            return;

        currentAmmo--;

        uIManager.UpdateAmmo(currentAmmo, totalAmmo);

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

    IEnumerator Reload()
    {
        isReloading = true;

        animator.SetBool("Reloading", true);

        yield return new WaitForSeconds(reloadTime - .24f);

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

        uIManager.UpdateAmmo(currentAmmo, totalAmmo);

        animator.SetBool("Reloading", false);

        yield return new WaitForSeconds(.25f);

        isReloading = false;
    }

    void Update()
    {
        if (currentAmmo <= 0 && totalAmmo > 0 && !isReloading)
        {
            StartCoroutine(Reload());
        }

        if ((spray && Input.GetButton("Fire1") || !spray && Input.GetButtonDown("Fire1")) && Time.time >= nextTimeToFire)
        {
            Shoot();
            nextTimeToFire = Time.time + 1f / fireRate;
        }

        if (Input.GetKeyDown(KeyCode.R) && !isReloading && (currentAmmo != cabinSize || totalAmmo > 0))
        {
            StartCoroutine(Reload());
        }
    }
}
