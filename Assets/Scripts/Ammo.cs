using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    [SerializeField]
    AmmoSlot[] ammoSlots;

    [System.Serializable]
    private class AmmoSlot
    {
        public AmmoType type;
        public int ammoAmount;
    }

    public int GetCurrentAmmo(AmmoType ammoType)
    {
        AmmoSlot slot = GetAmmoSlot(ammoType);
        if (slot != null)
        {
            return slot.ammoAmount;
        }
        return 0; // Hoặc giá trị mặc định khác
    }

    public void ReduceCurrentAmmo(AmmoType ammoType)
    {
        AmmoSlot slot = GetAmmoSlot(ammoType);
        if (slot != null)
        {
            slot.ammoAmount--;
        }
    }

    private AmmoSlot GetAmmoSlot(AmmoType ammoType)
    {
        foreach (AmmoSlot slot in ammoSlots)
        {
            if (slot.type == ammoType) return slot;
        }
        return null;
    }
}