using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] Camera FPCamera;
    [SerializeField] float range = 100f;
    [SerializeField] float damage = 30f;
    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] GameObject hitEffect;
    [SerializeField] Ammo ammoSlot;
    [SerializeField] AmmoType ammoType; // Biến đánh dấu loại đạn
    [SerializeField] float timeBetweenShots;
    [SerializeField] int pellets = 10; // Số lượng đạn bắn ra mỗi lần
    [SerializeField] float spreadAngle = 5f; // Góc phân tán của đạn
    [SerializeField] bool isShotgun = false; // Biến kiểm tra vũ khí có phải là shotgun hay không
    [SerializeField] Recoil recoil;
    [SerializeField] bool isAutomatic = false; // Biến kiểm tra vũ khí có thể bắn liên thanh hay không

    bool canShoot = true;

    [SerializeField] GameObject fleshHitEffect;

    void Update()
    {
        if (isAutomatic)
        {
            if (Input.GetButton("Fire1") && canShoot == true)
            {
                StartCoroutine(Shoot());
            }
        }
        else
        {
            if (Input.GetButtonDown("Fire1") && canShoot == true)
            {
                StartCoroutine(Shoot());
            }
        }
    }

    IEnumerator Shoot()
    {
        canShoot = false;
        if (ammoSlot.GetCurrentAmmo(ammoType) > 0)
        {
            PlayMuzzleFlash();
            ProcessRaycast();
            recoil.RecoilFire();
            ammoSlot.ReduceCurrentAmmo(ammoType);
        }
        yield return new WaitForSeconds(timeBetweenShots);
        canShoot = true;
    }

    private void PlayMuzzleFlash()
    {
        muzzleFlash.Play();
    }

    private void ProcessRaycast()
    {
        if (isShotgun)
        {
            for (int i = 0; i < pellets; i++)
            {
                Vector3 direction = FPCamera.transform.forward;
                direction.x += UnityEngine.Random.Range(-spreadAngle, spreadAngle);
                direction.y += UnityEngine.Random.Range(-spreadAngle, spreadAngle);

                RaycastHit hit;
                if (Physics.Raycast(FPCamera.transform.position, direction, out hit, range))
                {
                    CreateHitImpact(hit);
                    Debug.Log("I hit this thing: " + hit.transform.name);

                    EnemyHealth target = hit.transform.GetComponent<EnemyHealth>();
                    if (target != null)
                    {
                        target.TakeDamage(damage);
                    }
                }
            }
        }
        else
        {
            RaycastHit hit;
            if (Physics.Raycast(FPCamera.transform.position, FPCamera.transform.forward, out hit, range))
            {
                CreateHitImpact(hit);
                Debug.Log("I hit this thing: " + hit.transform.name);

                EnemyHealth target = hit.transform.GetComponent<EnemyHealth>();
                if (target != null)
                {
                    target.TakeDamage(damage);
                }
            }
        }
    }

    private void CreateHitImpact(RaycastHit hit)
    {
        GameObject impact;

        if (hit.transform.GetComponent<EnemyHealth>() != null)
        {
            impact = fleshHitEffect;
        }
        else
        {
            impact = hitEffect;
        }

        GameObject instantiatedImpact = Instantiate(impact, hit.point, Quaternion.LookRotation(hit.normal));
        instantiatedImpact.transform.parent = hit.transform;
        Destroy(instantiatedImpact, 1);
    }
}