using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerWeaponsManager : MonoBehaviour
{
    public List<WeaponController> startingWeapons = new List<WeaponController>();
    public Transform weaponRoot;
    public LayerMask FPSWeaponLayer;

    public UnityAction<WeaponController, int> onAddWeapon;//add weapon event
    public UnityAction<WeaponController, int> onRemoveWeapon;//remove weapon event
    public int activeWeaponIndex { get; private set; }


    WeaponController[] m_WeaponSlots = new WeaponController[3];
    bool m_FireInputWasHeld = false;

    void Start()
    {
        activeWeaponIndex = 0;
        foreach (var weapon in startingWeapons)
        {
            AddWeapon(weapon);
        }
    }

    public bool AddWeapon(WeaponController weaponPrefab)
    {
        if (HasWeapon(weaponPrefab))
        {
            return false;
        }

        for (int i = 0; i < m_WeaponSlots.Length; i++)
        {
            if (m_WeaponSlots[i] == null)
            {
                WeaponController weaponInstance = Instantiate(weaponPrefab, weaponRoot);
                weaponInstance.transform.localPosition = Vector3.zero;
                weaponInstance.transform.localRotation = Quaternion.identity;

                weaponInstance.owner = gameObject;
                weaponInstance.sourcePrefab = weaponPrefab.gameObject;
                //weaponInstance.ShowWeapon(false);

                //This function converts a layermask to a layer index
                int layerIndex = Mathf.RoundToInt(Mathf.Log(FPSWeaponLayer.value, 2));
                foreach (Transform t in weaponInstance.gameObject.GetComponentsInChildren<Transform>(true))
                {
                    t.gameObject.layer = layerIndex;
                }

                m_WeaponSlots[i] = weaponInstance;

                if (onAddWeapon != null)
                {
                    onAddWeapon.Invoke(weaponInstance, i);
                }

                return true;
            }
        }
        return false;

    }

    public bool HasWeapon(WeaponController weaponPrefab)
    {
        // Checks if we already have a weapon coming from the specified prefab
        foreach (var w in m_WeaponSlots)
        {
            if (w != null && w.sourcePrefab == weaponPrefab.gameObject)
            {
                return true;
            }
        }

        return false;
    }

    // Update is called once per frame
    void Update()
    {
        WeaponController activeWeapon = GetActiveWeapon();
        if (activeWeapon)
        {
            bool hasFired = activeWeapon.HandleShootInputs(
                GetFireInputDown(),
                GetFireInputHeld(),
                GetFireInputReleased());

        }
    }

    private void LateUpdate()
    {
        m_FireInputWasHeld = GetFireInputHeld();
    }

    public WeaponController GetActiveWeapon()
    {
        return GetWeaponAtSlotIndex(activeWeaponIndex);
    }

    public WeaponController GetWeaponAtSlotIndex(int index)
    {
        // find the active weapon in our weapon slots based on our active weapon index
        if (index >= 0 &&
            index < m_WeaponSlots.Length)
        {
            return m_WeaponSlots[index];
        }

        // if we didn't find a valid active weapon in our weapon slots, return null
        return null;
    }

    public bool GetFireInputDown()
    {
        return GetFireInputHeld() && !m_FireInputWasHeld;
    }

    public bool GetFireInputReleased()
    {
        return !GetFireInputHeld() && m_FireInputWasHeld;
    }

    public bool GetFireInputHeld()
    {
        if (GameFlowManager.CanProcessInput())
        {
            return Input.GetButton("Fire");
        }

        return false;
    }
}
