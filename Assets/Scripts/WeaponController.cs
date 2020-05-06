using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponShootType
{
    Manual,//single shoot
    Automatic,//auto shoot
    Charge,//charge shoot
}

public class WeaponController : MonoBehaviour
{
    //inspector
    public GameObject weaponRoot;
    public Transform weaponMuzzle;
    public WeaponShootType shootType;
    public float maxAmmo = 30;
    public float delayBetweenShots = 0.5f;
    public int bulletsPerShot = 1;
    public float bulletSpreadAngle = 0;
    public Projectile projectilePrefab;
    public GameObject muzzleFlashPrefab;

    //public member
    public GameObject owner { get; set; }
    public GameObject sourcePrefab { get; set; }
    public bool isWeaponActive { get; private set; }
    public float currentCharge { get; private set; }

    //private member
    float m_CurrentAmmo;
    float m_LastTimeShot = 0;

    // Start is called before the first frame update
    void Start()
    {
        m_CurrentAmmo = maxAmmo;
    }

    public void ShowWeapon(bool show)
    {
        weaponRoot.SetActive(show);

        isWeaponActive = show;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool HandleShootInputs(bool inputDown, bool inputHeld, bool inputUp)
    {
        switch (shootType)
        {
            case WeaponShootType.Manual:
                if (inputDown)
                {
                    return TryShoot();
                }
                return false;

            case WeaponShootType.Automatic:
                if (inputHeld)
                {
                    return TryShoot();
                }
                return false;

            case WeaponShootType.Charge:
                //if (inputHeld)
                //{
                //    TryBeginCharge();
                //}
                //// Check if we released charge or if the weapon shoot autmatically when it's fully charged
                //if (inputUp)
                //{
                //    return TryReleaseCharge();
                //}
                return false;

            default:
                return false;
        }
    }

    bool TryShoot()
    {
        if (m_CurrentAmmo >= 1f
            && m_LastTimeShot + delayBetweenShots < Time.time)
        {
            HandleShoot();
            m_CurrentAmmo -= 1;

            return true;
        }

        return false;
    }

    void HandleShoot()
    {
        // spawn all bullets with random direction
        for (int i = 0; i < bulletsPerShot; i++)
        {
            Vector3 shotDirection = GetShotDirectionWithinSpread(weaponMuzzle);
            Projectile newProjectile = Instantiate(projectilePrefab, weaponMuzzle.position, Quaternion.LookRotation(shotDirection));
            newProjectile.Shoot(this);
        }

        //// muzzle flash
        if (muzzleFlashPrefab != null)
        {
            GameObject muzzleFlashInstance = Instantiate(muzzleFlashPrefab, weaponMuzzle.position, weaponMuzzle.rotation, weaponMuzzle.transform);
            Destroy(muzzleFlashInstance, 2f);
        }

        //m_LastTimeShot = Time.time;

        //// play shoot SFX
        //if (shootSFX)
        //{
        //    m_ShootAudioSource.PlayOneShot(shootSFX);
        //}

        //// Trigger attack animation if there is any
        //if (weaponAnimator)
        //{
        //    weaponAnimator.SetTrigger(k_AnimAttackParameter);
        //}

        //// Callback on shoot
        //if (onShoot != null)
        //{
        //    onShoot();
        //}
    }
    public Vector3 GetShotDirectionWithinSpread(Transform shootTransform)
    {
        float spreadAngleRatio = bulletSpreadAngle / 180f;
        Vector3 spreadWorldDirection = Vector3.Slerp(shootTransform.forward, UnityEngine.Random.insideUnitSphere, spreadAngleRatio);

        return spreadWorldDirection;
    }
}
