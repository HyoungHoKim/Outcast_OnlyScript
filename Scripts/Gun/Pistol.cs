using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Pistol : MonoBehaviour
{
    public GameObject weaponModel;

    public float CurrentAmmo;
    public float ammoCapacity;
    public float reloadTime = 2.0f;
    public bool reloadAutomatically = true;

    public GameObject projectile;
    public Transform projectileSpawnSpot;

    public float recoilKickBack = 0.1f;                            
    public float recoilRecoveryRate = 0.01f;


    //public Transform shellSpitPosition;
    public GameObject shell;                            
    public float shellSpitForce = 1.0f;                 
    public float shellForceRandom = 0.5f;               
    public float shellSpitTorqueX = 0.0f;               
    public float shellSpitTorqueY = 0.0f;               
    public float shellTorqueRandom = 1.0f;

    public GameObject[] muzzleEffects = new GameObject[] { null };
    public Transform muzzleEffectPosition;

    public AudioClip fireSound;
    public AudioClip reloadSound;
    public AudioClip dryFireSound;

    protected virtual void Awake()
    {

        if(GetComponent<AudioSource>() == null)
        {
            gameObject.AddComponent(typeof(AudioSource));
        }

        if(muzzleEffectPosition == null)
        {
            muzzleEffectPosition = gameObject.transform;
        }

        if(projectileSpawnSpot == null)
        {
            projectileSpawnSpot = gameObject.transform;
        }

        if(weaponModel == null)
        {
            weaponModel = gameObject;
        }
    }

    protected virtual void Update()
    {
        //if (reloadAutomatically && CurrentAmmo <= 0)
        //    Reload();

        //if(CurrentAmmo > 0)
        //weaponModel.transform.position = Vector3.Lerp(weaponModel.transform.position, transform.position, recoilRecoveryRate * Time.deltaTime);
        //weaponModel.transform.rotation = Quaternion.Lerp(weaponModel.transform.rotation, transform.rotation, recoilRecoveryRate * Time.deltaTime);
    }


    protected void Fire()
    {

        Debug.Log("Fire!!!!");

        if (CurrentAmmo <= 0)
        {
            DryFire();

            return;
        }

        CurrentAmmo--;

        Instantiate(projectile, projectileSpawnSpot);

       // Recoil();

        GameObject muzfx = muzzleEffects[Random.Range(0, muzzleEffects.Length)];

        if(muzfx != null)
        {
            Instantiate(muzfx, muzzleEffectPosition.position, muzzleEffectPosition.rotation);
        }

        //GameObject shellGO = Instantiate(shell, shellSpitPosition.position, shellSpitPosition.rotation) as GameObject;
        //shellGO.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(shellSpitForce + Random.Range(0, shellForceRandom), 0, 0), ForceMode.Impulse);
        //shellGO.GetComponent<Rigidbody>().AddRelativeTorque(new Vector3(shellSpitTorqueX + Random.Range(-shellTorqueRandom, shellForceRandom), shellSpitTorqueY + Random.Range(-shellTorqueRandom, shellTorqueRandom), 0), ForceMode.Impulse);


        GetComponent<AudioSource>().PlayOneShot(fireSound);
    }

    protected void DryFire()
    {
        GetComponent<AudioSource>().PlayOneShot(dryFireSound);
    }

    protected void Recoil()
    {
        weaponModel.transform.Translate(new Vector3(0, -recoilKickBack, 0), Space.Self);
    }
    public void Reload()
    {
        CurrentAmmo = ammoCapacity;
        GetComponent<AudioSource>().PlayOneShot(reloadSound);

        SendMessageUpwards("OnEasyWeaponsReload", SendMessageOptions.DontRequireReceiver);
    }
}
