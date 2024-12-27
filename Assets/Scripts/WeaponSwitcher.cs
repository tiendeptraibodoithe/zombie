using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    [SerializeField] public int currentWeapon = 0;

    public event Action<int> OnWeaponChanged; // Sự kiện thông báo vũ khí đã thay đổi

    void Start()
    {
        SetWeaponActive();
    }

    private void SetWeaponActive()
    {
        int weaponIndex = 0;
        foreach (Transform weapon in transform)
        {
            bool isActive = weaponIndex == currentWeapon;
            weapon.gameObject.SetActive(isActive);

            weaponIndex++;
        }
    }

    void Update()
    {
        int previousWeapon = currentWeapon;

        ProcessKeyInput();

        if (previousWeapon != currentWeapon)
        {
            SetWeaponActive();
            OnWeaponChanged?.Invoke(currentWeapon); // Gọi sự kiện khi đổi vũ khí
        }
    }

    private void ProcessKeyInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentWeapon = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentWeapon = 1;
        }
    }
}
